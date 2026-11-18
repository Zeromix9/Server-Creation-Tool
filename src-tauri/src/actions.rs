use mlua::{Function, Lua, MultiValue, Table, Value as LuaValue};
use reqwest::header::CONTENT_DISPOSITION;
use serde::{Deserialize, Serialize};
use std::collections::{HashMap, HashSet};
use std::fs::{self, File};
use std::io::{self, BufReader, Read};
use std::path::{Path, PathBuf};
use std::process::Command;
use std::sync::{Arc, Mutex};
use tauri::AppHandle;
use zip::ZipArchive;

use crate::servers::{
    find_sct_data_dir, resolve_instance_dir, SimpleActionDto,
};
use crate::signing::{verify_server_package, PackageTrustDto};
use crate::steamcmd::emit_log;

pub fn verify_server_package_by_source(source_file: &str) -> PackageTrustDto {
    match find_server_archive_path(source_file) {
        Some(arch) => verify_server_package(&arch),
        None => PackageTrustDto {
            is_official: false,
            status: "missing".to_string(),
            reason: format!("Server package '{}' was not found.", source_file),
            package_sha256: String::new(),
        },
    }
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct ActionExecResultDto {
    pub success: bool,
    pub message: String,
    pub logs: Vec<String>,
    pub updated_properties: HashMap<String, String>,
}

fn resolve_tokens(
    template: &str,
    server_dir: &Path,
    data_dir: &Path,
    codename: &str,
    instance_name: &str,
    properties: &HashMap<String, String>,
) -> String {
    if template.is_empty() {
        return String::new();
    }
    let server_str = server_dir.to_string_lossy();
    let data_str = data_dir.to_string_lossy();

    let mut result = replace_case_insensitive(template, "{SERVER_DIR}", &server_str);
    result = replace_case_insensitive(&result, "{INSTALL_DIR}", &server_str);
    result = replace_case_insensitive(&result, "{DATA_DIR}", &data_str);
    result = replace_case_insensitive(&result, "{CODENAME}", codename);
    result = replace_case_insensitive(&result, "{INSTANCE_NAME}", instance_name);

    for (k, v) in properties {
        let token = format!("{{{}}}", k);
        result = replace_case_insensitive(&result, &token, v);
    }
    result
}

fn replace_case_insensitive(input: &str, pattern: &str, replacement: &str) -> String {
    if pattern.is_empty() {
        return input.to_string();
    }
    let lower_input = input.to_lowercase();
    let lower_pat = pattern.to_lowercase();
    let mut out = String::with_capacity(input.len());
    let mut pos = 0;
    while let Some(idx) = lower_input[pos..].find(&lower_pat) {
        let abs = pos + idx;
        out.push_str(&input[pos..abs]);
        out.push_str(replacement);
        pos = abs + pattern.len();
    }
    out.push_str(&input[pos..]);
    out
}

fn resolve_relative_in_dir(base_dir: &Path, rel: &str) -> Result<PathBuf, String> {
    let trimmed = rel.trim().trim_start_matches(['/', '\\']);
    if trimmed.is_empty() || trimmed == "." {
        return Ok(base_dir.to_path_buf());
    }
    let candidate = Path::new(trimmed);
    if candidate.is_absolute() {
        return Err(format!("Absolute paths are not allowed: '{}'", rel));
    }
    let mut out = base_dir.to_path_buf();
    for part in trimmed.split(['/', '\\']) {
        let seg = part.trim();
        if seg.is_empty() || seg == "." {
            continue;
        }
        if seg == ".." {
            return Err(format!("Parent directory traversal ('..') is not allowed: '{}'", rel));
        }
        out.push(seg);
    }
    Ok(out)
}

fn split_args(input: &str) -> Vec<String> {
    let mut args = Vec::new();
    let mut cur = String::new();
    let mut in_quotes = false;
    let mut quote_char = '"';

    for ch in input.chars() {
        if in_quotes {
            if ch == quote_char {
                in_quotes = false;
            } else {
                cur.push(ch);
            }
        } else if ch == '"' || ch == '\'' {
            in_quotes = true;
            quote_char = ch;
        } else if ch.is_whitespace() {
            if !cur.is_empty() {
                args.push(std::mem::take(&mut cur));
            }
        } else {
            cur.push(ch);
        }
    }
    if !cur.is_empty() {
        args.push(cur);
    }
    args
}

fn find_server_archive_path(source_file: &str) -> Option<PathBuf> {
    let trimmed = source_file.trim();
    if trimmed.is_empty() {
        return None;
    }
    let data_dir = find_sct_data_dir()?;
    let candidate = data_dir.join(trimmed);
    if candidate.is_file() {
        Some(candidate)
    } else {
        None
    }
}

fn read_archive_entry_bytes(
    archive_path: &Path,
    folder_name: &str,
    rel_inside_data: &str,
) -> Result<(Vec<u8>, String), String> {
    let file = File::open(archive_path)
        .map_err(|e| format!("Failed to open server archive '{}': {}", archive_path.display(), e))?;
    let mut archive = ZipArchive::new(BufReader::new(file))
        .map_err(|e| format!("Invalid server archive '{}': {}", archive_path.display(), e))?;

    let normalized_rel = rel_inside_data
        .trim()
        .replace('\\', "/")
        .trim_start_matches('/')
        .to_string();
    let folder_norm = folder_name.trim().trim_matches('/');

    let mut candidates = Vec::new();
    if !folder_norm.is_empty() {
        candidates.push(format!("{}/data/{}", folder_norm, normalized_rel));
        candidates.push(format!("{}/{}", folder_norm, normalized_rel));
    }
    candidates.push(format!("data/{}", normalized_rel));
    candidates.push(normalized_rel.clone());

    for cand in &candidates {
        let mut matched_idx = None;
        for i in 0..archive.len() {
            if let Ok(entry) = archive.by_index(i) {
                let name = entry.name().replace('\\', "/");
                if !entry.is_dir() && name.eq_ignore_ascii_case(cand) {
                    matched_idx = Some(i);
                    break;
                }
            }
        }
        if let Some(idx) = matched_idx {
            let mut entry = archive.by_index(idx).map_err(|e| e.to_string())?;
            let mut buf = Vec::with_capacity(entry.size() as usize);
            entry.read_to_end(&mut buf).map_err(|e| e.to_string())?;
            let fname = Path::new(&normalized_rel)
                .file_name()
                .and_then(|s| s.to_str())
                .unwrap_or("file.bin")
                .to_string();
            return Ok((buf, fname));
        }
    }

    Err(format!(
        "File '{}' was not found inside '{}'.",
        rel_inside_data,
        archive_path.display()
    ))
}

async fn download_bytes_from_url(url: &str) -> Result<(Vec<u8>, String), String> {
    let client = reqwest::Client::builder()
        .user_agent("ServerCreationTool/6.0")
        .redirect(reqwest::redirect::Policy::limited(10))
        .build()
        .map_err(|e| format!("Failed to initialize HTTP client: {}", e))?;

    let resp = client
        .get(url)
        .send()
        .await
        .map_err(|e| format!("Failed to download '{}': {}", url, e))?;

    if !resp.status().is_success() {
        return Err(format!("HTTP {} while downloading '{}'", resp.status(), url));
    }

    let mut file_name = url
        .split('?')
        .next()
        .unwrap_or(url)
        .rsplit('/')
        .next()
        .unwrap_or("download.bin")
        .trim()
        .to_string();

    if let Some(cd) = resp
        .headers()
        .get(CONTENT_DISPOSITION)
        .and_then(|v| v.to_str().ok())
    {
        for part in cd.split(';') {
            let trimmed = part.trim();
            if let Some(rest) = trimmed
                .strip_prefix("filename=")
                .or_else(|| trimmed.strip_prefix("filename*="))
            {
                let cleaned = rest
                    .trim()
                    .trim_start_matches("UTF-8''")
                    .trim_matches('"')
                    .trim();
                if !cleaned.is_empty() {
                    file_name = cleaned.to_string();
                }
            }
        }
    }

    if file_name.is_empty() {
        file_name = "download.bin".to_string();
    }

    let bytes = resp
        .bytes()
        .await
        .map_err(|e| format!("Failed reading download stream: {}", e))?;
    Ok((bytes.to_vec(), file_name))
}

fn extract_zip_bytes(bytes: &[u8], dest_dir: &Path, overwrite: bool) -> Result<usize, String> {
    let cursor = io::Cursor::new(bytes);
    let mut archive = ZipArchive::new(cursor)
        .map_err(|e| format!("Invalid zip archive in action: {}", e))?;

    let mut count = 0usize;
    for i in 0..archive.len() {
        let mut entry = archive.by_index(i).map_err(|e| e.to_string())?;
        let Some(rel) = entry.enclosed_name() else {
            continue;
        };
        let out_path = dest_dir.join(rel);
        if entry.is_dir() {
            fs::create_dir_all(&out_path).map_err(|e| e.to_string())?;
        } else {
            if !overwrite && out_path.exists() {
                continue;
            }
            if let Some(parent) = out_path.parent() {
                fs::create_dir_all(parent).map_err(|e| e.to_string())?;
            }
            let mut out_file = File::create(&out_path)
                .map_err(|e| format!("Failed to write '{}': {}", out_path.display(), e))?;
            io::copy(&mut entry, &mut out_file)
                .map_err(|e| format!("Failed to extract '{}': {}", out_path.display(), e))?;
            count += 1;
        }
    }
    Ok(count)
}

fn copy_dir_recursive(src: &Path, dst: &Path, overwrite: bool) -> Result<usize, String> {
    fs::create_dir_all(dst)
        .map_err(|e| format!("Failed to create directory '{}': {}", dst.display(), e))?;
    let mut copied = 0usize;
    let entries = fs::read_dir(src)
        .map_err(|e| format!("Failed to read directory '{}': {}", src.display(), e))?;
    for entry in entries.flatten() {
        let path = entry.path();
        let target = dst.join(entry.file_name());
        if path.is_dir() {
            copied += copy_dir_recursive(&path, &target, overwrite)?;
        } else {
            if !overwrite && target.exists() {
                continue;
            }
            fs::copy(&path, &target).map_err(|e| {
                format!(
                    "Failed to copy '{}' to '{}': {}",
                    path.display(),
                    target.display(),
                    e
                )
            })?;
            copied += 1;
        }
    }
    Ok(copied)
}

#[allow(clippy::too_many_arguments)]
pub async fn execute_simple_action(
    app: Option<&AppHandle>,
    library_path: &str,
    codename: &str,
    folder_name: &str,
    source_file: &str,
    instance_name: &str,
    server_id: &str,
    action_id: &str,
    actions: &[SimpleActionDto],
    properties: &HashMap<String, String>,
    allow_untrusted: bool,
) -> Result<ActionExecResultDto, String> {
    let (server_dir, _) = resolve_instance_dir(library_path, codename, instance_name)?;
    fs::create_dir_all(&server_dir)
        .map_err(|e| format!("Failed to ensure server directory '{}': {}", server_dir.display(), e))?;

    let mut logs = Vec::new();
    let mut visited = HashSet::new();
    let archive_path = find_server_archive_path(source_file);
    let data_dir = find_sct_data_dir().unwrap_or_else(|| server_dir.clone());
    let is_official = archive_path
        .as_deref()
        .map(|p| verify_server_package(p).is_official)
        .unwrap_or(false);

    execute_simple_action_recursive(
        app,
        &server_dir,
        &data_dir,
        codename,
        folder_name,
        instance_name,
        server_id,
        archive_path.as_deref(),
        action_id,
        actions,
        properties,
        is_official || allow_untrusted,
        &mut visited,
        &mut logs,
    )
    .await?;

    Ok(ActionExecResultDto {
        success: true,
        message: logs.last().cloned().unwrap_or_else(|| "Action completed.".to_string()),
        logs,
        updated_properties: properties.clone(),
    })
}

#[allow(clippy::too_many_arguments)]
fn execute_simple_action_recursive<'a>(
    app: Option<&'a AppHandle>,
    server_dir: &'a Path,
    data_dir: &'a Path,
    codename: &'a str,
    folder_name: &'a str,
    instance_name: &'a str,
    server_id: &'a str,
    archive_path: Option<&'a Path>,
    action_id: &'a str,
    actions: &'a [SimpleActionDto],
    properties: &'a HashMap<String, String>,
    allow_run_program: bool,
    visited: &'a mut HashSet<String>,
    logs: &'a mut Vec<String>,
) -> std::pin::Pin<Box<dyn std::future::Future<Output = Result<(), String>> + Send + 'a>> {
    Box::pin(async move {
        if !visited.insert(action_id.to_string()) {
            return Ok(());
        }

        let act = actions
            .iter()
            .find(|a| a.id == action_id)
            .ok_or_else(|| format!("SimpleAction '{}' was not found.", action_id))?;

        let log_line = |msg: String, logs: &mut Vec<String>| {
            if let Some(handle) = app {
                emit_log(handle, server_id, msg.clone());
            }
            logs.push(msg);
        };

        log_line(
            format!("Running SimpleAction '{}' ({})...", act.label, act.action_type),
            logs,
        );

        let kind = act.action_type.trim().to_lowercase();
        match kind.as_str() {
            "file_drop" => {
                let target_rel = resolve_tokens(
                    &act.target_dir,
                    server_dir,
                    data_dir,
                    codename,
                    instance_name,
                    properties,
                );
                let dest_dir = resolve_relative_in_dir(server_dir, &target_rel)?;
                fs::create_dir_all(&dest_dir)
                    .map_err(|e| format!("Failed to create target directory '{}': {}", dest_dir.display(), e))?;

                let source_val = resolve_tokens(
                    &act.source_file_name,
                    server_dir,
                    data_dir,
                    codename,
                    instance_name,
                    properties,
                );
                let (bytes, file_name) = if act.download_from_link {
                    download_bytes_from_url(&source_val).await?
                } else {
                    let arch = archive_path.ok_or_else(|| {
                        format!("Server archive not found for file_drop '{}'.", source_val)
                    })?;
                    read_archive_entry_bytes(arch, folder_name, &source_val)?
                };

                let is_zip = file_name.to_lowercase().ends_with(".zip");
                if act.extract_archive && is_zip {
                    let extracted = extract_zip_bytes(&bytes, &dest_dir, act.overwrite)?;
                    log_line(
                        format!("Extracted {} file(s) from '{}' into '{}'.", extracted, file_name, dest_dir.display()),
                        logs,
                    );
                } else {
                    let dest_file = dest_dir.join(&file_name);
                    if !act.overwrite && dest_file.exists() {
                        log_line(
                            format!("Skipped '{}' (already exists, overwrite disabled).", dest_file.display()),
                            logs,
                        );
                    } else {
                        fs::write(&dest_file, &bytes)
                            .map_err(|e| format!("Failed to write '{}': {}", dest_file.display(), e))?;
                        log_line(format!("Dropped file '{}'.", dest_file.display()), logs);
                    }
                }
            }
            "delete_file" | "delete" => {
                let target_rel = resolve_tokens(
                    &act.delete_path,
                    server_dir,
                    data_dir,
                    codename,
                    instance_name,
                    properties,
                );
                let target = resolve_relative_in_dir(server_dir, &target_rel)?;
                if target == server_dir {
                    return Err("Refusing to delete the root server directory.".to_string());
                }
                if target.is_dir() {
                    fs::remove_dir_all(&target)
                        .map_err(|e| format!("Failed to delete folder '{}': {}", target.display(), e))?;
                    log_line(format!("Deleted folder '{}'.", target.display()), logs);
                } else if target.is_file() {
                    fs::remove_file(&target)
                        .map_err(|e| format!("Failed to delete file '{}': {}", target.display(), e))?;
                    log_line(format!("Deleted file '{}'.", target.display()), logs);
                } else {
                    log_line(format!("Delete skipped (not found): '{}'.", target.display()), logs);
                }
            }
            "copy_file" | "copy" => {
                let src_rel = resolve_tokens(
                    &act.copy_source,
                    server_dir,
                    data_dir,
                    codename,
                    instance_name,
                    properties,
                );
                let dst_rel = resolve_tokens(
                    &act.copy_dest,
                    server_dir,
                    data_dir,
                    codename,
                    instance_name,
                    properties,
                );
                let src = resolve_relative_in_dir(server_dir, &src_rel)?;
                let dst = resolve_relative_in_dir(server_dir, &dst_rel)?;

                if src.is_dir() {
                    let copied = copy_dir_recursive(&src, &dst, act.overwrite)?;
                    log_line(
                        format!("Copied {} file(s) from '{}' to '{}'.", copied, src.display(), dst.display()),
                        logs,
                    );
                } else if src.is_file() {
                    let final_dst = if dst.is_dir() {
                        dst.join(src.file_name().unwrap_or_default())
                    } else {
                        dst
                    };
                    if !act.overwrite && final_dst.exists() {
                        log_line(
                            format!("Skipped copy to '{}' (already exists).", final_dst.display()),
                            logs,
                        );
                    } else {
                        if let Some(parent) = final_dst.parent() {
                            fs::create_dir_all(parent).map_err(|e| e.to_string())?;
                        }
                        fs::copy(&src, &final_dst).map_err(|e| {
                            format!(
                                "Failed to copy '{}' to '{}': {}",
                                src.display(),
                                final_dst.display(),
                                e
                            )
                        })?;
                        log_line(
                            format!("Copied '{}' to '{}'.", src.display(), final_dst.display()),
                            logs,
                        );
                    }
                } else {
                    return Err(format!("Copy source not found: '{}'.", src.display()));
                }
            }
            "run_program" => {
                if !allow_run_program {
                    return Err("UNTRUSTED_PACKAGE: Running external programs from an unsigned .server package requires explicit user approval.".to_string());
                }
                let exec_raw = resolve_tokens(
                    &act.run_exec,
                    server_dir,
                    data_dir,
                    codename,
                    instance_name,
                    properties,
                );
                let args_raw = resolve_tokens(
                    &act.run_args,
                    server_dir,
                    data_dir,
                    codename,
                    instance_name,
                    properties,
                );
                let trimmed_exec = exec_raw.trim();
                if trimmed_exec.is_empty() {
                    return Err("run_program action has an empty executable.".to_string());
                }

                let resolved_exec = if let Ok(rel_path) = resolve_relative_in_dir(server_dir, trimmed_exec) {
                    if rel_path.exists() {
                        rel_path.to_string_lossy().into_owned()
                    } else {
                        trimmed_exec.to_string()
                    }
                } else {
                    trimmed_exec.to_string()
                };

                let args = split_args(&args_raw);
                let server_dir_owned = server_dir.to_path_buf();
                let output = tauri::async_runtime::spawn_blocking(move || {
                    let mut cmd = Command::new(&resolved_exec);
                    cmd.current_dir(&server_dir_owned);
                    cmd.args(&args);
                    #[cfg(target_os = "windows")]
                    {
                        use std::os::windows::process::CommandExt;
                        cmd.creation_flags(0x0800_0000);
                    }
                    cmd.output()
                })
                .await
                .map_err(|e| format!("Failed waiting for program: {}", e))?
                .map_err(|e| format!("Failed to run '{}': {}", exec_raw, e))?;

                let stdout = String::from_utf8_lossy(&output.stdout);
                let stderr = String::from_utf8_lossy(&output.stderr);
                for line in stdout.lines().chain(stderr.lines()) {
                    let t = line.trim();
                    if !t.is_empty() {
                        log_line(t.to_string(), logs);
                    }
                }
                if !output.status.success() {
                    return Err(format!(
                        "Program '{}' exited with status {}.",
                        exec_raw,
                        output
                            .status
                            .code()
                            .map(|c| c.to_string())
                            .unwrap_or_else(|| "unknown".to_string())
                    ));
                }
                log_line(format!("Program '{}' completed.", exec_raw), logs);
            }
            other => {
                return Err(format!("Unsupported SimpleAction type '{}'.", other));
            }
        }

        if act.enable_chaining {
            for next_id in &act.chained_action_ids {
                if !next_id.trim().is_empty() {
                    execute_simple_action_recursive(
                        app,
                        server_dir,
                        data_dir,
                        codename,
                        folder_name,
                        instance_name,
                        server_id,
                        archive_path,
                        next_id,
                        actions,
                        properties,
                        allow_run_program,
                        visited,
                        logs,
                    )
                    .await?;
                }
            }
        }

        Ok(())
    })
}

