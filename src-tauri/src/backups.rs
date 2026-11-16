use serde::Serialize;
use std::fs::{self, File};
use std::io::{self, BufReader, BufWriter};
use std::path::{Path, PathBuf};
use std::time::{SystemTime, UNIX_EPOCH};
use tauri::AppHandle;
use zip::write::SimpleFileOptions;
use zip::{CompressionMethod, ZipArchive, ZipWriter};

use crate::servers::{
    clear_readonly_recursive, clear_instance_incomplete, format_bytes, is_instance_incomplete,
    resolve_instance_dir, INCOMPLETE_INSTALL_MARKER,
};
use crate::steamcmd::{emit_log, emit_task_state};

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct BackupEntryDto {
    pub file_name: String,
    pub full_path: String,
    pub size_bytes: u64,
    pub size: String,
    pub modified_ms: u64,
}

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct BackupCreatedDto {
    pub file_name: String,
    pub full_path: String,
    pub size_bytes: u64,
    pub size: String,
    pub file_count: usize,
}

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct RestoreResultDto {
    pub directory: String,
    pub file_count: usize,
}

fn sanitize_slug(raw: &str, fallback: &str) -> String {
    let trimmed = raw.trim();
    if trimmed.is_empty() {
        return fallback.to_string();
    }
    let out: String = trimmed
        .chars()
        .map(|c| {
            if c.is_ascii_alphanumeric() || c == '-' || c == '_' {
                c
            } else {
                '_'
            }
        })
        .collect();
    if out.is_empty() {
        fallback.to_string()
    } else {
        out
    }
}

/// Converts UNIX epoch seconds to (year, month, day, hour, minute, second) in UTC.
fn format_utc_timestamp(secs: u64) -> String {
    let days = (secs / 86400) as i64;
    let rem = secs % 86400;
    let hour = rem / 3600;
    let minute = (rem % 3600) / 60;
    let second = rem % 60;

    // Howard Hinnant's civil_from_days algorithm.
    let z = days + 719468;
    let era = if z >= 0 { z } else { z - 146096 } / 146097;
    let doe = z - era * 146097;
    let yoe = (doe - doe / 1460 + doe / 36524 - doe / 146096) / 365;
    let y = yoe + era * 400;
    let doy = doe - (365 * yoe + yoe / 4 - yoe / 100);
    let mp = (5 * doy + 2) / 153;
    let d = doy - (153 * mp + 2) / 5 + 1;
    let m = mp + if mp < 10 { 3 } else { -9 };
    let year = y + if m <= 2 { 1 } else { 0 };

    format!(
        "{:04}-{:02}-{:02}_{:02}-{:02}-{:02}",
        year, m, d, hour, minute, second
    )
}

fn now_timestamp_label() -> String {
    let secs = SystemTime::now()
        .duration_since(UNIX_EPOCH)
        .map(|d| d.as_secs())
        .unwrap_or(0);
    format_utc_timestamp(secs)
}

pub fn default_backup_dir(library_path: &str, codename: &str, instance_name: &str) -> Result<PathBuf, String> {
    let lib_trimmed = library_path.trim();
    if lib_trimmed.is_empty() {
        return Err("No Server Library location is configured.".to_string());
    }
    let code_slug = sanitize_slug(codename, "server");
    let inst_raw = if instance_name.trim().is_empty() {
        "Default"
    } else {
        instance_name.trim()
    };
    let inst_slug = sanitize_slug(inst_raw, "Default");
    Ok(PathBuf::from(lib_trimmed)
        .join("backups")
        .join(format!("{}_{}", code_slug, inst_slug)))
}

pub fn default_backup_file_name(codename: &str, instance_name: &str) -> String {
    let code_slug = sanitize_slug(codename, "server");
    let inst_raw = if instance_name.trim().is_empty() {
        "Default"
    } else {
        instance_name.trim()
    };
    let inst_slug = sanitize_slug(inst_raw, "Default");
    format!("{}_{}_{}.zip", code_slug, inst_slug, now_timestamp_label())
}

