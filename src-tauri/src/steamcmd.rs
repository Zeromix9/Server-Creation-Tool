use serde::Serialize;
use std::fs::{self, File};
use std::io::{BufReader, Read, Write};
use std::path::{Path, PathBuf};
use std::process::{ChildStdin, Command, Stdio};
use std::sync::atomic::{AtomicBool, AtomicU64, Ordering};
use std::sync::{Arc, Mutex, OnceLock};
use tauri::{AppHandle, Emitter};

use crate::servers::{
    clear_instance_incomplete, ensure_library_directories, mark_instance_incomplete,
    resolve_instance_dir,
};

#[cfg(target_os = "windows")]
const STEAMCMD_WINDOWS_URL: &str = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";

#[cfg(target_os = "windows")]
const CREATE_NO_WINDOW: u32 = 0x0800_0000;

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum CancelState {
    None,
    Cancelling,
    ForceStopped,
}

pub struct ActiveSteamTask {
    pub task_id: u64,
    pub server_id: String,
    pub task_label: String,
    pub server_dir: Option<PathBuf>,
    pub pid: Option<u32>,
    pub stdin: Option<ChildStdin>,
    pub cancel_state: CancelState,
}

static NEXT_TASK_ID: AtomicU64 = AtomicU64::new(1);
static ACTIVE_TASK: OnceLock<Mutex<Option<ActiveSteamTask>>> = OnceLock::new();

fn active_task_slot() -> &'static Mutex<Option<ActiveSteamTask>> {
    ACTIVE_TASK.get_or_init(|| Mutex::new(None))
}

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct ServerLogPayload {
    pub server_id: String,
    pub line: String,
}

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct TaskStatePayload {
    pub active: bool,
    pub server_id: String,
    pub message: String,
    pub known_progress: bool,
    pub progress: f64,
    pub extra_info: String,
    pub cancelling: bool,
}

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct SteamCmdRunResult {
    pub directory: String,
    pub cancelled: bool,
}

fn emit_log(app: &AppHandle, server_id: &str, line: impl Into<String>) {
    if server_id.is_empty() {
        return;
    }
    let _ = app.emit(
        "sct-server-log",
        ServerLogPayload {
            server_id: server_id.to_string(),
            line: line.into(),
        },
    );
}

fn emit_task_state(
    app: &AppHandle,
    active: bool,
    server_id: &str,
    message: impl Into<String>,
    known_progress: bool,
    progress: f64,
    extra_info: impl Into<String>,
    cancelling: bool,
) {
    let _ = app.emit(
        "sct-task-state",
        TaskStatePayload {
            active,
            server_id: server_id.to_string(),
            message: message.into(),
            known_progress,
            progress,
            extra_info: extra_info.into(),
            cancelling,
        },
    );
}

struct TaskGuard {
    app: AppHandle,
    task_id: u64,
    server_id: String,
}

impl Drop for TaskGuard {
    fn drop(&mut self) {
        if let Ok(mut guard) = active_task_slot().lock() {
            if guard.as_ref().map(|t| t.task_id) == Some(self.task_id) {
                *guard = None;
            }
        }
        emit_task_state(
            &self.app,
            false,
            &self.server_id,
            "",
            false,
            0.0,
            "",
            false,
        );
    }
}

fn begin_steam_task(
    app: &AppHandle,
    server_id: &str,
    task_label: &str,
    server_dir: Option<PathBuf>,
) -> Result<(u64, TaskGuard), String> {
    let mut guard = active_task_slot()
        .lock()
        .map_err(|_| "Failed to acquire SteamCMD task lock.".to_string())?;

    if guard.is_some() {
        return Err("Another SteamCMD operation is already in progress.".to_string());
    }

    let task_id = NEXT_TASK_ID.fetch_add(1, Ordering::SeqCst);
    *guard = Some(ActiveSteamTask {
        task_id,
        server_id: server_id.to_string(),
        task_label: task_label.to_string(),
        server_dir,
        pid: None,
        stdin: None,
        cancel_state: CancelState::None,
    });
    drop(guard);

    emit_task_state(
        app,
        true,
        server_id,
        task_label,
        false,
        0.0,
        "",
        false,
    );

    Ok((
        task_id,
        TaskGuard {
            app: app.clone(),
            task_id,
            server_id: server_id.to_string(),
        },
    ))
}

