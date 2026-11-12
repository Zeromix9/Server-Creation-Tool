use base64::Engine;
use serde::Serialize;
use serde_json::Value;
use std::collections::{HashMap, HashSet};
use std::fs::{self, File};
use std::io::Read;
use std::path::{Path, PathBuf};

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct ServerInstanceInfo {
    pub name: String,
    pub folder_name: String,
    pub full_path: String,
    pub is_installed: bool,
    pub size_bytes: u64,
    pub size: String,
}

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct ServerMetadataDto {
    pub id: String,
    pub codename: String,
    pub folder_name: String,
    pub name: String,
    pub names: HashMap<String, String>,
    pub steam_app_id: String,
    pub steam_app_ids: HashMap<String, String>,
    pub is_steam_server: bool,
    pub supports_windows: bool,
    pub supports_linux: bool,
    pub base_platform: String,
    pub logo_url: String,
    pub logo_width: u32,
    pub logo_height: u32,
    pub is_installed: bool,
    pub size_bytes: u64,
    pub size: String,
    pub current_instance: String,
    pub instances: Vec<ServerInstanceInfo>,
    pub source_file: String,
}

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct LibraryScanResult {
    pub library_path: String,
    pub library_exists: bool,
    pub has_steamcmd_folder: bool,
    pub has_steamcmd_binary: bool,
    pub sct_data_path: String,
    pub servers: Vec<ServerMetadataDto>,
}

/// Resolves sct_data next to the executable first, then walks ancestors for repo dev builds.
pub fn find_sct_data_dir() -> Option<PathBuf> {
    if let Ok(exe_path) = std::env::current_exe() {
        if let Some(exe_dir) = exe_path.parent() {
            let next_to_exe = exe_dir.join("sct_data");
            if next_to_exe.is_dir() {
                return Some(next_to_exe);
            }

            let mut cursor = exe_dir.to_path_buf();
            for _ in 0..6 {
                let candidate = cursor.join("sct_data");
                if candidate.is_dir() {
                    return Some(candidate);
                }
                if !cursor.pop() {
                    break;
                }
            }
        }
    }

    if let Ok(cwd) = std::env::current_dir() {
        let mut cursor = cwd;
        for _ in 0..4 {
            let candidate = cursor.join("sct_data");
            if candidate.is_dir() {
                return Some(candidate);
            }
            if !cursor.pop() {
                break;
            }
        }
    }

    None
}

pub fn ensure_library_directories(library_path: &str) -> Result<String, String> {
    let trimmed = library_path.trim();
    if trimmed.is_empty() {
        return Err("Library path cannot be empty.".to_string());
    }

    let lib_dir = PathBuf::from(trimmed);
    fs::create_dir_all(&lib_dir)
        .map_err(|e| format!("Failed to create library directory '{}': {}", lib_dir.display(), e))?;

    let steamcmd_dir = lib_dir.join("steamcmd");
    fs::create_dir_all(&steamcmd_dir)
        .map_err(|e| format!("Failed to create steamcmd directory '{}': {}", steamcmd_dir.display(), e))?;

    Ok(lib_dir.to_string_lossy().into_owned())
}

fn dir_has_any_entry(path: &Path) -> bool {
    match fs::read_dir(path) {
        Ok(mut entries) => entries.next().is_some(),
        Err(_) => false,
    }
}

fn calculate_dir_size(path: &Path) -> u64 {
    let mut total = 0u64;
    let Ok(entries) = fs::read_dir(path) else {
        return 0;
    };

    for entry in entries.flatten() {
        let entry_path = entry.path();
        if let Ok(meta) = fs::symlink_metadata(&entry_path) {
            if meta.is_file() {
                total = total.saturating_add(meta.len());
            } else if meta.is_dir() && !meta.file_type().is_symlink() {
                total = total.saturating_add(calculate_dir_size(&entry_path));
            }
        }
    }

    total
}

fn format_bytes(bytes: u64) -> String {
    const KB: f64 = 1024.0;
    const MB: f64 = KB * 1024.0;
    const GB: f64 = MB * 1024.0;

    if bytes == 0 {
        return "0 B".to_string();
    }

    let b = bytes as f64;
    if b >= GB {
        format!("{:.2} GB", b / GB)
    } else if b >= MB {
        format!("{:.1} MB", b / MB)
    } else if b >= KB {
        format!("{:.0} KB", b / KB)
    } else {
        format!("{} B", bytes)
    }
}