fn read_funcs_lua_source(
    codename: &str,
    folder_name: &str,
    source_file: &str,
) -> Result<String, String> {
    let arch = find_server_archive_path(source_file)
        .ok_or_else(|| format!("Server archive '{}' was not found.", source_file))?;
    let file = File::open(&arch)
        .map_err(|e| format!("Failed to open '{}': {}", arch.display(), e))?;
    let mut archive = ZipArchive::new(BufReader::new(file))
        .map_err(|e| format!("Invalid archive '{}': {}", arch.display(), e))?;

    let expected_name = format!("{}_funcs.lua", codename).to_lowercase();
    let folder_norm = folder_name.trim().trim_matches('/').to_lowercase();
    let expected_in_folder = format!("{}/{}", folder_norm, expected_name);
    let funcs_in_folder = format!("{}/funcs.lua", folder_norm);

    let mut fallback_idx = None;
    let mut exact_idx = None;

    for i in 0..archive.len() {
        if let Ok(entry) = archive.by_index(i) {
            if entry.is_dir() {
                continue;
            }
            let name_lower = entry.name().replace('\\', "/").to_lowercase();
            if name_lower == expected_in_folder || name_lower == expected_name {
                exact_idx = Some(i);
                break;
            }
            if name_lower == funcs_in_folder
                || name_lower == "funcs.lua"
                || name_lower.ends_with("_funcs.lua")
            {
                fallback_idx = Some(i);
            }
        }
    }

    let idx = exact_idx.or(fallback_idx).ok_or_else(|| {
        format!(
            "'{}_funcs.lua' was not found in '{}'.",
            codename,
            arch.display()
        )
    })?;

    let mut entry = archive.by_index(idx).map_err(|e| e.to_string())?;
    let mut source = String::new();
    entry
        .read_to_string(&mut source)
        .map_err(|e| format!("Failed to read Lua script: {}", e))?;
    Ok(source)
}