struct CollectedEntries {
    dirs: Vec<String>,
    files: Vec<(PathBuf, String, u64)>,
    total_bytes: u64,
}

fn collect_server_entries(
    root: &Path,
    skip_paths: &[PathBuf],
) -> Result<CollectedEntries, String> {
    let mut out = CollectedEntries {
        dirs: Vec::new(),
        files: Vec::new(),
        total_bytes: 0,
    };
    collect_recursive(root, root, skip_paths, &mut out)?;
    Ok(out)
}

fn collect_recursive(
    root: &Path,
    current: &Path,
    skip_paths: &[PathBuf],
    out: &mut CollectedEntries,
) -> Result<(), String> {
    let entries = fs::read_dir(current)
        .map_err(|e| format!("Failed to read '{}': {}", current.display(), e))?;

    for entry in entries.flatten() {
        let path = entry.path();
        if skip_paths.iter().any(|skip| skip == &path) {
            continue;
        }
        let name = entry.file_name().to_string_lossy().to_string();
        if name.eq_ignore_ascii_case(INCOMPLETE_INSTALL_MARKER) {
            continue;
        }

        let rel = path
            .strip_prefix(root)
            .map_err(|e| e.to_string())?
            .to_string_lossy()
            .replace('\\', "/");
        if rel.is_empty() {
            continue;
        }

        let meta = fs::symlink_metadata(&path)
            .map_err(|e| format!("Failed to inspect '{}': {}", path.display(), e))?;
        if meta.file_type().is_symlink() {
            continue;
        }
        if meta.is_dir() {
            out.dirs.push(rel);
            collect_recursive(root, &path, skip_paths, out)?;
        } else if meta.is_file() {
            let len = meta.len();
            out.total_bytes = out.total_bytes.saturating_add(len);
            out.files.push((path, rel, len));
        }
    }

    Ok(())
}

