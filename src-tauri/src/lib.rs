use std::sync::atomic::{AtomicBool, Ordering};
use tauri::menu::{ContextMenu, MenuBuilder, MenuItemBuilder};
use tauri::tray::{MouseButton, MouseButtonState, TrayIconBuilder, TrayIconEvent};
use tauri::Manager;

static CLOSE_TO_TRAY: AtomicBool = AtomicBool::new(false);

/// Release mode flag: true automatically for `cargo tauri build` (`!debug_assertions`),
/// or when compiled with `SCT_RELEASE=1` / `SCT_RELEASE=true` (e.g. during `cargo tauri dev`).
pub const IS_RELEASE: bool = !cfg!(debug_assertions)
    || match option_env!("SCT_RELEASE") {
        Some(v) => matches!(v.as_bytes(), b"1" | b"true" | b"TRUE"),
        None => false,
    };

#[tauri::command]
fn is_release_mode() -> bool {
    IS_RELEASE
}

mod actions;
mod backups;
mod catalog;
mod logger;
mod non_steam;
mod servers;
mod signing;
mod steamcmd;

#[tauri::command]
fn show_settings_context_menu(
    window: tauri::WebviewWindow,
    x: f64,
    y: f64,
    files: Vec<String>,
    library_path: String,
    server_id: String,
) -> Result<(), String> {
    let mut builder = MenuBuilder::new(&window);
    for file in &files {
        let label = format!("Edit {}", file);
        let full_path = std::path::Path::new(&library_path)
            .join(&server_id)
            .join(file)
            .to_string_lossy()
            .to_string();
        let item = MenuItemBuilder::with_id(&full_path, &label)
            .build(&window)
            .map_err(|e| e.to_string())?;
        builder = builder.item(&item);
    }
    let menu = builder.build().map_err(|e| e.to_string())?;
    let pos = tauri::Position::Logical(tauri::LogicalPosition::new(x, y));
    menu.popup_at(window.as_ref().window(), pos).map_err(|e| e.to_string())?;
    Ok(())
}

#[tauri::command]
fn start_drag(window: tauri::WebviewWindow) -> Result<(), String> {
    window.start_dragging().map_err(|e| e.to_string())
}

#[tauri::command]
fn window_minimize(window: tauri::WebviewWindow) -> Result<(), String> {
    window.minimize().map_err(|e| e.to_string())
}

#[tauri::command]
async fn window_close(app: tauri::AppHandle, window: tauri::WebviewWindow) -> Result<(), String> {
    if window.label() == "main" {
        if CLOSE_TO_TRAY.load(Ordering::Relaxed) {
            let _ = window.hide();
        } else {
            app.exit(0);
        }
    } else {
        let _ = window.close();
    }
    Ok(())
}

#[tauri::command]
fn set_close_to_tray(enabled: bool) {
    CLOSE_TO_TRAY.store(enabled, Ordering::Relaxed);
}

fn resolve_settings_sct_data_dir() -> Result<std::path::PathBuf, String> {
    if IS_RELEASE && !cfg!(test) {
        let exe = std::env::current_exe()
            .map_err(|e| format!("Failed to resolve startup directory: {}", e))?;
        let exe_dir = exe
            .parent()
            .ok_or_else(|| "Failed to resolve startup directory.".to_string())?;
        return Ok(exe_dir.join("sct_data"));
    }
    if let Some(dir) = servers::find_sct_data_dir() {
        return Ok(dir);
    }
    let exe = std::env::current_exe()
        .map_err(|e| format!("Failed to resolve startup directory: {}", e))?;
    let exe_dir = exe
        .parent()
        .ok_or_else(|| "Failed to resolve startup directory.".to_string())?;
    Ok(exe_dir.join("sct_data"))
}

#[tauri::command]
fn load_app_settings() -> Result<Option<String>, String> {
    let dir = resolve_settings_sct_data_dir()?;
    let path = dir.join("settings.json");
    if !path.is_file() {
        return Ok(None);
    }
    let raw = std::fs::read_to_string(&path)
        .map_err(|e| format!("Failed to read '{}': {}", path.display(), e))?;
    Ok(Some(raw))
}