fn lua_value_to_string(val: &LuaValue) -> String {
    match val {
        LuaValue::String(s) => s.to_str().map(|v| v.to_string()).unwrap_or_default(),
        LuaValue::Integer(n) => n.to_string(),
        LuaValue::Number(n) => n.to_string(),
        LuaValue::Boolean(b) => b.to_string(),
        LuaValue::Nil => String::new(),
        other => format!("{:?}", other),
    }
}

#[allow(clippy::too_many_arguments)]
pub async fn execute_funcs_method(
    app: Option<&AppHandle>,
    library_path: &str,
    codename: &str,
    folder_name: &str,
    source_file: &str,
    instance_name: &str,
    server_id: &str,
    method_name: &str,
    optional: bool,
    properties: &HashMap<String, String>,
    allow_untrusted: bool,
) -> Result<ActionExecResultDto, String> {
    let trimmed_method = method_name.trim().trim_end_matches("()");
    if trimmed_method.is_empty() {
        return Err("Function name is empty.".to_string());
    }

    let trust = verify_server_package_by_source(source_file);
    if !trust.is_official && !allow_untrusted {
        return Err(format!("UNTRUSTED_PACKAGE: {}", trust.reason));
    }

    let (server_dir, _) = resolve_instance_dir(library_path, codename, instance_name)?;
    let lua_source = read_funcs_lua_source(codename, folder_name, source_file)?;
    let data_dir = find_sct_data_dir().unwrap_or_else(|| server_dir.clone());

    let codename_owned = codename.to_string();
    let instance_owned = instance_name.to_string();
    let method_owned = trimmed_method.to_string();
    let server_id_owned = server_id.to_string();
    let props_owned = properties.clone();
    let app_cloned = app.cloned();

    let (logs, updated_props) = tauri::async_runtime::spawn_blocking(move || -> Result<(Vec<String>, HashMap<String, String>), String> {
        let lua = Lua::new();
        let logs_shared: Arc<Mutex<Vec<String>>> = Arc::new(Mutex::new(Vec::new()));

        // Override global print(...) to forward output to the server terminal and result logs.
        {
            let logs_ref = Arc::clone(&logs_shared);
            let app_ref = app_cloned.clone();
            let sid_ref = server_id_owned.clone();
            let print_fn = lua
                .create_function(move |_, args: MultiValue| {
                    let line = args
                        .iter()
                        .map(lua_value_to_string)
                        .collect::<Vec<_>>()
                        .join("\t");
                    if let Some(ref handle) = app_ref {
                        emit_log(handle, &sid_ref, line.clone());
                    }
                    if let Ok(mut guard) = logs_ref.lock() {
                        guard.push(line);
                    }
                    Ok(())
                })
                .map_err(|e| format!("Failed to register print(): {}", e))?;
            lua.globals()
                .set("print", print_fn)
                .map_err(|e| format!("Failed to set print(): {}", e))?;
        }

        // Build global Context table matching MakeServer's _funcs.lua contract.
        let ctx_table = lua.create_table().map_err(|e| e.to_string())?;
        let server_str = server_dir.to_string_lossy().into_owned();
        let data_str = data_dir.to_string_lossy().into_owned();
        ctx_table.set("ServerDir", server_str.as_str()).map_err(|e| e.to_string())?;
        ctx_table.set("InstallDir", server_str.as_str()).map_err(|e| e.to_string())?;
        ctx_table.set("DataDir", data_str.as_str()).map_err(|e| e.to_string())?;
        ctx_table.set("Codename", codename_owned.as_str()).map_err(|e| e.to_string())?;
        let os_name = if cfg!(target_os = "windows") { "windows" } else { "linux" };
        ctx_table.set("OS", os_name).map_err(|e| e.to_string())?;
        ctx_table.set("IsWindows", cfg!(target_os = "windows")).map_err(|e| e.to_string())?;
        ctx_table.set("IsLinux", cfg!(target_os = "linux")).map_err(|e| e.to_string())?;

        let props_table = lua.create_table().map_err(|e| e.to_string())?;
        for (k, v) in &props_owned {
            props_table.set(k.as_str(), v.as_str()).map_err(|e| e.to_string())?;
        }
        ctx_table.set("Properties", props_table).map_err(|e| e.to_string())?;

        let get_prop_fn = lua
            .create_function(|_, (self_tbl, key, default_val): (Table, String, Option<String>)| {
                let props: Table = self_tbl.get("Properties")?;
                let target = key.to_lowercase();
                for pair in props.pairs::<String, LuaValue>() {
                    if let Ok((k, v)) = pair {
                        if k.to_lowercase() == target {
                            let s = lua_value_to_string(&v);
                            if !s.is_empty() {
                                return Ok(s);
                            }
                        }
                    }
                }
                Ok(default_val.unwrap_or_default())
            })
            .map_err(|e| e.to_string())?;
        ctx_table.set("GetProperty", get_prop_fn).map_err(|e| e.to_string())?;

        let set_prop_fn = lua
            .create_function(|_, (self_tbl, key, val): (Table, String, LuaValue)| {
                let props: Table = self_tbl.get("Properties")?;
                let target = key.to_lowercase();
                let mut matched_key = None;
                for pair in props.pairs::<String, LuaValue>() {
                    if let Ok((k, _)) = pair {
                        if k.to_lowercase() == target {
                            matched_key = Some(k);
                            break;
                        }
                    }
                }
                let final_key = matched_key.unwrap_or(key);
                props.set(final_key, lua_value_to_string(&val))?;
                Ok(())
            })
            .map_err(|e| e.to_string())?;
        ctx_table.set("SetProperty", set_prop_fn).map_err(|e| e.to_string())?;

        let srv_dir_for_tokens = server_dir.clone();
        let data_dir_for_tokens = data_dir.clone();
        let code_for_tokens = codename_owned.clone();
        let inst_for_tokens = instance_owned.clone();
        let resolve_tokens_fn = lua
            .create_function(move |_, (self_tbl, template): (Table, String)| {
                let props: Table = self_tbl.get("Properties")?;
                let mut map = HashMap::new();
                for pair in props.pairs::<String, LuaValue>() {
                    if let Ok((k, v)) = pair {
                        map.insert(k, lua_value_to_string(&v));
                    }
                }
                Ok(resolve_tokens(
                    &template,
                    &srv_dir_for_tokens,
                    &data_dir_for_tokens,
                    &code_for_tokens,
                    &inst_for_tokens,
                    &map,
                ))
            })
            .map_err(|e| e.to_string())?;
        ctx_table
            .set("ResolveTokens", resolve_tokens_fn)
            .map_err(|e| e.to_string())?;

        // Cross-platform filesystem helpers for _funcs.lua (accept both Context:Fn(...) and Context.Fn(...)).
        fn strip_self_args(args: MultiValue) -> Vec<LuaValue> {
            let mut items: Vec<LuaValue> = args.into_iter().collect();
            if matches!(items.first(), Some(LuaValue::Table(_))) {
                items.remove(0);
            }
            items
        }

        fn to_os_path(raw: &str) -> PathBuf {
            #[cfg(not(target_os = "windows"))]
            {
                PathBuf::from(raw.replace('\\', "/"))
            }
            #[cfg(target_os = "windows")]
            {
                PathBuf::from(raw)
            }
        }

        let make_dir_fn = lua
            .create_function(|_, args: MultiValue| {
                let items = strip_self_args(args);
                let path_str = items
                    .first()
                    .map(lua_value_to_string)
                    .unwrap_or_default();
                if path_str.trim().is_empty() {
                    return Err(mlua::Error::RuntimeError("MakeDir: path is empty.".into()));
                }
                let p = to_os_path(path_str.trim());
                fs::create_dir_all(&p).map_err(|e| {
                    mlua::Error::RuntimeError(format!("MakeDir('{}') failed: {}", p.display(), e))
                })?;
                Ok(true)
            })
            .map_err(|e| e.to_string())?;
        ctx_table.set("MakeDir", make_dir_fn).map_err(|e| e.to_string())?;

        let delete_fn = lua
            .create_function(|_, args: MultiValue| {
                let items = strip_self_args(args);
                let path_str = items
                    .first()
                    .map(lua_value_to_string)
                    .unwrap_or_default();
                if path_str.trim().is_empty() {
                    return Err(mlua::Error::RuntimeError("Delete: path is empty.".into()));
                }
                let p = to_os_path(path_str.trim());
                if p.is_dir() {
                    fs::remove_dir_all(&p).map_err(|e| {
                        mlua::Error::RuntimeError(format!("Delete('{}') failed: {}", p.display(), e))
                    })?;
                    Ok(true)
                } else if p.is_file() {
                    fs::remove_file(&p).map_err(|e| {
                        mlua::Error::RuntimeError(format!("Delete('{}') failed: {}", p.display(), e))
                    })?;
                    Ok(true)
                } else {
                    Ok(false)
                }
            })
            .map_err(|e| e.to_string())?;
        ctx_table.set("Delete", delete_fn).map_err(|e| e.to_string())?;

        let copy_fn = lua
            .create_function(|_, args: MultiValue| {
                let items = strip_self_args(args);
                let src_str = items.first().map(lua_value_to_string).unwrap_or_default();
                let dst_str = items.get(1).map(lua_value_to_string).unwrap_or_default();
                let overwrite = match items.get(2) {
                    Some(LuaValue::Boolean(b)) => *b,
                    _ => true,
                };
                if src_str.trim().is_empty() || dst_str.trim().is_empty() {
                    return Err(mlua::Error::RuntimeError(
                        "Copy: source and destination paths are required.".into(),
                    ));
                }
                let src = to_os_path(src_str.trim());
                let dst = to_os_path(dst_str.trim());
                if src.is_dir() {
                    copy_dir_recursive(&src, &dst, overwrite)
                        .map_err(mlua::Error::RuntimeError)?;
                    Ok(true)
                } else if src.is_file() {
                    let final_dst = if dst.is_dir() {
                        dst.join(src.file_name().unwrap_or_default())
                    } else {
                        dst
                    };
                    if !overwrite && final_dst.exists() {
                        return Ok(false);
                    }
                    if let Some(parent) = final_dst.parent() {
                        fs::create_dir_all(parent).map_err(|e| {
                            mlua::Error::RuntimeError(format!("Copy failed: {}", e))
                        })?;
                    }
                    fs::copy(&src, &final_dst).map_err(|e| {
                        mlua::Error::RuntimeError(format!(
                            "Copy('{}' -> '{}') failed: {}",
                            src.display(),
                            final_dst.display(),
                            e
                        ))
                    })?;
                    Ok(true)
                } else {
                    Err(mlua::Error::RuntimeError(format!(
                        "Copy: source '{}' not found.",
                        src.display()
                    )))
                }
            })
            .map_err(|e| e.to_string())?;
        ctx_table.set("Copy", copy_fn).map_err(|e| e.to_string())?;

        let read_text_fn = lua
            .create_function(|_, args: MultiValue| {
                let items = strip_self_args(args);
                let path_str = items.first().map(lua_value_to_string).unwrap_or_default();
                if path_str.trim().is_empty() {
                    return Err(mlua::Error::RuntimeError("ReadText: path is empty.".into()));
                }
                let p = to_os_path(path_str.trim());
                fs::read_to_string(&p).map_err(|e| {
                    mlua::Error::RuntimeError(format!("ReadText('{}') failed: {}", p.display(), e))
                })
            })
            .map_err(|e| e.to_string())?;
        ctx_table.set("ReadText", read_text_fn).map_err(|e| e.to_string())?;

        let write_text_fn = lua
            .create_function(|_, args: MultiValue| {
                let items = strip_self_args(args);
                let path_str = items.first().map(lua_value_to_string).unwrap_or_default();
                let content = items.get(1).map(lua_value_to_string).unwrap_or_default();
                if path_str.trim().is_empty() {
                    return Err(mlua::Error::RuntimeError("WriteText: path is empty.".into()));
                }
                let p = to_os_path(path_str.trim());
                if let Some(parent) = p.parent() {
                    fs::create_dir_all(parent).map_err(|e| {
                        mlua::Error::RuntimeError(format!("WriteText failed: {}", e))
                    })?;
                }
                fs::write(&p, content).map_err(|e| {
                    mlua::Error::RuntimeError(format!("WriteText('{}') failed: {}", p.display(), e))
                })?;
                Ok(true)
            })
            .map_err(|e| e.to_string())?;
        ctx_table.set("WriteText", write_text_fn).map_err(|e| e.to_string())?;

        let exists_fn = lua
            .create_function(|_, args: MultiValue| {
                let items = strip_self_args(args);
                let path_str = items.first().map(lua_value_to_string).unwrap_or_default();
                if path_str.trim().is_empty() {
                    return Ok(false);
                }
                Ok(to_os_path(path_str.trim()).exists())
            })
            .map_err(|e| e.to_string())?;
        ctx_table.set("Exists", exists_fn).map_err(|e| e.to_string())?;

        lua.globals()
            .set("Context", ctx_table)
            .map_err(|e| format!("Failed to inject Context: {}", e))?;

        lua.load(&lua_source)
            .set_name(format!("{}_funcs.lua", codename_owned))
            .exec()
            .map_err(|e| format!("Lua error in {}_funcs.lua: {}", codename_owned, e))?;

        let globals = lua.globals();
        let target_fn: Option<Function> = match globals.get::<LuaValue>(method_owned.as_str()) {
            Ok(LuaValue::Function(f)) => Some(f),
            _ => {
                let target_lower = method_owned.to_lowercase();
                let mut found = None;
                for pair in globals.pairs::<String, LuaValue>() {
                    if let Ok((k, LuaValue::Function(f))) = pair {
                        if k.to_lowercase() == target_lower {
                            found = Some(f);
                            break;
                        }
                    }
                }
                found
            }
        };

        let Some(func) = target_fn else {
            if optional {
                let collected_logs = logs_shared.lock().map(|g| g.clone()).unwrap_or_default();
                return Ok((collected_logs, props_owned));
            }
            return Err(format!(
                "Function '{}' was not found in {}_funcs.lua.",
                method_owned, codename_owned
            ));
        };

        func.call::<()>(())
            .map_err(|e| format!("{}() failed: {}", method_owned, e))?;

        let mut updated = HashMap::new();
        if let Ok(ctx) = globals.get::<Table>("Context") {
            if let Ok(props) = ctx.get::<Table>("Properties") {
                for pair in props.pairs::<String, LuaValue>() {
                    if let Ok((k, v)) = pair {
                        updated.insert(k, lua_value_to_string(&v));
                    }
                }
            }
        }

        let collected_logs = logs_shared.lock().map(|g| g.clone()).unwrap_or_default();
        Ok((collected_logs, updated))
    })
    .await
    .map_err(|e| format!("Lua runner task failed: {}", e))??;

    Ok(ActionExecResultDto {
        success: true,
        message: if logs.is_empty() {
            format!("{}() completed.", trimmed_method)
        } else {
            logs.join("\n")
        },
        logs,
        updated_properties: updated_props,
    })
}