fn is_task_cancelled(task_id: u64) -> bool {
    if let Ok(guard) = active_task_slot().lock() {
        if let Some(task) = guard.as_ref() {
            if task.task_id == task_id {
                return task.cancel_state != CancelState::None;
            }
        }
    }
    false
}

fn update_task_label(app: &AppHandle, task_id: u64, new_label: &str, known_progress: bool, progress: f64, extra_info: &str) {
    let mut sid = String::new();
    let mut cancelling = false;
    if let Ok(mut guard) = active_task_slot().lock() {
        if let Some(task) = guard.as_mut() {
            if task.task_id == task_id {
                if task.cancel_state == CancelState::None {
                    task.task_label = new_label.to_string();
                }
                sid = task.server_id.clone();
                cancelling = task.cancel_state != CancelState::None;
            } else {
                return;
            }
        } else {
            return;
        }
    }
    let msg = if cancelling {
        "Cancelling SteamCMD..."
    } else {
        new_label
    };
    emit_task_state(
        app,
        true,
        &sid,
        msg,
        if cancelling { false } else { known_progress },
        progress,
        if cancelling { "" } else { extra_info },
        cancelling,
    );
}

#[cfg(target_os = "windows")]
fn run_taskkill(pid: u32, force: bool) {
    use std::os::windows::process::CommandExt;
    let mut cmd = Command::new("taskkill");
    cmd.arg("/PID").arg(pid.to_string()).arg("/T");
    if force {
        cmd.arg("/F");
    }
    cmd.creation_flags(CREATE_NO_WINDOW);
    let _ = cmd.stdout(Stdio::null()).stderr(Stdio::null()).status();
}

pub fn cancel_steamcmd_task(app: &AppHandle, force: bool) -> Result<bool, String> {
    let (task_id, server_id, pid_opt, mut stdin_opt, server_dir_opt) = {
        let mut guard = active_task_slot()
            .lock()
            .map_err(|_| "Failed to lock active SteamCMD task.".to_string())?;
        let Some(task) = guard.as_mut() else {
            return Ok(false);
        };

        task.cancel_state = if force {
            CancelState::ForceStopped
        } else if task.cancel_state == CancelState::None {
            CancelState::Cancelling
        } else {
            task.cancel_state
        };

        (
            task.task_id,
            task.server_id.clone(),
            task.pid,
            task.stdin.take(),
            task.server_dir.clone(),
        )
    };

    if let Some(ref dir) = server_dir_opt {
        let _ = mark_instance_incomplete(dir);
    }

    emit_task_state(
        app,
        true,
        &server_id,
        if force {
            "Force stopping SteamCMD..."
        } else {
            "Cancelling SteamCMD..."
        },
        false,
        0.0,
        "",
        true,
    );

    if force {
        emit_log(app, &server_id, "Force stopping SteamCMD process...");
        if let Some(pid) = pid_opt {
            #[cfg(target_os = "windows")]
            run_taskkill(pid, true);
        }
    } else {
        emit_log(app, &server_id, "Cancelling SteamCMD operation...");
        if let Some(ref mut stdin) = stdin_opt {
            let _ = stdin.write_all(b"quit\r\n");
            let _ = stdin.flush();
        }
        drop(stdin_opt);

        if let Some(pid) = pid_opt {
            #[cfg(target_os = "windows")]
            {
                run_taskkill(pid, false);

                // If SteamCMD is running without a window or busy inside +app_update,
                // WM_CLOSE from non-forced taskkill may not close it; ensure it closes after a brief grace window.
                std::thread::spawn(move || {
                    for _ in 0..30 {
                        std::thread::sleep(std::time::Duration::from_millis(100));
                        let still_same = active_task_slot()
                            .lock()
                            .ok()
                            .and_then(|g| g.as_ref().map(|t| t.task_id == task_id))
                            .unwrap_or(false);
                        if !still_same {
                            return;
                        }
                    }
                    let should_kill = active_task_slot()
                        .lock()
                        .ok()
                        .and_then(|g| {
                            g.as_ref().map(|t| {
                                t.task_id == task_id && t.cancel_state != CancelState::None
                            })
                        })
                        .unwrap_or(false);
                    if should_kill {
                        run_taskkill(pid, true);
                    }
                });
            }
        }
    }

    Ok(true)
}

