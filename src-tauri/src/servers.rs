use base64::Engine;
use serde::{Deserialize, Serialize};
use serde_json::Value;
use std::collections::{HashMap, HashSet};
use std::fs::{self, File};
use std::io::Read;
use std::path::{Path, PathBuf};
use std::process::Command;

pub const INCOMPLETE_INSTALL_MARKER: &str = ".sct_incomplete";

/// Optional hook attached to a start option ("Run action first" in MakeServer).
/// Points at a SimpleAction or a _funcs.cs function. It is not a start option itself.
#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct PreLaunchHookDto {
    /// "Action" or "Function"
    pub kind: String,
    #[serde(default)]
    pub simple_action_id: String,
    #[serde(default)]
    pub function_name: String,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct StartOptionDto {
    pub id: String,
    pub label: String,
    pub text: HashMap<String, String>,
    /// "Basic" (executable), "Script", "Function" or "Action"
    pub option_type: String,
    /// Executable/script path, or function name when option_type is "Function". Empty for "Action".
    pub file_path: String,
    pub args: String,
    pub is_default: bool,
    #[serde(default)]
    pub is_platform_specific: bool,
    #[serde(default)]
    pub target_platform: String,
    /// Set when RunActionFirst is enabled, or always for "Action" options.
    #[serde(default)]
    pub pre_launch: Option<PreLaunchHookDto>,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct SimpleActionDto {
    pub id: String,
    pub label: String,
    pub title: HashMap<String, String>,
    /// e.g. "file_drop", "run_program"
    pub action_type: String,
    pub when: String,
    pub target_language: String,
    pub source_file_name: String,
    pub download_from_link: bool,
    pub target_dir: String,
    pub overwrite: bool,
    pub extract_archive: bool,
    pub delete_path: String,
    pub copy_source: String,
    pub copy_dest: String,
    pub run_exec: String,
    pub run_args: String,
    pub enable_chaining: bool,
    pub chained_action_ids: Vec<String>,
    pub is_platform_specific: bool,
    pub target_platform: String,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct CustomContainerDto {
    pub id: String,
    /// "tools_maintenance", "settings" or "automation_watchdog"
    pub target_tab: String,
    pub name: String,
    pub names: HashMap<String, String>,
    pub columns: u32,
    pub is_locked: bool,
    pub is_config_files_container: bool,
    pub is_visible: bool,
    pub config_file_paths: String,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct SubMenuButtonDto {
    pub text: String,
    pub texts: HashMap<String, String>,
    pub fore_color: String,
    pub back_color: String,
    /// "function" or "simple_action"
    pub action_type: String,
    pub button_func_name: String,
    pub simple_action_id: String,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct CustomControlDto {
    pub text: String,
    pub texts: HashMap<String, String>,
    pub control_type: String,
    pub container_id: String,
    pub row: u32,
    pub column: u32,
    pub fore_color: String,
    pub back_color: String,
    /// "function" or "simple_action"
    pub action_type: String,
    pub button_func_name: String,
    pub simple_action_id: String,
    pub is_system_guide_btn: bool,
    pub use_system_default_text: bool,
    pub submenu_buttons: Vec<SubMenuButtonDto>,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct CustomFieldDto {
    pub name: String,
    pub value: String,
    pub values: HashMap<String, String>,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct ServerLanguageDto {
    pub code: String,
    pub name: String,
}

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct AvailableStartOption {
    pub id: String,
    pub label: String,
    pub option_type: String,
    pub file_path: String,
    pub resolved_path: String,
    pub args: String,
    pub is_default: bool,
}

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct ServerInstanceInfo {
    pub name: String,
    pub folder_name: String,
    pub full_path: String,
    pub is_installed: bool,
    pub has_incomplete_install: bool,
    pub size_bytes: u64,
    pub size: String,
}

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct ServerMetadataDto {
    pub schema_version: u32,
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
    pub supports_stdout_redirect: bool,
    pub base_platform: String,
    pub files_download_link: String,
    pub files_download_links: HashMap<String, String>,
    pub are_files_local: bool,
    pub auto_extract_files: bool,
    pub run_custom_install_func: bool,
    pub simple_actions_enabled: bool,
    pub logo_url: String,
    pub logo_width: u32,
    pub logo_height: u32,
    pub is_installed: bool,
    pub has_incomplete_install: bool,
    pub size_bytes: u64,
    pub size: String,
    pub current_instance: String,
    pub instances: Vec<ServerInstanceInfo>,
    pub start_options: Vec<StartOptionDto>,
    pub simple_actions: Vec<SimpleActionDto>,
    pub custom_containers: Vec<CustomContainerDto>,
    pub custom_controls: Vec<CustomControlDto>,
    pub guide_doc_link: String,
    pub guide_doc_links: HashMap<String, String>,
    pub custom_fields: Vec<CustomFieldDto>,
    pub languages: Vec<ServerLanguageDto>,
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

pub fn is_instance_incomplete(path: &Path) -> bool {
    path.join(INCOMPLETE_INSTALL_MARKER).is_file()
}

pub fn mark_instance_incomplete(path: &Path) -> Result<(), String> {
    fs::create_dir_all(path)
        .map_err(|e| format!("Failed to create server folder '{}': {}", path.display(), e))?;
    let marker = path.join(INCOMPLETE_INSTALL_MARKER);
    fs::write(&marker, b"incomplete\n")
        .map_err(|e| format!("Failed to write incomplete marker '{}': {}", marker.display(), e))
}

pub fn clear_instance_incomplete(path: &Path) {
    let marker = path.join(INCOMPLETE_INSTALL_MARKER);
    if marker.exists() {
        let _ = fs::remove_file(marker);
    }
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
        if entry
            .file_name()
            .to_string_lossy()
            .eq_ignore_ascii_case(INCOMPLETE_INSTALL_MARKER)
        {
            continue;
        }
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

#[cfg(target_os = "windows")]
const CURRENT_OS: &str = "windows";
#[cfg(not(target_os = "windows"))]
const CURRENT_OS: &str = "linux";

fn value_to_string(v: &Value) -> Option<String> {
    if let Some(s) = v.as_str() {
        Some(s.to_string())
    } else if let Some(n) = v.as_u64() {
        Some(n.to_string())
    } else if let Some(n) = v.as_i64() {
        Some(n.to_string())
    } else {
        None
    }
}

fn get_str(item: &Value, key: &str, default: &str) -> String {
    item.get(key)
        .and_then(|v| v.as_str())
        .unwrap_or(default)
        .trim()
        .to_string()
}

fn get_bool(item: &Value, key: &str, default: bool) -> bool {
    item.get(key).and_then(|v| v.as_bool()).unwrap_or(default)
}

fn get_u32(item: &Value, key: &str, default: u32) -> u32 {
    item.get(key)
        .and_then(|v| v.as_u64())
        .map(|n| n as u32)
        .unwrap_or(default)
}

fn normalize_lang(lang: &str) -> String {
    let l = lang.trim().to_lowercase();
    match l.as_str() {
        "" | "english" => "en".to_string(),
        "german" => "de".to_string(),
        _ => l,
    }
}

fn normalize_platform(p: &str) -> String {
    let clean = p.trim().to_lowercase();
    if clean.starts_with("win") {
        "windows".to_string()
    } else if clean.starts_with("lin") {
        "linux".to_string()
    } else {
        clean
    }
}

/// Mirrors MakeServer LocalizedField: a plain string means "en", an object maps lang codes to text.
fn parse_localized(val: Option<&Value>) -> HashMap<String, String> {
    let mut map = HashMap::new();
    match val {
        Some(Value::String(s)) if !s.trim().is_empty() => {
            map.insert("en".to_string(), s.trim().to_string());
        }
        Some(Value::Object(obj)) => {
            for (k, v) in obj {
                if let Some(s) = v.as_str() {
                    if !s.trim().is_empty() {
                        map.insert(normalize_lang(k), s.trim().to_string());
                    }
                }
            }
        }
        _ => {}
    }
    map
}

/// Returns the text for "en" (LocalizedField.ToString fallback in MakeServer).
fn resolve_localized(map: &HashMap<String, String>) -> String {
    map.get("en").cloned().unwrap_or_default()
}

/// Mirrors MakeServer PlatformField: a plain string means "windows", an object maps platforms to values.
fn parse_platform_field(val: Option<&Value>) -> HashMap<String, String> {
    let mut map = HashMap::new();
    match val {
        Some(Value::Object(obj)) => {
            for (k, v) in obj {
                let Some(s) = value_to_string(v) else {
                    continue;
                };
                let p = normalize_platform(k);
                if p == "windows" || p == "linux" {
                    map.insert(p, s);
                } else if k.eq_ignore_ascii_case("en") || k.eq_ignore_ascii_case("english") {
                    map.insert("windows".to_string(), s);
                }
            }
        }
        Some(v) => {
            if let Some(s) = value_to_string(v) {
                if !s.is_empty() {
                    map.insert("windows".to_string(), s);
                }
            }
        }
        None => {}
    }
    map
}

/// PlatformField.Get: current OS value, then BasePlatform value, else empty.
fn resolve_platform_field(map: &HashMap<String, String>, base_platform: &str) -> String {
    if let Some(v) = map.get(CURRENT_OS) {
        if !v.trim().is_empty() {
            return v.trim().to_string();
        }
    }
    let base = normalize_platform(base_platform);
    map.get(&base)
        .map(|v| v.trim().to_string())
        .unwrap_or_default()
}

fn platform_value(item: &Value, key: &str, base_platform: &str) -> String {
    resolve_platform_field(&parse_platform_field(item.get(key)), base_platform)
}

fn extract_string_array(val: Option<&Value>) -> Vec<String> {
    val.and_then(|v| v.as_array())
        .map(|arr| {
            arr.iter()
                .filter_map(|item| item.as_str().map(|s| s.trim().to_string()))
                .filter(|s| !s.is_empty())
                .collect()
        })
        .unwrap_or_default()
}

/// Same rule as MakeServer VisibleStartOptions / VisibleSimpleActions.
fn is_platform_allowed(item: &Value) -> bool {
    if !get_bool(item, "IsPlatformSpecific", false) {
        return true;
    }
    let target = get_str(item, "TargetPlatform", "all");
    target.is_empty()
        || target.eq_ignore_ascii_case("all")
        || normalize_platform(&target) == CURRENT_OS
}

fn extract_start_options(val: Option<&Value>, base_platform: &str) -> Vec<StartOptionDto> {
    let Some(arr) = val.and_then(|v| v.as_array()) else {
        return Vec::new();
    };

    let mut out = Vec::new();
    for (idx, item) in arr.iter().enumerate() {
        if !is_platform_allowed(item) {
            continue;
        }

        // Id is a 64-bit tick count, keep it as a string so JS does not lose precision.
        let id = item
            .get("Id")
            .and_then(value_to_string)
            .map(|s| s.trim().to_string())
            .filter(|s| !s.is_empty())
            .unwrap_or_else(|| format!("opt_{}", idx));

        let option_type = get_str(item, "Type", "Basic");
        let file_path = if option_type.eq_ignore_ascii_case("Action") {
            String::new()
        } else {
            platform_value(item, "File", base_platform)
        };
        let args = platform_value(item, "Args", base_platform);

        let text = parse_localized(item.get("Text"));
        let label = Some(resolve_localized(&text))
            .filter(|s| !s.is_empty())
            .or_else(|| Some(file_path.clone()).filter(|s| !s.is_empty()))
            .unwrap_or_else(|| "Unnamed Option".to_string());

        // "Action" options always run their hook; other types only when RunActionFirst is set.
        let has_hook = option_type.eq_ignore_ascii_case("Action") || get_bool(item, "RunActionFirst", false);
        let pre_launch = if has_hook {
            let kind = if get_str(item, "PreLaunchType", "Action").eq_ignore_ascii_case("Function") {
                "Function"
            } else {
                "Action"
            };
            Some(PreLaunchHookDto {
                kind: kind.to_string(),
                simple_action_id: if kind == "Action" {
                    get_str(item, "PreLaunchSimpleActionId", "")
                } else {
                    String::new()
                },
                function_name: if kind == "Function" {
                    get_str(item, "PreLaunchFunction", "")
                } else {
                    String::new()
                },
            })
        } else {
            None
        };

        out.push(StartOptionDto {
            id,
            label,
            text,
            option_type,
            file_path,
            args,
            is_default: get_bool(item, "IsDefault", false),
            is_platform_specific: get_bool(item, "IsPlatformSpecific", false),
            target_platform: get_str(item, "TargetPlatform", "all"),
            pre_launch,
        });
    }

    out
}

fn extract_simple_actions(val: Option<&Value>, base_platform: &str) -> Vec<SimpleActionDto> {
    let Some(arr) = val.and_then(|v| v.as_array()) else {
        return Vec::new();
    };

    let mut out = Vec::new();
    for (idx, item) in arr.iter().enumerate() {
        if !is_platform_allowed(item) {
            continue;
        }

        let id = Some(get_str(item, "Id", ""))
            .filter(|s| !s.is_empty())
            .unwrap_or_else(|| format!("sa_{}", idx));
        let title = parse_localized(item.get("Title"));
        let label = Some(resolve_localized(&title))
            .filter(|s| !s.is_empty())
            .unwrap_or_else(|| id.clone());

        out.push(SimpleActionDto {
            id,
            label,
            title,
            action_type: get_str(item, "Type", "file_drop"),
            when: get_str(item, "When", "manual_trigger"),
            target_language: get_str(item, "TargetLanguage", "all"),
            source_file_name: platform_value(item, "SourceFileName", base_platform),
            download_from_link: get_bool(item, "DownloadFromLink", false),
            target_dir: platform_value(item, "TargetDir", base_platform),
            overwrite: get_bool(item, "Overwrite", true),
            extract_archive: get_bool(item, "ExtractArchive", false),
            delete_path: platform_value(item, "DeletePath", base_platform),
            copy_source: platform_value(item, "CopySource", base_platform),
            copy_dest: platform_value(item, "CopyDest", base_platform),
            run_exec: platform_value(item, "RunExec", base_platform),
            run_args: platform_value(item, "RunArgs", base_platform),
            enable_chaining: get_bool(item, "EnableChaining", false),
            chained_action_ids: extract_string_array(item.get("ChainedActionIds")),
            is_platform_specific: get_bool(item, "IsPlatformSpecific", false),
            target_platform: get_str(item, "TargetPlatform", "all"),
        });
    }

    out
}

fn extract_custom_containers(val: Option<&Value>) -> Vec<CustomContainerDto> {
    let Some(arr) = val.and_then(|v| v.as_array()) else {
        return Vec::new();
    };

    arr.iter()
        .enumerate()
        .map(|(idx, item)| {
            let id = Some(get_str(item, "Id", ""))
                .filter(|s| !s.is_empty())
                .unwrap_or_else(|| format!("cnt_{}", idx));
            let names = parse_localized(item.get("Name"));
            let name = Some(resolve_localized(&names))
                .filter(|s| !s.is_empty())
                .unwrap_or_else(|| id.clone());
            CustomContainerDto {
                id,
                target_tab: get_str(item, "TargetTab", "tools_maintenance"),
                name,
                names,
                columns: get_u32(item, "Columns", 1).clamp(1, 3),
                is_locked: get_bool(item, "IsLocked", false),
                is_config_files_container: get_bool(item, "IsConfigFilesContainer", false),
                is_visible: get_bool(item, "IsVisible", true),
                config_file_paths: get_str(item, "ConfigFilePaths", ""),
            }
        })
        .collect()
}

fn extract_submenu_buttons(val: Option<&Value>) -> Vec<SubMenuButtonDto> {
    let Some(arr) = val.and_then(|v| v.as_array()) else {
        return Vec::new();
    };

    arr.iter()
        .map(|item| {
            let texts = parse_localized(item.get("Text"));
            SubMenuButtonDto {
                text: resolve_localized(&texts),
                texts,
                fore_color: get_str(item, "ForeColor", "#ffffff"),
                back_color: get_str(item, "BackColor", "#1f2937"),
                action_type: get_str(item, "ActionType", "function"),
                button_func_name: get_str(item, "ButtonFuncName", ""),
                simple_action_id: get_str(item, "SimpleActionId", ""),
            }
        })
        .collect()
}

fn extract_custom_controls(val: Option<&Value>) -> Vec<CustomControlDto> {
    let Some(arr) = val.and_then(|v| v.as_array()) else {
        return Vec::new();
    };

    arr.iter()
        .map(|item| {
            let texts = parse_localized(item.get("Text"));
            CustomControlDto {
                text: resolve_localized(&texts),
                texts,
                control_type: get_str(item, "Type", "Button"),
                container_id: get_str(item, "ContainerId", "locked_extra"),
                row: get_u32(item, "Row", 0),
                column: get_u32(item, "Column", 0),
                fore_color: get_str(item, "ForeColor", "#ffffff"),
                back_color: get_str(item, "BackColor", "#1f2937"),
                action_type: get_str(item, "ActionType", "function"),
                button_func_name: get_str(item, "ButtonFuncName", ""),
                simple_action_id: get_str(item, "SimpleActionId", ""),
                is_system_guide_btn: get_bool(item, "IsSystemGuideBtn", false),
                use_system_default_text: get_bool(item, "UseSystemDefaultText", true),
                submenu_buttons: extract_submenu_buttons(item.get("SubmenuButtons")),
            }
        })
        .collect()
}

fn extract_custom_fields(val: Option<&Value>) -> Vec<CustomFieldDto> {
    let Some(arr) = val.and_then(|v| v.as_array()) else {
        return Vec::new();
    };

    arr.iter()
        .filter_map(|item| {
            let name = get_str(item, "Name", "");
            if name.is_empty() {
                return None;
            }
            let values = parse_localized(item.get("Value"));
            Some(CustomFieldDto {
                name,
                value: resolve_localized(&values),
                values,
            })
        })
        .collect()
}

fn default_lang_name(code: &str) -> String {
    match code {
        "en" => "English".to_string(),
        "de" => "German".to_string(),
        other => other.to_string(),
    }
}

/// Mirrors MakeServer LanguageItemJsonConverter: accepts "en" strings or { Code, Name } objects.
fn extract_languages(val: Option<&Value>) -> Vec<ServerLanguageDto> {
    let Some(arr) = val.and_then(|v| v.as_array()) else {
        return Vec::new();
    };

    arr.iter()
        .filter_map(|item| {
            let (code, name) = match item {
                Value::String(s) => (normalize_lang(s), String::new()),
                Value::Object(_) => (get_str(item, "Code", ""), get_str(item, "Name", "")),
                _ => return None,
            };
            if code.is_empty() {
                return None;
            }
            let name = if name.is_empty() { default_lang_name(&code) } else { name };
            Some(ServerLanguageDto { code, name })
        })
        .collect()
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

    let names = parse_localized(meta.get("Name"));
    let display_name = Some(resolve_localized(&names))
        .filter(|s| !s.is_empty())
        .unwrap_or_else(|| codename.clone());

    // Defaults below follow MakeServer EngineData defaults.
    let schema_version = get_u32(&meta, "SchemaVersion", 1);
    let supports_windows = get_bool(&meta, "SupportsWindows", true);
    let supports_linux = get_bool(&meta, "SupportsLinux", true);
    let supports_stdout_redirect = get_bool(&meta, "SupportsStdoutRedirect", false);
    let base_platform = normalize_platform(&get_str(&meta, "BasePlatform", "windows"));

    let steam_app_ids = parse_platform_field(meta.get("SteamAppId"));
    let steam_app_id = resolve_platform_field(&steam_app_ids, &base_platform);
    let is_steam_server = get_bool(&meta, "IsSteamServer", true);

    let files_download_links = parse_platform_field(meta.get("FilesDownloadLink"));
    let files_download_link = resolve_platform_field(&files_download_links, &base_platform);
    let are_files_local = get_bool(&meta, "AreFilesLocal", false);
    let auto_extract_files = get_bool(&meta, "AutoExtractFiles", true);
    let run_custom_install_func = get_bool(&meta, "RunCustomInstallFunc", false);
    let simple_actions_enabled = get_bool(&meta, "SimpleActionsEnabled", true);

    let logo_width = get_u32(&meta, "LogoResolutionX", 100);
    let logo_height = get_u32(&meta, "LogoResolutionY", 100);

    let logo_url = if let Some((idx, mime)) = logo_index {
        let mut entry = archive.by_index(idx).map_err(|e| e.to_string())?;
        let mut bytes = Vec::with_capacity(entry.size() as usize);
        entry.read_to_end(&mut bytes).map_err(|e| e.to_string())?;
        let b64 = base64::engine::general_purpose::STANDARD.encode(&bytes);
        format!("data:{};base64,{}", mime, b64)
    } else {
        "img/servers/toolIcon(1).png".to_string()
    };

    let start_options = extract_start_options(meta.get("StartOptionsList"), &base_platform);
    let simple_actions = extract_simple_actions(meta.get("SimpleActionsList"), &base_platform);
    let custom_containers = extract_custom_containers(meta.get("CustomContainers"));
    let custom_controls = extract_custom_controls(meta.get("CustomControls"));
    let guide_doc_links = parse_localized(meta.get("GuideDocLink"));
    let guide_doc_link = resolve_localized(&guide_doc_links);
    let custom_fields = extract_custom_fields(meta.get("CustomFields"));
    let languages = extract_languages(meta.get("Languages"));

    let default_folder = format!("{}_ds", codename);

    Ok(ServerMetadataDto {
        schema_version,
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
        supports_stdout_redirect,
        base_platform,
        files_download_link,
        files_download_links,
        are_files_local,
        auto_extract_files,
        run_custom_install_func,
        simple_actions_enabled,
        logo_url,
        logo_width,
        logo_height,
        is_installed: false,
        has_incomplete_install: false,
        size_bytes: 0,
        size: "-".to_string(),
        current_instance: "Default".to_string(),
        instances: vec![ServerInstanceInfo {
            name: "Default".to_string(),
            folder_name: default_folder,
            full_path: String::new(),
            is_installed: false,
            has_incomplete_install: false,
            size_bytes: 0,
            size: "-".to_string(),
        }],
        start_options,
        simple_actions,
        custom_containers,
        custom_controls,
        guide_doc_link,
        guide_doc_links,
        custom_fields,
        languages,
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

            let has_incomplete_install = is_instance_incomplete(dir_path);
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
                has_incomplete_install,
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
                    has_incomplete_install: false,
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
                has_incomplete_install: false,
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
        srv.has_incomplete_install = active_inst.has_incomplete_install;
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

pub fn resolve_instance_dir(
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

    clear_instance_incomplete(&server_dir);

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

fn resolve_relative_start_file(server_dir: &Path, rel_raw: &str) -> Option<PathBuf> {
    let trimmed = rel_raw.trim().trim_start_matches(['/', '\\']);
    if trimmed.is_empty() {
        return None;
    }

    let mut path = server_dir.to_path_buf();
    for part in trimmed.split(['/', '\\']) {
        let seg = part.trim();
        if seg.is_empty() || seg == "." {
            continue;
        }
        if seg == ".." {
            return None;
        }
        path.push(seg);
    }

    Some(path)
}

pub fn get_available_start_options(
    library_path: &str,
    codename: &str,
    instance_name: &str,
    options: &[StartOptionDto],
) -> Result<Vec<AvailableStartOption>, String> {
    let (server_dir, _) = resolve_instance_dir(library_path, codename, instance_name)?;
    if !server_dir.is_dir() {
        return Ok(Vec::new());
    }

    let mut available = Vec::new();
    for opt in options {
        // Only Executable ("Basic") and Script options point at a file on disk.
        // Function and Action options need the funcs/actions runtime, which is not implemented yet.
        let is_file_based = opt.option_type.eq_ignore_ascii_case("Basic")
            || opt.option_type.eq_ignore_ascii_case("Script");
        if !is_file_based || opt.file_path.trim().is_empty() {
            continue;
        }
        let Some(full_path) = resolve_relative_start_file(&server_dir, &opt.file_path) else {
            continue;
        };
        if full_path.is_file() {
            available.push(AvailableStartOption {
                id: opt.id.clone(),
                label: opt.label.clone(),
                option_type: opt.option_type.clone(),
                file_path: opt.file_path.clone(),
                resolved_path: full_path.to_string_lossy().into_owned(),
                args: opt.args.clone(),
                is_default: opt.is_default,
            });
        }
    }

    Ok(available)
}

pub fn launch_server_option(file_path: &str, args: &str) -> Result<(), String> {
    let target = PathBuf::from(file_path.trim());
    if !target.is_file() {
        return Err(format!("Start file not found: '{}'", target.display()));
    }

    let working_dir = target
        .parent()
        .map(|p| p.to_path_buf())
        .ok_or_else(|| "Failed to resolve start file directory.".to_string())?;

    let extra_args: Vec<String> = args
        .split_whitespace()
        .filter(|s| !s.is_empty())
        .map(|s| s.to_string())
        .collect();

    #[cfg(target_os = "windows")]
    {
        let ext = target
            .extension()
            .and_then(|e| e.to_str())
            .unwrap_or("")
            .to_lowercase();

        if ext == "bat" || ext == "cmd" {
            let mut cmd = Command::new("cmd");
            cmd.arg("/C")
                .arg("start")
                .arg("")
                .arg("/D")
                .arg(&working_dir)
                .arg(&target);
            for arg in &extra_args {
                cmd.arg(arg);
            }
            cmd.spawn()
                .map_err(|e| format!("Failed to start '{}': {}", target.display(), e))?;
            return Ok(());
        }
    }

    let mut cmd = Command::new(&target);
    cmd.current_dir(&working_dir);
    for arg in &extra_args {
        cmd.arg(arg);
    }
    cmd.spawn()
        .map_err(|e| format!("Failed to start '{}': {}", target.display(), e))?;

    Ok(())
}

#[cfg(test)]
mod tests {
    use super::{
        clear_instance_incomplete, delete_server_instance, extract_start_options,
        get_available_start_options,
        load_servers_with_library, mark_instance_incomplete, mock_install_instance,
        scan_library_folders, PreLaunchHookDto, ServerInstanceInfo, ServerMetadataDto,
        StartOptionDto,
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
            schema_version: 2,
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
            supports_stdout_redirect: false,
            base_platform: "windows".to_string(),
            files_download_link: String::new(),
            files_download_links: HashMap::new(),
            are_files_local: false,
            auto_extract_files: true,
            run_custom_install_func: false,
            simple_actions_enabled: false,
            logo_url: String::new(),
            logo_width: 100,
            logo_height: 100,
            is_installed: false,
            has_incomplete_install: false,
            size_bytes: 0,
            size: "-".to_string(),
            current_instance: "Default".to_string(),
            instances: vec![ServerInstanceInfo {
                name: "Default".to_string(),
                folder_name: format!("{}_ds", codename),
                full_path: String::new(),
                is_installed: false,
                has_incomplete_install: false,
                size_bytes: 0,
                size: "-".to_string(),
            }],
            start_options: Vec::new(),
            simple_actions: Vec::new(),
            custom_containers: Vec::new(),
            custom_controls: Vec::new(),
            guide_doc_link: String::new(),
            guide_doc_links: HashMap::new(),
            custom_fields: Vec::new(),
            languages: Vec::new(),
            source_file: String::new(),
        }
    }

    #[test]
    fn filters_available_start_options_on_disk() {
        let lib = make_temp_dir("start_opts");
        let cs_dir = lib.join("cs_ds");
        fs::create_dir_all(cs_dir.join("bin")).unwrap();
        fs::write(cs_dir.join("Run.bat"), b"@echo off").unwrap();

        let opts = vec![
            StartOptionDto {
                id: "1".to_string(),
                label: "Run.bat".to_string(),
                text: HashMap::new(),
                option_type: "Script".to_string(),
                file_path: "Run.bat".to_string(),
                args: String::new(),
                is_default: true,
                is_platform_specific: false,
                target_platform: "all".to_string(),
                pre_launch: None,
            },
            StartOptionDto {
                id: "2".to_string(),
                label: "srcds.exe (GUI)".to_string(),
                text: HashMap::new(),
                option_type: "Basic".to_string(),
                file_path: "/bin/srcds.exe".to_string(),
                args: String::new(),
                is_default: false,
                is_platform_specific: false,
                target_platform: "all".to_string(),
                pre_launch: None,
            },
            StartOptionDto {
                id: "3".to_string(),
                label: "Action only".to_string(),
                text: HashMap::new(),
                option_type: "Action".to_string(),
                file_path: String::new(),
                args: String::new(),
                is_default: false,
                is_platform_specific: false,
                target_platform: "all".to_string(),
                pre_launch: Some(PreLaunchHookDto {
                    kind: "Action".to_string(),
                    simple_action_id: "sa_1".to_string(),
                    function_name: String::new(),
                }),
            },
        ];

        let lib_str = lib.to_string_lossy().into_owned();
        let avail1 = get_available_start_options(&lib_str, "cs", "Default", &opts).unwrap();
        assert_eq!(avail1.len(), 1);
        assert_eq!(avail1[0].label, "Run.bat");

        fs::write(cs_dir.join("bin").join("srcds.exe"), b"exe").unwrap();
        let avail2 = get_available_start_options(&lib_str, "cs", "Default", &opts).unwrap();
        assert_eq!(avail2.len(), 2);
        assert_eq!(avail2[1].label, "srcds.exe (GUI)");

        let _ = fs::remove_dir_all(&lib);
    }

    #[test]
    fn detects_incomplete_install_marker() {
        let lib = make_temp_dir("incomplete_marker");
        let cs_dir = lib.join("cs_ds");
        mark_instance_incomplete(&cs_dir).unwrap();

        let mut servers = vec![dummy_meta("cs")];
        scan_library_folders(&lib, &mut servers);

        assert!(servers[0].is_installed);
        assert!(servers[0].has_incomplete_install);
        assert_eq!(servers[0].size_bytes, 0);

        clear_instance_incomplete(&cs_dir);
        fs::write(cs_dir.join("hlds.exe"), b"123456").unwrap();

        let mut servers_after = vec![dummy_meta("cs")];
        scan_library_folders(&lib, &mut servers_after);
        assert!(servers_after[0].is_installed);
        assert!(!servers_after[0].has_incomplete_install);
        assert_eq!(servers_after[0].size_bytes, 6);

        let _ = fs::remove_dir_all(&lib);
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
        assert_eq!(cs.schema_version, 2);
        assert_eq!(cs.name, "Counter Strike 1.6");
        assert_eq!(cs.steam_app_id, "90");
        assert_eq!(cs.folder_name, "cs");
        assert!(cs.is_steam_server);
        assert_eq!(cs.start_options.len(), 2);
        assert_eq!(cs.custom_containers.len(), 3);
        assert_eq!(cs.custom_controls.len(), 1);
        assert_eq!(cs.languages.len(), 2);
        assert_eq!(cs.logo_url, "img/servers/toolIcon(1).png");
    }

    #[test]
    fn loads_real_beamng_server_from_repo_sct_data() {
        let res = load_servers_with_library("");
        let beamng = res.servers.iter().find(|s| s.codename == "beamng");
        assert!(beamng.is_some(), "Expected beamng.server to be loaded from sct_data");
        let beamng = beamng.unwrap();
        assert_eq!(beamng.schema_version, 2);
        assert_eq!(beamng.name, "BeamNG.Drive");
        assert!(!beamng.is_steam_server);
        assert!(beamng.supports_windows);
        assert!(beamng.supports_linux);
        assert_eq!(
            beamng.files_download_link,
            "https://github.com/BeamMP/BeamMP-Server/releases/latest/download/BeamMP-Server.exe"
        );
        assert!(beamng.logo_url.starts_with("data:image/png;base64,"));
        assert_eq!(beamng.start_options.len(), 1);
        assert_eq!(beamng.start_options[0].file_path, "\\BeamMP-Server.exe");
        assert!(beamng.start_options[0].pre_launch.is_none());
        assert_eq!(beamng.simple_actions.len(), 1);
        assert_eq!(beamng.simple_actions[0].action_type, "run_program");
        assert_eq!(beamng.simple_actions[0].run_exec, "Dummy-action");
        assert_eq!(beamng.custom_containers.len(), 5);
        assert_eq!(beamng.custom_controls.len(), 8);
        assert_eq!(beamng.languages.len(), 2);
    }

    #[test]
    fn parses_start_option_types_and_pre_launch_hooks() {
        let other_os = if cfg!(target_os = "windows") { "linux" } else { "windows" };
        let json = serde_json::json!([
            {
                "Id": 639301181206990000u64, "Text": "Plain string title", "Type": "Basic",
                "File": "srv.exe", "Args": { "windows": "-win", "linux": "-lin" },
                "RunActionFirst": false, "PreLaunchType": "Action",
                "PreLaunchSimpleActionId": "sa_ignored", "IsPlatformSpecific": false,
                "TargetPlatform": "all", "IsDefault": true
            },
            {
                "Id": 2, "Text": { "en": "With hook" }, "Type": "Script",
                "File": { "windows": "run.bat" }, "Args": "",
                "RunActionFirst": true, "PreLaunchType": "Function",
                "PreLaunchFunction": "PreLaunch", "PreLaunchSimpleActionId": "sa_x",
                "IsPlatformSpecific": false, "TargetPlatform": "all", "IsDefault": false
            },
            {
                "Id": 3, "Text": { "en": "Action entry" }, "Type": "Action",
                "File": "should_be_ignored.exe", "RunActionFirst": false,
                "PreLaunchType": "Action", "PreLaunchSimpleActionId": "sa_1",
                "IsPlatformSpecific": false, "TargetPlatform": "all", "IsDefault": false
            },
            {
                "Id": 4, "Text": { "en": "Other OS only" }, "Type": "Basic",
                "File": { "windows": "a.exe", "linux": "a.sh" },
                "IsPlatformSpecific": true,
                "TargetPlatform": other_os
            }
        ]);

        let opts = extract_start_options(Some(&json), "windows");
        assert_eq!(opts.len(), 3);

        assert_eq!(opts[0].id, "639301181206990000");
        assert_eq!(opts[0].label, "Plain string title");
        assert!(opts[0].pre_launch.is_none());
        #[cfg(target_os = "windows")]
        assert_eq!(opts[0].args, "-win");

        let hook = opts[1].pre_launch.as_ref().expect("RunActionFirst should create a hook");
        assert_eq!(hook.kind, "Function");
        assert_eq!(hook.function_name, "PreLaunch");
        assert!(hook.simple_action_id.is_empty());

        assert_eq!(opts[2].option_type, "Action");
        assert!(opts[2].file_path.is_empty());
        let hook = opts[2].pre_launch.as_ref().expect("Action options always have a hook");
        assert_eq!(hook.kind, "Action");
        assert_eq!(hook.simple_action_id, "sa_1");
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