#[tauri::command]
fn save_app_settings(json: String) -> Result<(), String> {
    let dir = resolve_settings_sct_data_dir()?;
    std::fs::create_dir_all(&dir)
        .map_err(|e| format!("Failed to create '{}': {}", dir.display(), e))?;
    let path = dir.join("settings.json");
    std::fs::write(&path, json)
        .map_err(|e| format!("Failed to write '{}': {}", path.display(), e))
}

#[tauri::command]
fn set_run_on_startup(enabled: bool) -> Result<(), String> {
    #[cfg(target_os = "windows")]
    {
        use std::os::windows::process::CommandExt;
        const CREATE_NO_WINDOW: u32 = 0x08000000;
        const RUN_KEY: &str = r"HKCU\Software\Microsoft\Windows\CurrentVersion\Run";
        const VALUE_NAME: &str = "Server Creation Tool";

        if enabled {
            let exe = std::env::current_exe()
                .map_err(|e| format!("Failed to resolve executable path: {}", e))?;
            let quoted = format!("\"{}\"", exe.to_string_lossy());
            let output = std::process::Command::new("reg")
                .args(["add", RUN_KEY, "/v", VALUE_NAME, "/t", "REG_SZ", "/d", &quoted, "/f"])
                .creation_flags(CREATE_NO_WINDOW)
                .output()
                .map_err(|e| format!("Failed to update startup registry: {}", e))?;
            if !output.status.success() {
                let err = String::from_utf8_lossy(&output.stderr).trim().to_string();
                return Err(if err.is_empty() {
                    "Failed to enable Run on startup in Windows registry.".to_string()
                } else {
                    err
                });
            }
        } else {
            let query = std::process::Command::new("reg")
                .args(["query", RUN_KEY, "/v", VALUE_NAME])
                .creation_flags(CREATE_NO_WINDOW)
                .output();
            if let Ok(q) = query {
                if q.status.success() {
                    let output = std::process::Command::new("reg")
                        .args(["delete", RUN_KEY, "/v", VALUE_NAME, "/f"])
                        .creation_flags(CREATE_NO_WINDOW)
                        .output()
                        .map_err(|e| format!("Failed to remove startup registry entry: {}", e))?;
                    if !output.status.success() {
                        let err = String::from_utf8_lossy(&output.stderr).trim().to_string();
                        return Err(if err.is_empty() {
                            "Failed to disable Run on startup in Windows registry.".to_string()
                        } else {
                            err
                        });
                    }
                }
            }
        }
        Ok(())
    }
    #[cfg(not(target_os = "windows"))]
    {
        let config_dir = std::env::var_os("XDG_CONFIG_HOME")
            .map(std::path::PathBuf::from)
            .or_else(|| std::env::var_os("HOME").map(|h| std::path::PathBuf::from(h).join(".config")))
            .ok_or_else(|| "Failed to locate user config directory.".to_string())?;
        let autostart_dir = config_dir.join("autostart");
        let desktop_file = autostart_dir.join("server-creation-tool.desktop");

        if enabled {
            let exe = std::env::current_exe()
                .map_err(|e| format!("Failed to resolve executable path: {}", e))?;
            std::fs::create_dir_all(&autostart_dir)
                .map_err(|e| format!("Failed to create autostart directory: {}", e))?;
            let content = format!(
                "[Desktop Entry]\nType=Application\nName=Server Creation Tool\nExec=\"{}\"\nTerminal=false\nX-GNOME-Autostart-enabled=true\n",
                exe.to_string_lossy()
            );
            std::fs::write(&desktop_file, content)
                .map_err(|e| format!("Failed to write '{}': {}", desktop_file.display(), e))?;
        } else if desktop_file.exists() {
            std::fs::remove_file(&desktop_file)
                .map_err(|e| format!("Failed to remove '{}': {}", desktop_file.display(), e))?;
        }
        Ok(())
    }
}

fn open_subwindow(
    app: &tauri::AppHandle,
    label: &str,
    url: &str,
    title: &str,
    width: f64,
    height: f64,
) -> Result<(), String> {
    if let Some(window) = app.get_webview_window(label) {
        let _ = window.unminimize();
        let _ = window.show();
        let _ = window.set_focus();
    } else {
        tauri::WebviewWindowBuilder::new(app, label, tauri::WebviewUrl::App(url.into()))
            .title(title)
            .inner_size(width, height)
            .resizable(false)
            .maximizable(false)
            .decorations(false)
            .center()
            .build()
            .map_err(|e| e.to_string())?;
    }
    Ok(())
}

