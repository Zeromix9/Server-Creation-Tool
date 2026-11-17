function getTauriCore() {
  if (window.__TAURI__ && window.__TAURI__.core) {
    return window.__TAURI__.core;
  }
  return null;
}

function getTauriWindow() {
  if (
    window.__TAURI__ &&
    window.__TAURI__.window &&
    typeof window.__TAURI__.window.getCurrentWindow === 'function'
  ) {
    return window.__TAURI__.window.getCurrentWindow();
  }
  return null;
}

window.sctDragWindow = function () {
  const core = getTauriCore();
  if (core) {
    core.invoke('start_drag').catch(function () {
      const win = getTauriWindow();
      if (win) win.startDragging();
    });
  } else {
    const win = getTauriWindow();
    if (win) win.startDragging();
  }
};

window.sctMinimizeWindow = function () {
  const core = getTauriCore();
  if (core) {
    core.invoke('window_minimize').catch(function () {
      const win = getTauriWindow();
      if (win) win.minimize();
    });
  } else {
    const win = getTauriWindow();
    if (win) win.minimize();
  }
};

window.sctCloseWindow = function () {
  const core = getTauriCore();
  if (core) {
    core.invoke('window_close').catch(function () {
      const win = getTauriWindow();
      if (win) win.destroy();
    });
  } else {
    const win = getTauriWindow();
    if (win) {
      win.destroy();
    } else {
      window.close();
    }
  }
};

window.sctSetCloseToTray = function (enabled) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('set_close_to_tray', { enabled: Boolean(enabled) });
  }
  return Promise.resolve();
};

window.sctSetRunOnStartup = function (enabled) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('set_run_on_startup', { enabled: Boolean(enabled) });
  }
  return Promise.resolve();
};

window.sctOpenUrl = function (url) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('open_url', { url: url });
  }
  window.open(url, '_blank');
  return Promise.resolve();
};

window.sctOpenAboutWindow = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('open_about_window');
  }
  window.open('/about.html', 'about', 'width=530,height=504,menubar=no,toolbar=no,location=no,status=no');
  return Promise.resolve();
};

window.sctOpenFaqWindow = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('open_faq_window');
  }
  window.open('/faq.html', 'faq', 'width=632,height=430,menubar=no,toolbar=no,location=no,status=no');
  return Promise.resolve();
};

window.sctGetElementRect = function (selector) {
  const el = document.querySelector(selector);
  if (el) {
    const r = el.getBoundingClientRect();
    return {
      top: Math.round(r.top),
      bottom: Math.round(r.bottom),
      left: Math.round(r.left),
      right: Math.round(window.innerWidth - r.right),
      width: Math.round(r.width),
      height: Math.round(r.height),
      windowHeight: Math.round(window.innerHeight),
      windowWidth: Math.round(window.innerWidth)
    };
  }
  return {
    top: 0,
    bottom: 0,
    left: 0,
    right: 0,
    width: 0,
    height: 0,
    windowHeight: Math.round(window.innerHeight),
    windowWidth: Math.round(window.innerWidth)
  };
};

window.sctStartSteamCmd = function (installDir) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('start_steamcmd', { installDir: installDir || '' });
  }
  return Promise.resolve(false);
};

window.sctClearSteamCmdCache = function (installDir) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('clear_steamcmd_cache', { installDir: installDir || '' });
  }
  return Promise.resolve(false);
};

let sctWidthAnimId = null;
let sctCurrentWindowWidth = window.innerWidth || 820;

window.sctAnimateWindowWidth = function (targetWidth, durationMs = 350) {
  if (sctWidthAnimId) {
    cancelAnimationFrame(sctWidthAnimId);
    sctWidthAnimId = null;
  }

  const startWidth = sctCurrentWindowWidth;
  const delta = targetWidth - startWidth;
  if (Math.abs(delta) < 1) {
    sctCurrentWindowWidth = targetWidth;
    return Promise.resolve();
  }

  const startTime = performance.now();
  const core = getTauriCore();

  function easeInOut(t) {
    return t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
  }

  return new Promise(function (resolve) {
    function step(now) {
      const elapsed = now - startTime;
      const progress = Math.min(elapsed / durationMs, 1);
      const eased = easeInOut(progress);
      const w = Math.round(startWidth + delta * eased);
      sctCurrentWindowWidth = w;

      if (core) {
        core.invoke('window_set_width', { width: w }).catch(function () {});
      }

      if (progress < 1) {
        sctWidthAnimId = requestAnimationFrame(step);
      } else {
        sctCurrentWindowWidth = targetWidth;
        if (core) {
          core.invoke('window_set_width', { width: targetWidth }).catch(function () {});
        }
        sctWidthAnimId = null;
        resolve();
      }
    }
    sctWidthAnimId = requestAnimationFrame(step);
  });
};

