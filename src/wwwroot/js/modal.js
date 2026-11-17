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
      const startupList = Array.isArray(m.tempStartupServers) ? m.tempStartupServers : [];
      const startupListHtml =
        startupList.length === 0
          ? '<div class="sct-settings-startup-empty">No servers are set to run on startup.</div>'
          : startupList
              .map(function (item, idx) {
                const inst = item.instanceName || 'Default';
                return (
                  '<div class="sct-settings-startup-row">' +
                  '<span class="sct-settings-startup-name">' +
                  escapeHtml((item.serverName || item.serverId) + ' (' + inst + ')') +
                  '</span>' +
                  '<button type="button" class="sct-btn sct-btn-danger sct-settings-startup-remove" data-remove-startup-idx="' +
                  idx +
                  '">Remove</button>' +
                  '</div>'
                );
              })
              .join('');

      bodyHtml =
        '<div class="sct-modal-body" style="display: flex; flex-direction: column; gap: 14px;">' +
        '<div>' +
        '<label class="sct-field-label">Server Library Location:</label>' +
        '<div style="display: flex; gap: 6px; margin-top: 4px;">' +
        '<input type="text" id="sct-modal-lib-path" class="sct-input" style="flex: 1;" value="' + escapeHtml(s.serverLibraryPath) + '" />' +
        '<button type="button" class="sct-btn" style="padding: 4px 10px;" data-modal-action="settings-browse">Browse...</button>' +
        '</div>' +
        '</div>' +
        '<div>' +
        '<label class="sct-field-label">Default Text Editor:</label>' +
        '<div style="display: flex; gap: 6px; margin-top: 4px;">' +
        '<input type="text" id="sct-modal-text-editor" class="sct-input" style="flex: 1;" value="' + escapeHtml(s.defaultTextEditor || 'notepad.exe') + '" placeholder="notepad.exe" />' +
        '<button type="button" class="sct-btn" style="padding: 4px 10px;" data-modal-action="settings-browse-editor">Browse...</button>' +
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
        '<input type="checkbox" id="sct-modal-run-startup" ' + (s.runOnStartup ? 'checked' : '') + ' />' +
        'Run on startup' +
        '</label>' +
        '</div>' +
        '<div>' +
        '<label class="sct-field-label">Servers Set to Run on Startup:</label>' +
        '<div class="sct-settings-startup-list">' +
        startupListHtml +
        '</div>' +
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
    } else if (m.type === 'launch') {
      const optionsHtml = m.options
        .map(function (opt) {
          const isSelected = opt.id === m.selectedId;
          const isDefault = opt.id === m.defaultId;
          return (
            '<div class="sct-launch-option-row ' +
            (isSelected ? 'selected' : '') +
            '" data-launch-opt-id="' +
            escapeHtml(opt.id) +
            '">' +
            '<div class="sct-launch-option-info">' +
            '<div class="sct-launch-option-title">' +
            escapeHtml(opt.label) +
            '</div>' +
            '<div class="sct-launch-option-sub">' +
            escapeHtml(opt.filePath) +
            '</div>' +
            '</div>' +
            '<label class="sct-launch-default-radio" title="Set as default launch option" data-launch-radio-wrap="true">' +
            '<input type="radio" name="sct-launch-default" data-launch-default-id="' +
            escapeHtml(opt.id) +
            '" ' +
            (isDefault ? 'checked' : '') +
            ' />' +
            '<span>Default</span>' +
            '</label>' +
            '</div>'
          );
        })
        .join('');

      bodyHtml =
        '<div class="sct-modal-body sct-launch-body">' +
        '<div class="sct-launch-subtitle">Choose how to launch ' +
        escapeHtml(m.serverName) +
        ':</div>' +
        '<div class="sct-launch-options-list">' +
        optionsHtml +
        '</div>' +
        '</div>';
      footerHtml =
        '<div class="sct-modal-footer">' +
        '<button type="button" class="sct-btn sct-btn-play" data-modal-action="launch-play">▶ Play</button>' +
        '<button type="button" class="sct-btn" data-modal-action="launch-cancel">Cancel</button>' +
        '</div>';
    } else if (m.type === 'backup-dest') {
      const msg =
        'Where would you like to save the backup for ' +
        m.serverName +
        ' (' +
        m.instanceName +
        ')?\n\n' +
        'Default folder:\n' +
        m.defaultFolderHint;
      bodyHtml =
        '<div class="sct-modal-body sct-msgbox-body">' +
        '<div class="sct-msgbox-row">' +
        '<span class="sct-msgbox-icon">💾</span>' +
        '<div class="sct-msgbox-text">' + escapeHtml(msg) + '</div>' +
        '</div>' +
        '</div>';
      footerHtml =
        '<div class="sct-modal-footer">' +
        '<button type="button" class="sct-btn" data-modal-action="backup-dest-default">Default</button>' +
        '<button type="button" class="sct-btn" data-modal-action="backup-dest-custom">Choose location</button>' +
        '<button type="button" class="sct-btn" data-modal-action="backup-dest-cancel">Cancel</button>' +
        '</div>';
    } else if (m.type === 'restore-light') {
      const formatDate = function (ms) {
        if (!ms) return '';
        const d = new Date(ms);
        if (Number.isNaN(d.getTime())) return '';
        const pad = function (n) {
          return String(n).padStart(2, '0');
        };
        return (
          d.getFullYear() +
          '-' +
          pad(d.getMonth() + 1) +
          '-' +
          pad(d.getDate()) +
          ' ' +
          pad(d.getHours()) +
          ':' +
          pad(d.getMinutes())
        );
      };

      const listHtml =
        m.backups.length === 0
          ? '<div class="sct-restore-empty">' +
            'No backups found for <strong>' +
            escapeHtml(m.serverName) +
            ' (' +
            escapeHtml(m.instanceName) +
            ')</strong>.<br />Click <strong>Browse Elsewhere...</strong> below to select a backup archive from your PC.' +
            '</div>'
          : m.backups
              .map(function (b) {
                const isSelected = b.fullPath === m.selectedPath;
                const dateText = formatDate(b.modifiedMs);
                return (
                  '<div class="sct-restore-item ' +
                  (isSelected ? 'selected' : '') +
                  '" data-restore-path="' +
                  escapeHtml(b.fullPath) +
                  '">' +
                  '<div class="sct-restore-item-main">' +
                  '<div class="sct-restore-item-name">' +
                  escapeHtml(b.fileName) +
                  '</div>' +
                  '<div class="sct-restore-item-meta">' +
                  escapeHtml(dateText ? dateText + '  •  ' + b.size : b.size) +
                  '</div>' +
                  '</div>' +
                  '</div>'
                );
              })
              .join('');

      const pendingDeleteItem =
        m.confirmDeletePath &&
        m.backups.find(function (b) {
          return b.fullPath === m.confirmDeletePath;
        });
      const deleteConfirmHtml = pendingDeleteItem
        ? '<div class="sct-restore-delete-confirm">' +
          '<span class="sct-restore-delete-confirm-text">Delete <strong>' +
          escapeHtml(pendingDeleteItem.fileName) +
          '</strong>?</span>' +
          '<div class="sct-restore-delete-confirm-actions">' +
          '<button type="button" class="sct-btn sct-btn-danger" data-modal-action="restore-delete-confirm" ' +
          (m.isDeletingBackup ? 'disabled' : '') +
          '>' +
          (m.isDeletingBackup ? 'Deleting...' : 'Confirm Delete') +
          '</button>' +
          '<button type="button" class="sct-btn" data-modal-action="restore-delete-cancel" ' +
          (m.isDeletingBackup ? 'disabled' : '') +
          '>Cancel</button>' +
          '</div>' +
          '</div>'
        : '';
      const deleteErrorHtml = m.deleteError
        ? '<div class="sct-restore-delete-error">' +
          escapeHtml(m.deleteError) +
          '</div>'
        : '';

      bodyHtml =
        '<div class="sct-modal-body sct-restore-body">' +
        '<div class="sct-restore-subtitle">Select a backup for ' +
        escapeHtml(m.serverName) +
        ' (' +
        escapeHtml(m.instanceName) +
        '):</div>' +
        '<div class="sct-restore-list">' +
        listHtml +
        '</div>' +
        deleteConfirmHtml +
        deleteErrorHtml +
        '</div>';
      footerHtml =
        '<div class="sct-modal-footer sct-restore-footer">' +
        '<button type="button" class="sct-btn" data-modal-action="restore-browse">Browse Elsewhere...</button>' +
        '<div class="sct-restore-footer-right">' +
        '<button type="button" class="sct-btn sct-btn-danger" id="sct-restore-delete-btn" data-modal-action="restore-delete" ' +
        (!m.selectedPath || m.isDeletingBackup ? 'disabled' : '') +
        '>Delete</button>' +
        '<button type="button" class="sct-btn" id="sct-restore-confirm-btn" data-modal-action="restore-confirm" ' +
        (!m.selectedPath || m.isDeletingBackup ? 'disabled' : '') +
        '>Restore</button>' +
        '<button type="button" class="sct-btn" data-modal-action="restore-cancel">Cancel</button>' +
        '</div>' +
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
    const editorInput = document.getElementById('sct-modal-text-editor');
    const autoUpdates = document.getElementById('sct-modal-auto-updates');
    const closeTray = document.getElementById('sct-modal-close-tray');
    const runStartup = document.getElementById('sct-modal-run-startup');

    if (libInput) activeModal.tempSettings.serverLibraryPath = libInput.value;
    if (editorInput) activeModal.tempSettings.defaultTextEditor = editorInput.value.trim() || 'notepad.exe';
    if (autoUpdates) activeModal.tempSettings.autoCheckUpdates = autoUpdates.checked;
    if (closeTray) activeModal.tempSettings.closeToTray = closeTray.checked;
    if (runStartup) activeModal.tempSettings.runOnStartup = runStartup.checked;
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
        } else if (action === 'settings-browse-editor') {
          syncSettingsFromDom();
          try {
            const picked = await window.sctPickFile();
            if (picked && activeModal && activeModal.type === 'settings') {
              activeModal.tempSettings.defaultTextEditor = picked;
              const editorInput = document.getElementById('sct-modal-text-editor');
              if (editorInput) editorInput.value = picked;
            }
          } catch {}
        } else if (action === 'settings-save') {
          syncSettingsFromDom();
          const result = Object.assign({}, activeModal.tempSettings, {
            startupServers: Array.isArray(activeModal.tempStartupServers)
              ? activeModal.tempStartupServers.slice()
              : []
          });
          closeModal(result);
        } else if (action === 'settings-cancel' || action === 'launch-cancel' || action === 'backup-dest-cancel' || action === 'restore-cancel') {
          closeModal(null);
        } else if (action === 'backup-dest-default') {
          closeModal('default');
        } else if (action === 'backup-dest-custom') {
          closeModal('custom');
        } else if (action === 'restore-confirm') {
          const chosen =
            activeModal.backups.find(function (b) {
              return b.fullPath === activeModal.selectedPath;
            }) || null;
          if (chosen) {
            closeModal(chosen);
          }
        } else if (action === 'restore-delete') {
          if (activeModal.selectedPath && !activeModal.isDeletingBackup) {
            activeModal.confirmDeletePath = activeModal.selectedPath;
            activeModal.deleteError = '';
            render();
          }
        } else if (action === 'restore-delete-cancel') {
          if (!activeModal.isDeletingBackup) {
            activeModal.confirmDeletePath = '';
            render();
          }
        } else if (action === 'restore-delete-confirm') {
          const targetPath = activeModal.confirmDeletePath;
          const targetItem =
            activeModal.backups.find(function (b) {
              return b.fullPath === targetPath;
            }) || null;
          if (
            targetItem &&
            typeof activeModal.onDeleteBackup === 'function' &&
            !activeModal.isDeletingBackup
          ) {
            activeModal.isDeletingBackup = true;
            activeModal.deleteError = '';
            render();
            try {
              await activeModal.onDeleteBackup(targetItem);
              if (activeModal && activeModal.type === 'restore-light') {
                const idx = activeModal.backups.findIndex(function (b) {
                  return b.fullPath === targetPath;
                });
                activeModal.backups = activeModal.backups.filter(function (b) {
                  return b.fullPath !== targetPath;
                });
                const nextItem =
                  activeModal.backups[idx] ||
                  activeModal.backups[idx - 1] ||
                  activeModal.backups[0] ||
                  null;
                activeModal.selectedPath = nextItem ? nextItem.fullPath : '';
                activeModal.confirmDeletePath = '';
              }
            } catch (err) {
              if (activeModal && activeModal.type === 'restore-light') {
                activeModal.deleteError =
                  (err && err.message) || String(err || 'Failed to delete backup.');
              }
            } finally {
              if (activeModal && activeModal.type === 'restore-light') {
                activeModal.isDeletingBackup = false;
                render();
              }
            }
          }
        } else if (action === 'restore-browse') {
          if (typeof activeModal.onBrowseElsewhere === 'function') {
            try {
              const picked = await activeModal.onBrowseElsewhere();
              if (picked && activeModal && activeModal.type === 'restore-light') {
                const name = picked.split(/[\\/]/).pop() || picked;
                closeModal({ fullPath: picked, fileName: name });
              }
            } catch {}
          }
        } else if (action === 'launch-play') {
          const chosen =
            activeModal.options.find(function (opt) {
              return opt.id === activeModal.selectedId;
            }) || activeModal.options[0] || null;
          closeModal(chosen);
        } else if (action === 'titlebar-close') {
          if (activeModal.type === 'confirm') {
            closeModal(false);
          } else if (
            activeModal.type === 'settings' ||
            activeModal.type === 'launch' ||
            activeModal.type === 'backup-dest' ||
            activeModal.type === 'restore-light'
          ) {
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

    root.querySelectorAll('[data-remove-startup-idx]').forEach(function (btn) {
      btn.addEventListener('click', function (e) {
        e.stopPropagation();
        if (!activeModal || activeModal.type !== 'settings') return;
        syncSettingsFromDom();
        const idx = parseInt(btn.getAttribute('data-remove-startup-idx'), 10);
        if (!Number.isNaN(idx) && Array.isArray(activeModal.tempStartupServers)) {
          activeModal.tempStartupServers.splice(idx, 1);
          render();
        }
      });
    });

    root.querySelectorAll('[data-launch-opt-id]').forEach(function (row) {
      row.addEventListener('click', function (e) {
        if (!activeModal || activeModal.type !== 'launch') return;
        if (e.target.closest('[data-launch-radio-wrap]')) return;
        const optId = row.getAttribute('data-launch-opt-id');
        activeModal.selectedId = optId;
        root.querySelectorAll('[data-launch-opt-id]').forEach(function (el) {
          el.classList.toggle('selected', el.getAttribute('data-launch-opt-id') === optId);
        });
      });

      row.addEventListener('dblclick', function (e) {
        if (!activeModal || activeModal.type !== 'launch') return;
        if (e.target.closest('[data-launch-radio-wrap]')) return;
        const optId = row.getAttribute('data-launch-opt-id');
        const chosen = activeModal.options.find(function (opt) {
          return opt.id === optId;
        });
        if (chosen) {
          closeModal(chosen);
        }
      });
    });

    root.querySelectorAll('[data-launch-default-id]').forEach(function (radio) {
      radio.addEventListener('change', function () {
        if (!activeModal || activeModal.type !== 'launch' || !radio.checked) return;
        const optId = radio.getAttribute('data-launch-default-id');
        activeModal.defaultId = optId;
        activeModal.selectedId = optId;
        if (typeof activeModal.onSetDefault === 'function') {
          activeModal.onSetDefault(optId);
        }
        root.querySelectorAll('[data-launch-opt-id]').forEach(function (el) {
          el.classList.toggle('selected', el.getAttribute('data-launch-opt-id') === optId);
        });
      });
    });

    root.querySelectorAll('[data-restore-path]').forEach(function (row) {
      row.addEventListener('click', function () {
        if (!activeModal || activeModal.type !== 'restore-light') return;
        const path = row.getAttribute('data-restore-path');
        activeModal.selectedPath = path;
        if (activeModal.confirmDeletePath && activeModal.confirmDeletePath !== path) {
          activeModal.confirmDeletePath = '';
        }
        activeModal.deleteError = '';
        root.querySelectorAll('[data-restore-path]').forEach(function (el) {
          el.classList.toggle('selected', el.getAttribute('data-restore-path') === path);
        });
        const confirmBtn = document.getElementById('sct-restore-confirm-btn');
        if (confirmBtn) {
          confirmBtn.disabled = !path || Boolean(activeModal.isDeletingBackup);
        }
        const deleteBtn = document.getElementById('sct-restore-delete-btn');
        if (deleteBtn) {
          deleteBtn.disabled = !path || Boolean(activeModal.isDeletingBackup);
        }
      });

      row.addEventListener('dblclick', function () {
        if (!activeModal || activeModal.type !== 'restore-light') return;
        const path = row.getAttribute('data-restore-path');
        const chosen = activeModal.backups.find(function (b) {
          return b.fullPath === path;
        });
        if (chosen) {
          closeModal(chosen);
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

    showSettings: function (currentSettings, startupServers) {
      return new Promise(function (resolve) {
        const list = Array.isArray(startupServers)
          ? startupServers.map(function (item) {
              return Object.assign({}, item);
            })
          : [];
        activeModal = {
          type: 'settings',
          title: 'Settings',
          tempSettings: {
            serverLibraryPath: currentSettings.serverLibraryPath,
            language: currentSettings.language,
            autoCheckUpdates: Boolean(currentSettings.autoCheckUpdates),
            closeToTray: Boolean(currentSettings.closeToTray),
            runOnStartup: Boolean(currentSettings.runOnStartup),
            defaultTextEditor: currentSettings.defaultTextEditor || 'notepad.exe'
          },
          tempStartupServers: list,
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
    },

    showStartOptions: function (serverName, options, defaultOptionId, onSetDefault) {
      return new Promise(function (resolve) {
        const list = Array.isArray(options) ? options : [];
        const matchedSaved =
          defaultOptionId &&
          list.find(function (opt) {
            return opt.id === defaultOptionId;
          });
        const matchedMeta = list.find(function (opt) {
          return Boolean(opt.isDefault);
        });
        const initialId =
          (matchedSaved && matchedSaved.id) ||
          (matchedMeta && matchedMeta.id) ||
          (list[0] && list[0].id) ||
          '';

        activeModal = {
          type: 'launch',
          title: 'Select launch option',
          serverName: serverName || 'Server',
          options: list,
          selectedId: initialId,
          defaultId: initialId,
          onSetDefault: onSetDefault,
          iconUrl: 'img/icons/start_img.png',
          modalClass: 'sct-modal-launch',
          resolve: resolve
        };
        render();
      });
    },

    showBackupDestination: function (serverName, instanceName, defaultFolderHint) {
      return new Promise(function (resolve) {
        activeModal = {
          type: 'backup-dest',
          title: 'Backup Server',
          serverName: serverName || 'Server',
          instanceName: instanceName || 'Default',
          defaultFolderHint: defaultFolderHint || '',
          iconUrl: 'img/icons/icons8-server-24.png',
          modalClass: 'sct-modal-msgbox',
          resolve: resolve
        };
        render();
      });
    },

    showRestoreBackup: function (serverName, instanceName, backups, onBrowseElsewhere, onDeleteBackup) {
      return new Promise(function (resolve) {
        const list = Array.isArray(backups) ? backups : [];
        activeModal = {
          type: 'restore-light',
          title: 'Restore Backup',
          serverName: serverName || 'Server',
          instanceName: instanceName || 'Default',
          backups: list,
          selectedPath: list[0] ? list[0].fullPath : '',
          confirmDeletePath: '',
          isDeletingBackup: false,
          deleteError: '',
          onBrowseElsewhere: onBrowseElsewhere,
          onDeleteBackup: onDeleteBackup,
          iconUrl: 'img/icons/icons8-rotate2-24.png',
          modalClass: 'sct-modal-restore-light',
          resolve: resolve
        };
        render();
      });
    }
  };
})();