#[tauri::command]
async fn open_about_window(app: tauri::AppHandle) -> Result<(), String> {
    open_subwindow(&app, "about", "about.html", "About", 530.0, 504.0)
}

#[tauri::command]
async fn open_faq_window(app: tauri::AppHandle) -> Result<(), String> {
    open_subwindow(
        &app,
        "faq",
        "faq.html",
        "FAQ - Frequently Asked Questions",
        632.0,
        430.0,
    )
}

#[tauri::command]
fn start_steamcmd(install_dir: String) -> Result<bool, String> {
    let mut candidates = Vec::new();

    if !install_dir.is_empty() {
        let base = std::path::PathBuf::from(&install_dir);
        candidates.push(base.join("steamcmd.exe"));
        candidates.push(base.join("steamcmd").join("steamcmd.exe"));
        candidates.push(base.join("steamcmd.sh"));
        candidates.push(base.join("steamcmd").join("steamcmd.sh"));
        candidates.push(base.join("steamcmd"));
    }

    candidates.push(std::path::PathBuf::from("C:\\ServerCreationTool\\steamcmd.exe"));
    candidates.push(std::path::PathBuf::from("C:\\ServerCreationTool\\steamcmd\\steamcmd.exe"));
    if let Ok(home) = std::env::var("HOME") {
        let home_path = std::path::PathBuf::from(home);
        candidates.push(home_path.join(".local/share/Steam/steamcmd/steamcmd.sh"));
        candidates.push(home_path.join("steamcmd/steamcmd.sh"));
        candidates.push(std::path::PathBuf::from("/usr/games/steamcmd"));
    }

    for path in candidates {
        if path.exists() {
            let mut cmd = std::process::Command::new(&path);
            #[cfg(target_os = "windows")]
            {
                use std::os::windows::process::CommandExt;
                const CREATE_NEW_CONSOLE: u32 = 0x0000_0010;
                cmd.creation_flags(CREATE_NEW_CONSOLE);
            }
            cmd.spawn().map_err(|e| e.to_string())?;
            return Ok(true);
        }
    }
    Ok(false)
}

#[tauri::command]
fn clear_steamcmd_cache(install_dir: String) -> Result<bool, String> {
    let mut cleared = false;
    let mut dirs = Vec::new();

    if !install_dir.is_empty() {
        dirs.push(std::path::PathBuf::from(&install_dir).join("appcache"));
        dirs.push(std::path::PathBuf::from(&install_dir).join("steamcmd").join("appcache"));
    }
    dirs.push(std::path::PathBuf::from("C:\\ServerCreationTool\\appcache"));
    dirs.push(std::path::PathBuf::from("C:\\ServerCreationTool\\steamcmd\\appcache"));

    for dir in dirs {
        if dir.exists() {
            if std::fs::remove_dir_all(&dir).is_ok() {
                cleared = true;
            }
        }
    }
    Ok(cleared)
}

#[tauri::command]
fn window_set_width(app: tauri::AppHandle, width: f64) -> Result<(), String> {
    if let Some(window) = app.get_webview_window("main") {
        let scale = window.scale_factor().unwrap_or(1.0);
        let h = window.inner_size().map(|s| (s.height as f64) / scale).unwrap_or(480.0);
        window
            .set_size(tauri::LogicalSize::new(width, h))
            .map_err(|e| e.to_string())?;
    } else if let Some((_, window)) = app.webview_windows().into_iter().next() {
        let scale = window.scale_factor().unwrap_or(1.0);
        let h = window.inner_size().map(|s| (s.height as f64) / scale).unwrap_or(480.0);
        window
            .set_size(tauri::LogicalSize::new(width, h))
            .map_err(|e| e.to_string())?;
    }
    Ok(())
}

#[tauri::command]
fn pick_folder() -> Result<Option<String>, String> {
    let folder = rfd::FileDialog::new()
        .set_title("Select Server Library Location")
        .pick_folder();

    match folder {
        Some(dir) => Ok(Some(dir.to_string_lossy().to_string())),
        None => Ok(None),
    }
}

