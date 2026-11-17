use std::fs::{self, File};
use std::io::{self, BufReader, Read, Write};
use std::path::{Path, PathBuf};
use tauri::AppHandle;
use zip::ZipArchive;

use crate::servers::{
    clear_instance_incomplete, ensure_library_directories, find_sct_data_dir, format_bytes,
    mark_instance_incomplete, resolve_instance_dir,
};
use crate::steamcmd::{
    begin_steam_task, emit_log, is_task_cancelled, update_task_label, SteamCmdRunResult,
};

fn sanitize_file_name(raw: &str) -> Option<String> {
    let trimmed = raw
        .trim()
        .trim_matches('"')
        .trim_matches('\'')
        .split(['/', '\\'])
        .last()
        .unwrap_or("")
        .trim();
    if trimmed.is_empty() || trimmed == "." || trimmed == ".." {
        return None;
    }
    let clean: String = trimmed
        .chars()
        .filter(|c| !matches!(c, '<' | '>' | ':' | '"' | '/' | '\\' | '|' | '?' | '*'))
        .collect();
    if clean.is_empty() {
        None
    } else {
        Some(clean)
    }
}

pub(crate) fn filename_from_content_disposition(header_val: &str) -> Option<String> {
    for part in header_val.split(';') {
        let p = part.trim();
        if let Some(rest) = p
            .strip_prefix("filename*=")
            .or_else(|| p.strip_prefix("FILENAME*="))
        {
            let value = rest.split("''").last().unwrap_or(rest);
            if let Some(name) = sanitize_file_name(value) {
                return Some(name);
            }
        }
        if let Some(rest) = p
            .strip_prefix("filename=")
            .or_else(|| p.strip_prefix("FILENAME="))
        {
            if let Some(name) = sanitize_file_name(rest) {
                return Some(name);
            }
        }
    }
    None
}

pub(crate) fn filename_from_url(url: &str) -> Option<String> {
    let without_query = url.split(['?', '#']).next().unwrap_or(url);
    let last_seg = without_query.rsplit('/').next().unwrap_or("");
    sanitize_file_name(last_seg)
}

fn file_looks_like_zip(path: &Path, file_name: &str) -> bool {
    if file_name.to_lowercase().ends_with(".zip") {
        return true;
    }
    let Ok(mut f) = File::open(path) else {
        return false;
    };
    let mut magic = [0u8; 4];
    matches!(f.read_exact(&mut magic), Ok(()) if magic == [0x50, 0x4B, 0x03, 0x04])
}

pub(crate) fn extract_zip_into_dir<F>(
    zip_path: &Path,
    dest_dir: &Path,
    mut on_progress: F,
) -> Result<usize, String>
where
    F: FnMut(usize, usize),
{
    let file = File::open(zip_path)
        .map_err(|e| format!("Failed to open archive '{}': {}", zip_path.display(), e))?;
    let mut archive = ZipArchive::new(BufReader::new(file))
        .map_err(|e| format!("Invalid ZIP archive '{}': {}", zip_path.display(), e))?;

    let total = archive.len();
    for i in 0..total {
        let entry = archive
            .by_index(i)
            .map_err(|e| format!("Corrupt entry in archive: {}", e))?;
        if entry.enclosed_name().is_none() {
            return Err(format!(
                "Archive contains unsafe path: '{}'",
                entry.name()
            ));
        }
    }

    let mut extracted_files = 0usize;
    for i in 0..total {
        let mut entry = archive.by_index(i).map_err(|e| e.to_string())?;
        let Some(rel_path) = entry.enclosed_name() else {
            continue;
        };
        let out_path = dest_dir.join(rel_path);
        if entry.is_dir() {
            fs::create_dir_all(&out_path).map_err(|e| e.to_string())?;
        } else {
            if let Some(parent) = out_path.parent() {
                fs::create_dir_all(parent).map_err(|e| e.to_string())?;
            }
            let mut out_file = File::create(&out_path)
                .map_err(|e| format!("Failed to write '{}': {}", out_path.display(), e))?;
            io::copy(&mut entry, &mut out_file)
                .map_err(|e| format!("Failed to extract '{}': {}", out_path.display(), e))?;
            extracted_files += 1;
        }
        on_progress(i + 1, total);
    }

    Ok(extracted_files)
}

fn find_local_server_rel_path(norm_entry: &str) -> Option<&str> {
    let lower = norm_entry.to_lowercase();
    if let Some(rest) = lower.strip_prefix("data/local_server_files/") {
        if rest.is_empty() {
            return None;
        }
        return Some(&norm_entry["data/local_server_files/".len()..]);
    }
    if let Some(pos) = lower.find("/data/local_server_files/") {
        let start = pos + "/data/local_server_files/".len();
        if start >= norm_entry.len() {
            return None;
        }
        return Some(&norm_entry[start..]);
    }
    None
}

