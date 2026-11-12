(function () {
  function escapeHtml(value) {
    return String(value ?? '')
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;');
  }

  let activeModal = null;
  let isFlashing = false;

  function getRoot() {
    return document.getElementById('sct-modal-root');
  }

  function closeModal(result) {
    if (!activeModal) return;
    const resolver = activeModal.resolve;
    activeModal = null;
    render();
    if (resolver) {
      resolver(result);
    }
  }

  function flashModal() {
    if (isFlashing) return;
    isFlashing = true;
    const win = document.querySelector('#sct-modal-root .sct-modal-window');
    if (win) {
      win.classList.add('sct-modal-flash');
    }
    setTimeout(function () {
      isFlashing = false;
      const currentWin = document.querySelector('#sct-modal-root .sct-modal-window');
      if (currentWin) {
        currentWin.classList.remove('sct-modal-flash');
      }
    }, 380);
  }

  function render() {
    const root = getRoot();
    if (!root) return;

    if (!activeModal) {
      root.innerHTML = '';
      return;
    }

    const m = activeModal;
    let bodyHtml = '';
    let footerHtml = '';

    if (m.type === 'alert') {
      bodyHtml =
        '<div class="sct-modal-body sct-msgbox-body">' +
        '<div class="sct-msgbox-row">' +
        '<span class="sct-msgbox-icon">' + escapeHtml(m.icon) + '</span>' +
        '<div class="sct-msgbox-text">' + escapeHtml(m.message) + '</div>' +
        '</div>' +
        '</div>';
      footerHtml =
        '<div class="sct-modal-footer">' +
        '<button type="button" class="sct-btn" data-modal-action="alert-ok">OK</button>' +
        '</div>';
    } else if (m.type === 'confirm') {
      bodyHtml =
        '<div class="sct-modal-body sct-msgbox-body">' +
        '<div class="sct-msgbox-row">' +
        '<span class="sct-msgbox-icon">' + escapeHtml(m.icon) + '</span>' +
        '<div class="sct-msgbox-text">' + escapeHtml(m.message) + '</div>' +
        '</div>' +
        '</div>';
      footerHtml =
        '<div class="sct-modal-footer">' +
        '<button type="button" class="sct-btn" data-modal-action="confirm-yes">' + escapeHtml(m.okLabel) + '</button>' +
        '<button type="button" class="sct-btn" data-modal-action="confirm-no">' + escapeHtml(m.cancelLabel) + '</button>' +
        '</div>';
    } else if (m.type === 'update') {
      const msg =
        'A new version of Server Creation Tool is available!\n\n' +
        'Current version: ' + m.currentVersion + '\n' +
        'Latest version: ' + m.latestVersion + '\n\n' +
        'Would you like to open the GitHub Releases page to download it?';
      bodyHtml =
        '<div class="sct-modal-body sct-msgbox-body" style="flex-direction: column; align-items: stretch; gap: 12px;">' +
        '<div class="sct-msgbox-row">' +
        '<span class="sct-msgbox-icon">🔄</span>' +
        '<div class="sct-msgbox-text">' + escapeHtml(msg) + '</div>' +
        '</div>' +
        '<label style="display: flex; align-items: center; gap: 8px; font-size: 12px; color: #cbd5e1; cursor: pointer; padding-left: 46px;">' +
        '<input type="checkbox" id="sct-modal-update-disable" ' + (m.disableAutoCheck ? 'checked' : '') + ' />' +
        'Disable automatic update checks on startup' +
        '</label>' +
        '</div>';
      footerHtml =
        '<div class="sct-modal-footer">' +
        '<button type="button" class="sct-btn" data-modal-action="update-download">Download</button>' +
        '<button type="button" class="sct-btn" data-modal-action="update-later">Later</button>' +
        '</div>';
    } else if (m.type === 'settings') {
      const s = m.tempSettings;
      bodyHtml =
        '<div class="sct-modal-body" style="display: flex; flex-direction: column; gap: 14px;">' +
        '<div>' +
        '<label class="sct-field-label">Server Library Location:</label>' +
        '<div style="display: flex; gap: 6px; margin-top: 4px;">' +
        '<input type="text" id="sct-modal-lib-path" class="sct-input" style="flex: 1;" value="' + escapeHtml(s.serverLibraryPath) + '" />' +
        '<button type="button" class="sct-btn" style="padding: 4px 10px;" data-modal-action="settings-browse">Browse...</button>' +
        '</div>' +
        '</div>' +
        '<div style="display: flex; flex-direction: column; gap: 8px; margin-top: 4px;">' +
        '<label style="display: flex; align-items: center; gap: 8px; font-size: 12px; cursor: pointer;">' +
        '<input type="checkbox" id="sct-modal-auto-updates" ' + (s.autoCheckUpdates ? 'checked' : '') + ' />' +
        'Automatically check for updates on startup' +
        '</label>' +
        '<label style="display: flex; align-items: center; gap: 8px; font-size: 12px; cursor: pointer;">' +
        '<input type="checkbox" id="sct-modal-close-tray" ' + (s.closeToTray ? 'checked' : '') + ' />' +
        'Minimize to system tray when closing' +
        '</label>' +
        '<label style="display: flex; align-items: center; gap: 8px; font-size: 12px; cursor: pointer;">' +
        '<input type="checkbox" id="sct-modal-steamcmd-min" ' + (s.runSteamCmdMinimized ? 'checked' : '') + ' />' +
        'Run SteamCMD in background / minimized' +
        '</label>' +
        '</div>' +
        '</div>';
      footerHtml =
        '<div class="sct-modal-footer">' +
        '<button type="button" class="sct-btn" data-modal-action="settings-save">Save Settings</button>' +
        '<button type="button" class="sct-btn" data-modal-action="settings-cancel">Cancel</button>' +
        '</div>';
    } else if (m.type === 'guide') {
      bodyHtml =
        '<div class="sct-modal-body" style="max-height: 320px; overflow-y: auto;">' +
        '<div style="white-space: pre-wrap; font-size: 12px; line-height: 1.6; color: #cbd5e1; background: #16202e; padding: 12px; border-radius: 4px; border: 1px solid #2d3f58;">' +
        escapeHtml(m.guideText) +
        '</div>' +
        '</div>';
      footerHtml =
        '<div class="sct-modal-footer">' +
        '<button type="button" class="sct-btn" data-modal-action="close">Done</button>' +
        '</div>';
    }

    root.innerHTML =
      '<div class="sct-modal-backdrop" data-modal-backdrop="true">' +
      '<div class="sct-window-frame sct-modal-window ' + escapeHtml(m.modalClass) + '">' +
      '<div class="sct-titlebar">' +
      '<div class="sct-titlebar-left">' +
      '<img class="sct-titlebar-icon sct-modal-titlebar-icon" src="' + escapeHtml(m.iconUrl) + '" alt="" />' +
      '<span class="sct-titlebar-title">' + escapeHtml(m.title) + '</span>' +
      '</div>' +
      '<div class="sct-titlebar-controls">' +
      '<button type="button" class="sct-win-btn sct-win-btn-close" data-modal-action="titlebar-close" title="Close" aria-label="Close"></button>' +
      '</div>' +
      '</div>' +
      '<div class="sct-window-inner">' +
      '<div class="sct-border-left"></div>' +
      '<div class="sct-modal-client">' +
      bodyHtml +
      footerHtml +
      '</div>' +
      '<div class="sct-border-right"></div>' +
      '</div>' +
      '<div class="sct-border-bottom"></div>' +
      '</div>' +
      '</div>';

    bindEvents(root);
  }

  function syncSettingsFromDom() {
    if (!activeModal || activeModal.type !== 'settings') return;
    const libInput = document.getElementById('sct-modal-lib-path');
    const autoUpdates = document.getElementById('sct-modal-auto-updates');
    const closeTray = document.getElementById('sct-modal-close-tray');
    const steamCmdMin = document.getElementById('sct-modal-steamcmd-min');

    if (libInput) activeModal.tempSettings.serverLibraryPath = libInput.value;
    if (autoUpdates) activeModal.tempSettings.autoCheckUpdates = autoUpdates.checked;
    if (closeTray) activeModal.tempSettings.closeToTray = closeTray.checked;
    if (steamCmdMin) activeModal.tempSettings.runSteamCmdMinimized = steamCmdMin.checked;
  }

  function bindEvents(root) {
    const backdrop = root.querySelector('[data-modal-backdrop]');
    if (backdrop) {
      backdrop.addEventListener('click', function (e) {
        if (e.target === backdrop) {
          flashModal();
        }
      });
    }

    root.querySelectorAll('[data-modal-action]').forEach(function (btn) {
      btn.addEventListener('click', async function (e) {
        e.stopPropagation();
        const action = btn.getAttribute('data-modal-action');
        if (!activeModal) return;

        if (action === 'alert-ok' || action === 'close') {
          closeModal(true);
        } else if (action === 'confirm-yes') {
          closeModal(true);
        } else if (action === 'confirm-no') {
          closeModal(false);
        } else if (action === 'update-download' || action === 'update-later') {
          const cb = document.getElementById('sct-modal-update-disable');
          const disableAutoCheck = cb ? cb.checked : Boolean(activeModal.disableAutoCheck);
          closeModal({
            openReleasePage: action === 'update-download',
            disableAutoCheck: disableAutoCheck
          });
        } else if (action === 'settings-browse') {
          syncSettingsFromDom();
          try {
            const picked = await window.sctPickFolder();
            if (picked && activeModal && activeModal.type === 'settings') {
              activeModal.tempSettings.serverLibraryPath = picked;
              const libInput = document.getElementById('sct-modal-lib-path');
              if (libInput) libInput.value = picked;
            }
          } catch {}
        } else if (action === 'settings-save') {
          syncSettingsFromDom();
          closeModal(Object.assign({}, activeModal.tempSettings));
        } else if (action === 'settings-cancel') {
          closeModal(null);
        } else if (action === 'titlebar-close') {
          if (activeModal.type === 'confirm') {
            closeModal(false);
          } else if (activeModal.type === 'settings') {
            closeModal(null);
          } else if (activeModal.type === 'update') {
            const cb = document.getElementById('sct-modal-update-disable');
            closeModal({
              openReleasePage: false,
              disableAutoCheck: cb ? cb.checked : Boolean(activeModal.disableAutoCheck)
            });
          } else {
            closeModal(true);
          }
        }
      });
    });
  }

  window.SctModal = {
    alert: function (message, title, icon) {
      return new Promise(function (resolve) {
        activeModal = {
          type: 'alert',
          title: title || 'Information',
          message: message || '',
          icon: icon || 'ℹ️',
          iconUrl: 'img/icons/icons8-server-24.png',
          modalClass: 'sct-modal-msgbox',
          resolve: resolve
        };
        render();
      });
    },

    confirm: function (message, title, icon, okLabel, cancelLabel) {
      return new Promise(function (resolve) {
        activeModal = {
          type: 'confirm',
          title: title || 'Confirm',
          message: message || '',
          icon: icon || '❓',
          okLabel: okLabel || 'Yes',
          cancelLabel: cancelLabel || 'No',
          iconUrl: 'img/icons/icons8-server-24.png',
          modalClass: 'sct-modal-msgbox',
          resolve: resolve
        };
        render();
      });
    },

    showUpdateAvailable: function (currentVersion, latestVersion, disableAutoCheckInitially) {
      return new Promise(function (resolve) {
        activeModal = {
          type: 'update',
          title: 'Update Available',
          currentVersion: currentVersion || '',
          latestVersion: latestVersion || '',
          disableAutoCheck: Boolean(disableAutoCheckInitially),
          iconUrl: 'img/icons/icons8-server-24.png',
          modalClass: 'sct-modal-msgbox',
          resolve: resolve
        };
        render();
      });
    },

    showSettings: function (currentSettings) {
      return new Promise(function (resolve) {
        activeModal = {
          type: 'settings',
          title: 'Settings',
          tempSettings: {
            serverLibraryPath: currentSettings.serverLibraryPath,
            language: currentSettings.language,
            autoCheckUpdates: Boolean(currentSettings.autoCheckUpdates),
            closeToTray: Boolean(currentSettings.closeToTray),
            runSteamCmdMinimized: Boolean(currentSettings.runSteamCmdMinimized),
            defaultTextEditor: currentSettings.defaultTextEditor || 'notepad.exe'
          },
          iconUrl: 'img/icons/settingsBtn.Image.png',
          modalClass: 'sct-modal-settings',
          resolve: resolve
        };
        render();
      });
    },

    showGuide: function (gameTitle, guideContent) {
      return new Promise(function (resolve) {
        activeModal = {
          type: 'guide',
          title: 'Installation Guide: ' + (gameTitle || ''),
          guideText: guideContent || '',
          iconUrl: 'img/icons/1091.png',
          modalClass: 'sct-modal-guide',
          resolve: resolve
        };
        render();
      });
    }
  };
})();