pub fn create_backup_core<F>(
    library_path: &str,
    codename: &str,
    instance_name: &str,
    destination_path: Option<&str>,
    mut on_progress: F,
) -> Result<BackupCreatedDto, String>
where
    F: FnMut(usize, usize, u64, u64),
{
    let (server_dir, _) = resolve_instance_dir(library_path, codename, instance_name)?;
    if !server_dir.is_dir() {
        return Err(format!(
            "Server folder does not exist: '{}'",
            server_dir.display()
        ));
    }
    if is_instance_incomplete(&server_dir) {
        return Err(
            "Cannot back up an incomplete installation. Finish updating/installing the server first."
                .to_string(),
        );
    }

    let dest_path = match destination_path.map(str::trim).filter(|s| !s.is_empty()) {
        Some(custom) => {
            let mut p = PathBuf::from(custom);
            let has_zip_ext = p
                .extension()
                .and_then(|e| e.to_str())
                .map(|e| e.eq_ignore_ascii_case("zip"))
                .unwrap_or(false);
            if !has_zip_ext {
                p.set_extension("zip");
            }
            p
        }
        None => {
            let dir = default_backup_dir(library_path, codename, instance_name)?;
            dir.join(default_backup_file_name(codename, instance_name))
        }
    };

    if let Some(parent) = dest_path.parent() {
        fs::create_dir_all(parent).map_err(|e| {
            format!(
                "Failed to create backup directory '{}': {}",
                parent.display(),
                e
            )
        })?;
    }

    let tmp_path = dest_path.with_extension("zip.tmp");
    if tmp_path.exists() {
        let _ = fs::remove_file(&tmp_path);
    }

    let skip_paths = vec![dest_path.clone(), tmp_path.clone()];
    let collected = collect_server_entries(&server_dir, &skip_paths)?;
    let total_files = collected.files.len();
    let total_bytes = collected.total_bytes;

    if total_files == 0 && collected.dirs.is_empty() {
        return Err("Server folder is empty; nothing to back up.".to_string());
    }

    let write_res: Result<(), String> = (|| {
        let tmp_file = File::create(&tmp_path).map_err(|e| {
            format!(
                "Failed to create backup file '{}': {}",
                tmp_path.display(),
                e
            )
        })?;
        let mut writer = ZipWriter::new(BufWriter::new(tmp_file));
        let dir_opts = SimpleFileOptions::default().compression_method(CompressionMethod::Stored);
        let file_opts =
            SimpleFileOptions::default().compression_method(CompressionMethod::Deflated);

        for dir_rel in &collected.dirs {
            let entry_name = if dir_rel.ends_with('/') {
                dir_rel.clone()
            } else {
                format!("{}/", dir_rel)
            };
            writer
                .add_directory(entry_name, dir_opts)
                .map_err(|e| format!("Failed to add directory to backup: {}", e))?;
        }

        let mut done_bytes = 0u64;
        for (idx, (src_path, rel_name, file_len)) in collected.files.iter().enumerate() {
            writer
                .start_file(rel_name, file_opts)
                .map_err(|e| format!("Failed to add '{}' to backup: {}", rel_name, e))?;
            let src_file = File::open(src_path)
                .map_err(|e| format!("Failed to read '{}': {}", src_path.display(), e))?;
            let mut reader = BufReader::new(src_file);
            io::copy(&mut reader, &mut writer)
                .map_err(|e| format!("Failed to compress '{}': {}", rel_name, e))?;

            done_bytes = done_bytes.saturating_add(*file_len);
            on_progress(idx + 1, total_files, done_bytes, total_bytes);
        }

        writer
            .finish()
            .map_err(|e| format!("Failed to finalize backup archive: {}", e))?;
        Ok(())
    })();

    if let Err(err) = write_res {
        let _ = fs::remove_file(&tmp_path);
        return Err(err);
    }

    if dest_path.exists() {
        fs::remove_file(&dest_path).map_err(|e| {
            let _ = fs::remove_file(&tmp_path);
            format!(
                "Failed to replace existing backup '{}': {}",
                dest_path.display(),
                e
            )
        })?;
    }
    fs::rename(&tmp_path, &dest_path).map_err(|e| {
        let _ = fs::remove_file(&tmp_path);
        format!(
            "Failed to finalize backup '{}': {}",
            dest_path.display(),
            e
        )
    })?;

    let final_size = fs::metadata(&dest_path).map(|m| m.len()).unwrap_or(0);
    let file_name = dest_path
        .file_name()
        .and_then(|s| s.to_str())
        .unwrap_or("backup.zip")
        .to_string();

    Ok(BackupCreatedDto {
        file_name,
        full_path: dest_path.to_string_lossy().into_owned(),
        size_bytes: final_size,
        size: format_bytes(final_size),
        file_count: total_files,
    })
}

