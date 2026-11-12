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

window.sctCheckForUpdates = function () {
  const core = getTauriCore();
  if (core) {
    return core.invoke('check_for_updates');
  }
  return Promise.reject(new Error('Update check requires the desktop app.'));
};

window.sctLoadSettings = function () {
  try {
    return window.localStorage ? window.localStorage.getItem('sct_app_settings') : null;
  } catch {
    return null;
  }
};

window.sctSaveSettings = function (json) {
  try {
    if (window.localStorage) {
      window.localStorage.setItem('sct_app_settings', json);
    }
  } catch {}
};

window.sctLoadFavorites = function () {
  try {
    const raw = window.localStorage ? window.localStorage.getItem('sct_favorites') : null;
    if (!raw) return [];
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

window.sctSaveFavorites = function (list) {
  try {
    if (window.localStorage) {
      window.localStorage.setItem('sct_favorites', JSON.stringify(list || []));
    }
  } catch {}
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

window.sctOpenFile = function (path) {
  const core = getTauriCore();
  if (core) {
    return core.invoke('open_file', { path: path });
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