pub fn steamcmd_exe_path(library_path: &Path) -> PathBuf {
    library_path.join("steamcmd").join("steamcmd.exe")
}

pub fn build_app_update_args(
    install_dir: &Path,
    steam_app_id: &str,
    validate: bool,
) -> Vec<String> {
    let mut args = vec![
        "+force_install_dir".to_string(),
        install_dir.to_string_lossy().into_owned(),
        "+login".to_string(),
        "anonymous".to_string(),
        "+app_update".to_string(),
        steam_app_id.trim().to_string(),
    ];
    if validate {
        args.push("validate".to_string());
    }
    args.push("+quit".to_string());
    args
}

pub fn parse_steamcmd_progress(line: &str) -> Option<f64> {
    let lower = line.to_lowercase();
    let idx = lower.find("progress:")?;
    let after = line[idx + "progress:".len()..].trim_start();
    let token: String = after
        .chars()
        .take_while(|c| c.is_ascii_digit() || *c == '.')
        .collect();
    if token.is_empty() {
        return None;
    }
    let val = token.parse::<f64>().ok()?;
    Some(val.clamp(0.0, 100.0))
}

async fn ensure_steamcmd_windows_inner(
    app: &AppHandle,
    library_path: &str,
    server_id: &str,
    task_id: u64,
) -> Result<PathBuf, String> {
    let resolved_lib = ensure_library_directories(library_path)?;
    let lib_dir = PathBuf::from(&resolved_lib);
    let exe_path = steamcmd_exe_path(&lib_dir);

    if exe_path.is_file() {
        return Ok(exe_path);
    }

    #[cfg(not(target_os = "windows"))]
    {
        let _ = (app, server_id, task_id);
        return Err("SteamCMD automatic setup is currently implemented for Windows only.".to_string());
    }

    #[cfg(target_os = "windows")]
    {
        emit_log(app, server_id, "SteamCMD not found. Downloading steamcmd.zip...");
        update_task_label(app, task_id, "Downloading SteamCMD...", true, 0.0, "0%");

        let client = reqwest::Client::builder()
            .timeout(std::time::Duration::from_secs(120))
            .user_agent(concat!("Server-Creation-Tool/", env!("CARGO_PKG_VERSION")))
            .build()
            .map_err(|e| format!("Failed to initialize HTTP client: {}", e))?;

        let mut response = client
            .get(STEAMCMD_WINDOWS_URL)
            .send()
            .await
            .map_err(|e| format!("Failed to download SteamCMD: {}", e))?;

        if !response.status().is_success() {
            return Err(format!(
                "SteamCMD download failed with HTTP status {}",
                response.status()
            ));
        }

        let total_len = response.content_length().unwrap_or(0);
        let mut downloaded: u64 = 0;
        let mut bytes = Vec::new();

        while let Some(chunk) = response
            .chunk()
            .await
            .map_err(|e| format!("Failed to read SteamCMD archive stream: {}", e))?
        {
            if is_task_cancelled(task_id) {
                return Err("SteamCMD download cancelled.".to_string());
            }
            downloaded += chunk.len() as u64;
            bytes.extend_from_slice(&chunk);

            if total_len > 0 {
                let pct = ((downloaded as f64 / total_len as f64) * 100.0).clamp(0.0, 100.0);
                let extra = format!("{:.0}%", pct);
                update_task_label(app, task_id, "Downloading SteamCMD...", true, pct, &extra);
            } else {
                update_task_label(app, task_id, "Downloading SteamCMD...", false, 0.0, "");
            }
        }

        if is_task_cancelled(task_id) {
            return Err("SteamCMD download cancelled.".to_string());
        }

        let steamcmd_dir = lib_dir.join("steamcmd");
        let zip_path = steamcmd_dir.join("steamcmd.zip");
        fs::write(&zip_path, &bytes)
            .map_err(|e| format!("Failed to write '{}': {}", zip_path.display(), e))?;

        emit_log(app, server_id, "Extracting SteamCMD...");
        update_task_label(app, task_id, "Extracting SteamCMD...", false, 0.0, "");

        let extract_dir = steamcmd_dir.clone();
        let zip_file_path = zip_path.clone();
        tauri::async_runtime::spawn_blocking(move || -> Result<(), String> {
            let file = File::open(&zip_file_path)
                .map_err(|e| format!("Failed to open '{}': {}", zip_file_path.display(), e))?;
            let mut archive = zip::ZipArchive::new(BufReader::new(file))
                .map_err(|e| format!("Invalid steamcmd.zip archive: {}", e))?;

            for i in 0..archive.len() {
                let mut entry = archive.by_index(i).map_err(|e| e.to_string())?;
                let Some(rel_path) = entry.enclosed_name().map(|p| p.to_path_buf()) else {
                    continue;
                };
                let out_path = extract_dir.join(rel_path);
                if entry.is_dir() {
                    fs::create_dir_all(&out_path).map_err(|e| e.to_string())?;
                } else {
                    if let Some(parent) = out_path.parent() {
                        fs::create_dir_all(parent).map_err(|e| e.to_string())?;
                    }
                    let mut out_file = File::create(&out_path).map_err(|e| e.to_string())?;
                    std::io::copy(&mut entry, &mut out_file).map_err(|e| e.to_string())?;
                }
            }

            let _ = fs::remove_file(&zip_file_path);
            Ok(())
        })
        .await
        .map_err(|e| format!("Failed to extract SteamCMD: {}", e))??;

        if !exe_path.is_file() {
            return Err("steamcmd.exe was not found after extraction.".to_string());
        }

        emit_log(app, server_id, "SteamCMD is ready.");
        Ok(exe_path)
    }
}