fn extract_string_map(val: Option<&Value>) -> HashMap<String, String> {
    let mut map = HashMap::new();
    let Some(v) = val else {
        return map;
    };

    if let Some(obj) = v.as_object() {
        for (k, item) in obj {
            if let Some(s) = item.as_str() {
                map.insert(k.clone(), s.to_string());
            } else if let Some(n) = item.as_i64() {
                map.insert(k.clone(), n.to_string());
            } else if let Some(n) = item.as_u64() {
                map.insert(k.clone(), n.to_string());
            }
        }
    } else if let Some(s) = v.as_str() {
        if !s.trim().is_empty() {
            map.insert("default".to_string(), s.to_string());
        }
    } else if let Some(n) = v.as_u64() {
        map.insert("default".to_string(), n.to_string());
    }

    map
}

fn pick_preferred_map_value(map: &HashMap<String, String>, preferred_keys: &[&str]) -> Option<String> {
    for key in preferred_keys {
        if let Some(val) = map.get(*key) {
            if !val.trim().is_empty() {
                return Some(val.clone());
            }
        }
    }
    map.values()
        .find(|v| !v.trim().is_empty())
        .cloned()
}

fn logo_mime_from_name(name_lower: &str) -> Option<&'static str> {
    if name_lower.ends_with(".png") {
        Some("image/png")
    } else if name_lower.ends_with(".jpg") || name_lower.ends_with(".jpeg") {
        Some("image/jpeg")
    } else if name_lower.ends_with(".webp") {
        Some("image/webp")
    } else if name_lower.ends_with(".gif") {
        Some("image/gif")
    } else if name_lower.ends_with(".svg") {
        Some("image/svg+xml")
    } else {
        None
    }
}