#[tauri::command]
fn pick_file() -> Result<Option<String>, String> {
    let file = rfd::FileDialog::new()
        .set_title("Select Text Editor Executable")
        .add_filter("Executable (*.exe)", &["exe"])
        .pick_file();

    match file {
        Some(f) => Ok(Some(f.to_string_lossy().to_string())),
        None => Ok(None),
    }
}

#[tauri::command]
fn open_folder(path: String) -> Result<(), String> {
    let trimmed = path.trim();
    if trimmed.is_empty() {
        return Err("Folder path is empty.".to_string());
    }
    let target = std::path::PathBuf::from(trimmed);
    if !target.is_dir() {
        return Err(format!("Folder does not exist: {}", target.display()));
    }

    #[cfg(target_os = "windows")]
    {
        let normalized = target.to_string_lossy().replace('/', "\\");
        std::process::Command::new("explorer")
            .arg(&normalized)
            .spawn()
            .map_err(|e| format!("Failed to open folder '{}': {}", normalized, e))?;
    }
    #[cfg(not(target_os = "windows"))]
    {
        tauri_plugin_opener::open_path(&target, None::<&str>)
            .map_err(|e| format!("Failed to open folder '{}': {}", target.display(), e))?;
    }

    Ok(())
}

#[tauri::command]
fn open_app_logs() -> Result<(), String> {
    let logs_dir = logger::get_logs_dir()?;
    std::fs::create_dir_all(&logs_dir)
        .map_err(|e| format!("Failed to create logs folder: {}", e))?;
    open_folder(logs_dir.to_string_lossy().into_owned())
}

#[tauri::command]
fn append_app_log(message: String) {
    logger::append(message);
}

#[tauri::command]
fn open_file(path: String, editor: Option<String>) {
    let custom_editor = editor
        .as_deref()
        .map(str::trim)
        .filter(|s| !s.is_empty());

    if let Some(ed) = custom_editor {
        if std::process::Command::new(ed).arg(&path).spawn().is_ok() {
            return;
        }
    }

    #[cfg(target_os = "windows")]
    {
        if tauri_plugin_opener::open_path(&path, None::<&str>).is_err() {
            let _ = std::process::Command::new("notepad")
                .arg(&path)
                .spawn();
        }
    }
    #[cfg(not(target_os = "windows"))]
    {
        let _ = tauri_plugin_opener::open_path(&path, None::<&str>);
    }
}

#[tauri::command]
fn open_url(url: String) {
    let _ = tauri_plugin_opener::open_url(&url, None::<&str>);
}

#[tauri::command]
async fn check_online_server_catalog(force: Option<bool>) -> catalog::CatalogSyncResultDto {
    catalog::check_online_catalog(force.unwrap_or(false)).await
}

#[tauri::command]
async fn sync_online_server_catalog(
    app: tauri::AppHandle,
    force: Option<bool>,
) -> catalog::CatalogSyncResultDto {
    catalog::sync_online_catalog(Some(&app), force.unwrap_or(false)).await
}

#[tauri::command]
async fn load_servers(library_path: String) -> Result<servers::LibraryScanResult, String> {
    tauri::async_runtime::spawn_blocking(move || servers::load_servers_with_library(&library_path))
        .await
        .map_err(|e| format!("Failed to scan library: {}", e))
}

fn log_err<T>(context: &str, res: Result<T, String>) -> Result<T, String> {
    if let Err(ref err) = res {
        logger::append(format!("ERROR [{}]: {}", context, err));
    }
    res
}

#[tauri::command]
fn ensure_library_location(library_path: String) -> Result<String, String> {
    log_err(
        "ensure_library_location",
        servers::ensure_library_directories(&library_path),
    )
}

#[tauri::command]
async fn mock_install_server(
    library_path: String,
    codename: String,
    instance_name: String,
) -> Result<String, String> {
    let res = tauri::async_runtime::spawn_blocking(move || {
        servers::mock_install_instance(&library_path, &codename, &instance_name)
    })
    .await
    .map_err(|e| format!("Failed to run mock install: {}", e))?;
    log_err("mock_install_server", res)
}