pub async fn ensure_steamcmd_windows(
    app: &AppHandle,
    library_path: &str,
    server_id: &str,
) -> Result<PathBuf, String> {
    let (task_id, _guard) = begin_steam_task(app, server_id, "Downloading SteamCMD...", None)?;
    ensure_steamcmd_windows_inner(app, library_path, server_id, task_id).await
}

fn stream_reader_lines<R: Read>(
    app: AppHandle,
    server_id: String,
    task_id: u64,
    task_label: String,
    saw_error: Arc<AtomicBool>,
    mut reader: R,
) {
    let mut buf = [0u8; 2048];
    let mut line_buf = Vec::new();

    let handle_line = |raw: &[u8]| {
        let text = String::from_utf8_lossy(raw).trim().to_string();
        if text.is_empty() {
            return;
        }
        let lower = text.to_lowercase();
        if lower.starts_with("error! app") || lower.contains("failed (") {
            saw_error.store(true, Ordering::SeqCst);
        }
        if let Some(pct) = parse_steamcmd_progress(&text) {
            let extra = format!("{:.0}%", pct);
            update_task_label(&app, task_id, &task_label, true, pct, &extra);
        }
        emit_log(&app, &server_id, text);
    };

    loop {
        match reader.read(&mut buf) {
            Ok(0) => break,
            Ok(n) => {
                for &b in &buf[..n] {
                    if b == b'\n' || b == b'\r' {
                        if !line_buf.is_empty() {
                            handle_line(&line_buf);
                            line_buf.clear();
                        }
                    } else {
                        line_buf.push(b);
                    }
                }
            }
            Err(_) => break,
        }
    }

    if !line_buf.is_empty() {
        handle_line(&line_buf);
    }
}