window.sctSetWindowWidth = function (width) {
  sctCurrentWindowWidth = width;
  const core = getTauriCore();
  if (core) {
    return core.invoke('window_set_width', { width: width });
  }
  return Promise.resolve();
};

window.sctPickFolder = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('pick_folder');
  }
  return Promise.resolve(null);
};

window.sctPickFile = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('pick_file');
  }
  return Promise.resolve(null);
};

window.sctOpenFolder = function (path) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('open_folder', { path: path });
  }
  return Promise.resolve();
};

window.sctOpenAppLogs = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('open_app_logs');
  }
  return Promise.resolve();
};

window.sctAppendLog = function (message) {
  const core = getTauriCore();
  if (core && message) {
    return core.invoke('append_app_log', { message: String(message) });
  }
  return Promise.resolve();
};

window.sctCheckForUpdates = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('check_for_updates');
  }
  return Promise.reject(new Error('Update check requires the desktop app.'));
};

window.sctIsReleaseMode = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('is_release_mode');
  }
  return Promise.resolve(false);
};

window.sctLoadAppSettingsFile = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('load_app_settings');
  }
  return Promise.resolve(null);
};

window.sctSaveAppSettingsFile = function (json) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('save_app_settings', { json: String(json || '{}') });
  }
  return Promise.resolve();
};

window.sctCheckServerCatalog = function (force) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('check_online_server_catalog', { force: Boolean(force) });
  }
  return Promise.resolve({
    checked: false,
    availableFiles: [],
    availableNames: [],
    updatedFiles: [],
    skippedFiles: [],
    error: null
  });
};

window.sctSyncServerCatalog = function (force) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('sync_online_server_catalog', { force: Boolean(force) });
  }
  return Promise.resolve({
    checked: false,
    availableFiles: [],
    availableNames: [],
    updatedFiles: [],
    skippedFiles: [],
    error: null
  });
};

window.sctLoadServers = function (libraryPath) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('load_servers', { libraryPath: libraryPath || '' });
  }
  return Promise.resolve({
    libraryPath: libraryPath || '',
    libraryExists: false,
    hasSteamcmdFolder: false,
    hasSteamcmdBinary: false,
    sctDataPath: '',
    servers: []
  });
};

window.sctEnsureLibraryLocation = function (libraryPath) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('ensure_library_location', { libraryPath: libraryPath || '' });
  }
  return Promise.resolve(libraryPath || '');
};

window.sctMockInstallServer = function (libraryPath, codename, instanceName) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('mock_install_server', {
      libraryPath: libraryPath || '',
      codename: codename || '',
      instanceName: instanceName || 'Default'
    });
  }
  return Promise.reject(new Error('Install requires the desktop app.'));
};

window.sctDeleteServer = function (libraryPath, codename, instanceName) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('delete_server', {
      libraryPath: libraryPath || '',
      codename: codename || '',
      instanceName: instanceName || 'Default'
    });
  }
  return Promise.reject(new Error('Delete requires the desktop app.'));
};

window.sctRenameServerInstance = function (libraryPath, codename, oldInstanceName, newInstanceName) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('rename_server_instance', {
      libraryPath: libraryPath || '',
      codename: codename || '',
      oldInstanceName: oldInstanceName || 'Default',
      newInstanceName: newInstanceName || ''
    });
  }
  return Promise.reject(new Error('Rename requires the desktop app.'));
};

window.sctLoadInstanceSettings = function (libraryPath, codename, instanceName) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('load_instance_settings', {
      libraryPath: libraryPath || '',
      codename: codename || '',
      instanceName: instanceName || 'Default'
    });
  }
  return Promise.resolve(null);
};

window.sctSaveInstanceSettings = function (libraryPath, codename, instanceName, json) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('save_instance_settings', {
      libraryPath: libraryPath || '',
      codename: codename || '',
      instanceName: instanceName || 'Default',
      json: String(json || '{}')
    });
  }
  return Promise.resolve();
};