pub(crate) fn extract_local_files_from_server_archive(
    archive_path: &Path,
    server_dir: &Path,
    auto_extract_files: bool,
) -> Result<usize, String> {
    let file = File::open(archive_path)
        .map_err(|e| format!("Failed to open '{}': {}", archive_path.display(), e))?;
    let mut archive = ZipArchive::new(BufReader::new(file))
        .map_err(|e| format!("Invalid .server archive '{}': {}", archive_path.display(), e))?;

    fs::create_dir_all(server_dir)
        .map_err(|e| format!("Failed to create '{}': {}", server_dir.display(), e))?;

    let mut matched_indices: Vec<(usize, String, bool)> = Vec::new();
    for i in 0..archive.len() {
        let entry = archive.by_index(i).map_err(|e| e.to_string())?;
        let norm = entry.name().replace('\\', "/");
        let Some(rel) = find_local_server_rel_path(&norm) else {
            continue;
        };
        let rel_trimmed = rel.trim_start_matches('/');
        if rel_trimmed.is_empty() || rel_trimmed.contains("..") {
            continue;
        }
        matched_indices.push((i, rel_trimmed.to_string(), entry.is_dir()));
    }

    if matched_indices.is_empty() {
        return Err(format!(
            "No files found under data/local_server_files/ inside '{}'.",
            archive_path.display()
        ));
    }

    let mut count = 0usize;
    let mut nested_zips: Vec<PathBuf> = Vec::new();

    for (idx, rel_str, is_dir) in matched_indices {
        let mut entry = archive.by_index(idx).map_err(|e| e.to_string())?;
        let out_path = server_dir.join(rel_str.replace('/', std::path::MAIN_SEPARATOR_STR));
        if is_dir {
            fs::create_dir_all(&out_path).map_err(|e| e.to_string())?;
        } else {
            if let Some(parent) = out_path.parent() {
                fs::create_dir_all(parent).map_err(|e| e.to_string())?;
            }
            let mut out_file = File::create(&out_path)
                .map_err(|e| format!("Failed to write '{}': {}", out_path.display(), e))?;
            io::copy(&mut entry, &mut out_file)
                .map_err(|e| format!("Failed to copy '{}': {}", out_path.display(), e))?;
            count += 1;

            if auto_extract_files
                && out_path
                    .extension()
                    .and_then(|e| e.to_str())
                    .map(|e| e.eq_ignore_ascii_case("zip"))
                    .unwrap_or(false)
            {
                nested_zips.push(out_path);
            }
        }
    }

    for zip_file in nested_zips {
        let extracted = extract_zip_into_dir(&zip_file, server_dir, |_, _| {})?;
        let _ = fs::remove_file(&zip_file);
        count = count.saturating_add(extracted);
    }

    Ok(count)
}