pub fn list_server_backups(
    library_path: &str,
    codename: &str,
    instance_name: &str,
) -> Result<Vec<BackupEntryDto>, String> {
    let instance_dir = default_backup_dir(library_path, codename, instance_name)?;
    let backups_root = PathBuf::from(library_path.trim()).join("backups");

    let code_slug = sanitize_slug(codename, "server");
    let inst_raw = if instance_name.trim().is_empty() {
        "Default"
    } else {
        instance_name.trim()
    };
    let inst_slug = sanitize_slug(inst_raw, "Default");
    let flat_prefix = format!("{}_{}_", code_slug.to_lowercase(), inst_slug.to_lowercase());

    let mut out = Vec::new();
    let mut collect_from_dir = |dir: &Path, require_prefix: bool| {
        let Ok(entries) = fs::read_dir(dir) else {
            return;
        };
        for entry in entries.flatten() {
            let path = entry.path();
            if !path.is_file() {
                continue;
            }
            let is_zip = path
                .extension()
                .and_then(|e| e.to_str())
                .map(|e| e.eq_ignore_ascii_case("zip"))
                .unwrap_or(false);
            if !is_zip {
                continue;
            }
            let file_name = entry.file_name().to_string_lossy().to_string();
            if require_prefix && !file_name.to_lowercase().starts_with(&flat_prefix) {
                continue;
            }
            let meta = match fs::metadata(&path) {
                Ok(m) => m,
                Err(_) => continue,
            };
            let size_bytes = meta.len();
            let modified_ms = meta
                .modified()
                .ok()
                .and_then(|t| t.duration_since(UNIX_EPOCH).ok())
                .map(|d| d.as_millis() as u64)
                .unwrap_or(0);
            out.push(BackupEntryDto {
                file_name,
                full_path: path.to_string_lossy().into_owned(),
                size_bytes,
                size: format_bytes(size_bytes),
                modified_ms,
            });
        }
    };

    if instance_dir.is_dir() {
        collect_from_dir(&instance_dir, false);
    }
    if backups_root.is_dir() {
        collect_from_dir(&backups_root, true);
    }

    out.sort_by(|a, b| {
        b.modified_ms
            .cmp(&a.modified_ms)
            .then_with(|| b.file_name.cmp(&a.file_name))
    });
    Ok(out)
}

pub fn restore_backup_core<F>(
    library_path: &str,
    codename: &str,
    instance_name: &str,
    backup_path: &str,
    mut on_progress: F,
) -> Result<RestoreResultDto, String>
where
    F: FnMut(usize, usize),
{
    let zip_path = PathBuf::from(backup_path.trim());
    if !zip_path.is_file() {
        return Err(format!("Backup file not found: '{}'", zip_path.display()));
    }

    // Validate archive before modifying the server directory.
    let file = File::open(&zip_path)
        .map_err(|e| format!("Failed to open '{}': {}", zip_path.display(), e))?;
    let mut archive = ZipArchive::new(BufReader::new(file))
        .map_err(|e| format!("Invalid backup archive '{}': {}", zip_path.display(), e))?;

    let total_entries = archive.len();
    for i in 0..total_entries {
        let entry = archive
            .by_index(i)
            .map_err(|e| format!("Corrupt entry in backup archive: {}", e))?;
        if entry.enclosed_name().is_none() {
            return Err(format!(
                "Backup archive contains an unsafe path: '{}'",
                entry.name()
            ));
        }
    }

    let (server_dir, _) = resolve_instance_dir(library_path, codename, instance_name)?;
    if server_dir.exists() {
        if fs::remove_dir_all(&server_dir).is_err() {
            clear_readonly_recursive(&server_dir);
            fs::remove_dir_all(&server_dir).map_err(|e| {
                format!(
                    "Failed to clear existing server folder '{}': {}",
                    server_dir.display(),
                    e
                )
            })?;
        }
    }
    fs::create_dir_all(&server_dir).map_err(|e| {
        format!(
            "Failed to create server folder '{}': {}",
            server_dir.display(),
            e
        )
    })?;

    let mut extracted_files = 0usize;
    for i in 0..total_entries {
        let mut entry = archive.by_index(i).map_err(|e| e.to_string())?;
        let Some(rel_path) = entry.enclosed_name() else {
            continue;
        };
        let out_path = server_dir.join(rel_path);
        if entry.is_dir() {
            fs::create_dir_all(&out_path).map_err(|e| e.to_string())?;
        } else {
            if let Some(parent) = out_path.parent() {
                fs::create_dir_all(parent).map_err(|e| e.to_string())?;
            }
            let mut out_file = File::create(&out_path).map_err(|e| {
                format!("Failed to write '{}': {}", out_path.display(), e)
            })?;
            io::copy(&mut entry, &mut out_file).map_err(|e| {
                format!("Failed to extract '{}': {}", out_path.display(), e)
            })?;
            extracted_files += 1;
        }
        on_progress(i + 1, total_entries);
    }

    clear_instance_incomplete(&server_dir);

    Ok(RestoreResultDto {
        directory: server_dir.to_string_lossy().into_owned(),
        file_count: extracted_files,
    })
}

