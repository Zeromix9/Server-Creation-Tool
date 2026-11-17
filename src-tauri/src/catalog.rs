use serde::{Deserialize, Serialize};
use std::fs;
use std::path::{Path, PathBuf};
use std::time::Duration;

use crate::logger;
use crate::servers;
use crate::signing::{sha256_hex_file, verify_server_package};
use crate::steamcmd;

pub const CATALOG_BASE_URL: &str =
    "https://raw.githubusercontent.com/SteliosLL/SCT-Server-Catalog/main/";
pub const CATALOG_INDEX_URL: &str =
    "https://raw.githubusercontent.com/SteliosLL/SCT-Server-Catalog/main/index.json";

#[derive(Debug, Deserialize)]
struct CatalogIndex {
    #[allow(dead_code)]
    version: Option<u32>,
    #[serde(default)]
    servers: Vec<CatalogServerEntry>,
}

#[derive(Debug, Deserialize)]
struct CatalogServerEntry {
    file: String,
    #[serde(default, alias = "displayName", alias = "title")]
    name: String,
    #[serde(default)]
    sha256: String,
    #[serde(default)]
    url: String,
}

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct CatalogSyncResultDto {
    pub checked: bool,
    pub available_files: Vec<String>,
    pub available_names: Vec<String>,
    pub updated_files: Vec<String>,
    pub skipped_files: Vec<String>,
    pub error: Option<String>,
}

fn is_safe_server_filename(name: &str) -> bool {
    let trimmed = name.trim();
    !trimmed.is_empty()
        && trimmed.to_lowercase().ends_with(".server")
        && !trimmed.contains('/')
        && !trimmed.contains('\\')
        && !trimmed.contains("..")
}

fn resolve_entry_urls(entry: &CatalogServerEntry) -> Vec<String> {
    let raw_url = entry.url.trim();
    if raw_url.starts_with("https://") || raw_url.starts_with("http://") {
        return vec![raw_url.to_string()];
    }
    if !raw_url.is_empty() {
        return vec![format!(
            "{}{}",
            CATALOG_BASE_URL,
            raw_url.trim_start_matches('/')
        )];
    }
    let file_name = entry.file.trim();
    vec![
        format!("{}{}", CATALOG_BASE_URL, file_name),
        format!("{}servers/{}", CATALOG_BASE_URL, file_name),
    ]
}

fn resolve_sync_sct_data_dir() -> Result<PathBuf, String> {
    if crate::IS_RELEASE && !cfg!(test) {
        let exe = std::env::current_exe()
            .map_err(|e| format!("Failed to resolve startup directory: {}", e))?;
        let exe_dir = exe
            .parent()
            .ok_or_else(|| "Failed to resolve startup directory.".to_string())?;
        let dir = exe_dir.join("sct_data");
        fs::create_dir_all(&dir)
            .map_err(|e| format!("Failed to create '{}': {}", dir.display(), e))?;
        return Ok(dir);
    }
    if let Some(dir) = servers::find_sct_data_dir() {
        return Ok(dir);
    }
    let exe = std::env::current_exe()
        .map_err(|e| format!("Failed to resolve startup directory: {}", e))?;
    let exe_dir = exe
        .parent()
        .ok_or_else(|| "Failed to resolve startup directory.".to_string())?;
    let dir = exe_dir.join("sct_data");
    fs::create_dir_all(&dir)
        .map_err(|e| format!("Failed to create '{}': {}", dir.display(), e))?;
    Ok(dir)
}

pub async fn check_online_catalog(force: bool) -> CatalogSyncResultDto {
    run_online_catalog(None, force, false).await
}

pub async fn sync_online_catalog(
    app: Option<&tauri::AppHandle>,
    force: bool,
) -> CatalogSyncResultDto {
    run_online_catalog(app, force, true).await
}

async fn run_online_catalog(
    app: Option<&tauri::AppHandle>,
    force: bool,
    download: bool,
) -> CatalogSyncResultDto {
    if !crate::IS_RELEASE && !force {
        return CatalogSyncResultDto {
            checked: false,
            available_files: Vec::new(),
            available_names: Vec::new(),
            updated_files: Vec::new(),
            skipped_files: Vec::new(),
            error: None,
        };
    }

    let sct_data_dir = match resolve_sync_sct_data_dir() {
        Ok(d) => d,
        Err(e) => {
            return CatalogSyncResultDto {
                checked: false,
                available_files: Vec::new(),
                available_names: Vec::new(),
                updated_files: Vec::new(),
                skipped_files: Vec::new(),
                error: Some(e),
            };
        }
    };

    match sync_into_dir(app, &sct_data_dir, CATALOG_INDEX_URL, download).await {
        Ok(res) => res,
        Err(err) => {
            logger::append(format!("Online catalog sync skipped: {}", err));
            CatalogSyncResultDto {
                checked: false,
                available_files: Vec::new(),
                available_names: Vec::new(),
                updated_files: Vec::new(),
                skipped_files: Vec::new(),
                error: Some(err),
            }
        }
    }
}