pub async fn run_non_steam_install_or_update(
    app: AppHandle,
    library_path: String,
    codename: String,
    instance_name: String,
    server_id: String,
    server_name: String,
    download_link: String,
    are_files_local: bool,
    auto_extract_files: bool,
    source_file: String,
) -> Result<SteamCmdRunResult, String> {
    let _ = ensure_library_directories(&library_path)?;
    let (server_dir, _) = resolve_instance_dir(&library_path, &codename, &instance_name)?;
    fs::create_dir_all(&server_dir)
        .map_err(|e| format!("Failed to create server folder '{}': {}", server_dir.display(), e))?;

    let task_label = format!("Installing {}...", server_name);
    let (task_id, _guard) = begin_steam_task(
        &app,
        &server_id,
        &task_label,
        Some(server_dir.clone()),
    )?;

    mark_instance_incomplete(&server_dir)?;
    let server_dir_str = server_dir.to_string_lossy().into_owned();

    if are_files_local {
        emit_log(
            &app,
            &server_id,
            format!("Copying local server files for {}...", server_name),
        );
        update_task_label(&app, task_id, &task_label, false, 0.0, "");

        let data_dir = find_sct_data_dir()
            .ok_or_else(|| "Could not locate sct_data directory.".to_string())?;
        let archive_name = if source_file.trim().is_empty() {
            format!("{}.server", codename.trim())
        } else {
            source_file.trim().to_string()
        };
        let archive_path = data_dir.join(&archive_name);
        if !archive_path.is_file() {
            return Err(format!(
                "Server definition archive not found: '{}'",
                archive_path.display()
            ));
        }

        let dir_clone = server_dir.clone();
        let res = tauri::async_runtime::spawn_blocking(move || {
            extract_local_files_from_server_archive(&archive_path, &dir_clone, auto_extract_files)
        })
        .await
        .map_err(|e| format!("Local install worker failed: {}", e))?;

        match res {
            Ok(file_count) => {
                if is_task_cancelled(task_id) {
                    let _ = mark_instance_incomplete(&server_dir);
                    emit_log(&app, &server_id, "Installation cancelled.");
                    return Ok(SteamCmdRunResult {
                        directory: server_dir_str,
                        cancelled: true,
                    });
                }
                clear_instance_incomplete(&server_dir);
                emit_log(
                    &app,
                    &server_id,
                    format!("Installed {} local file(s) successfully.", file_count),
                );
                return Ok(SteamCmdRunResult {
                    directory: server_dir_str,
                    cancelled: false,
                });
            }
            Err(err) => {
                let _ = mark_instance_incomplete(&server_dir);
                emit_log(&app, &server_id, format!("Install failed: {}", err));
                return Err(err);
            }
        }
    }

    let url = download_link.trim().to_string();
    if url.is_empty() {
        return Err("No download link is configured for this server.".to_string());
    }

    emit_log(&app, &server_id, format!("Downloading from {}...", url));
    update_task_label(
        &app,
        task_id,
        &format!("Downloading {}...", server_name),
        false,
        0.0,
        "",
    );

    let client = reqwest::Client::builder()
        .user_agent(concat!("Server-Creation-Tool/", env!("CARGO_PKG_VERSION")))
        .build()
        .map_err(|e| format!("Failed to initialize HTTP client: {}", e))?;

    let mut response = client
        .get(&url)
        .send()
        .await
        .map_err(|e| format!("Download request failed: {}", e))?;

    if !response.status().is_success() {
        return Err(format!(
            "Download failed with HTTP status {}",
            response.status()
        ));
    }

    let resolved_name = response
        .headers()
        .get(reqwest::header::CONTENT_DISPOSITION)
        .and_then(|v| v.to_str().ok())
        .and_then(filename_from_content_disposition)
        .or_else(|| filename_from_url(response.url().as_str()))
        .or_else(|| filename_from_url(&url))
        .unwrap_or_else(|| "server_files.bin".to_string());

    let total_bytes = response.content_length().unwrap_or(0);
    let tmp_path = server_dir.join(".sct_download.tmp");
    if tmp_path.exists() {
        let _ = fs::remove_file(&tmp_path);
    }

    let download_res: Result<(), String> = async {
        let mut out_file = File::create(&tmp_path)
            .map_err(|e| format!("Failed to create '{}': {}", tmp_path.display(), e))?;
        let mut downloaded = 0u64;
        let mut last_pct: i32 = -1;

        while let Some(chunk) = response
            .chunk()
            .await
            .map_err(|e| format!("Download interrupted: {}", e))?
        {
            if is_task_cancelled(task_id) {
                return Ok(());
            }
            out_file
                .write_all(&chunk)
                .map_err(|e| format!("Failed to write download: {}", e))?;
            downloaded = downloaded.saturating_add(chunk.len() as u64);

            if total_bytes > 0 {
                let pct = ((downloaded as f64 / total_bytes as f64) * 100.0).clamp(0.0, 100.0);
                let rounded = pct.floor() as i32;
                if rounded != last_pct {
                    last_pct = rounded;
                    update_task_label(
                        &app,
                        task_id,
                        &format!("Downloading {}...", server_name),
                        true,
                        pct,
                        &format!("{} / {}", format_bytes(downloaded), format_bytes(total_bytes)),
                    );
                }
            } else {
                update_task_label(
                    &app,
                    task_id,
                    &format!("Downloading {}...", server_name),
                    false,
                    0.0,
                    &format_bytes(downloaded),
                );
            }
        }
        out_file
            .flush()
            .map_err(|e| format!("Failed to flush download: {}", e))?;
        Ok(())
    }
    .await;

    if let Err(err) = download_res {
        let _ = fs::remove_file(&tmp_path);
        let _ = mark_instance_incomplete(&server_dir);
        emit_log(&app, &server_id, format!("Download failed: {}", err));
        return Err(err);
    }

    if is_task_cancelled(task_id) {
        let _ = fs::remove_file(&tmp_path);
        let _ = mark_instance_incomplete(&server_dir);
        emit_log(&app, &server_id, "Download was cancelled.");
        return Ok(SteamCmdRunResult {
            directory: server_dir_str,
            cancelled: true,
        });
    }

    if auto_extract_files && file_looks_like_zip(&tmp_path, &resolved_name) {
        emit_log(&app, &server_id, format!("Extracting {}...", resolved_name));
        let app_clone = app.clone();
        let sname_clone = server_name.clone();
        let tmp_clone = tmp_path.clone();
        let dir_clone = server_dir.clone();

        let extract_res = tauri::async_runtime::spawn_blocking(move || {
            let mut last_pct: i32 = -1;
            extract_zip_into_dir(&tmp_clone, &dir_clone, |done, total| {
                let pct = if total > 0 {
                    ((done as f64 / total as f64) * 100.0).clamp(0.0, 100.0)
                } else {
                    100.0
                };
                let rounded = pct.floor() as i32;
                if rounded != last_pct || done == total {
                    last_pct = rounded;
                    update_task_label(
                        &app_clone,
                        task_id,
                        &format!("Extracting {}...", sname_clone),
                        true,
                        pct,
                        &format!("{}/{}", done, total),
                    );
                }
            })
        })
        .await
        .map_err(|e| format!("Extraction worker failed: {}", e))?;

        let _ = fs::remove_file(&tmp_path);
        match extract_res {
            Ok(count) => {
                clear_instance_incomplete(&server_dir);
                emit_log(
                    &app,
                    &server_id,
                    format!("Extracted {} file(s) successfully.", count),
                );
                Ok(SteamCmdRunResult {
                    directory: server_dir_str,
                    cancelled: false,
                })
            }
            Err(err) => {
                let _ = mark_instance_incomplete(&server_dir);
                emit_log(&app, &server_id, format!("Extraction failed: {}", err));
                Err(err)
            }
        }
    } else {
        let final_path = server_dir.join(&resolved_name);
        if final_path.exists() {
            let _ = fs::remove_file(&final_path);
        }
        fs::rename(&tmp_path, &final_path).map_err(|e| {
            let _ = fs::remove_file(&tmp_path);
            format!(
                "Failed to save downloaded file '{}': {}",
                final_path.display(),
                e
            )
        })?;

        clear_instance_incomplete(&server_dir);
        emit_log(
            &app,
            &server_id,
            format!("Saved {} successfully.", resolved_name),
        );
        Ok(SteamCmdRunResult {
            directory: server_dir_str,
            cancelled: false,
        })
    }
}