pub async fn run_backup_task(
    app: &AppHandle,
    library_path: String,
    codename: String,
    instance_name: String,
    server_id: String,
    server_name: String,
    destination_path: Option<String>,
) -> Result<BackupCreatedDto, String> {
    let app_handle = app.clone();
    let sid = server_id.clone();
    let sname = server_name.clone();

    emit_task_state(
        &app_handle,
        true,
        &sid,
        format!("Backing up {}...", sname),
        false,
        0.0,
        "",
        false,
    );
    emit_log(
        &app_handle,
        &sid,
        format!("Starting backup for {} ({})...", sname, instance_name),
    );

    let res = tauri::async_runtime::spawn_blocking(move || {
        let mut last_pct: i32 = -1;
        create_backup_core(
            &library_path,
            &codename,
            &instance_name,
            destination_path.as_deref(),
            |done_files, total_files, done_bytes, total_bytes| {
                let pct = if total_bytes > 0 {
                    ((done_bytes as f64 / total_bytes as f64) * 100.0).clamp(0.0, 100.0)
                } else if total_files > 0 {
                    ((done_files as f64 / total_files as f64) * 100.0).clamp(0.0, 100.0)
                } else {
                    100.0
                };
                let rounded = pct.floor() as i32;
                if rounded != last_pct || done_files == total_files {
                    last_pct = rounded;
                    emit_task_state(
                        &app_handle,
                        true,
                        &sid,
                        format!("Backing up {}...", sname),
                        true,
                        pct,
                        format!("{}/{} files", done_files, total_files),
                        false,
                    );
                }
            },
        )
    })
    .await
    .map_err(|e| format!("Backup worker failed: {}", e))?;

    emit_task_state(app, false, "", "", false, 0.0, "", false);

    match &res {
        Ok(created) => {
            emit_log(
                app,
                &server_id,
                format!(
                    "Backup created: {} ({}, {} files).",
                    created.full_path, created.size, created.file_count
                ),
            );
        }
        Err(err) => {
            emit_log(app, &server_id, format!("Backup failed: {}", err));
        }
    }

    res
}

pub async fn run_restore_task(
    app: &AppHandle,
    library_path: String,
    codename: String,
    instance_name: String,
    server_id: String,
    server_name: String,
    backup_path: String,
) -> Result<RestoreResultDto, String> {
    let app_handle = app.clone();
    let sid = server_id.clone();
    let sname = server_name.clone();

    emit_task_state(
        &app_handle,
        true,
        &sid,
        format!("Restoring {}...", sname),
        false,
        0.0,
        "",
        false,
    );
    emit_log(
        &app_handle,
        &sid,
        format!(
            "Restoring {} ({}) from {}...",
            sname, instance_name, backup_path
        ),
    );

    let res = tauri::async_runtime::spawn_blocking(move || {
        let mut last_pct: i32 = -1;
        restore_backup_core(
            &library_path,
            &codename,
            &instance_name,
            &backup_path,
            |done_entries, total_entries| {
                let pct = if total_entries > 0 {
                    ((done_entries as f64 / total_entries as f64) * 100.0).clamp(0.0, 100.0)
                } else {
                    100.0
                };
                let rounded = pct.floor() as i32;
                if rounded != last_pct || done_entries == total_entries {
                    last_pct = rounded;
                    emit_task_state(
                        &app_handle,
                        true,
                        &sid,
                        format!("Restoring {}...", sname),
                        true,
                        pct,
                        format!("{}/{}", done_entries, total_entries),
                        false,
                    );
                }
            },
        )
    })
    .await
    .map_err(|e| format!("Restore worker failed: {}", e))?;

    emit_task_state(app, false, "", "", false, 0.0, "", false);

    match &res {
        Ok(restored) => {
            emit_log(
                app,
                &server_id,
                format!(
                    "Restore complete ({} files) to {}.",
                    restored.file_count, restored.directory
                ),
            );
        }
        Err(err) => {
            emit_log(app, &server_id, format!("Restore failed: {}", err));
        }
    }

    res
}