#[tauri::command]
async fn delete_server(
    library_path: String,
    codename: String,
    instance_name: String,
) -> Result<bool, String> {
    let res = tauri::async_runtime::spawn_blocking(move || {
        servers::delete_server_instance_cancellable(&library_path, &codename, &instance_name)
    })
    .await
    .map_err(|e| format!("Failed to delete server: {}", e))?;
    log_err("delete_server", res)
}

#[tauri::command]
fn cancel_delete_task() {
    servers::cancel_delete_task();
}

#[tauri::command]
fn verify_server_package(source_file: String) -> signing::PackageTrustDto {
    actions::verify_server_package_by_source(&source_file)
}

#[tauri::command]
async fn execute_simple_action(
    app: tauri::AppHandle,
    library_path: String,
    codename: String,
    folder_name: String,
    source_file: String,
    instance_name: String,
    server_id: String,
    action_id: String,
    actions_list: Vec<servers::SimpleActionDto>,
    properties: std::collections::HashMap<String, String>,
    allow_untrusted: Option<bool>,
) -> Result<actions::ActionExecResultDto, String> {
    let res = actions::execute_simple_action(
        Some(&app),
        &library_path,
        &codename,
        &folder_name,
        &source_file,
        &instance_name,
        &server_id,
        &action_id,
        &actions_list,
        &properties,
        allow_untrusted.unwrap_or(false),
    )
    .await;
    log_err("execute_simple_action", res)
}

#[tauri::command]
async fn execute_funcs_method(
    app: tauri::AppHandle,
    library_path: String,
    codename: String,
    folder_name: String,
    source_file: String,
    instance_name: String,
    server_id: String,
    method_name: String,
    optional: bool,
    properties: std::collections::HashMap<String, String>,
    allow_untrusted: Option<bool>,
) -> Result<actions::ActionExecResultDto, String> {
    let res = actions::execute_funcs_method(
        Some(&app),
        &library_path,
        &codename,
        &folder_name,
        &source_file,
        &instance_name,
        &server_id,
        &method_name,
        optional,
        &properties,
        allow_untrusted.unwrap_or(false),
    )
    .await;
    log_err("execute_funcs_method", res)
}

#[tauri::command]
fn get_available_start_options(
    library_path: String,
    codename: String,
    instance_name: String,
    options: Vec<servers::StartOptionDto>,
) -> Result<Vec<servers::AvailableStartOption>, String> {
    log_err(
        "get_available_start_options",
        servers::get_available_start_options(&library_path, &codename, &instance_name, &options),
    )
}

#[tauri::command]
fn launch_server_option(
    app: tauri::AppHandle,
    server_id: String,
    instance_name: String,
    file_path: String,
    args: String,
) -> Result<u32, String> {
    log_err(
        "launch_server_option",
        servers::launch_server_option(&app, &server_id, &instance_name, &file_path, &args),
    )
}

#[tauri::command]
fn stop_server_instance(
    app: tauri::AppHandle,
    server_id: String,
    instance_name: String,
) -> Result<bool, String> {
    log_err(
        "stop_server_instance",
        servers::stop_server_instance(&app, &server_id, &instance_name),
    )
}

#[tauri::command]
fn get_running_instances() -> Vec<servers::RunningInstanceDto> {
    servers::get_running_instances()
}

#[tauri::command]
async fn download_steamcmd(app: tauri::AppHandle, library_path: String) -> Result<String, String> {
    let res = steamcmd::ensure_steamcmd_windows(&app, &library_path, "")
        .await
        .map(|exe_path| exe_path.to_string_lossy().into_owned());
    log_err("download_steamcmd", res)
}

#[tauri::command]
async fn install_or_update_steam_server(
    app: tauri::AppHandle,
    library_path: String,
    codename: String,
    instance_name: String,
    steam_app_id: String,
    server_id: String,
    minimized: bool,
    validate: bool,
) -> Result<steamcmd::SteamCmdRunResult, String> {
    let res = steamcmd::run_steamcmd_app_update(
        app,
        library_path,
        codename,
        instance_name,
        steam_app_id,
        server_id,
        minimized,
        validate,
    )
    .await;
    log_err("install_or_update_steam_server", res)
}