window.sctGetAvailableStartOptions = function (libraryPath, codename, instanceName, options) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('get_available_start_options', {
      libraryPath: libraryPath || '',
      codename: codename || '',
      instanceName: instanceName || 'Default',
      options: options || []
    });
  }
  return Promise.resolve([]);
};

window.sctLaunchServerOption = function (serverId, instanceName, filePath, args) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('launch_server_option', {
      serverId: serverId || '',
      instanceName: instanceName || 'Default',
      filePath: filePath || '',
      args: args || ''
    });
  }
  return Promise.reject(new Error('Launching a server requires the desktop app.'));
};

window.sctStopServerInstance = function (serverId, instanceName) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('stop_server_instance', {
      serverId: serverId || '',
      instanceName: instanceName || 'Default'
    });
  }
  return Promise.resolve(false);
};

window.sctGetRunningInstances = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('get_running_instances');
  }
  return Promise.resolve([]);
};

window.sctDownloadSteamCmd = function (libraryPath) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('download_steamcmd', {
      libraryPath: libraryPath || ''
    });
  }
  return Promise.reject(new Error('SteamCMD setup requires the desktop app.'));
};

window.sctInstallOrUpdateSteamServer = function (params) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('install_or_update_steam_server', {
      libraryPath: params.libraryPath || '',
      codename: params.codename || '',
      instanceName: params.instanceName || 'Default',
      steamAppId: params.steamAppId || '',
      serverId: params.serverId || '',
      minimized: Boolean(params.minimized),
      validate: Boolean(params.validate)
    });
  }
  return Promise.reject(new Error('SteamCMD install requires the desktop app.'));
};

window.sctInstallOrUpdateNonSteamServer = function (params) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('install_or_update_non_steam_server', {
      libraryPath: params.libraryPath || '',
      codename: params.codename || '',
      instanceName: params.instanceName || 'Default',
      serverId: params.serverId || '',
      serverName: params.serverName || '',
      downloadLink: params.downloadLink || '',
      areFilesLocal: Boolean(params.areFilesLocal),
      autoExtractFiles: params.autoExtractFiles !== undefined ? Boolean(params.autoExtractFiles) : true,
      sourceFile: params.sourceFile || ''
    });
  }
  return Promise.reject(new Error('Server install requires the desktop app.'));
};

window.sctCancelSteamCmd = function (force) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('cancel_steamcmd', { force: Boolean(force) });
  }
  return Promise.resolve(false);
};

window.sctMarkServerIncomplete = function (libraryPath, codename, instanceName) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('mark_server_incomplete', {
      libraryPath: libraryPath || '',
      codename: codename || '',
      instanceName: instanceName || 'Default'
    });
  }
  return Promise.resolve();
};

window.sctListenServerLogs = function (handler) {
  if (
    window.__TAURI__ &&
    window.__TAURI__.event &&
    typeof window.__TAURI__.event.listen === 'function'
  ) {
    return window.__TAURI__.event.listen('sct-server-log', function (event) {
      if (event && event.payload) {
        handler(event.payload);
      }
    });
  }
  return Promise.resolve(function () {});
};

window.sctListenTaskState = function (handler) {
  if (
    window.__TAURI__ &&
    window.__TAURI__.event &&
    typeof window.__TAURI__.event.listen === 'function'
  ) {
    return window.__TAURI__.event.listen('sct-task-state', function (event) {
      if (event && event.payload) {
        handler(event.payload);
      }
    });
  }
  return Promise.resolve(function () {});
};

window.sctListenServerProcess = function (handler) {
  if (
    window.__TAURI__ &&
    window.__TAURI__.event &&
    typeof window.__TAURI__.event.listen === 'function'
  ) {
    return window.__TAURI__.event.listen('sct-server-process', function (event) {
      if (event && event.payload) {
        handler(event.payload);
      }
    });
  }
  return Promise.resolve(function () {});
};

window.sctOpenFile = function (path, editor) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('open_file', { path: path, editor: editor || null });
  }
  return Promise.resolve();
};

window.sctCopyToClipboard = function (text) {
  if (navigator.clipboard && navigator.clipboard.writeText) {
    return navigator.clipboard.writeText(text);
  }
  const ta = document.createElement('textarea');
  ta.value = text;
  document.body.appendChild(ta);
  ta.select();
  document.execCommand('copy');
  document.body.removeChild(ta);
  return Promise.resolve();
};