async fn sync_into_dir(
    app: Option<&tauri::AppHandle>,
    sct_data_dir: &Path,
    index_url: &str,
    download: bool,
) -> Result<CatalogSyncResultDto, String> {
    let client = reqwest::Client::builder()
        .timeout(Duration::from_secs(8))
        .user_agent(concat!("ServerCreationTool/", env!("CARGO_PKG_VERSION")))
        .build()
        .map_err(|e| format!("HTTP client error: {}", e))?;

    let resp = client
        .get(index_url)
        .send()
        .await
        .map_err(|e| format!("Failed to fetch catalog index: {}", e))?;

    if !resp.status().is_success() {
        return Err(format!("Catalog index returned HTTP {}", resp.status()));
    }

    let raw_bytes = resp
        .bytes()
        .await
        .map_err(|e| format!("Failed reading catalog index: {}", e))?;
    let index: CatalogIndex = serde_json::from_slice(&raw_bytes)
        .map_err(|e| format!("Invalid catalog index.json: {}", e))?;

    let mut available_files = Vec::new();
    let mut available_names = Vec::new();
    let mut updated_files = Vec::new();
    let mut skipped_files = Vec::new();
    let mut errors = Vec::new();
    let mut pending_entries: Vec<(&CatalogServerEntry, String, PathBuf, String)> = Vec::new();

    for entry in &index.servers {
        let file_name = entry.file.trim();
        if !is_safe_server_filename(file_name) {
            continue;
        }

        let dest_path = sct_data_dir.join(file_name);
        let expected_sha = entry.sha256.trim().to_string();

        if dest_path.is_file() && !expected_sha.is_empty() {
            if let Ok(local_sha) = sha256_hex_file(&dest_path) {
                if local_sha.eq_ignore_ascii_case(&expected_sha) {
                    skipped_files.push(file_name.to_string());
                    continue;
                }
            }
        }

        let display_name = if entry.name.trim().is_empty() {
            file_name.to_string()
        } else {
            entry.name.trim().to_string()
        };
        available_files.push(file_name.to_string());
        available_names.push(display_name);
        if download {
            pending_entries.push((entry, file_name.to_string(), dest_path, expected_sha));
        }
    }

    if !download || pending_entries.is_empty() {
        return Ok(CatalogSyncResultDto {
            checked: true,
            available_files,
            available_names,
            updated_files,
            skipped_files,
            error: None,
        });
    }

    let total = pending_entries.len();
    if let Some(handle) = app {
        steamcmd::emit_task_state(
            handle,
            true,
            "",
            format!("Downloading {}...", pending_entries[0].1),
            true,
            0.0,
            format!("0/{}", total),
            false,
        );
    }

    for (idx, (entry, file_name, dest_path, expected_sha)) in pending_entries.into_iter().enumerate() {
        if let Some(handle) = app {
            let pct = (idx as f64 / total as f64) * 100.0;
            steamcmd::emit_task_state(
                handle,
                true,
                "",
                format!("Downloading {}...", file_name),
                true,
                pct,
                format!("{}/{}", idx, total),
                false,
            );
        }

        let urls = resolve_entry_urls(entry);
        let mut downloaded_resp = None;
        for url in &urls {
            match client.get(url).send().await {
                Ok(r) if r.status().is_success() => {
                    downloaded_resp = Some(r);
                    break;
                }
                Ok(r) => {
                    logger::append(format!(
                        "Catalog sync: '{}' ({}) returned HTTP {}",
                        file_name,
                        url,
                        r.status()
                    ));
                }
                Err(e) => {
                    logger::append(format!(
                        "Catalog sync: failed downloading '{}' ({}): {}",
                        file_name, url, e
                    ));
                }
            }
        }
        let Some(mut pkg_resp) = downloaded_resp else {
            errors.push(format!("{}: failed to download from catalog", file_name));
            continue;
        };

        let content_len = pkg_resp.content_length().unwrap_or(0);
        let mut bytes: Vec<u8> = if content_len > 0 {
            Vec::with_capacity(content_len as usize)
        } else {
            Vec::new()
        };
        let mut read_err = None;
        while let Some(chunk_res) = pkg_resp.chunk().await.transpose() {
            match chunk_res {
                Ok(chunk) => {
                    bytes.extend_from_slice(&chunk);
                    if let Some(handle) = app {
                        let sub = if content_len > 0 {
                            (bytes.len() as f64 / content_len as f64).min(1.0)
                        } else {
                            0.5
                        };
                        let pct = ((idx as f64 + sub) / total as f64) * 100.0;
                        steamcmd::emit_task_state(
                            handle,
                            true,
                            "",
                            format!("Downloading {}...", file_name),
                            true,
                            pct,
                            format!("{}/{}", idx, total),
                            false,
                        );
                    }
                }
                Err(e) => {
                    read_err = Some(e);
                    break;
                }
            }
        }
        if let Some(e) = read_err {
            logger::append(format!("Catalog sync: failed reading '{}': {}", file_name, e));
            errors.push(format!("{}: failed reading response ({})", file_name, e));
            continue;
        }

        let tmp_path = sct_data_dir.join(format!("{}.tmp", file_name));
        if let Err(e) = fs::write(&tmp_path, &bytes) {
            logger::append(format!("Catalog sync: failed writing '{}': {}", file_name, e));
            errors.push(format!("{}: failed writing temp file ({})", file_name, e));
            let _ = fs::remove_file(&tmp_path);
            continue;
        }

        if !expected_sha.is_empty() {
            let actual_sha = sha256_hex_file(&tmp_path).unwrap_or_default();
            if !actual_sha.eq_ignore_ascii_case(&expected_sha) {
                logger::append(format!(
                    "Catalog sync: SHA-256 mismatch for '{}' (expected {}, got {})",
                    file_name, expected_sha, actual_sha
                ));
                errors.push(format!(
                    "{}: SHA-256 mismatch (index {}, file {})",
                    file_name, expected_sha, actual_sha
                ));
                let _ = fs::remove_file(&tmp_path);
                continue;
            }
        }

        let trust = verify_server_package(&tmp_path);
        if !trust.is_official {
            logger::append(format!(
                "Catalog sync: rejected '{}' ({})",
                file_name, trust.reason
            ));
            errors.push(format!("{}: signature rejected ({})", file_name, trust.reason));
            let _ = fs::remove_file(&tmp_path);
            continue;
        }

        if dest_path.exists() {
            let _ = fs::remove_file(&dest_path);
        }
        if let Err(e) = fs::rename(&tmp_path, &dest_path) {
            logger::append(format!("Catalog sync: failed replacing '{}': {}", file_name, e));
            errors.push(format!("{}: failed replacing file ({})", file_name, e));
            let _ = fs::remove_file(&tmp_path);
            continue;
        }

        logger::append(format!("Catalog sync: updated '{}'", file_name));
        updated_files.push(file_name.to_string());

        if let Some(handle) = app {
            let done = idx + 1;
            let pct = (done as f64 / total as f64) * 100.0;
            steamcmd::emit_task_state(
                handle,
                true,
                "",
                format!("Downloaded {}", file_name),
                true,
                pct,
                format!("{}/{}", done, total),
                false,
            );
        }
    }

    if let Some(handle) = app {
        steamcmd::emit_task_state(handle, false, "", "", false, 0.0, "", false);
    }

    Ok(CatalogSyncResultDto {
        checked: true,
        available_files,
        available_names,
        updated_files,
        skipped_files,
        error: if errors.is_empty() {
            None
        } else {
            Some(errors.join("\n"))
        },
    })
}