#[tauri::command]
async fn install_or_update_non_steam_server(
    app: tauri::AppHandle,
    library_path: String,
    codename: String,
    instance_name: String,
    server_id: String,
    server_name: String,
    download_link: String,
    are_files_local: bool,
    auto_extract_files: bool,
    source_file: String,
) -> Result<steamcmd::SteamCmdRunResult, String> {
    let res = non_steam::run_non_steam_install_or_update(
        app,
        library_path,
        codename,
        instance_name,
        server_id,
        server_name,
        download_link,
        are_files_local,
        auto_extract_files,
        source_file,
    )
    .await;
    log_err("install_or_update_non_steam_server", res)
}

#[tauri::command]
fn cancel_steamcmd(app: tauri::AppHandle, force: bool) -> Result<bool, String> {
    log_err("cancel_steamcmd", steamcmd::cancel_steamcmd_task(&app, force))
}

#[tauri::command]
fn mark_server_incomplete(
    library_path: String,
    codename: String,
    instance_name: String,
) -> Result<(), String> {
    let res = (|| {
        let (server_dir, _) =
            servers::resolve_instance_dir(&library_path, &codename, &instance_name)?;
        servers::mark_instance_incomplete(&server_dir)
    })();
    log_err("mark_server_incomplete", res)
}

#[tauri::command]
fn get_app_version() -> String {
    env!("CARGO_PKG_VERSION").to_string()
}

#[derive(serde::Serialize)]
#[serde(rename_all = "camelCase")]
struct UpdateCheckResult {
    update_available: bool,
    current_version: String,
    latest_version: String,
    release_url: String,
}

fn parse_version_tuple(raw: &str) -> Option<(u64, u64, u64, u64)> {
    let trimmed = raw.trim().trim_start_matches(['v', 'V']);
    let core = trimmed.split(['-', '+']).next().unwrap_or("");
    if core.is_empty() {
        return None;
    }

    let mut parts = [0u64; 4];
    let mut count = 0usize;
    for segment in core.split('.') {
        if count >= 4 {
            break;
        }
        parts[count] = segment.parse::<u64>().ok()?;
        count += 1;
    }

    if count == 0 {
        None
    } else {
        Some((parts[0], parts[1], parts[2], parts[3]))
    }
}

#[tauri::command]
async fn check_for_updates() -> Result<UpdateCheckResult, String> {
    const LATEST_RELEASE_URL: &str =
        "https://github.com/Zeromix9/Server-Creation-Tool/releases/latest";
    const RELEASES_PAGE_URL: &str =
        "https://github.com/Zeromix9/Server-Creation-Tool/releases";

    let current_version = env!("CARGO_PKG_VERSION").to_string();

    let client = reqwest::Client::builder()
        .redirect(reqwest::redirect::Policy::none())
        .timeout(std::time::Duration::from_secs(8))
        .user_agent(concat!("Server-Creation-Tool/", env!("CARGO_PKG_VERSION")))
        .build()
        .map_err(|e| format!("Failed to initialize HTTP client: {}", e))?;

    let response = client
        .head(LATEST_RELEASE_URL)
        .send()
        .await
        .map_err(|e| format!("Failed to reach GitHub: {}", e))?;

    let location = response
        .headers()
        .get(reqwest::header::LOCATION)
        .and_then(|v| v.to_str().ok());

    let Some(loc) = location else {
        return Err(format!("Unexpected GitHub response status: {}", response.status()));
    };

    let Some((_, tag)) = loc.split_once("/releases/tag/") else {
        return Ok(UpdateCheckResult {
            update_available: false,
            current_version: current_version.clone(),
            latest_version: current_version,
            release_url: RELEASES_PAGE_URL.to_string(),
        });
    };

    let latest_version = tag.trim().trim_end_matches('/').to_string();
    let release_url = format!(
        "https://github.com/Zeromix9/Server-Creation-Tool/releases/tag/{}",
        latest_version
    );

    let update_available = match (
        parse_version_tuple(&latest_version),
        parse_version_tuple(&current_version),
    ) {
        (Some(latest), Some(current)) => latest > current,
        _ => false,
    };

    Ok(UpdateCheckResult {
        update_available,
        current_version,
        latest_version,
        release_url,
    })
}

