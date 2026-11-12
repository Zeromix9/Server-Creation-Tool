use tauri::Manager;
use tauri::menu::{ContextMenu, MenuBuilder, MenuItemBuilder};

mod servers;

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
async fn window_close(window: tauri::WebviewWindow) -> Result<(), String> {
    if window.label() == "main" {
        std::process::exit(0);
    } else {
        let _ = window.close();
    }
    Ok(())
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
            std::process::Command::new(&path)
                .spawn()
                .map_err(|e| e.to_string())?;
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

fn get_logs_dir() -> Result<std::path::PathBuf, String> {
    let exe_path = std::env::current_exe()
        .map_err(|e| format!("Failed to locate executable: {}", e))?;
    let exe_dir = exe_path
        .parent()
        .ok_or_else(|| "Failed to locate executable directory".to_string())?;
    Ok(exe_dir.join("sct_tool_logs"))
}

#[tauri::command]
fn open_app_logs() -> Result<(), String> {
    let logs_dir = get_logs_dir()?;
    std::fs::create_dir_all(&logs_dir)
        .map_err(|e| format!("Failed to create logs folder: {}", e))?;
    open_folder(logs_dir.to_string_lossy().into_owned())
}

#[tauri::command]
fn open_file(path: String) {
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
async fn load_servers(library_path: String) -> Result<servers::LibraryScanResult, String> {
    tauri::async_runtime::spawn_blocking(move || servers::load_servers_with_library(&library_path))
        .await
        .map_err(|e| format!("Failed to scan library: {}", e))
}

#[tauri::command]
fn ensure_library_location(library_path: String) -> Result<String, String> {
    servers::ensure_library_directories(&library_path)
}

#[tauri::command]
async fn mock_install_server(
    library_path: String,
    codename: String,
    instance_name: String,
) -> Result<String, String> {
    tauri::async_runtime::spawn_blocking(move || {
        servers::mock_install_instance(&library_path, &codename, &instance_name)
    })
    .await
    .map_err(|e| format!("Failed to run mock install: {}", e))?
}

#[tauri::command]
async fn delete_server(
    library_path: String,
    codename: String,
    instance_name: String,
) -> Result<bool, String> {
    tauri::async_runtime::spawn_blocking(move || {
        servers::delete_server_instance(&library_path, &codename, &instance_name)
    })
    .await
    .map_err(|e| format!("Failed to delete server: {}", e))?
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

pub fn run() {
    tauri::Builder::default()
        .plugin(tauri_plugin_opener::init())
        .setup(|_app| {
            if let Ok(logs_dir) = get_logs_dir() {
                let _ = std::fs::create_dir_all(logs_dir);
            }
            Ok(())
        })
        .on_menu_event(|_app, event| {
            let path = event.id().as_ref();
            #[cfg(target_os = "windows")]
            {
                let _ = std::process::Command::new("notepad")
                    .arg(path)
                    .spawn();
            }
            #[cfg(not(target_os = "windows"))]
            {
                let _ = tauri_plugin_opener::open_path(path, None::<&str>);
            }
        })
        .on_window_event(|window, event| {
            if window.label() == "main" {
                if let tauri::WindowEvent::CloseRequested { .. } | tauri::WindowEvent::Destroyed = event {
                    std::process::exit(0);
                }
            }
        })
        .invoke_handler(tauri::generate_handler![
            start_drag,
            window_minimize,
            window_close,
            open_about_window,
            open_faq_window,
            window_set_width,
            start_steamcmd,
            clear_steamcmd_cache,
            pick_folder,
            pick_file,
            open_folder,
            open_app_logs,
            open_file,
            open_url,
            load_servers,
            ensure_library_location,
            mock_install_server,
            delete_server,
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