#[cfg(test)]
mod tests {
    use super::{
        extract_local_files_from_server_archive, extract_zip_into_dir,
        filename_from_content_disposition, filename_from_url,
    };
    use std::fs::{self, File};
    use std::io::Write;
    use std::time::{SystemTime, UNIX_EPOCH};
    use zip::write::SimpleFileOptions;
    use zip::ZipWriter;

    fn make_temp_dir(name: &str) -> std::path::PathBuf {
        let nanos = SystemTime::now()
            .duration_since(UNIX_EPOCH)
            .unwrap()
            .as_nanos();
        let dir = std::env::temp_dir().join(format!("sct_nonsteam_test_{}_{}", name, nanos));
        fs::create_dir_all(&dir).unwrap();
        dir
    }

    #[test]
    fn derives_download_filename_from_url_and_headers() {
        assert_eq!(
            filename_from_url(
                "https://github.com/BeamMP/BeamMP-Server/releases/latest/download/BeamMP-Server.exe"
            ),
            Some("BeamMP-Server.exe".to_string())
        );
        assert_eq!(
            filename_from_content_disposition("attachment; filename=\"server_v2.zip\""),
            Some("server_v2.zip".to_string())
        );
    }

    #[test]
    fn extracts_zip_into_server_dir_with_auto_extract() {
        let dir = make_temp_dir("zip_extract");
        let zip_path = dir.join("pkg.zip");
        let dest_dir = dir.join("out");
        fs::create_dir_all(&dest_dir).unwrap();

        {
            let f = File::create(&zip_path).unwrap();
            let mut w = ZipWriter::new(f);
            let opts = SimpleFileOptions::default();
            w.start_file("bin/server.exe", opts).unwrap();
            w.write_all(b"exe-bytes").unwrap();
            w.finish().unwrap();
        }

        let count = extract_zip_into_dir(&zip_path, &dest_dir, |_, _| {}).unwrap();
        assert_eq!(count, 1);
        assert_eq!(
            fs::read(dest_dir.join("bin").join("server.exe")).unwrap(),
            b"exe-bytes"
        );

        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn copies_local_server_files_from_server_archive() {
        let dir = make_temp_dir("local_files");
        let server_pkg = dir.join("custom.server");
        let dest_dir = dir.join("custom_ds");

        {
            let f = File::create(&server_pkg).unwrap();
            let mut w = ZipWriter::new(f);
            let opts = SimpleFileOptions::default();
            w.start_file("custom/metadata.json", opts).unwrap();
            w.write_all(b"{}").unwrap();
            w.start_file("custom/data/local_server_files/Run.bat", opts)
                .unwrap();
            w.write_all(b"@echo off").unwrap();
            w.finish().unwrap();
        }

        let count =
            extract_local_files_from_server_archive(&server_pkg, &dest_dir, false).unwrap();
        assert_eq!(count, 1);
        assert_eq!(fs::read(dest_dir.join("Run.bat")).unwrap(), b"@echo off");

        let _ = fs::remove_dir_all(&dir);
    }
}