#[tauri::command]
fn pick_backup_save_path(default_file_name: String) -> Result<Option<String>, String> {
    let file_name = if default_file_name.trim().is_empty() {
        "server_backup.zip".to_string()
    } else {
        default_file_name.trim().to_string()
    };
    let file = rfd::FileDialog::new()
        .set_title("Save Server Backup")
        .set_file_name(&file_name)
        .add_filter("ZIP Archive (*.zip)", &["zip"])
        .save_file();

    Ok(file.map(|f| f.to_string_lossy().to_string()))
}

#[tauri::command]
fn pick_backup_open_path() -> Result<Option<String>, String> {
    let file = rfd::FileDialog::new()
        .set_title("Select Backup Archive")
        .add_filter("ZIP Archive (*.zip)", &["zip"])
        .pick_file();

    Ok(file.map(|f| f.to_string_lossy().to_string()))
}

#[tauri::command]
async fn backup_server(
    app: tauri::AppHandle,
    library_path: String,
    codename: String,
    instance_name: String,
    server_id: String,
    server_name: String,
    destination_path: Option<String>,
) -> Result<backups::BackupCreatedDto, String> {
    let res = backups::run_backup_task(
        &app,
        library_path,
        codename,
        instance_name,
        server_id,
        server_name,
        destination_path,
    )
    .await;
    log_err("backup_server", res)
}

#[tauri::command]
fn cancel_backup_task() {
    backups::cancel_backup_task();
}

#[tauri::command]
fn cancel_restore_task() {
    backups::cancel_restore_task();
}

#[tauri::command]
fn list_server_backups(
    library_path: String,
    codename: String,
    instance_name: String,
) -> Result<Vec<backups::BackupEntryDto>, String> {
    log_err(
        "list_server_backups",
        backups::list_server_backups(&library_path, &codename, &instance_name),
    )
}

#[tauri::command]
fn delete_server_backup(
    library_path: String,
    codename: String,
    instance_name: String,
    backup_path: String,
) -> Result<(), String> {
    log_err(
        "delete_server_backup",
        backups::delete_server_backup(&library_path, &codename, &instance_name, &backup_path),
    )
}

#[tauri::command]
async fn restore_server_backup(
    app: tauri::AppHandle,
    library_path: String,
    codename: String,
    instance_name: String,
    server_id: String,
    server_name: String,
    backup_path: String,
) -> Result<backups::RestoreResultDto, String> {
    let res = backups::run_restore_task(
        &app,
        library_path,
        codename,
        instance_name,
        server_id,
        server_name,
        backup_path,
    )
    .await;
    log_err("restore_server_backup", res)
}

#[tauri::command]
fn load_instance_settings(
    library_path: String,
    codename: String,
    instance_name: String,
) -> Result<Option<String>, String> {
    log_err(
        "load_instance_settings",
        servers::load_instance_settings(&library_path, &codename, &instance_name),
    )
}

#[tauri::command]
fn save_instance_settings(
    library_path: String,
    codename: String,
    instance_name: String,
    json: String,
) -> Result<(), String> {
    log_err(
        "save_instance_settings",
        servers::save_instance_settings(&library_path, &codename, &instance_name, &json),
    )
}

#[tauri::command]
fn rename_server_instance(
    library_path: String,
    codename: String,
    old_instance_name: String,
    new_instance_name: String,
) -> Result<String, String> {
    log_err(
        "rename_server_instance",
        servers::rename_server_instance(
            &library_path,
            &codename,
            &old_instance_name,
            &new_instance_name,
        ),
    )
}

fn show_main_window(app: &tauri::AppHandle) {
    if let Some(window) = app.get_webview_window("main") {
        let _ = window.unminimize();
        let _ = window.show();
        let _ = window.set_focus();
    }
}