pub async fn create_preconfigured_config_file(
    app: Option<&AppHandle>,
    library_path: &str,
    codename: &str,
    folder_name: &str,
    source_file: &str,
    instance_name: &str,
    server_id: &str,
    mode: &str,
    source: &str,
    link: &str,
    dest_path: &str,
    properties: &HashMap<String, String>,
) -> Result<String, String> {
    let (server_dir, _) = resolve_instance_dir(library_path, codename, instance_name)?;
    let data_dir = find_sct_data_dir().unwrap_or_else(|| server_dir.clone());
    let archive_path = find_server_archive_path(source_file);

    let resolved_dest = resolve_tokens(
        dest_path,
        &server_dir,
        &data_dir,
        codename,
        instance_name,
        properties,
    );
    let trimmed_dest = resolved_dest.trim();
    if trimmed_dest.is_empty() {
        return Err("PreconfiguredConfigDestPath is empty.".to_string());
    }
    let is_dir_hint = trimmed_dest.ends_with('/') || trimmed_dest.ends_with('\\');
    let base_dest = resolve_relative_in_dir(&server_dir, trimmed_dest)?;

    let mode_norm = mode.trim().to_lowercase();
    let (bytes, file_name) = if mode_norm == "link" {
        let raw_url = if !link.trim().is_empty() { link } else { source };
        let resolved_url = resolve_tokens(
            raw_url,
            &server_dir,
            &data_dir,
            codename,
            instance_name,
            properties,
        );
        if resolved_url.trim().is_empty() {
            return Err("PreconfiguredConfigLink is empty.".to_string());
        }
        download_bytes_from_url(resolved_url.trim()).await?
    } else {
        let resolved_src = resolve_tokens(
            source,
            &server_dir,
            &data_dir,
            codename,
            instance_name,
            properties,
        );
        if resolved_src.trim().is_empty() {
            return Err("PreconfiguredConfigSource is empty.".to_string());
        }
        let arch = archive_path.ok_or_else(|| {
            format!(
                "Server archive not found for PreconfiguredConfigSource '{}'.",
                resolved_src
            )
        })?;
        read_archive_entry_bytes(&arch, folder_name, resolved_src.trim())?
    };

    let dest_file = if is_dir_hint || base_dest.is_dir() {
        base_dest.join(&file_name)
    } else {
        base_dest
    };

    if let Some(parent) = dest_file.parent() {
        fs::create_dir_all(parent).map_err(|e| {
            format!(
                "Failed to create directory '{}': {}",
                parent.display(),
                e
            )
        })?;
    }

    fs::write(&dest_file, &bytes).map_err(|e| {
        format!(
            "Failed to write preconfigured config file '{}': {}",
            dest_file.display(),
            e
        )
    })?;

    if let Some(handle) = app {
        crate::steamcmd::emit_log(
            handle,
            server_id,
            format!("Created preconfigured config file '{}'.", dest_file.display()),
        );
    }

    Ok(dest_file.to_string_lossy().into_owned())
}