fn parse_server_archive(archive_path: &Path) -> Result<ServerMetadataDto, String> {
    let file = File::open(archive_path)
        .map_err(|e| format!("Failed to open '{}': {}", archive_path.display(), e))?;
    let mut archive = zip::ZipArchive::new(file)
        .map_err(|e| format!("Invalid .server archive '{}': {}", archive_path.display(), e))?;

    let mut metadata_index: Option<usize> = None;
    let mut logo_index: Option<(usize, &'static str)> = None;

    for i in 0..archive.len() {
        let entry = archive.by_index(i).map_err(|e| e.to_string())?;
        if entry.is_dir() {
            continue;
        }

        let norm = entry.name().replace('\\', "/");
        let lower = norm.to_lowercase();

        if lower == "metadata.json" || lower.ends_with("/metadata.json") {
            metadata_index = Some(i);
        } else if let Some(mime) = logo_mime_from_name(&lower) {
            if lower.starts_with("data/logo.") || lower.contains("/data/logo.") {
                logo_index = Some((i, mime));
            } else if logo_index.is_none() && (lower.starts_with("data/") || lower.contains("/data/")) {
                logo_index = Some((i, mime));
            }
        }
    }

    let meta_idx = metadata_index.ok_or_else(|| {
        format!(
            "No metadata.json found inside '{}'",
            archive_path.display()
        )
    })?;

    let meta_text = {
        let mut entry = archive.by_index(meta_idx).map_err(|e| e.to_string())?;
        let mut buf = String::new();
        entry.read_to_string(&mut buf).map_err(|e| e.to_string())?;
        buf
    };

    let meta: Value = serde_json::from_str(&meta_text).map_err(|e| {
        format!(
            "Failed to parse metadata.json in '{}': {}",
            archive_path.display(),
            e
        )
    })?;

    let fallback_stem = archive_path
        .file_stem()
        .and_then(|s| s.to_str())
        .unwrap_or("server")
        .trim()
        .to_string();

    let codename = meta
        .get("Codename")
        .and_then(|v| v.as_str())
        .map(str::trim)
        .filter(|s| !s.is_empty())
        .unwrap_or(&fallback_stem)
        .to_string();

    let folder_name = meta
        .get("RootFolder")
        .and_then(|v| v.as_str())
        .map(str::trim)
        .filter(|s| !s.is_empty())
        .unwrap_or(&codename)
        .to_string();

    let names = extract_string_map(meta.get("Name"));
    let display_name = if !names.is_empty() {
        pick_preferred_map_value(&names, &["en", "default"]).unwrap_or_else(|| codename.clone())
    } else if let Some(raw_name) = meta.get("Name").and_then(|v| v.as_str()) {
        if raw_name.trim().is_empty() {
            codename.clone()
        } else {
            raw_name.trim().to_string()
        }
    } else {
        codename.clone()
    };

    let steam_app_ids = extract_string_map(meta.get("SteamAppId"));
    #[cfg(target_os = "windows")]
    let platform_keys = ["windows", "default", "linux"];
    #[cfg(not(target_os = "windows"))]
    let platform_keys = ["linux", "default", "windows"];

    let steam_app_id =
        pick_preferred_map_value(&steam_app_ids, &platform_keys).unwrap_or_default();

    let is_steam_server = meta
        .get("IsSteamServer")
        .and_then(|v| v.as_bool())
        .unwrap_or(!steam_app_id.is_empty());

    let supports_windows = meta
        .get("SupportsWindows")
        .and_then(|v| v.as_bool())
        .unwrap_or(true);

    let supports_linux = meta
        .get("SupportsLinux")
        .and_then(|v| v.as_bool())
        .unwrap_or(false);

    let base_platform = meta
        .get("BasePlatform")
        .and_then(|v| v.as_str())
        .unwrap_or("windows")
        .to_string();

    let logo_width = meta
        .get("LogoResolutionX")
        .and_then(|v| v.as_u64())
        .unwrap_or(100) as u32;

    let logo_height = meta
        .get("LogoResolutionY")
        .and_then(|v| v.as_u64())
        .unwrap_or(100) as u32;

    let logo_url = if let Some((idx, mime)) = logo_index {
        let mut entry = archive.by_index(idx).map_err(|e| e.to_string())?;
        let mut bytes = Vec::with_capacity(entry.size() as usize);
        entry.read_to_end(&mut bytes).map_err(|e| e.to_string())?;
        let b64 = base64::engine::general_purpose::STANDARD.encode(&bytes);
        format!("data:{};base64,{}", mime, b64)
    } else {
        "img/servers/toolIcon(1).png".to_string()
    };

    let default_folder = format!("{}_ds", codename);

    Ok(ServerMetadataDto {
        id: codename.to_lowercase(),
        codename,
        folder_name,
        name: display_name,
        names,
        steam_app_id,
        steam_app_ids,
        is_steam_server,
        supports_windows,
        supports_linux,
        base_platform,
        logo_url,
        logo_width,
        logo_height,
        is_installed: false,
        size_bytes: 0,
        size: "-".to_string(),
        current_instance: "Default".to_string(),
        instances: vec![ServerInstanceInfo {
            name: "Default".to_string(),
            folder_name: default_folder,
            full_path: String::new(),
            is_installed: false,
            size_bytes: 0,
            size: "-".to_string(),
        }],
        source_file: archive_path
            .file_name()
            .and_then(|s| s.to_str())
            .unwrap_or_default()
            .to_string(),
    })
}

fn scan_library_folders(library_dir: &Path, servers: &mut [ServerMetadataDto]) {
    let known_codenames: HashSet<String> =
        servers.iter().map(|s| s.codename.to_lowercase()).collect();

    let subdirs: Vec<(String, PathBuf)> = match fs::read_dir(library_dir) {
        Ok(entries) => entries
            .flatten()
            .filter_map(|e| {
                let p = e.path();
                if p.is_dir() {
                    let name = e.file_name().to_string_lossy().into_owned();
                    Some((name, p))
                } else {
                    None
                }
            })
            .collect(),
        Err(_) => Vec::new(),
    };

    for srv in servers.iter_mut() {
        let code_lower = srv.codename.to_lowercase();
        let default_folder_expected = format!("{}_ds", srv.codename);
        let default_folder_lower = format!("{}_ds", code_lower);
        let prefix_lower = format!("{}_", code_lower);

        let mut detected_instances: Vec<ServerInstanceInfo> = Vec::new();
        let mut has_default_folder = false;

        for (dir_name, dir_path) in &subdirs {
            let dir_lower = dir_name.to_lowercase();
            if dir_lower == "steamcmd" {
                continue;
            }

            let instance_name_opt = if dir_lower == default_folder_lower {
                Some("Default".to_string())
            } else if dir_lower.starts_with(&prefix_lower)
                && dir_lower.ends_with("_ds")
                && dir_name.len() > prefix_lower.len() + 3
            {
                // Avoid claiming a folder that is another server's <other_codename>_ds folder.
                let stem_lower = &dir_lower[..dir_lower.len() - 3];
                if known_codenames.contains(stem_lower) && stem_lower != code_lower {
                    None
                } else {
                    let middle = &dir_name[prefix_lower.len()..dir_name.len() - 3];
                    if middle.trim().is_empty() {
                        None
                    } else {
                        Some(middle.to_string())
                    }
                }
            } else {
                None
            };

            let Some(inst_name) = instance_name_opt else {
                continue;
            };

            if inst_name == "Default" {
                has_default_folder = true;
            }

            let installed = dir_has_any_entry(dir_path);
            let size_bytes = if installed {
                calculate_dir_size(dir_path)
            } else {
                0
            };

            detected_instances.push(ServerInstanceInfo {
                name: inst_name,
                folder_name: dir_name.clone(),
                full_path: dir_path.to_string_lossy().into_owned(),
                is_installed: installed,
                size_bytes,
                size: if installed {
                    format_bytes(size_bytes)
                } else {
                    "-".to_string()
                },
            });
        }

        // Only keep instances that are actually installed, plus always keep Default available.
        let mut installed_only: Vec<ServerInstanceInfo> = detected_instances
            .into_iter()
            .filter(|i| i.is_installed)
            .collect();

        installed_only.sort_by(|a, b| {
            if a.name == "Default" {
                std::cmp::Ordering::Less
            } else if b.name == "Default" {
                std::cmp::Ordering::Greater
            } else {
                a.name.to_lowercase().cmp(&b.name.to_lowercase())
            }
        });

        let has_default_installed = installed_only.iter().any(|i| i.name == "Default");
        let any_installed = !installed_only.is_empty();

        if !has_default_installed && (!any_installed || has_default_folder) {
            let default_path = library_dir.join(&default_folder_expected);
            installed_only.insert(
                0,
                ServerInstanceInfo {
                    name: "Default".to_string(),
                    folder_name: default_folder_expected.clone(),
                    full_path: default_path.to_string_lossy().into_owned(),
                    is_installed: false,
                    size_bytes: 0,
                    size: "-".to_string(),
                },
            );
        } else if installed_only.is_empty() {
            let default_path = library_dir.join(&default_folder_expected);
            installed_only.push(ServerInstanceInfo {
                name: "Default".to_string(),
                folder_name: default_folder_expected.clone(),
                full_path: default_path.to_string_lossy().into_owned(),
                is_installed: false,
                size_bytes: 0,
                size: "-".to_string(),
            });
        }

        // Prefer Default if installed; otherwise select the first installed custom instance.
        let active_inst = installed_only
            .iter()
            .find(|i| i.is_installed)
            .or_else(|| installed_only.first())
            .cloned()
            .unwrap();

        srv.is_installed = any_installed;
        srv.current_instance = active_inst.name;
        srv.size_bytes = active_inst.size_bytes;
        srv.size = active_inst.size;
        srv.instances = installed_only;
    }
}

pub fn load_servers_with_library(library_path: &str) -> LibraryScanResult {
    let trimmed_lib = library_path.trim().to_string();
    let lib_dir = PathBuf::from(&trimmed_lib);
    let library_exists = !trimmed_lib.is_empty() && lib_dir.is_dir();

    let steamcmd_dir = lib_dir.join("steamcmd");
    let has_steamcmd_folder = library_exists && steamcmd_dir.is_dir();
    let has_steamcmd_binary = library_exists
        && (steamcmd_dir.join("steamcmd.exe").is_file()
            || steamcmd_dir.join("steamcmd.sh").is_file());

    let mut servers = Vec::new();
    let sct_data_dir = find_sct_data_dir();
    let sct_data_path = sct_data_dir
        .as_ref()
        .map(|p| p.to_string_lossy().into_owned())
        .unwrap_or_default();

    if let Some(data_dir) = sct_data_dir {
        if let Ok(entries) = fs::read_dir(&data_dir) {
            for entry in entries.flatten() {
                let path = entry.path();
                if path.is_file() {
                    let is_server_file = path
                        .extension()
                        .and_then(|e| e.to_str())
                        .map(|e| e.eq_ignore_ascii_case("server"))
                        .unwrap_or(false);
                    if is_server_file {
                        match parse_server_archive(&path) {
                            Ok(srv) => servers.push(srv),
                            Err(err) => eprintln!("SCT server parse error: {}", err),
                        }
                    }
                }
            }
        }
    }

    servers.sort_by(|a, b| a.name.to_lowercase().cmp(&b.name.to_lowercase()));

    if library_exists {
        scan_library_folders(&lib_dir, &mut servers);
    }

    LibraryScanResult {
        library_path: trimmed_lib,
        library_exists,
        has_steamcmd_folder,
        has_steamcmd_binary,
        sct_data_path,
        servers,
    }
}

fn sanitize_single_segment(raw: &str, label: &str) -> Result<String, String> {
    let trimmed = raw.trim();
    if trimmed.is_empty() {
        return Err(format!("{} cannot be empty.", label));
    }
    if trimmed.contains('/') || trimmed.contains('\\') || trimmed.contains("..") {
        return Err(format!("Invalid {} '{}'.", label, trimmed));
    }
    Ok(trimmed.to_string())
}

fn resolve_instance_dir(
    library_path: &str,
    codename: &str,
    instance_name: &str,
) -> Result<(PathBuf, String), String> {
    let lib_trimmed = library_path.trim();
    if lib_trimmed.is_empty() {
        return Err("No Server Library location is configured.".to_string());
    }

    let safe_codename = sanitize_single_segment(codename, "server codename")?;
    let trimmed_inst = instance_name.trim();
    let folder_name = if trimmed_inst.is_empty() || trimmed_inst.eq_ignore_ascii_case("Default") {
        format!("{}_ds", safe_codename)
    } else {
        let safe_inst = sanitize_single_segment(trimmed_inst, "instance name")?;
        format!("{}_{}_ds", safe_codename, safe_inst)
    };

    Ok((PathBuf::from(lib_trimmed).join(&folder_name), folder_name))
}

pub fn mock_install_instance(
    library_path: &str,
    codename: &str,
    instance_name: &str,
) -> Result<String, String> {
    let _ = ensure_library_directories(library_path)?;
    let (server_dir, folder_name) = resolve_instance_dir(library_path, codename, instance_name)?;

    fs::create_dir_all(&server_dir)
        .map_err(|e| format!("Failed to create server folder '{}': {}", server_dir.display(), e))?;

    let marker_path = server_dir.join("server_installed.txt");
    let display_instance = if instance_name.trim().is_empty() {
        "Default"
    } else {
        instance_name.trim()
    };
    let content = format!(
        "Server Creation Tool - Mock Install\r\nCodename: {}\r\nInstance: {}\r\nFolder: {}\r\n",
        codename.trim(),
        display_instance,
        folder_name
    );

    fs::write(&marker_path, content)
        .map_err(|e| format!("Failed to write marker file '{}': {}", marker_path.display(), e))?;

    Ok(server_dir.to_string_lossy().into_owned())
}

fn clear_readonly_recursive(path: &Path) {
    if let Ok(meta) = fs::symlink_metadata(path) {
        let mut perms = meta.permissions();
        if perms.readonly() {
            perms.set_readonly(false);
            let _ = fs::set_permissions(path, perms);
        }
        if meta.is_dir() && !meta.file_type().is_symlink() {
            if let Ok(entries) = fs::read_dir(path) {
                for entry in entries.flatten() {
                    clear_readonly_recursive(&entry.path());
                }
            }
        }
    }
}

pub fn delete_server_instance(
    library_path: &str,
    codename: &str,
    instance_name: &str,
) -> Result<bool, String> {
    let (expected_dir, expected_name) = resolve_instance_dir(library_path, codename, instance_name)?;
    let lib_dir = PathBuf::from(library_path.trim());

    let server_dir = if expected_dir.is_dir() {
        expected_dir
    } else if lib_dir.is_dir() {
        let found = fs::read_dir(&lib_dir)
            .ok()
            .and_then(|entries| {
                entries.flatten().find_map(|entry| {
                    let path = entry.path();
                    if path.is_dir()
                        && entry
                            .file_name()
                            .to_string_lossy()
                            .eq_ignore_ascii_case(&expected_name)
                    {
                        Some(path)
                    } else {
                        None
                    }
                })
            });
        match found {
            Some(dir) => dir,
            None => return Ok(false),
        }
    } else {
        return Ok(false);
    };

    if fs::remove_dir_all(&server_dir).is_err() {
        clear_readonly_recursive(&server_dir);
        fs::remove_dir_all(&server_dir)
            .map_err(|e| format!("Failed to delete server folder '{}': {}", server_dir.display(), e))?;
    }

    Ok(true)
}

#[cfg(test)]
mod tests {
    use super::{
        delete_server_instance, load_servers_with_library, mock_install_instance,
        scan_library_folders, ServerInstanceInfo, ServerMetadataDto,
    };
    use std::collections::HashMap;
    use std::fs;
    use std::time::{SystemTime, UNIX_EPOCH};

    fn make_temp_dir(name: &str) -> std::path::PathBuf {
        let nanos = SystemTime::now()
            .duration_since(UNIX_EPOCH)
            .unwrap()
            .as_nanos();
        let dir = std::env::temp_dir().join(format!("sct_test_{}_{}", name, nanos));
        fs::create_dir_all(&dir).unwrap();
        dir
    }

    fn dummy_meta(codename: &str) -> ServerMetadataDto {
        ServerMetadataDto {
            id: codename.to_lowercase(),
            codename: codename.to_string(),
            folder_name: codename.to_string(),
            name: codename.to_string(),
            names: HashMap::new(),
            steam_app_id: "90".to_string(),
            steam_app_ids: HashMap::new(),
            is_steam_server: true,
            supports_windows: true,
            supports_linux: false,
            base_platform: "windows".to_string(),
            logo_url: String::new(),
            logo_width: 100,
            logo_height: 100,
            is_installed: false,
            size_bytes: 0,
            size: "-".to_string(),
            current_instance: "Default".to_string(),
            instances: vec![ServerInstanceInfo {
                name: "Default".to_string(),
                folder_name: format!("{}_ds", codename),
                full_path: String::new(),
                is_installed: false,
                size_bytes: 0,
                size: "-".to_string(),
            }],
            source_file: String::new(),
        }
    }

    #[test]
    fn detects_empty_vs_nonempty_and_instances() {
        let lib = make_temp_dir("lib");
        fs::create_dir_all(lib.join("steamcmd")).unwrap();
        // Empty csgo_ds should NOT be considered installed
        fs::create_dir_all(lib.join("csgo_ds")).unwrap();
        // Non-empty csgo_surf_ds SHOULD be detected as custom instance 'surf'
        fs::create_dir_all(lib.join("csgo_surf_ds")).unwrap();
        fs::write(lib.join("csgo_surf_ds").join("srcds.exe"), b"binary").unwrap();
        // Non-empty cs_ds SHOULD be detected as Default instance
        fs::create_dir_all(lib.join("cs_ds")).unwrap();
        fs::write(lib.join("cs_ds").join("hlds.exe"), b"1234").unwrap();

        let mut servers = vec![dummy_meta("cs"), dummy_meta("csgo")];
        scan_library_folders(&lib, &mut servers);

        assert!(servers[0].is_installed);
        assert_eq!(servers[0].current_instance, "Default");
        assert_eq!(servers[0].size_bytes, 4);

        assert!(servers[1].is_installed);
        assert_eq!(servers[1].current_instance, "surf");
        assert_eq!(servers[1].instances.len(), 2);
        assert_eq!(servers[1].instances[0].name, "Default");
        assert!(!servers[1].instances[0].is_installed);
        assert_eq!(servers[1].instances[1].name, "surf");
        assert!(servers[1].instances[1].is_installed);

        let _ = fs::remove_dir_all(&lib);
    }

    #[test]
    fn loads_real_cs_server_from_repo_sct_data() {
        let res = load_servers_with_library("");
        let cs = res.servers.iter().find(|s| s.codename == "cs");
        assert!(cs.is_some(), "Expected cs.server to be loaded from sct_data");
        let cs = cs.unwrap();
        assert_eq!(cs.name, "Counter Strike 1.6");
        assert_eq!(cs.steam_app_id, "90");
        assert_eq!(cs.folder_name, "cs");
        assert!(cs.logo_url.starts_with("data:image/png;base64,"));
    }

    #[test]
    fn mock_install_and_delete_create_and_remove_folder() {
        let lib = make_temp_dir("mock_ops");
        let lib_str = lib.to_string_lossy().into_owned();

        let created = mock_install_instance(&lib_str, "cs", "Default").unwrap();
        let created_path = std::path::PathBuf::from(&created);
        assert!(created_path.is_dir());
        assert_eq!(
            created_path.file_name().and_then(|s| s.to_str()),
            Some("cs_ds")
        );
        let marker = created_path.join("server_installed.txt");
        assert!(marker.is_file());

        // Ensure real delete removes directories even when files are read-only
        let mut perms = fs::metadata(&marker).unwrap().permissions();
        perms.set_readonly(true);
        fs::set_permissions(&marker, perms).unwrap();

        let deleted = delete_server_instance(&lib_str, "cs", "Default").unwrap();
        assert!(deleted);
        assert!(!created_path.exists());

        let _ = fs::remove_dir_all(&lib);
    }
}