pub fn run() {
    tauri::Builder::default()
        .plugin(tauri_plugin_opener::init())
        .setup(|app| {
            let prev_hook = std::panic::take_hook();
            std::panic::set_hook(Box::new(move |info| {
                logger::append(format!("PANIC: {}", info));
                prev_hook(info);
            }));
            if let Ok(logs_dir) = logger::get_logs_dir() {
                let _ = std::fs::create_dir_all(logs_dir);
            }
            logger::append(format!(
                "Application started (v{})",
                env!("CARGO_PKG_VERSION")
            ));

            let show_item = MenuItemBuilder::with_id("tray_show", "Show Server Creation Tool")
                .build(app)?;
            let exit_item = MenuItemBuilder::with_id("tray_exit", "Exit")
                .build(app)?;
            let tray_menu = MenuBuilder::new(app)
                .item(&show_item)
                .separator()
                .item(&exit_item)
                .build()?;

            let mut tray_builder = TrayIconBuilder::with_id("sct_tray")
                .menu(&tray_menu)
                .tooltip("Server Creation Tool 6.0")
                .show_menu_on_left_click(false)
                .on_tray_icon_event(|tray, event| {
                    if let TrayIconEvent::Click {
                        button: MouseButton::Left,
                        button_state: MouseButtonState::Up,
                        ..
                    }
                    | TrayIconEvent::DoubleClick {
                        button: MouseButton::Left,
                        ..
                    } = event
                    {
                        show_main_window(tray.app_handle());
                    }
                });

            if let Some(icon) = app.default_window_icon().cloned() {
                tray_builder = tray_builder.icon(icon);
            }
            let _ = tray_builder.build(app)?;

            Ok(())
        })
        .on_menu_event(|app, event| {
            let id = event.id().as_ref();
            if id == "tray_show" {
                show_main_window(app);
                return;
            }
            if id == "tray_exit" {
                app.exit(0);
                return;
            }
            #[cfg(target_os = "windows")]
            {
                let _ = std::process::Command::new("notepad")
                    .arg(id)
                    .spawn();
            }
            #[cfg(not(target_os = "windows"))]
            {
                let _ = tauri_plugin_opener::open_path(id, None::<&str>);
            }
        })
        .on_window_event(|window, event| {
            if window.label() == "main" {
                match event {
                    tauri::WindowEvent::CloseRequested { api, .. } => {
                        if CLOSE_TO_TRAY.load(Ordering::Relaxed) {
                            api.prevent_close();
                            let _ = window.hide();
                        } else {
                            window.app_handle().exit(0);
                        }
                    }
                    tauri::WindowEvent::Destroyed => {
                        window.app_handle().exit(0);
                    }
                    _ => {}
                }
            }
        })
        .invoke_handler(tauri::generate_handler![
            start_drag,
            window_minimize,
            window_close,
            set_close_to_tray,
            is_release_mode,
            load_app_settings,
            save_app_settings,
            set_run_on_startup,
            open_about_window,
            open_faq_window,
            window_set_width,
            start_steamcmd,
            clear_steamcmd_cache,
            pick_folder,
            pick_file,
            pick_backup_save_path,
            pick_backup_open_path,
            backup_server,
            cancel_backup_task,
            cancel_restore_task,
            list_server_backups,
            delete_server_backup,
            restore_server_backup,
            open_folder,
            open_app_logs,
            append_app_log,
            open_file,
            open_url,
            check_online_server_catalog,
            sync_online_server_catalog,
            load_servers,
            ensure_library_location,
            mock_install_server,
            delete_server,
            cancel_delete_task,
            rename_server_instance,
            load_instance_settings,
            save_instance_settings,
            verify_server_package,
            execute_simple_action,
            execute_funcs_method,
            get_available_start_options,
            launch_server_option,
            stop_server_instance,
            get_running_instances,
            download_steamcmd,
            install_or_update_steam_server,
            install_or_update_non_steam_server,
            cancel_steamcmd,
            mark_server_incomplete,
            get_app_version,
            check_for_updates,
            show_settings_context_menu
        ])
        .run(tauri::generate_context!())
        .expect("error while running Server Creation Tool");
}

#[cfg(test)]
mod tests {
    use super::parse_version_tuple;

    #[test]
    fn normalizes_short_and_prefixed_versions() {
        assert_eq!(parse_version_tuple("v5.2"), parse_version_tuple("5.2.0"));
        assert!(parse_version_tuple("v5.3") > parse_version_tuple("5.2.0"));
        assert!(parse_version_tuple("v5.10.0") > parse_version_tuple("v5.2"));
        assert_eq!(parse_version_tuple("bad"), None);
    }
}