#[cfg(test)]
mod tests {
    use super::{create_backup_core, list_server_backups, restore_backup_core};
    use std::fs;
    use std::time::{SystemTime, UNIX_EPOCH};

    fn make_temp_dir(name: &str) -> std::path::PathBuf {
        let nanos = SystemTime::now()
            .duration_since(UNIX_EPOCH)
            .unwrap()
            .as_nanos();
        let dir = std::env::temp_dir().join(format!("sct_backup_test_{}_{}", name, nanos));
        fs::create_dir_all(&dir).unwrap();
        dir
    }

    #[test]
    fn backup_and_restore_roundtrip_in_default_location() {
        let lib = make_temp_dir("roundtrip");
        let lib_str = lib.to_string_lossy().into_owned();
        let cs_dir = lib.join("cs_ds");
        fs::create_dir_all(cs_dir.join("cstrike")).unwrap();
        fs::create_dir_all(cs_dir.join("empty_dir")).unwrap();
        fs::write(cs_dir.join("hlds.exe"), b"binary-v1").unwrap();
        fs::write(cs_dir.join("cstrike").join("server.cfg"), b"hostname test").unwrap();

        let created = create_backup_core(&lib_str, "cs", "Default", None, |_, _, _, _| {}).unwrap();
        assert_eq!(created.file_count, 2);
        assert!(std::path::Path::new(&created.full_path).is_file());

        let listed = list_server_backups(&lib_str, "cs", "Default").unwrap();
        assert_eq!(listed.len(), 1);
        assert_eq!(listed[0].file_name, created.file_name);

        let other_inst = list_server_backups(&lib_str, "cs", "surf").unwrap();
        assert!(other_inst.is_empty());

        // Mutate server folder and add an extra file to verify clean restore.
        fs::write(cs_dir.join("hlds.exe"), b"corrupted").unwrap();
        fs::write(cs_dir.join("extra.txt"), b"remove-me").unwrap();

        let restored =
            restore_backup_core(&lib_str, "cs", "Default", &created.full_path, |_, _| {}).unwrap();
        assert_eq!(restored.file_count, 2);
        assert_eq!(fs::read(cs_dir.join("hlds.exe")).unwrap(), b"binary-v1");
        assert_eq!(
            fs::read(cs_dir.join("cstrike").join("server.cfg")).unwrap(),
            b"hostname test"
        );
        assert!(cs_dir.join("empty_dir").is_dir());
        assert!(!cs_dir.join("extra.txt").exists());

        let _ = fs::remove_dir_all(&lib);
    }

    #[test]
    fn backup_to_custom_location_appends_zip_extension() {
        let lib = make_temp_dir("custom_loc");
        let custom_dir = make_temp_dir("custom_dest");
        let lib_str = lib.to_string_lossy().into_owned();
        let cs_dir = lib.join("cs_ds");
        fs::create_dir_all(&cs_dir).unwrap();
        fs::write(cs_dir.join("run.bat"), b"@echo off").unwrap();

        let custom_target = custom_dir.join("my_manual_backup");
        let custom_str = custom_target.to_string_lossy().into_owned();
        let created =
            create_backup_core(&lib_str, "cs", "Default", Some(&custom_str), |_, _, _, _| {})
                .unwrap();

        assert!(created.full_path.ends_with(".zip"));
        assert!(std::path::Path::new(&created.full_path).is_file());

        let _ = fs::remove_dir_all(&lib);
        let _ = fs::remove_dir_all(&custom_dir);
    }
}