#[cfg(test)]
mod tests {
    use super::{is_safe_server_filename, resolve_entry_urls, CatalogServerEntry, CATALOG_BASE_URL};

    #[test]
    fn validates_safe_server_filenames_and_urls() {
        assert!(is_safe_server_filename("cs.server"));
        assert!(is_safe_server_filename("beamng.server"));
        assert!(!is_safe_server_filename("../evil.server"));
        assert!(!is_safe_server_filename("sub/cs.server"));
        assert!(!is_safe_server_filename("not_a_server.zip"));

        let auto = CatalogServerEntry {
            file: "cs.server".to_string(),
            name: String::new(),
            sha256: String::new(),
            url: String::new(),
        };
        assert_eq!(
            resolve_entry_urls(&auto),
            vec![
                format!("{}cs.server", CATALOG_BASE_URL),
                format!("{}servers/cs.server", CATALOG_BASE_URL),
            ]
        );

        let rel = CatalogServerEntry {
            file: "cs.server".to_string(),
            name: String::new(),
            sha256: String::new(),
            url: "servers/cs.server".to_string(),
        };
        assert_eq!(
            resolve_entry_urls(&rel),
            vec![format!("{}servers/cs.server", CATALOG_BASE_URL)]
        );

        let abs = CatalogServerEntry {
            file: "cs.server".to_string(),
            name: String::new(),
            sha256: String::new(),
            url: "https://example.com/cs.server".to_string(),
        };
        assert_eq!(
            resolve_entry_urls(&abs),
            vec!["https://example.com/cs.server".to_string()]
        );
    }
}