#[cfg(test)]
mod tests {
    use super::{create_preconfigured_config_file, execute_funcs_method, execute_simple_action};
    use crate::servers::SimpleActionDto;
    use std::collections::HashMap;
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
        let dir = std::env::temp_dir().join(format!("sct_actions_test_{}_{}", name, nanos));
        fs::create_dir_all(&dir).unwrap();
        dir
    }

    fn make_action(id: &str, action_type: &str) -> SimpleActionDto {
        SimpleActionDto {
            id: id.to_string(),
            label: id.to_string(),
            title: HashMap::new(),
            action_type: action_type.to_string(),
            when: "manual_trigger".to_string(),
            target_language: "all".to_string(),
            source_file_name: String::new(),
            download_from_link: false,
            target_dir: String::new(),
            overwrite: true,
            extract_archive: false,
            delete_path: String::new(),
            copy_source: String::new(),
            copy_dest: String::new(),
            run_exec: String::new(),
            run_args: String::new(),
            enable_chaining: false,
            chained_action_ids: Vec::new(),
            is_platform_specific: false,
            target_platform: "all".to_string(),
        }
    }

    #[test]
    fn simple_action_copy_delete_and_chaining() {
        let lib = make_temp_dir("simple_chain");
        let lib_str = lib.to_string_lossy().into_owned();
        let srv_dir = lib.join("cs_ds");
        fs::create_dir_all(&srv_dir).unwrap();
        fs::write(srv_dir.join("source.cfg"), b"hostname test").unwrap();

        let mut copy_act = make_action("sa_copy", "copy_file");
        copy_act.copy_source = "source.cfg".to_string();
        copy_act.copy_dest = "backup/copied.cfg".to_string();
        copy_act.enable_chaining = true;
        copy_act.chained_action_ids = vec!["sa_del".to_string(), "sa_copy".to_string()];

        let mut del_act = make_action("sa_del", "delete_file");
        del_act.delete_path = "source.cfg".to_string();

        let actions = vec![copy_act, del_act];
        let props = HashMap::new();

        let res = tauri::async_runtime::block_on(execute_simple_action(
            None,
            &lib_str,
            "cs",
            "cs",
            "",
            "Default",
            "cs",
            "sa_copy",
            &actions,
            &props,
            true,
        ))
        .unwrap();

        assert!(res.success);
        assert!(!srv_dir.join("source.cfg").exists());
        assert_eq!(
            fs::read(srv_dir.join("backup").join("copied.cfg")).unwrap(),
            b"hostname test"
        );

        let _ = fs::remove_dir_all(&lib);
    }

    #[test]
    fn simple_action_file_drop_from_archive() {
        let lib = make_temp_dir("file_drop");
        let lib_str = lib.to_string_lossy().into_owned();
        let sct_data = crate::servers::find_sct_data_dir().unwrap();
        let archive_name = "__test_drop.server";
        let archive_path = sct_data.join(archive_name);

        {
            let file = File::create(&archive_path).unwrap();
            let mut zip = ZipWriter::new(file);
            let opts = SimpleFileOptions::default();
            zip.start_file("testdrop/data/sample.txt", opts).unwrap();
            zip.write_all(b"dropped-content").unwrap();
            zip.finish().unwrap();
        }

        let mut drop_act = make_action("sa_drop", "file_drop");
        drop_act.source_file_name = "sample.txt".to_string();
        drop_act.target_dir = "cfg".to_string();

        let res = tauri::async_runtime::block_on(execute_simple_action(
            None,
            &lib_str,
            "testdrop",
            "testdrop",
            archive_name,
            "Default",
            "testdrop",
            "sa_drop",
            &[drop_act],
            &HashMap::new(),
            true,
        ))
        .unwrap();

        assert!(res.success);
        assert_eq!(
            fs::read(lib.join("testdrop_ds").join("cfg").join("sample.txt")).unwrap(),
            b"dropped-content"
        );

        let _ = fs::remove_file(&archive_path);
        let _ = fs::remove_dir_all(&lib);
    }

    #[test]
    fn funcs_lua_in_archive_executes_method_and_updates_properties() {
        let lib = make_temp_dir("funcs_lua");
        let lib_str = lib.to_string_lossy().into_owned();
        fs::create_dir_all(lib.join("testlua_ds")).unwrap();

        let sct_data = crate::servers::find_sct_data_dir().unwrap();
        let lua_source = r#"
function PostInstall()
    local saveDir = Context:ResolveTokens("{SERVER_DIR}/saves")
    Context:MakeDir(saveDir)
    Context:WriteText(saveDir .. "/cfg.txt", "os=" .. Context.OS)
    Context:Copy(saveDir .. "/cfg.txt", saveDir .. "/cfg_copy.txt")
    local content = Context:ReadText(saveDir .. "/cfg_copy.txt")
    Context:Delete(saveDir .. "/cfg.txt")
    print("PostInstall saves: " .. saveDir .. " [" .. content .. "]")
    Context:SetProperty("InstalledAt", os.date("!%Y-%m-%dT%H:%M:%SZ"))
    Context:SetProperty("DetectedOS", Context.OS)
end
"#;

        let archive_name = "__test_lua.server";
        let archive_path = sct_data.join(archive_name);
        {
            let file = File::create(&archive_path).unwrap();
            let mut zip = ZipWriter::new(file);
            zip.start_file("testlua/testlua_funcs.lua", SimpleFileOptions::default())
                .unwrap();
            zip.write_all(lua_source.as_bytes()).unwrap();
            zip.finish().unwrap();
        }

        let res = tauri::async_runtime::block_on(execute_funcs_method(
            None,
            &lib_str,
            "testlua",
            "testlua",
            archive_name,
            "Default",
            "testlua",
            "PostInstall",
            false,
            &HashMap::new(),
            true,
        ))
        .unwrap();

        assert!(res.success);
        assert!(res.updated_properties.contains_key("InstalledAt"));
        assert!(res.updated_properties.contains_key("DetectedOS"));
        assert!(!lib.join("testlua_ds").join("saves").join("cfg.txt").exists());
        assert!(lib.join("testlua_ds").join("saves").join("cfg_copy.txt").is_file());
        assert!(res
            .logs
            .iter()
            .any(|l| l.contains("PostInstall saves:") && l.contains("os=")));

        let _ = fs::remove_file(&archive_path);
        let _ = fs::remove_dir_all(&lib);
    }

    #[test]
    fn preconfigured_config_data_file_copies_to_dest_path() {
        let lib = make_temp_dir("preconf_cfg");
        let lib_str = lib.to_string_lossy().into_owned();
        let sct_data = crate::servers::find_sct_data_dir().unwrap();
        let archive_name = "__test_preconf.server";
        let archive_path = sct_data.join(archive_name);

        {
            let file = File::create(&archive_path).unwrap();
            let mut zip = ZipWriter::new(file);
            let opts = SimpleFileOptions::default();
            zip.start_file("testpre/data/configs/server.cfg", opts).unwrap();
            zip.write_all(b"hostname \"SCT Test\"\n").unwrap();
            zip.finish().unwrap();
        }

        let created = tauri::async_runtime::block_on(create_preconfigured_config_file(
            None,
            &lib_str,
            "testpre",
            "testpre",
            archive_name,
            "Default",
            "testpre",
            "data_file",
            "configs/server.cfg",
            "",
            "cstrike/server.cfg",
            &HashMap::new(),
        ))
        .unwrap();

        let expected_path = lib.join("testpre_ds").join("cstrike").join("server.cfg");
        assert_eq!(std::path::PathBuf::from(&created), expected_path);
        assert_eq!(
            fs::read_to_string(&expected_path).unwrap(),
            "hostname \"SCT Test\"\n"
        );

        let _ = fs::remove_file(&archive_path);
        let _ = fs::remove_dir_all(&lib);
    }
}