window.sctPickBackupSavePath = function (defaultFileName) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('pick_backup_save_path', {
      defaultFileName: defaultFileName || 'server_backup.zip'
    });
  }
  return Promise.resolve(null);
};

window.sctPickBackupOpenPath = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('pick_backup_open_path');
  }
  return Promise.resolve(null);
};

window.sctBackupServer = function (params) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('backup_server', {
      libraryPath: params.libraryPath || '',
      codename: params.codename || '',
      instanceName: params.instanceName || 'Default',
      serverId: params.serverId || '',
      serverName: params.serverName || '',
      destinationPath: params.destinationPath || null
    });
  }
  return Promise.reject(new Error('Backup requires the desktop app.'));
};

window.sctCancelBackup = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('cancel_backup_task');
  }
  return Promise.resolve();
};

window.sctCancelRestore = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('cancel_restore_task');
  }
  return Promise.resolve();
};

window.sctCancelDelete = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('cancel_delete_task');
  }
  return Promise.resolve();
};

window.sctListServerBackups = function (libraryPath, codename, instanceName) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('list_server_backups', {
      libraryPath: libraryPath || '',
      codename: codename || '',
      instanceName: instanceName || 'Default'
    });
  }
  return Promise.resolve([]);
};

window.sctDeleteServerBackup = function (params) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('delete_server_backup', {
      libraryPath: params.libraryPath || '',
      codename: params.codename || '',
      instanceName: params.instanceName || 'Default',
      backupPath: params.backupPath || ''
    });
  }
  return Promise.reject(new Error('Delete backup requires the desktop app.'));
};

window.sctRestoreServerBackup = function (params) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('restore_server_backup', {
      libraryPath: params.libraryPath || '',
      codename: params.codename || '',
      instanceName: params.instanceName || 'Default',
      serverId: params.serverId || '',
      serverName: params.serverName || '',
      backupPath: params.backupPath || ''
    });
  }
  return Promise.reject(new Error('Restore requires the desktop app.'));
};

window.sctVerifyServerPackage = function (sourceFile) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('verify_server_package', {
      sourceFile: sourceFile || ''
    });
  }
  return Promise.resolve({
    isOfficial: false,
    status: 'unknown',
    reason: 'Desktop app required.',
    packageSha256: ''
  });
};

window.sctExecuteSimpleAction = function (params) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('execute_simple_action', {
      libraryPath: params.libraryPath || '',
      codename: params.codename || '',
      folderName: params.folderName || '',
      sourceFile: params.sourceFile || '',
      instanceName: params.instanceName || 'Default',
      serverId: params.serverId || '',
      actionId: params.actionId || '',
      actionsList: params.actionsList || [],
      properties: params.properties || {},
      allowUntrusted: Boolean(params.allowUntrusted)
    });
  }
  return Promise.reject(new Error('SimpleActions require the desktop app.'));
};

window.sctExecuteFuncsMethod = function (params) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('execute_funcs_method', {
      libraryPath: params.libraryPath || '',
      codename: params.codename || '',
      folderName: params.folderName || '',
      sourceFile: params.sourceFile || '',
      instanceName: params.instanceName || 'Default',
      serverId: params.serverId || '',
      methodName: params.methodName || '',
      optional: Boolean(params.optional),
      properties: params.properties || {},
      allowUntrusted: Boolean(params.allowUntrusted)
    });
  }
  return Promise.reject(new Error('_funcs.lua execution requires the desktop app.'));
};

window.sctShowContextMenu = async function (x, y, files, libraryPath, serverId) {
  const core = getTauriCore();
  if (core) {
    try {
      await core.invoke('show_settings_context_menu', {
        x: x,
        y: y,
        files: files || [],
        libraryPath: libraryPath || '',
        serverId: serverId || ''
      });
      return true;
    } catch (e) {
      console.error(e);
    }
  }
  return false;
};

document.addEventListener('mousedown', function (e) {
  const titlebar = e.target.closest('#sct-titlebar') || e.target.closest('.sct-about-titlebar');
  if (titlebar) {
    if (
      e.target.closest('.sct-titlebar-controls') ||
      e.target.closest('.sct-about-close-btn') ||
      e.target.closest('button')
    ) {
      return;
    }
    if (e.button === 0) {
      window.sctDragWindow();
    }
  }
});