pub async fn run_steamcmd_app_update(
    app: AppHandle,
    library_path: String,
    codename: String,
    instance_name: String,
    steam_app_id: String,
    server_id: String,
    minimized: bool,
    validate: bool,
) -> Result<SteamCmdRunResult, String> {
    let trimmed_app_id = steam_app_id.trim().to_string();
    if trimmed_app_id.is_empty() {
        return Err("No Steam App ID is configured for this server.".to_string());
    }

    let (server_dir, _) = resolve_instance_dir(&library_path, &codename, &instance_name)?;
    let is_existing = server_dir.is_dir()
        && fs::read_dir(&server_dir)
            .map(|mut e| e.next().is_some())
            .unwrap_or(false);

    let task_label = if is_existing {
        format!("Updating/Repairing {} server...", codename.to_uppercase())
    } else {
        format!("Installing {} server...", codename.to_uppercase())
    };

    let (task_id, _guard) = begin_steam_task(
        &app,
        &server_id,
        &task_label,
        Some(server_dir.clone()),
    )?;

    // Drop incomplete marker before starting so any interruption or cancellation is recorded on disk.
    mark_instance_incomplete(&server_dir)?;

    let steamcmd_exe = match ensure_steamcmd_windows_inner(&app, &library_path, &server_id, task_id).await {
        Ok(exe) => exe,
        Err(err) => {
            let _ = mark_instance_incomplete(&server_dir);
            if is_task_cancelled(task_id) {
                emit_log(&app, &server_id, "Installation/update was cancelled.");
                return Ok(SteamCmdRunResult {
                    directory: server_dir.to_string_lossy().into_owned(),
                    cancelled: true,
                });
            }
            return Err(err);
        }
    };

    if is_task_cancelled(task_id) {
        let _ = mark_instance_incomplete(&server_dir);
        emit_log(&app, &server_id, "Installation/update was cancelled.");
        return Ok(SteamCmdRunResult {
            directory: server_dir.to_string_lossy().into_owned(),
            cancelled: true,
        });
    }

    update_task_label(&app, task_id, &task_label, false, 0.0, "");

    let args = build_app_update_args(&server_dir, &trimmed_app_id, validate);
    emit_log(
        &app,
        &server_id,
        format!(
            "Running SteamCMD (App ID: {}) into '{}'...",
            trimmed_app_id,
            server_dir.display()
        ),
    );

    let steamcmd_dir = steamcmd_exe
        .parent()
        .map(|p| p.to_path_buf())
        .unwrap_or_else(|| PathBuf::from(&library_path));

    let server_dir_str = server_dir.to_string_lossy().into_owned();
    let app_clone = app.clone();
    let sid_clone = server_id.clone();
    let label_clone = task_label.clone();
    let saw_error = Arc::new(AtomicBool::new(false));
    let saw_error_clone = Arc::clone(&saw_error);

    let status_res = tauri::async_runtime::spawn_blocking(move || -> Result<std::process::ExitStatus, String> {
        let mut cmd = Command::new(&steamcmd_exe);
        cmd.args(&args).current_dir(&steamcmd_dir);

        if minimized {
            #[cfg(target_os = "windows")]
            {
                use std::os::windows::process::CommandExt;
                cmd.creation_flags(CREATE_NO_WINDOW);
            }
            cmd.stdin(Stdio::piped())
                .stdout(Stdio::piped())
                .stderr(Stdio::piped());

            let mut child = cmd
                .spawn()
                .map_err(|e| format!("Failed to launch SteamCMD: {}", e))?;

            let pid = child.id();
            let stdin = child.stdin.take();

            let already_cancelled = {
                if let Ok(mut guard) = active_task_slot().lock() {
                    if let Some(task) = guard.as_mut() {
                        if task.task_id == task_id {
                            task.pid = Some(pid);
                            task.stdin = stdin;
                            task.cancel_state != CancelState::None
                        } else {
                            true
                        }
                    } else {
                        true
                    }
                } else {
                    false
                }
            };

            if already_cancelled {
                #[cfg(target_os = "windows")]
                run_taskkill(pid, true);
                let _ = child.kill();
            }

            let stdout = child.stdout.take();
            let stderr = child.stderr.take();

            let t_out = stdout.map(|out| {
                let app_h = app_clone.clone();
                let sid = sid_clone.clone();
                let lbl = label_clone.clone();
                let err_flag = Arc::clone(&saw_error_clone);
                std::thread::spawn(move || stream_reader_lines(app_h, sid, task_id, lbl, err_flag, out))
            });

            let t_err = stderr.map(|err| {
                let app_h = app_clone.clone();
                let sid = sid_clone.clone();
                let lbl = label_clone.clone();
                let err_flag = Arc::clone(&saw_error_clone);
                std::thread::spawn(move || stream_reader_lines(app_h, sid, task_id, lbl, err_flag, err))
            });

            let exit_status = child
                .wait()
                .map_err(|e| format!("Failed waiting for SteamCMD: {}", e))?;

            if let Some(h) = t_out {
                let _ = h.join();
            }
            if let Some(h) = t_err {
                let _ = h.join();
            }

            Ok(exit_status)
        } else {
            let mut child = cmd
                .spawn()
                .map_err(|e| format!("Failed to launch SteamCMD: {}", e))?;
            let pid = child.id();
            let already_cancelled = {
                if let Ok(mut guard) = active_task_slot().lock() {
                    if let Some(task) = guard.as_mut() {
                        if task.task_id == task_id {
                            task.pid = Some(pid);
                            task.cancel_state != CancelState::None
                        } else {
                            true
                        }
                    } else {
                        true
                    }
                } else {
                    false
                }
            };
            if already_cancelled {
                #[cfg(target_os = "windows")]
                run_taskkill(pid, true);
                let _ = child.kill();
            }

            child
                .wait()
                .map_err(|e| format!("Failed waiting for SteamCMD: {}", e))
        }
    })
    .await
    .map_err(|e| format!("SteamCMD task failed: {}", e))?;

    let was_cancelled = is_task_cancelled(task_id);

    if was_cancelled {
        let _ = mark_instance_incomplete(&server_dir);
        emit_log(&app, &server_id, "SteamCMD installation/update was cancelled.");
        return Ok(SteamCmdRunResult {
            directory: server_dir_str,
            cancelled: true,
        });
    }

    let status = match status_res {
        Ok(s) => s,
        Err(err) => {
            let _ = mark_instance_incomplete(&server_dir);
            return Err(err);
        }
    };

    if !status.success() || saw_error.load(Ordering::SeqCst) {
        let _ = mark_instance_incomplete(&server_dir);
        let code = status
            .code()
            .map(|c| c.to_string())
            .unwrap_or_else(|| "unknown".to_string());
        let msg = if saw_error.load(Ordering::SeqCst) {
            format!("SteamCMD reported an error during installation (exit code {}).", code)
        } else {
            format!("SteamCMD exited with code {}.", code)
        };
        emit_log(&app, &server_id, &msg);
        return Err(msg);
    }

    clear_instance_incomplete(&server_dir);
    emit_log(&app, &server_id, "SteamCMD finished successfully.");
    Ok(SteamCmdRunResult {
        directory: server_dir_str,
        cancelled: false,
    })
}

#[cfg(test)]
mod tests {
    use super::{build_app_update_args, parse_steamcmd_progress, steamcmd_exe_path};
    use std::path::Path;

    #[test]
    fn builds_expected_steamcmd_arguments() {
        let dir = Path::new("C:\\Server_Library\\cs_ds");
        let args = build_app_update_args(dir, "90", true);
        assert_eq!(
            args,
            vec![
                "+force_install_dir",
                "C:\\Server_Library\\cs_ds",
                "+login",
                "anonymous",
                "+app_update",
                "90",
                "validate",
                "+quit"
            ]
        );
    }

    #[test]
    fn resolves_windows_steamcmd_exe_path() {
        let lib = Path::new("C:\\Server_Library");
        assert_eq!(
            steamcmd_exe_path(lib),
            Path::new("C:\\Server_Library\\steamcmd\\steamcmd.exe")
        );
    }

    #[test]
    fn parses_steamcmd_progress_lines() {
        let line = " Update state (0x61) downloading, progress: 42.75 (123456 / 288888)";
        assert_eq!(parse_steamcmd_progress(line), Some(42.75));
        assert_eq!(parse_steamcmd_progress("Connecting anonymously to Steam Public..."), None);
    }
}
