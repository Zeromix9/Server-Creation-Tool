function getTauriCore() {
  if (window.__TAURI__ && window.__TAURI__.core) {
    return window.__TAURI__.core;
  }
  return null;
}

function getTauriWindow() {
  if (window.__TAURI__ && window.__TAURI__.window && typeof window.__TAURI__.window.getCurrentWindow === 'function') {
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

document.addEventListener('mousedown', function (e) {
  const titlebar = e.target.closest('#sct-titlebar') || e.target.closest('.sct-about-titlebar');
  if (titlebar) {
    if (e.target.closest('.sct-titlebar-controls') || e.target.closest('.sct-about-close-btn') || e.target.closest('button')) {
      return;
    }
    if (e.button === 0) {
      window.sctDragWindow();
    }
  }
});
