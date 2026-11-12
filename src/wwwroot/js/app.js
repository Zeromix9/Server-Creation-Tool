(function () {
  function escapeHtml(value) {
    return String(value ?? '')
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;');
  }

  function pad2(n) {
    return String(n).padStart(2, '0');
  }

  function formatTimeHms(d) {
    return pad2(d.getHours()) + ':' + pad2(d.getMinutes()) + ':' + pad2(d.getSeconds());
  }

  function formatDateDmy(d) {
    return pad2(d.getDate()) + '/' + pad2(d.getMonth() + 1) + '/' + d.getFullYear();
  }

  function formatBackupTimestamp(d) {
    return (
      d.getFullYear() +
      pad2(d.getMonth() + 1) +
      pad2(d.getDate()) +
      '_' +
      pad2(d.getHours()) +
      pad2(d.getMinutes())
    );
  }

  const state = {
    isMoreOpen: false,
    isWelcomeActive: false,
    showFavoritesOnly: false,
    searchQuery: '',
    isInstanceMenuOpen: false,
    activeTab: 'general',
    isTerminalExpanded: false,
    isReloading: false,
    isCheckingForUpdates: false,
    isLangMenuOpen: false,
    isSteamCmdMenuOpen: false,
    langMenuTop: 70,
    langMenuLeft: 76,
    steamCmdMenuTop: 146,
    steamCmdMenuRight: 4,
    currentLang: 'en',
    settings: {
      serverLibraryPath: '',
      language: 'English',
      autoCheckUpdates: true,
      closeToTray: false,
      runSteamCmdMinimized: true,
      runOnStartup: false,
      defaultTextEditor: 'notepad.exe'
    },
    libraryStatus: {
      libraryExists: false,
      hasSteamcmdFolder: false,
      hasSteamcmdBinary: false,
      sctDataPath: ''
    },
    task: {
      active: false,
      serverId: '',
      message: '',
      knownProgress: false,
      progress: 0,
      extraInfo: '',
      cancelling: false
    },
    deletingServerId: '',
    favoriteIds: [],
    servers: [],
    selectedServer: null
  };

  function isSteamTaskBusy() {
    return (
      Boolean(state.task.active) ||
      Boolean(state.deletingServerId) ||
      state.servers.some(function (s) {
        return Boolean(s.isInstalling) || Boolean(s.isDeleting);
      })
    );
  }

  function isServerBusy(srv) {
    if (!srv) return false;
    return Boolean(
      srv.isInstalling ||
        srv.isDeleting ||
        (state.deletingServerId && state.deletingServerId === srv.id)
    );
  }

  function renderTaskBar() {
    const container = document.getElementById('sct-titlebar-task');
    if (!container) return;

    const busySrv = state.servers.find(function (s) {
      return Boolean(s.isInstalling) || Boolean(s.isDeleting);
    });
    const isVisible =
      Boolean(state.task.active) ||
      Boolean(state.deletingServerId) ||
      Boolean(busySrv);

    if (!isVisible) {
      container.style.display = 'none';
      return;
    }

    container.style.display = 'flex';

    const msgEl = document.getElementById('sct-titlebar-task-msg');
    const progEl = document.getElementById('sct-titlebar-task-prog');
    const fillEl = document.getElementById('sct-titlebar-task-prog-fill');
    const extraEl = document.getElementById('sct-titlebar-task-extra');

    const fallbackMsg = busySrv
      ? busySrv.isDeleting
        ? 'Deleting ' + busySrv.name + ' server...'
        : busySrv.isCancelling
        ? 'Cancelling SteamCMD...'
        : 'Installing ' + busySrv.name + ' server...'
      : 'Running Task...';

    if (msgEl) {
      msgEl.textContent = state.task.message || fallbackMsg;
    }

    const known = Boolean(state.task.knownProgress) && !state.task.cancelling;
    if (progEl) {
      progEl.classList.toggle('marquee', !known);
    }
    if (fillEl) {
      if (known) {
        const clamped = Math.max(0, Math.min(100, Number(state.task.progress) || 0));
        fillEl.style.width = Math.round(clamped) + '%';
      } else {
        fillEl.style.width = '34%';
      }
    }
    if (extraEl) {
      extraEl.textContent = known ? state.task.extraInfo || '' : '';
    }
  }

  function getServerFolderName(srv, instanceName) {
    const inst = instanceName || srv.currentInstance || 'Default';
    const match = (srv.instanceInfos || []).find(function (item) {
      return item.name === inst;
    });
    if (match && match.folderName) {
      return match.folderName;
    }
    if (inst === 'Default') {
      return srv.codename + '_ds';
    }
    return srv.codename + '_' + inst + '_ds';
  }

  function getServerInstallPath(srv, instanceName) {
    const folderName = getServerFolderName(srv, instanceName);
    const lib = (state.settings.serverLibraryPath || '').replace(/[\\/]+$/, '');
    if (!lib) return folderName;
    return lib + '\\' + folderName;
  }

  function getActiveInstanceInfo(srv) {
    return (srv.instanceInfos || []).find(function (item) {
      return item.name === srv.currentInstance;
    }) || null;
  }

  function getLocalizedText(map, fallback) {
    if (map && typeof map === 'object') {
      if (state.currentLang && map[state.currentLang]) {
        return map[state.currentLang];
      }
      if (map.en) {
        return map.en;
      }
    }
    return fallback || '';
  }

  function parseConfigFilePaths(rawPaths) {
    if (!rawPaths || typeof rawPaths !== 'string') return [];
    const out = [];
    rawPaths.split(/\r?\n/).forEach(function (line) {
      const trimmed = line.trim();
      if (!trimmed) return;
      const relativePath = trimmed.replace(/^[\\/]+/, '');
      const parts = trimmed.replace(/\\/g, '/').split('/').filter(Boolean);
      const fileName = parts.length > 0 ? parts[parts.length - 1] : trimmed;
      out.push({
        rawPath: trimmed,
        relativePath: relativePath,
        fileName: fileName
      });
    });
    return out;
  }

  function extractServerConfigFiles(containers) {
    if (!Array.isArray(containers)) return [];
    const seen = {};
    const list = [];
    containers.forEach(function (cont) {
      if (!cont || !cont.isConfigFilesContainer) return;
      parseConfigFilePaths(cont.configFilePaths).forEach(function (entry) {
        const key = entry.relativePath.toLowerCase();
        if (!seen[key]) {
          seen[key] = true;
          list.push(entry.rawPath);
        }
      });
    });
    return list;
  }

  function applyLanguageToServer(srv) {
    if (!srv) return;
    srv.name = getLocalizedText(srv.names, srv.name || srv.codename);
    srv.guideDocLink = getLocalizedText(srv.guideDocLinks, srv.guideDocLink);
    srv.hasGuide = Boolean(srv.guideDocLink);
    if (Array.isArray(srv.startOptions)) {
      srv.startOptions.forEach(function (opt) {
        opt.label = getLocalizedText(opt.text, opt.label || opt.filePath || '');
      });
    }
  }

  function mapServerDto(dto) {
    const prev = state.servers.find(function (s) {
      return s.id === dto.id;
    });
    const instanceInfos = Array.isArray(dto.instances) ? dto.instances : [];
    const instanceNames = instanceInfos.map(function (i) {
      return i.name;
    });
    if (instanceNames.length === 0) {
      instanceNames.push('Default');
    }

    const activeInstName =
      prev && instanceNames.includes(prev.currentInstance)
        ? prev.currentInstance
        : dto.currentInstance || instanceNames[0];

    const activeInfo = instanceInfos.find(function (i) {
      return i.name === activeInstName;
    });

    const isFav = state.favoriteIds.includes(dto.id);
    const localizedName = getLocalizedText(
      dto.names,
      dto.name || dto.codename
    );

    const localizedGuideLink = getLocalizedText(
      dto.guideDocLinks,
      dto.guideDocLink || ''
    );

    const customContainers = Array.isArray(dto.customContainers) ? dto.customContainers : [];
    const parsedConfigFiles = extractServerConfigFiles(customContainers);
    const configFiles =
      parsedConfigFiles.length > 0
        ? parsedConfigFiles
        : prev && Array.isArray(prev.configFiles)
        ? prev.configFiles
        : [];
    const watchedConfigFile =
      prev && prev.watchedConfigFile && configFiles.includes(prev.watchedConfigFile)
        ? prev.watchedConfigFile
        : configFiles[0] || '';

    return {
      schemaVersion: dto.schemaVersion || 1,
      id: dto.id,
      codename: dto.codename || dto.id,
      folderName: dto.folderName || dto.codename || dto.id,
      name: localizedName,
      names: dto.names || {},
      steamAppId: dto.steamAppId || '',
      steamAppIds: dto.steamAppIds || {},
      isSteamServer: Boolean(dto.isSteamServer),
      supportsWindows: Boolean(dto.supportsWindows),
      supportsLinux: Boolean(dto.supportsLinux),
      supportsStdoutRedirect: Boolean(dto.supportsStdoutRedirect),
      basePlatform: dto.basePlatform || 'windows',
      filesDownloadLink: dto.filesDownloadLink || '',
      filesDownloadLinks: dto.filesDownloadLinks || {},
      areFilesLocal: Boolean(dto.areFilesLocal),
      autoExtractFiles: dto.autoExtractFiles !== undefined ? Boolean(dto.autoExtractFiles) : true,
      runCustomInstallFunc: Boolean(dto.runCustomInstallFunc),
      simpleActionsEnabled: Boolean(dto.simpleActionsEnabled),
      logoUrl: dto.logoUrl || 'img/servers/toolIcon(1).png',
      logoWidth: Number(dto.logoWidth) || 100,
      logoHeight: Number(dto.logoHeight) || 100,
      size: activeInfo ? activeInfo.size : dto.size || '-',
      sizeBytes: activeInfo ? activeInfo.sizeBytes : dto.sizeBytes || 0,
      lastRan: prev ? prev.lastRan : 'Never',
      lastUpdated: prev ? prev.lastUpdated : 'Never',
      isInstalled: activeInfo ? Boolean(activeInfo.isInstalled) : Boolean(dto.isInstalled),
      hasIncompleteInstall: activeInfo
        ? Boolean(activeInfo.hasIncompleteInstall)
        : Boolean(dto.hasIncompleteInstall),
      isFavorite: isFav,
      hasGuide: Boolean(localizedGuideLink),
      guideDocLink: localizedGuideLink,
      guideDocLinks: dto.guideDocLinks || {},
      guideContent: prev ? prev.guideContent : '',
      instances: instanceNames,
      instanceInfos: instanceInfos,
      currentInstance: activeInstName,
      configFiles: configFiles,
      launchArgs: prev ? prev.launchArgs : '',
      terminalOutput: prev ? prev.terminalOutput : '',
      isRunning: prev ? prev.isRunning : false,
      isInstalling: prev ? prev.isInstalling : false,
      isCancelling: prev ? Boolean(prev.isCancelling) : false,
      isDeleting: prev ? Boolean(prev.isDeleting) : false,
      autoRestartOnCrash: prev ? prev.autoRestartOnCrash : true,
      runOnStartup: prev ? prev.runOnStartup : false,
      autoRestartOnConfigChange: prev ? prev.autoRestartOnConfigChange : false,
      watchedConfigFile: watchedConfigFile,
      scheduledRestart: prev ? prev.scheduledRestart : 'Disabled',
      updateOnLaunch: prev ? prev.updateOnLaunch : false,
      serverPort: prev ? prev.serverPort : 27015,
      maxPlayers: prev ? prev.maxPlayers : 16,
      runAsAdmin: prev ? prev.runAsAdmin : false,
      highPriority: prev ? prev.highPriority : true,
      enableConsoleWindow: prev ? prev.enableConsoleWindow : true,
      autoBackupBeforeLaunch: prev ? prev.autoBackupBeforeLaunch : false,
      customCheckStates: prev && prev.customCheckStates ? prev.customCheckStates : {},
      startOptions: Array.isArray(dto.startOptions)
        ? dto.startOptions.map(function (opt) {
            return {
              id: opt.id || '',
              label: getLocalizedText(
                opt.text,
                opt.label || opt.filePath || ''
              ),
              text: opt.text || {},
              optionType: opt.optionType || 'Basic',
              filePath: opt.filePath || '',
              args: opt.args || '',
              isDefault: Boolean(opt.isDefault),
              isPlatformSpecific: Boolean(opt.isPlatformSpecific),
              targetPlatform: opt.targetPlatform || 'all',
              // Optional hook (action or function) run before this option. Not a start option itself.
              preLaunch: opt.preLaunch || null
            };
          })
        : [],
      simpleActions: Array.isArray(dto.simpleActions) ? dto.simpleActions : [],
      customContainers: customContainers,
      customControls: Array.isArray(dto.customControls) ? dto.customControls : [],
      customFields: Array.isArray(dto.customFields) ? dto.customFields : [],
      languages: Array.isArray(dto.languages) ? dto.languages : [],
      sourceFile: dto.sourceFile || ''
    };
  }

  async function refreshServers(options) {
    const opts = options || {};
    const prevId =
      opts.preserveSelection && state.selectedServer
        ? state.selectedServer.id
        : null;

    const scan = await window.sctLoadServers(state.settings.serverLibraryPath || '');
    state.libraryStatus = {
      libraryExists: Boolean(scan && scan.libraryExists),
      hasSteamcmdFolder: Boolean(scan && scan.hasSteamcmdFolder),
      hasSteamcmdBinary: Boolean(scan && scan.hasSteamcmdBinary),
      sctDataPath: (scan && scan.sctDataPath) || ''
    };

    const rawServers = scan && Array.isArray(scan.servers) ? scan.servers : [];
    state.servers = rawServers.map(mapServerDto);

    if (prevId) {
      state.selectedServer =
        state.servers.find(function (s) {
          return s.id === prevId;
        }) ||
        state.servers[0] ||
        null;
    } else if (!state.selectedServer) {
      state.selectedServer =
        state.servers.find(function (s) {
          return s.id === 'cs';
        }) ||
        state.servers[0] ||
        null;
    } else {
      const currentId = state.selectedServer.id;
      state.selectedServer =
        state.servers.find(function (s) {
          return s.id === currentId;
        }) ||
        state.servers[0] ||
        null;
    }
  }

  async function refreshServerLikeClicked(serverId) {
    const targetId =
      serverId || (state.selectedServer && state.selectedServer.id) || null;
    await refreshServers({ preserveSelection: true });
    if (targetId) {
      const refreshed = state.servers.find(function (s) {
        return s.id === targetId;
      });
      if (refreshed) {
        refreshed.isInstalling = false;
        refreshed.isCancelling = false;
        refreshed.isDeleting = false;
        if (
          !state.isWelcomeActive &&
          state.selectedServer &&
          state.selectedServer.id === targetId
        ) {
          state.selectedServer = refreshed;
        }
      }
    }
    renderAll();
  }

  function loadFavorites() {
    state.favoriteIds = window.sctLoadFavorites();
  }

  function saveFavorites() {
    window.sctSaveFavorites(state.favoriteIds);
  }

  function loadSettings() {
    try {
      const json = window.sctLoadSettings();
      if (!json) return;
      const parsed = JSON.parse(json);
      if (!parsed || typeof parsed !== 'object') return;

      state.settings = {
        serverLibraryPath:
          parsed.serverLibraryPath ?? parsed.ServerLibraryPath ?? '',
        language: parsed.language ?? parsed.Language ?? 'English',
        autoCheckUpdates:
          parsed.autoCheckUpdates ?? parsed.AutoCheckUpdates ?? true,
        closeToTray: parsed.closeToTray ?? parsed.CloseToTray ?? false,
        runSteamCmdMinimized:
          parsed.runSteamCmdMinimized ?? parsed.RunSteamCmdMinimized ?? true,
        runOnStartup: parsed.runOnStartup ?? parsed.RunOnStartup ?? false,
        defaultTextEditor:
          parsed.defaultTextEditor ?? parsed.DefaultTextEditor ?? 'notepad.exe'
      };
      state.currentLang = state.settings.language === 'German' ? 'de' : 'en';
    } catch {}
  }

  function saveSettings() {
    try {
      window.sctSaveSettings(JSON.stringify(state.settings));
    } catch {}
  }

  function getFilteredServers() {
    const q = state.searchQuery.trim().toLowerCase();
    return state.servers.filter(function (s) {
      if (state.showFavoritesOnly && !s.isFavorite) return false;
      if (q && !s.name.toLowerCase().includes(q)) return false;
      return true;
    });
  }

  function appendTerminalLine(server, message) {
    const line = '[' + formatTimeHms(new Date()) + '] ' + message;
    server.terminalOutput = server.terminalOutput
      ? server.terminalOutput + '\n' + line
      : line;

    if (
      state.selectedServer &&
      state.selectedServer.id === server.id &&
      state.activeTab === 'general'
    ) {
      const textarea = document.querySelector('.sct-terminal-output');
      if (textarea) {
        textarea.value = server.terminalOutput;
        textarea.scrollTop = textarea.scrollHeight;
      }
    }
  }

  function closeContextMenus() {
    state.isLangMenuOpen = false;
    state.isSteamCmdMenuOpen = false;
    state.isInstanceMenuOpen = false;
  }

  function renderFrameAndLeftColumn() {
    const frame = document.getElementById('sct-window-frame');
    if (frame) {
      frame.classList.toggle('more-expanded', state.isMoreOpen);
    }

    const moreBtn = document.getElementById('sct-more-btn');
    if (moreBtn) {
      moreBtn.classList.toggle('open', state.isMoreOpen);
    }

    const langBtn = document.getElementById('sct-lang-btn');
    if (langBtn) {
      langBtn.classList.toggle('active', state.isLangMenuOpen);
    }

    const taskBusy = isSteamTaskBusy();

    const steamCmdBtn = document.getElementById('sct-steamcmd-btn');
    if (steamCmdBtn) {
      steamCmdBtn.classList.toggle('active', state.isSteamCmdMenuOpen);
      steamCmdBtn.disabled = taskBusy;
    }

    const favToggleBtn = document.getElementById('sct-fav-toggle-btn');
    const favToggleImg = document.getElementById('sct-fav-toggle-img');
    if (favToggleBtn) {
      favToggleBtn.classList.toggle('active', state.showFavoritesOnly);
    }
    if (favToggleImg) {
      favToggleImg.src = state.showFavoritesOnly
        ? 'img/icons/starChecked_16.png'
        : 'img/icons/starUnChecked_gray_16.png';
    }

    const welcomeBtn = document.getElementById('sct-welcome-btn');
    if (welcomeBtn) {
      welcomeBtn.classList.toggle('active', state.isWelcomeActive);
    }

    const reloadBtn = document.getElementById('sct-reload-btn');
    if (reloadBtn) {
      reloadBtn.classList.toggle('pressed', state.isReloading);
      reloadBtn.disabled = Boolean(state.deletingServerId);
    }

    const listEl = document.getElementById('sct-server-items-scroll');
    if (listEl) {
      const filtered = getFilteredServers();
      listEl.innerHTML = filtered
        .map(function (srv) {
          const isSelected =
            !state.isWelcomeActive &&
            state.selectedServer &&
            state.selectedServer.id === srv.id;
          const isAnyInstalled =
            Boolean(srv.isInstalled) ||
            (srv.instanceInfos || []).some(function (inst) {
              return Boolean(inst.isInstalled);
            });
          return (
            '<button type="button" class="sct-list-item-srv ' +
            (isAnyInstalled ? 'installed ' : '') +
            (srv.isRunning ? 'running ' : '') +
            (isSelected ? 'selected' : '') +
            '" data-srv-id="' +
            escapeHtml(srv.id) +
            '">' +
            '<span class="sct-srv-name">' +
            escapeHtml(srv.name) +
            '</span>' +
            (srv.isFavorite
              ? '<img src="img/icons/starChecked_16.png" class="sct-star-icon-small" alt="Favorite" />'
              : '') +
            '</button>'
          );
        })
        .join('');
    }
  }

  function renderWelcomeView() {
    return (
      '<div class="sct-welcome-container">' +
      '<div class="sct-welcome-banner">' +
      '<h2 class="sct-welcome-headline">Welcome to Server Creation Tool 5.0</h2>' +
      '<p class="sct-welcome-subtext">' +
      'This tool allows you to download, install and manage<br />' +
      'steam and non-steam servers quick and easy.' +
      '</p>' +
      '</div>' +
      '<div class="sct-donate-row">' +
      '<button type="button" class="sct-donate-button" data-action="open-donate" title="Support development via PayPal"></button>' +
      '</div>' +
      '<div class="sct-instructions-card">' +
      '<div class="sct-instructions-header">Basic Instructions:</div>' +
      '<div class="sct-instructions-text-area">' +
      '<p>1) Select the game you want and click the "Install Server" button.</p>' +
      '<p>2) If its the first time you\'re installing a server, select the installation folder, and then wait for it to finish.</p>' +
      '<p>3) Type "quit" in the command line after the installation has finished. - (only for steam servers)</p>' +
      '<p>4) Configure the server the way you like by editing the settings files (optional).</p>' +
      '<p>5) Start the server and enjoy!</p>' +
      '<p style="margin-top: 8px; color: #a0aec0;">' +
      'NOTE: If you need extra help, some servers include a guide in the "Extra" section.<br />' +
      'You can also go to the "Language" or "About" options for additional help, or join our Steam / Discord communities!' +
      '</p>' +
      '</div>' +
      '</div>' +
      '</div>'
    );
  }

  function renderCustomControlItem(srv, ctrl, ctrlIdx, cols, panelDisabledAttr) {
    const r = Math.max(0, Number(ctrl.row) || 0);
    const c = Math.min(cols - 1, Math.max(0, Number(ctrl.column) || 0));
    const gridPosStyle = 'grid-row: ' + (r + 1) + '; grid-column: ' + (c + 1) + ';';

    let label = getLocalizedText(ctrl.texts, ctrl.text || '');
    if (ctrl.isSystemGuideBtn && ctrl.useSystemDefaultText && !label) {
      label = state.currentLang === 'de' ? 'Installationsanleitung' : 'Installation Guide';
    }
    if (!label) {
      label = 'Control';
    }

    const type = (ctrl.controlType || 'Button').toLowerCase();
    if (type === 'checkbox') {
      const checked = Boolean(
        srv.customCheckStates && srv.customCheckStates[ctrlIdx]
      );
      return (
        '<label class="sct-custom-ctrl-checkbox" style="' +
        gridPosStyle +
        '" title="' +
        escapeHtml(label) +
        '">' +
        '<input type="checkbox" class="sct-win-checkbox" data-custom-ctrl-check="' +
        ctrlIdx +
        '" ' +
        (checked ? 'checked ' : '') +
        panelDisabledAttr +
        ' />' +
        '<span class="sct-tool-btn-text">' +
        escapeHtml(label) +
        '</span>' +
        '</label>'
      );
    }

    if (type === 'dropdownbutton') {
      const isOpen = state.openCustomDropdownIdx === ctrlIdx;
      const subs = Array.isArray(ctrl.submenuButtons) ? ctrl.submenuButtons : [];
      const menuHtml = isOpen
        ? '<div class="sct-custom-dropdown-menu" data-stop-propagation="true">' +
          (subs.length === 0
            ? '<div class="sct-auto-hint" style="padding: 4px 6px;">No submenu items</div>'
            : subs
                .map(function (sub, subIdx) {
                  const subLabel =
                    getLocalizedText(sub.texts, sub.text) || 'Option';
                  return (
                    '<button type="button" class="sct-custom-dropdown-item" data-custom-ctrl-sub="' +
                    ctrlIdx +
                    ':' +
                    subIdx +
                    '" ' +
                    panelDisabledAttr +
                    '>' +
                    escapeHtml(subLabel) +
                    '</button>'
                  );
                })
                .join('')) +
          '</div>'
        : '';

      return (
        '<div class="sct-custom-dropdown-wrap" style="' +
        gridPosStyle +
        '">' +
        '<button type="button" class="sct-tool-btn sct-custom-ctrl-btn" data-custom-ctrl-idx="' +
        ctrlIdx +
        '" ' +
        panelDisabledAttr +
        ' title="' +
        escapeHtml(label) +
        '">' +
        '<span class="sct-tool-btn-text">' +
        escapeHtml(label) +
        ' ▾</span>' +
        '</button>' +
        menuHtml +
        '</div>'
      );
    }

    const iconHtml = ctrl.isSystemGuideBtn
      ? '<img src="img/icons/1091.png" class="sct-tool-btn-icon" alt="" />'
      : '';

    return (
      '<button type="button" class="sct-tool-btn sct-custom-ctrl-btn" style="' +
      gridPosStyle +
      '" data-custom-ctrl-idx="' +
      ctrlIdx +
      '" ' +
      panelDisabledAttr +
      ' title="' +
      escapeHtml(label) +
      '">' +
      iconHtml +
      '<span class="sct-tool-btn-text">' +
      escapeHtml(label) +
      '</span>' +
      '</button>'
    );
  }

  function renderCustomContainerCard(srv, cont, isDeleting) {
    if (!cont || cont.isVisible === false) {
      return '';
    }
    const panelDisabledAttr = isDeleting ? 'disabled' : '';
    const title = getLocalizedText(cont.names, cont.name || cont.id || 'Container');
    const cols = Math.min(3, Math.max(1, Number(cont.columns) || 1));
    const cardClass =
      cont.targetTab === 'settings'
        ? 'sct-settings-card'
        : cont.targetTab === 'automation_watchdog'
        ? 'sct-auto-card'
        : 'sct-tools-section-card';

    if (cont.isConfigFilesContainer) {
      const entries = parseConfigFilePaths(cont.configFilePaths);
      let bodyHtml = '';
      if (entries.length === 0) {
        bodyHtml =
          '<div class="sct-auto-hint" style="padding: 4px 0;">No configuration files configured for ' +
          escapeHtml(srv.name) +
          '.</div>';
      } else {
        bodyHtml =
          '<div class="sct-custom-grid" style="grid-template-columns: repeat(' +
          cols +
          ', minmax(0, 1fr));">' +
          entries
            .map(function (entry) {
              return (
                '<button type="button" class="sct-config-file-row-btn" data-edit-config="' +
                escapeHtml(entry.rawPath) +
                '" ' +
                panelDisabledAttr +
                ' title="Open ' +
                escapeHtml(entry.rawPath) +
                ' in default text editor">' +
                '<img src="img/icons/1645.png" class="sct-config-file-icon" alt="" />' +
                '<span class="sct-config-file-name">' +
                escapeHtml(entry.fileName) +
                '</span>' +
                '<span class="sct-config-file-action">Edit ↗</span>' +
                '</button>'
              );
            })
            .join('') +
          '</div>';
      }

      return (
        '<div class="' +
        cardClass +
        '" data-container-id="' +
        escapeHtml(cont.id) +
        '">' +
        '<div class="sct-card-section-title">-' +
        escapeHtml(title) +
        '-</div>' +
        '<div class="sct-config-files-container">' +
        bodyHtml +
        '</div>' +
        '</div>'
      );
    }

    const allControls = Array.isArray(srv.customControls) ? srv.customControls : [];
    const matched = [];
    allControls.forEach(function (ctrl, idx) {
      if (ctrl && ctrl.containerId === cont.id) {
        matched.push({ ctrl: ctrl, idx: idx });
      }
    });

    if (matched.length === 0) {
      return (
        '<div class="' +
        cardClass +
        '" data-container-id="' +
        escapeHtml(cont.id) +
        '">' +
        '<div class="sct-card-section-title">-' +
        escapeHtml(title) +
        '-</div>' +
        '<div class="sct-auto-hint" style="padding: 4px 0;">No controls configured in this container.</div>' +
        '</div>'
      );
    }

    const controlsHtml = matched
      .map(function (item) {
        return renderCustomControlItem(
          srv,
          item.ctrl,
          item.idx,
          cols,
          panelDisabledAttr
        );
      })
      .join('');

    return (
      '<div class="' +
      cardClass +
      '" data-container-id="' +
      escapeHtml(cont.id) +
      '">' +
      '<div class="sct-card-section-title">-' +
      escapeHtml(title) +
      '-</div>' +
      '<div class="sct-custom-grid" style="grid-template-columns: repeat(' +
      cols +
      ', minmax(0, 1fr));">' +
      controlsHtml +
      '</div>' +
      '</div>'
    );
  }

  function renderCustomContainersForTab(srv, targetTab, isDeleting) {
    const containers = Array.isArray(srv.customContainers) ? srv.customContainers : [];
    return containers
      .filter(function (cont) {
        return cont && cont.targetTab === targetTab && cont.isVisible !== false;
      })
      .map(function (cont) {
        return renderCustomContainerCard(srv, cont, isDeleting);
      })
      .join('');
  }

  function renderTabContent(srv) {
    const activeInstInfo = getActiveInstanceInfo(srv);
    const activeFolderName = getServerFolderName(srv);
    const installLoc =
      (activeInstInfo && activeInstInfo.fullPath) ||
      getServerInstallPath(srv);

    if (state.activeTab === 'general') {
      const autoRestartText = srv.autoRestartOnCrash ? 'YES' : 'NO';
      const runOnStartupText = srv.runOnStartup ? 'YES' : 'NO';

      const overviewCardHtml = state.isTerminalExpanded
        ? ''
        : '<div class="sct-general-status-card">' +
          '<div class="sct-card-section-title">-Server Overview-</div>' +
          '<div class="sct-status-meta-grid">' +
          '<div class="sct-meta-entry">' +
          '<span class="sct-entry-label">Auto restart:</span>' +
          '<span class="sct-entry-value">' +
          autoRestartText +
          '</span>' +
          '</div>' +
          '<div class="sct-meta-entry">' +
          '<span class="sct-entry-label">Run on startup:</span>' +
          '<span class="sct-entry-value">' +
          runOnStartupText +
          '</span>' +
          '</div>' +
          '<div class="sct-meta-entry">' +
          '<span class="sct-entry-label">Active Instance:</span>' +
          '<span class="sct-entry-value">' +
          escapeHtml(srv.currentInstance) +
          '</span>' +
          '</div>' +
          '<div class="sct-meta-entry">' +
          '<span class="sct-entry-label">Folder Name:</span>' +
          '<span class="sct-entry-value">' +
          escapeHtml(activeFolderName) +
          '</span>' +
          '</div>' +
          '<div class="sct-meta-entry full-span">' +
          '<span class="sct-entry-label">Install Location:</span>' +
          '<span class="sct-entry-value path" title="' +
          escapeHtml(installLoc) +
          '">' +
          escapeHtml(installLoc) +
          '</span>' +
          '</div>' +
          '</div>' +
          '</div>';

      const expandTitle = state.isTerminalExpanded
        ? 'Restore overview layout'
        : 'Expand terminal output';
      const chevronPath = state.isTerminalExpanded
        ? 'M2 3.5L5 6.5L8 3.5'
        : 'M2 6.5L5 3.5L8 6.5';

      return (
        '<div class="sct-tab-pane">' +
        overviewCardHtml +
        '<div class="sct-terminal-wrapper ' +
        (state.isTerminalExpanded ? 'expanded' : '') +
        '">' +
        '<textarea class="sct-terminal-output" readonly placeholder="Terminal output will appear here...">' +
        escapeHtml(srv.terminalOutput) +
        '</textarea>' +
        '<button type="button" class="sct-terminal-expand-btn" data-action="toggle-terminal-expand" title="' +
        expandTitle +
        '" aria-label="' +
        expandTitle +
        '">' +
        '<svg width="10" height="10" viewBox="0 0 10 10" fill="none" aria-hidden="true">' +
        '<path d="' +
        chevronPath +
        '" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />' +
        '</svg>' +
        '</button>' +
        '</div>' +
        '</div>'
      );
    }

    const isDeleting = Boolean(
      srv.isDeleting || (state.deletingServerId && state.deletingServerId === srv.id)
    );
    const panelDisabledAttr = isDeleting ? 'disabled' : '';

    if (state.activeTab === 'tools') {
      const taskBusy = isSteamTaskBusy();
      const disabledAttr =
        srv.isInstalled && !srv.isInstalling && !isDeleting ? '' : 'disabled';
      const updateDisabledAttr =
        srv.isInstalled && !taskBusy && !isDeleting ? '' : 'disabled';
      const deleteDisabledAttr =
        srv.isInstalled && !taskBusy && !isDeleting ? '' : 'disabled';
      const openFolderDisabledAttr =
        srv.isInstalled && !isDeleting ? '' : 'disabled';

      const customToolsHtml = renderCustomContainersForTab(
        srv,
        'tools_maintenance',
        isDeleting
      );
      const hasToolsContainers =
        Array.isArray(srv.customContainers) &&
        srv.customContainers.some(function (c) {
          return c && c.targetTab === 'tools_maintenance';
        });
      const fallbackExtraHtml = hasToolsContainers
        ? ''
        : '<div class="sct-tools-section-card">' +
          '<div class="sct-card-section-title">-Extra-</div>' +
          '<div class="sct-tools-row-2">' +
          '<button type="button" class="sct-tool-btn" data-action="create-custom-batch" ' +
          disabledAttr +
          '>' +
          '<img src="img/icons/SquaregreenArrowRightBatch.png" class="sct-tool-btn-icon" alt="" />' +
          '<span class="sct-tool-btn-text">Create Custom Batch File</span>' +
          '</button>' +
          '<button type="button" class="sct-tool-btn" data-action="open-guide" ' +
          panelDisabledAttr +
          '>' +
          '<img src="img/icons/1091.png" class="sct-tool-btn-icon" alt="" />' +
          '<span class="sct-tool-btn-text">Manual Installation Guide</span>' +
          '</button>' +
          '</div>' +
          '</div>';

      return (
        '<div class="sct-tab-pane">' +
        '<div class="sct-tools-section-card">' +
        '<div class="sct-card-section-title">-General-</div>' +
        '<div class="sct-tools-row-3">' +
        '<button type="button" class="sct-tool-btn" data-action="update-or-repair" ' +
        updateDisabledAttr +
        '>' +
        '<img src="img/icons/icons8-rotate2-24.png" class="sct-tool-btn-icon" alt="" />' +
        '<span class="sct-tool-btn-text">Update</span>' +
        '</button>' +
        '<button type="button" class="sct-tool-btn" data-action="delete-server" ' +
        deleteDisabledAttr +
        '>' +
        '<img src="img/icons/1263.png" class="sct-tool-btn-icon" alt="" />' +
        '<span class="sct-tool-btn-text">Delete</span>' +
        '</button>' +
        '<button type="button" class="sct-tool-btn" data-action="open-folder" ' +
        openFolderDisabledAttr +
        '>' +
        '<img src="img/icons/468.png" class="sct-tool-btn-icon" alt="" />' +
        '<span class="sct-tool-btn-text">Open Folder</span>' +
        '</button>' +
        '</div>' +
        '</div>' +
        '<div class="sct-tools-section-card">' +
        '<div class="sct-card-section-title">-Backups-</div>' +
        '<div class="sct-tools-row-2">' +
        '<button type="button" class="sct-tool-btn" data-action="backup-server" ' +
        disabledAttr +
        '>' +
        '<span style="font-size: 12px; margin-right: 2px;">💾</span>' +
        '<span class="sct-tool-btn-text">Backup Server</span>' +
        '</button>' +
        '<button type="button" class="sct-tool-btn" data-action="restore-backup" ' +
        disabledAttr +
        '>' +
        '<span style="font-size: 12px; margin-right: 2px;">📦</span>' +
        '<span class="sct-tool-btn-text">Restore Backup</span>' +
        '</button>' +
        '</div>' +
        '</div>' +
        customToolsHtml +
        fallbackExtraHtml +
        '</div>'
      );
    }

    if (state.activeTab === 'settings') {
      const customSettingsHtml = renderCustomContainersForTab(
        srv,
        'settings',
        isDeleting
      );
      if (customSettingsHtml) {
        return '<div class="sct-tab-pane">' + customSettingsHtml + '</div>';
      }

      return (
        '<div class="sct-tab-pane">' +
        '<div class="sct-settings-card">' +
        '<div class="sct-card-section-title">-Game Server-</div>' +
        '<div class="sct-auto-hint" style="padding: 4px 0;">No controls configured in this container.</div>' +
        '</div>' +
        '<div class="sct-settings-card">' +
        '<div class="sct-card-section-title">-Manually Edit Config Files-</div>' +
        '<div class="sct-auto-hint" style="padding: 4px 0;">No configuration files configured for ' +
        escapeHtml(srv.name) +
        '.</div>' +
        '</div>' +
        '</div>'
      );
    }

    if (state.activeTab === 'automation') {
      const customAutoHtml = renderCustomContainersForTab(
        srv,
        'automation_watchdog',
        isDeleting
      );
      const configSelectHtml =
        srv.configFiles && srv.configFiles.length > 0
          ? '<select class="sct-auto-select" data-srv-select="watchedConfigFile" ' +
            (srv.autoRestartOnConfigChange && !isDeleting ? '' : 'disabled') +
            ' title="Select file to monitor">' +
            srv.configFiles
              .map(function (cfg) {
                return (
                  '<option value="' +
                  escapeHtml(cfg) +
                  '" ' +
                  (srv.watchedConfigFile === cfg ? 'selected' : '') +
                  '>' +
                  escapeHtml(cfg) +
                  '</option>'
                );
              })
              .join('') +
            '</select>'
          : '';

      const schedules = [
        'Disabled',
        'Every 6 Hours',
        'Every 12 Hours',
        'Daily (04:00 AM)',
        'Every 48 Hours'
      ];

      return (
        '<div class="sct-tab-pane">' +
        '<div class="sct-auto-card">' +
        '<div class="sct-card-section-title">-Process Watchdog-</div>' +
        '<div class="sct-auto-card-body">' +
        '<label class="sct-checkbox-label" title="Automatically relaunch server if the process terminates unexpectedly">' +
        '<input type="checkbox" class="sct-win-checkbox" data-srv-prop="autoRestartOnCrash" ' +
        (srv.autoRestartOnCrash ? 'checked ' : '') +
        panelDisabledAttr +
        ' />' +
        '<span class="sct-check-title">Auto-restart server on crash</span>' +
        '</label>' +
        '<div class="sct-auto-hint">Monitors the server process PID and restarts it immediately if an abnormal exit occurs.</div>' +
        '<div style="margin-top: 6px;">' +
        '<label class="sct-checkbox-label" title="Automatically start this server when Server Creation Tool launches">' +
        '<input type="checkbox" class="sct-win-checkbox" data-srv-prop="runOnStartup" ' +
        (srv.runOnStartup ? 'checked ' : '') +
        panelDisabledAttr +
        ' />' +
        '<span class="sct-check-title">Run on startup</span>' +
        '</label>' +
        '<div class="sct-auto-hint">Automatically starts this server when Server Creation Tool opens.</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="sct-auto-card">' +
        '<div class="sct-card-section-title">-Config File Watcher-</div>' +
        '<div class="sct-auto-card-body">' +
        '<div class="sct-auto-row-split">' +
        '<label class="sct-checkbox-label" title="Restart server when monitored config file is updated">' +
        '<input type="checkbox" class="sct-win-checkbox" data-srv-prop="autoRestartOnConfigChange" ' +
        (srv.autoRestartOnConfigChange ? 'checked ' : '') +
        panelDisabledAttr +
        ' />' +
        '<span class="sct-check-title">Restart on config edit:</span>' +
        '</label>' +
        configSelectHtml +
        '</div>' +
        '<div class="sct-auto-hint">Detects disk write events to the selected configuration file and triggers a graceful reboot.</div>' +
        '</div>' +
        '</div>' +
        '<div class="sct-auto-card">' +
        '<div class="sct-card-section-title">-Automated Schedules &amp; Updates-</div>' +
        '<div class="sct-auto-card-body">' +
        '<div class="sct-auto-row-split">' +
        '<span class="sct-auto-field-label">Scheduled Restart:</span>' +
        '<select class="sct-auto-select" data-srv-select="scheduledRestart" ' +
        panelDisabledAttr +
        ' title="Periodic automated restart schedule">' +
        schedules
          .map(function (opt) {
            return (
              '<option value="' +
              escapeHtml(opt) +
              '" ' +
              (srv.scheduledRestart === opt ? 'selected' : '') +
              '>' +
              escapeHtml(opt) +
              '</option>'
            );
          })
          .join('') +
        '</select>' +
        '</div>' +
        '<div style="margin-top: 6px;">' +
        '<label class="sct-checkbox-label" title="Check and validate server updates before starting">' +
        '<input type="checkbox" class="sct-win-checkbox" data-srv-prop="updateOnLaunch" ' +
        (srv.updateOnLaunch ? 'checked ' : '') +
        panelDisabledAttr +
        ' />' +
        '<span class="sct-check-title">Check &amp; update files before launch</span>' +
        '</label>' +
        '</div>' +
        '</div>' +
        '</div>' +
        customAutoHtml +
        '</div>'
      );
    }

    return '';
  }

  function renderServerView(srv) {
    const instancesPopupHtml = state.isInstanceMenuOpen
      ? '<div class="sct-instances-popup" data-stop-propagation="true">' +
        srv.instances
          .map(function (inst) {
            return (
              '<button type="button" class="sct-instance-option ' +
              (srv.currentInstance === inst ? 'active' : '') +
              '" data-switch-instance="' +
              escapeHtml(inst) +
              '">' +
              '<img class="sct-context-menu-icon" src="img/icons/icons8-dot-24.png" alt="" />' +
              '<span>' +
              escapeHtml(inst) +
              '</span>' +
              '</button>'
            );
          })
          .join('') +
        '<div class="sct-popup-divider"></div>' +
        '<button type="button" class="sct-instance-option" data-action="add-new-instance">' +
        '<img class="sct-context-menu-icon" src="img/icons/refresh16x161.png" style="width: 14px; height: 14px;" alt="" />' +
        '<span>Add New Instance...</span>' +
        '</button>' +
        '</div>'
      : '';

    const isDeleting = Boolean(
      srv.isDeleting || (state.deletingServerId && state.deletingServerId === srv.id)
    );
    const panelDisabledAttr = isDeleting ? 'disabled' : '';
    const taskBusy = isSteamTaskBusy();
    let actionBtnHtml = '';
    if (srv.isInstalling) {
      actionBtnHtml =
        '<button type="button" class="sct-srv-action-btn state-stop" data-action="cancel-install">' +
        '<img src="img/icons/stop_img.png" class="sct-srv-btn-icon" alt="" />' +
        '<span class="sct-srv-btn-text">' +
        (srv.isCancelling ? 'Cancelling' : 'Cancel') +
        '</span>' +
        '</button>';
    } else if (srv.isInstalled && srv.hasIncompleteInstall) {
      actionBtnHtml =
        '<button type="button" class="sct-srv-action-btn state-install" data-action="install-server" ' +
        (taskBusy || isDeleting ? 'disabled' : '') +
        '>' +
        '<img src="img/icons/install_img.png" class="sct-srv-btn-icon" alt="" />' +
        '<span class="sct-srv-btn-text">Update</span>' +
        '</button>';
    } else if (srv.isInstalled) {
      actionBtnHtml =
        '<button type="button" class="sct-srv-action-btn ' +
        (srv.isRunning ? 'state-stop' : 'state-start') +
        '" data-action="start-or-stop" ' +
        panelDisabledAttr +
        '>' +
        '<img src="' +
        (srv.isRunning ? 'img/icons/stop_img.png' : 'img/icons/start_img.png') +
        '" class="sct-srv-btn-icon" alt="" />' +
        '<span class="sct-srv-btn-text">' +
        (srv.isRunning ? 'Stop' : 'Start') +
        '</span>' +
        '</button>';
    } else {
      actionBtnHtml =
        '<button type="button" class="sct-srv-action-btn state-install" data-action="install-server" ' +
        (taskBusy || isDeleting ? 'disabled' : '') +
        '>' +
        '<img src="img/icons/install_img.png" class="sct-srv-btn-icon" alt="" />' +
        '<span class="sct-srv-btn-text">Install</span>' +
        '</button>';
    }

    const statusColor = isDeleting
      ? '#f87171'
      : srv.isInstalling
      ? '#f59e0b'
      : srv.isRunning
      ? '#4ade80'
      : srv.hasIncompleteInstall
      ? '#38bdf8'
      : srv.isInstalled
      ? '#38bdf8'
      : '#94a3b8';
    const statusText = isDeleting
      ? 'Deleting'
      : srv.isInstalling
      ? srv.isCancelling
        ? 'Cancelling'
        : 'Installing'
      : srv.isRunning
      ? 'Running'
      : srv.hasIncompleteInstall
      ? 'Incomplete'
      : srv.isInstalled
      ? 'Ready'
      : 'Uninstalled';

    const lastRanHtml =
      srv.lastRan && srv.lastRan !== 'Never'
        ? '<div class="sct-size-display" style="margin-left: 4px;">' +
          '<span class="sct-size-lbl-top">-LAST RAN-</span>' +
          '<span class="sct-size-lbl-val" style="font-size: 8.75pt;">' +
          escapeHtml(srv.lastRan) +
          '</span>' +
          '</div>'
        : '';

    const scaleX = Math.max(1, Number(srv.logoWidth) || 100) / 100;
    const scaleY = Math.max(1, Number(srv.logoHeight) || 100) / 100;
    const logoStyleAttr =
      scaleX !== 1 || scaleY !== 1
        ? ' style="transform: scale(' +
          scaleX +
          ', ' +
          scaleY +
          '); transform-origin: center center;"'
        : '';

    return (
      '<div class="sct-main-srv-panel">' +
      '<div class="sct-srv-header-row">' +
      '<button type="button" class="sct-instance-dropdown-btn ' +
      (state.isInstanceMenuOpen ? 'active' : '') +
      '" data-action="toggle-instance-menu" ' +
      panelDisabledAttr +
      '>' +
      '<span>' +
      escapeHtml(srv.name) +
      '</span>' +
      '<span class="sct-instance-badge">[' +
      escapeHtml(srv.currentInstance) +
      ']</span>' +
      '<span class="sct-caret">▼</span>' +
      '</button>' +
      instancesPopupHtml +
      '</div>' +
      '<div class="sct-center-logo-area">' +
      '<img id="sct-server-logo-img" src="' +
      escapeHtml(srv.logoUrl) +
      '" class="sct-logo-img"' +
      logoStyleAttr +
      ' alt="' +
      escapeHtml(srv.name) +
      '" />' +
      '</div>' +
      '<div class="sct-control-bar-panel">' +
      actionBtnHtml +
      '<div class="sct-size-display">' +
      '<span class="sct-size-lbl-top">-STATUS-</span>' +
      '<span class="sct-size-lbl-val" style="color: ' +
      statusColor +
      ';">' +
      statusText +
      '</span>' +
      '</div>' +
      '<div class="sct-size-display">' +
      '<span class="sct-size-lbl-top">-SIZE-</span>' +
      '<span class="sct-size-lbl-val">' +
      escapeHtml(srv.size) +
      '</span>' +
      '</div>' +
      lastRanHtml +
      '<div class="sct-quick-buttons">' +
      '<button type="button" class="sct-quick-icon-btn" data-action="toggle-favorite" title="Add to favourites" ' +
      panelDisabledAttr +
      '>' +
      '<img src="' +
      (srv.isFavorite ? 'img/icons/starChecked.png' : 'img/icons/starUnChecked.png') +
      '" alt="Star" />' +
      '</button>' +
      '<button type="button" class="sct-quick-icon-btn" data-action="open-server-info" title="Info" ' +
      panelDisabledAttr +
      '>' +
      '<img src="img/icons/srvInfBtn.BackgroundImage.png" alt="Info" />' +
      '</button>' +
      '</div>' +
      '</div>' +
      '<div class="sct-tab-strip">' +
      '<button type="button" class="sct-tab-btn ' +
      (state.activeTab === 'general' ? 'active' : '') +
      '" data-tab="general" ' +
      panelDisabledAttr +
      '>' +
      '<img src="img/icons/icons8-server-24.png" class="sct-tab-icon" alt="" />' +
      '<span>Overview</span>' +
      '</button>' +
      '<button type="button" class="sct-tab-btn ' +
      (state.activeTab === 'tools' ? 'active' : '') +
      '" data-tab="tools" ' +
      panelDisabledAttr +
      '>' +
      '<img src="img/icons/1645.png" class="sct-tab-icon" alt="" />' +
      '<span>Tools &amp; Maintenance</span>' +
      '</button>' +
      '<button type="button" class="sct-tab-btn ' +
      (state.activeTab === 'settings' ? 'active' : '') +
      '" data-tab="settings" ' +
      panelDisabledAttr +
      '>' +
      '<img src="img/icons/settingsBtn.Image.png" class="sct-tab-icon" alt="" />' +
      '<span>Settings</span>' +
      '</button>' +
      '<button type="button" class="sct-tab-btn ' +
      (state.activeTab === 'automation' ? 'active' : '') +
      '" data-tab="automation" ' +
      panelDisabledAttr +
      '>' +
      '<img src="img/icons/cmd_icon21.png" class="sct-tab-icon" alt="" />' +
      '<span>Automation &amp; Watchdog</span>' +
      '</button>' +
      '</div>' +
      '<div class="sct-tab-content">' +
      renderTabContent(srv) +
      '</div>' +
      '</div>'
    );
  }

  function renderRightColumn() {
    const col = document.getElementById('sct-right-column');
    if (!col) return;

    if (state.isWelcomeActive) {
      col.innerHTML = renderWelcomeView();
    } else if (state.selectedServer) {
      col.innerHTML = renderServerView(state.selectedServer);
      const logoImg = document.getElementById('sct-server-logo-img');
      if (logoImg) {
        logoImg.addEventListener('error', function () {
          if (state.selectedServer && state.selectedServer.logoUrl !== 'img/servers/toolIcon(1).png') {
            state.selectedServer.logoUrl = 'img/servers/toolIcon(1).png';
            logoImg.src = 'img/servers/toolIcon(1).png';
          }
        });
      }
    } else {
      col.innerHTML =
        '<div class="sct-welcome-container">' +
        '<div class="sct-instructions-card">' +
        '<div class="sct-instructions-header">No Server Metadata Found</div>' +
        '<div class="sct-instructions-text-area">' +
        '<p>No .server files were found in the sct_data folder.</p>' +
        '</div>' +
        '</div>' +
        '</div>';
    }
  }

  function renderContextMenus() {
    const root = document.getElementById('sct-context-menus-root');
    if (!root) return;

    const anyOpen =
      state.isLangMenuOpen ||
      state.isSteamCmdMenuOpen ||
      state.isInstanceMenuOpen;

    if (!anyOpen) {
      root.innerHTML = '';
      return;
    }

    let html = '<div class="sct-context-backdrop" data-action="close-context-menus"></div>';

    if (state.isLangMenuOpen) {
      html +=
        '<div class="sct-custom-context-menu" style="top: ' +
        state.langMenuTop +
        'px; left: ' +
        state.langMenuLeft +
        'px;" data-stop-propagation="true">' +
        '<button type="button" class="sct-context-menu-item ' +
        (state.currentLang === 'en' ? 'selected' : '') +
        '" data-select-lang="en">' +
        '<img class="sct-context-menu-icon" src="img/icons/icons8-great-britain-24.png" alt="" />' +
        '<span>English</span>' +
        '</button>' +
        '<div class="sct-context-menu-separator"></div>' +
        '<button type="button" class="sct-context-menu-item ' +
        (state.currentLang === 'de' ? 'selected' : '') +
        '" data-select-lang="de">' +
        '<img class="sct-context-menu-icon" src="img/icons/icons8-germany-24.png" alt="" />' +
        '<span>German</span>' +
        '</button>' +
        '</div>';
    }

    if (state.isSteamCmdMenuOpen) {
      html +=
        '<div class="sct-custom-context-menu" style="top: ' +
        state.steamCmdMenuTop +
        'px; right: ' +
        state.steamCmdMenuRight +
        'px;" data-stop-propagation="true">' +
        '<button type="button" class="sct-context-menu-item" data-action="steamcmd-start">' +
        '<img class="sct-context-menu-icon" src="img/icons/SquaregreenArrowRight.png" alt="" />' +
        '<span>Start</span>' +
        '</button>' +
        '<div class="sct-context-menu-separator"></div>' +
        '<button type="button" class="sct-context-menu-item" data-action="steamcmd-clear-cache">' +
        '<img class="sct-context-menu-icon" src="img/icons/x1616.png" alt="" />' +
        '<span>Clear Cache</span>' +
        '</button>' +
        '</div>';
    }

    root.innerHTML = html;
  }

  function renderAll() {
    renderTaskBar();
    renderFrameAndLeftColumn();
    renderRightColumn();
    renderContextMenus();
  }

  async function applyLibraryLocation(path) {
    const resolved = await window.sctEnsureLibraryLocation(path);
    state.settings.serverLibraryPath = resolved || path;
    saveSettings();
    await refreshServers({ preserveSelection: true });
    renderAll();
    return state.settings.serverLibraryPath;
  }

  async function ensureValidLibraryLocationOnStartup() {
    const currentPath = (state.settings.serverLibraryPath || '').trim();
    if (currentPath && state.libraryStatus.libraryExists) {
      return;
    }

    const message = !currentPath
      ? 'Welcome to Server Creation Tool!\n\nNo Server Library location has been configured yet.\nPlease select a folder where SteamCMD and your game servers will be stored.'
      : 'The configured Server Library folder could not be found:\n' +
        currentPath +
        '\n\nPlease select a valid Server Library location.';

    const wantsPick = await window.SctModal.confirm(
      message,
      'Select Server Library Location',
      '📁',
      'Select Folder',
      'Later'
    );

    if (!wantsPick) return;

    try {
      const picked = await window.sctPickFolder();
      if (picked) {
        await applyLibraryLocation(picked);
      }
    } catch {}
  }

  async function openSettingsModal() {
    const updated = await window.SctModal.showSettings(state.settings);
    if (updated) {
      const nextLib = (updated.serverLibraryPath || '').trim();
      state.settings = updated;
      if (nextLib) {
        try {
          const resolved = await window.sctEnsureLibraryLocation(nextLib);
          state.settings.serverLibraryPath = resolved || nextLib;
        } catch {}
      }
      saveSettings();
      await refreshServers({ preserveSelection: true });
      renderAll();
      await window.SctModal.alert('Settings have been saved.', 'Settings Saved');
    }
  }

  async function selectLibraryLocation() {
    try {
      const path = await window.sctPickFolder();
      if (path) {
        const finalPath = await applyLibraryLocation(path);
        await window.SctModal.alert(
          'Server Library Location updated to:\n' + finalPath,
          'Library Location'
        );
      }
    } catch {
      await openSettingsModal();
    }
  }

  async function checkForUpdates(isManual) {
    if (state.isCheckingForUpdates) return;
    state.isCheckingForUpdates = true;

    try {
      const result = await window.sctCheckForUpdates();
      if (result && result.updateAvailable) {
        const prompt = await window.SctModal.showUpdateAvailable(
          result.currentVersion,
          result.latestVersion,
          !state.settings.autoCheckUpdates
        );

        const newAutoCheck = !prompt.disableAutoCheck;
        if (newAutoCheck !== state.settings.autoCheckUpdates) {
          state.settings.autoCheckUpdates = newAutoCheck;
          saveSettings();
        }

        if (prompt.openReleasePage) {
          await window.sctOpenUrl(result.releaseUrl);
        }
      } else if (isManual && result) {
        await window.SctModal.alert(
          'You are running the latest version of Server Creation Tool (' +
            result.currentVersion +
            ').',
          'Check for Updates',
          '✅'
        );
      }
    } catch (err) {
      if (isManual) {
        const msg = err && err.message ? err.message : String(err);
        await window.SctModal.alert(
          'Failed to check for updates:\n' + msg,
          'Check for Updates',
          '⚠️'
        );
      }
    } finally {
      state.isCheckingForUpdates = false;
    }
  }

  async function executeSteamInstallOrUpdate(srv, opts) {
    if (!srv || isSteamTaskBusy()) return;

    if (!(state.settings.serverLibraryPath || '').trim() || !state.libraryStatus.libraryExists) {
      await ensureValidLibraryLocationOnStartup();
      if (!(state.settings.serverLibraryPath || '').trim() || !state.libraryStatus.libraryExists) {
        return;
      }
    }

    if (!srv.isSteamServer || !srv.steamAppId) {
      await window.SctModal.alert(
        'Non-Steam server installation is not implemented yet.',
        opts.errorTitle || 'Install',
        '⚠️'
      );
      return;
    }

    const targetId = srv.id;
    srv.isInstalling = true;
    srv.isCancelling = false;
    state.task = {
      active: true,
      serverId: targetId,
      message:
        (opts.isUpdate ? 'Updating/Repairing ' : 'Installing ') +
        srv.name +
        ' server...',
      knownProgress: false,
      progress: 0,
      extraInfo: '',
      cancelling: false
    };
    renderAll();

    try {
      const result = await window.sctInstallOrUpdateSteamServer({
        libraryPath: state.settings.serverLibraryPath,
        codename: srv.codename,
        instanceName: srv.currentInstance,
        steamAppId: srv.steamAppId,
        serverId: targetId,
        minimized: state.settings.runSteamCmdMinimized,
        validate: Boolean(opts.validate)
      });

      state.task.active = false;
      await refreshServerLikeClicked(targetId);

      if (result && result.cancelled) {
        return;
      }

      const updatedSrv = state.servers.find(function (s) {
        return s.id === targetId;
      });
      if (updatedSrv) {
        updatedSrv.lastUpdated = formatDateDmy(new Date());
      }
      renderAll();

      await window.SctModal.alert(opts.successMessage, opts.successTitle);
    } catch (err) {
      state.task.active = false;
      await refreshServerLikeClicked(targetId);
      const msg = err && err.message ? err.message : String(err);
      await window.SctModal.alert(
        opts.errorPrefix + '\n' + msg,
        opts.errorTitle,
        '⚠️'
      );
    }
  }

  async function handleAction(action) {
    const srv = state.selectedServer;

    switch (action) {
      case 'minimize-window':
        window.sctMinimizeWindow();
        break;

      case 'close-window':
        window.sctCloseWindow();
        break;

      case 'select-library-location':
        await selectLibraryLocation();
        break;

      case 'open-settings':
        await openSettingsModal();
        break;

      case 'toggle-lang-menu': {
        state.isSteamCmdMenuOpen = false;
        state.isInstanceMenuOpen = false;
        if (state.isLangMenuOpen) {
          state.isLangMenuOpen = false;
        } else {
          const rect = window.sctGetElementRect('#sct-lang-btn');
          if (rect && rect.bottom > 0) {
            state.langMenuTop = rect.bottom;
            state.langMenuLeft = rect.left;
          }
          state.isLangMenuOpen = true;
        }
        renderAll();
        break;
      }

      case 'open-group-website':
        await window.sctOpenUrl('https://steamcommunity.com/groups/ServerTool');
        break;

      case 'open-about':
        await window.sctOpenAboutWindow();
        break;

      case 'toggle-more-panel': {
        state.isMoreOpen = !state.isMoreOpen;
        renderFrameAndLeftColumn();
        const targetWidth = state.isMoreOpen ? 930 : 820;
        window.sctAnimateWindowWidth(targetWidth, 350);
        break;
      }

      case 'toggle-favorites-filter':
        state.showFavoritesOnly = !state.showFavoritesOnly;
        renderFrameAndLeftColumn();
        break;

      case 'select-welcome':
        state.isWelcomeActive = true;
        state.isInstanceMenuOpen = false;
        renderAll();
        break;

      case 'reload-servers':
        state.isReloading = true;
        renderFrameAndLeftColumn();
        await refreshServers({ preserveSelection: true });
        renderAll();
        setTimeout(function () {
          state.isReloading = false;
          renderFrameAndLeftColumn();
        }, 200);
        break;

      case 'open-donate':
        await window.sctOpenUrl('https://paypal.me/Zeromix');
        break;

      case 'toggle-instance-menu':
        state.isLangMenuOpen = false;
        state.isSteamCmdMenuOpen = false;
        state.isInstanceMenuOpen = !state.isInstanceMenuOpen;
        renderAll();
        break;

      case 'add-new-instance': {
        state.isInstanceMenuOpen = false;
        if (!srv) break;
        const newName = 'instance' + (srv.instances.length + 1);
        srv.instances.push(newName);
        srv.instanceInfos.push({
          name: newName,
          folderName: srv.codename + '_' + newName + '_ds',
          fullPath: getServerInstallPath(srv, newName),
          isInstalled: false,
          hasIncompleteInstall: false,
          sizeBytes: 0,
          size: '-'
        });
        srv.currentInstance = newName;
        srv.isInstalled = false;
        srv.hasIncompleteInstall = false;
        srv.size = '-';
        srv.sizeBytes = 0;
        renderAll();
        await window.SctModal.alert(
          "New instance '" + newName + "' selected (" + getServerFolderName(srv, newName) + ').',
          'Instance Created'
        );
        break;
      }

      case 'start-or-stop': {
        if (!srv || isServerBusy(srv)) break;
        if (srv.isRunning) {
          srv.isRunning = false;
          appendTerminalLine(
            srv,
            'Stopped ' + srv.name + ' (' + srv.currentInstance + ').'
          );
          renderAll();
          await window.SctModal.alert(
            'Server process for ' +
              srv.name +
              ' (' +
              srv.currentInstance +
              ') stopped.',
            'Server Stopped',
            '⏹'
          );
        } else {
          try {
            const available = await window.sctGetAvailableStartOptions(
              state.settings.serverLibraryPath,
              srv.codename,
              srv.currentInstance,
              srv.startOptions || []
            );

            if (!available || available.length === 0) {
              await window.SctModal.alert(
                'No start files were found for ' +
                  srv.name +
                  ' (' +
                  srv.currentInstance +
                  ').',
                'No Start Files Found',
                '⚠️'
              );
              break;
            }

            let chosen = null;
            if (available.length === 1) {
              chosen = available[0];
            } else {
              const prefKey = srv.id + ':' + srv.currentInstance;
              const savedDefaultId = window.sctLoadDefaultStartOption(prefKey);
              chosen = await window.SctModal.showStartOptions(
                srv.name,
                available,
                savedDefaultId,
                function (newDefaultId) {
                  window.sctSaveDefaultStartOption(prefKey, newDefaultId);
                }
              );
              if (!chosen) {
                break;
              }
            }

            await window.sctLaunchServerOption(chosen.resolvedPath, chosen.args || '');
            srv.lastRan = formatDateDmy(new Date());
            appendTerminalLine(
              srv,
              'Launched ' +
                srv.name +
                ' (' +
                srv.currentInstance +
                ') using ' +
                chosen.label +
                '.'
            );
            renderAll();
          } catch (err) {
            const msg = err && err.message ? err.message : String(err);
            await window.SctModal.alert(
              'Failed to start ' + srv.name + ':\n' + msg,
              'Start Error',
              '⚠️'
            );
          }
        }
        break;
      }

      case 'install-server': {
        if (!srv || isSteamTaskBusy()) break;
        const isResumeUpdate = Boolean(srv.isInstalled && srv.hasIncompleteInstall);
        await executeSteamInstallOrUpdate(srv, {
          isUpdate: isResumeUpdate,
          validate: true,
          successTitle: isResumeUpdate ? 'Updated' : 'Installed',
          successMessage: isResumeUpdate
            ? 'Update & validation completed successfully for ' + srv.name + '!'
            : 'Installation completed successfully for ' + srv.name + '!',
          errorTitle: isResumeUpdate ? 'Update Error' : 'Install Error',
          errorPrefix:
            (isResumeUpdate ? 'Failed to update ' : 'Failed to install ') +
            srv.name +
            ':'
        });
        break;
      }

      case 'cancel-install': {
        const activeSrv =
          state.servers.find(function (s) {
            return Boolean(s.isInstalling);
          }) || srv;
        if (!activeSrv || !activeSrv.isInstalling) break;

        if (!activeSrv.isCancelling) {
          activeSrv.isCancelling = true;
          state.task.cancelling = true;
          state.task.message = 'Cancelling SteamCMD...';
          state.task.knownProgress = false;
          state.task.extraInfo = '';
          renderAll();
          try {
            await window.sctCancelSteamCmd(false);
          } catch {}
        } else {
          const confirmForce = await window.SctModal.confirm(
            'Do you want to force stop the app',
            'Force Stop',
            '⚠️',
            'Yes',
            'No'
          );
          if (confirmForce && activeSrv.isInstalling) {
            try {
              await window.sctCancelSteamCmd(true);
            } catch {}
          }
        }
        break;
      }

      case 'toggle-favorite':
        if (srv) {
          srv.isFavorite = !srv.isFavorite;
          if (srv.isFavorite) {
            if (!state.favoriteIds.includes(srv.id)) {
              state.favoriteIds.push(srv.id);
            }
          } else {
            state.favoriteIds = state.favoriteIds.filter(function (id) {
              return id !== srv.id;
            });
          }
          saveFavorites();
          renderAll();
        }
        break;

      case 'open-server-info': {
        if (!srv) break;
        const customFieldsLines = Array.isArray(srv.customFields)
          ? srv.customFields
              .map(function (cf) {
                if (!cf || !cf.name) return '';
                const val = getLocalizedText(cf.values, cf.value || '');
                return cf.name + ': ' + val;
              })
              .filter(Boolean)
          : [];
        const customFieldsText =
          customFieldsLines.length > 0
            ? '\n' + customFieldsLines.join('\n')
            : '';

        await window.SctModal.alert(
          'Game: ' +
            srv.name +
            '\n' +
            'Codename: ' +
            srv.codename +
            '\n' +
            'Folder Name: ' +
            getServerFolderName(srv) +
            '\n' +
            'Steam App ID: ' +
            (srv.isSteamServer ? srv.steamAppId || 'N/A' : 'Non-Steam') +
            '\n' +
            'Directory: ' +
            getServerInstallPath(srv) +
            '\n' +
            'Disk Size: ' +
            srv.size +
            '\n' +
            'Status: ' +
            (srv.hasIncompleteInstall
              ? 'Incomplete Installation'
              : srv.isInstalled
              ? 'Installed'
              : 'Not Installed') +
            '\n' +
            'Active Instance: ' +
            srv.currentInstance +
            customFieldsText,
          srv.name + ' Information'
        );
        break;
      }

      case 'toggle-terminal-expand':
        state.isTerminalExpanded = !state.isTerminalExpanded;
        renderRightColumn();
        break;

      case 'update-or-repair': {
        if (!srv || isSteamTaskBusy()) break;
        const confirmed = await window.SctModal.confirm(
          'Validate and repair installation files for ' + srv.name + '?',
          'Update/Repair',
          '🔄'
        );
        if (!confirmed) break;

        await executeSteamInstallOrUpdate(srv, {
          isUpdate: true,
          validate: true,
          successTitle: 'Verification Complete',
          successMessage: 'Update & validation finished for ' + srv.name + '.',
          errorTitle: 'Update Error',
          errorPrefix: 'Failed to update ' + srv.name + ':'
        });
        break;
      }

      case 'delete-server': {
        if (!srv || isSteamTaskBusy()) break;
        const confirmed = await window.SctModal.confirm(
          'Are you sure you want to delete server files for ' +
            srv.name +
            ' (' +
            srv.currentInstance +
            ')?',
          'Delete Confirmation',
          '⚠️',
          'Delete',
          'Cancel'
        );
        if (!confirmed) break;

        closeContextMenus();
        state.deletingServerId = srv.id;
        srv.isDeleting = true;
        state.task = {
          active: true,
          serverId: srv.id,
          message: 'Deleting ' + srv.name + ' (' + srv.currentInstance + ')...',
          knownProgress: false,
          progress: 0,
          extraInfo: '',
          cancelling: false
        };
        renderAll();

        try {
          await window.sctDeleteServer(
            state.settings.serverLibraryPath,
            srv.codename,
            srv.currentInstance
          );
          state.task.active = false;
          state.deletingServerId = '';
          srv.isDeleting = false;
          await refreshServerLikeClicked(srv.id);
          const updatedSrv = state.selectedServer || srv;
          appendTerminalLine(
            updatedSrv,
            'Deleted ' + srv.name + ' (' + srv.currentInstance + ') files from library.'
          );
          renderAll();
          await window.SctModal.alert(
            'Server ' + srv.name + ' removed from library.',
            'Deleted'
          );
        } catch (err) {
          state.task.active = false;
          state.deletingServerId = '';
          srv.isDeleting = false;
          await refreshServerLikeClicked(srv.id);
          const msg = err && err.message ? err.message : String(err);
          await window.SctModal.alert(
            'Failed to delete server:\n' + msg,
            'Delete Error',
            '⚠️'
          );
        }
        break;
      }

      case 'open-folder':
        if (!srv) break;
        try {
          const activeInst = getActiveInstanceInfo(srv);
          const targetPath =
            (activeInst && activeInst.fullPath) || getServerInstallPath(srv);
          await window.sctOpenFolder(targetPath);
        } catch (err) {
          const msg = err && err.message ? err.message : String(err);
          await window.SctModal.alert(
            'Failed to open folder:\n' + msg,
            'Open Folder',
            '⚠️'
          );
        }
        break;

      case 'backup-server': {
        if (!srv) break;
        if (!srv.isInstalled) {
          await window.SctModal.alert(
            'Cannot backup a server that is not installed.',
            'Backup Server',
            '⚠️'
          );
          break;
        }
        const backupName =
          srv.id +
          '_' +
          srv.currentInstance.replace(/\s+/g, '_') +
          '_' +
          formatBackupTimestamp(new Date()) +
          '.zip';
        await window.SctModal.alert(
          'Backup archive created successfully:\n' +
            state.settings.serverLibraryPath +
            '\\backups\\' +
            backupName,
          'Server Backup',
          '💾'
        );
        break;
      }

      case 'restore-backup':
        if (!srv) break;
        if (!srv.isInstalled) {
          await window.SctModal.alert(
            'Cannot restore a backup to a server that is not installed.',
            'Restore Backup',
            '⚠️'
          );
          break;
        }
        await window.SctModal.alert(
          'Looking for backup archives in:\n' +
            state.settings.serverLibraryPath +
            '\\backups\n\nSelect an archive to restore.',
          'Restore Backup',
          '📦'
        );
        break;

      case 'create-custom-batch':
        if (!srv) break;
        await window.SctModal.alert(
          'Generated start.bat script in ' + srv.name + ' folder.',
          'Batch File Created',
          '📄'
        );
        break;

      case 'open-guide': {
        if (!srv) break;
        const guideUrl = getLocalizedText(
          srv.guideDocLinks,
          srv.guideDocLink || ''
        ).trim();
        if (guideUrl && /^https?:\/\//i.test(guideUrl)) {
          await window.sctOpenUrl(guideUrl);
        } else if (srv.guideContent) {
          await window.SctModal.showGuide(srv.name, srv.guideContent);
        } else {
          await window.SctModal.alert(
            'No installation guide link is configured for ' + srv.name + '.',
            'Installation Guide',
            'ℹ️'
          );
        }
        break;
      }

      case 'save-server-settings':
        if (!srv) break;
        await window.SctModal.alert(
          'Settings for ' + srv.name + ' have been saved successfully.',
          'Settings Saved',
          '💾'
        );
        break;

      case 'reset-server-settings':
        if (!srv) break;
        srv.serverPort = 27015;
        srv.maxPlayers = 16;
        srv.runAsAdmin = false;
        srv.highPriority = true;
        srv.enableConsoleWindow = true;
        srv.autoBackupBeforeLaunch = false;
        renderRightColumn();
        break;

      case 'open-app-logs':
        try {
          await window.sctOpenAppLogs();
        } catch (err) {
          await window.SctModal.alert(
            'Failed to open logs folder: ' + (err && err.message ? err.message : String(err)),
            'Error'
          );
        }
        break;

      case 'toggle-steamcmd-menu': {
        if (isSteamTaskBusy()) break;
        state.isLangMenuOpen = false;
        state.isInstanceMenuOpen = false;
        if (state.isSteamCmdMenuOpen) {
          state.isSteamCmdMenuOpen = false;
        } else {
          const rect = window.sctGetElementRect('#sct-steamcmd-btn');
          if (rect && rect.bottom > 0) {
            state.steamCmdMenuTop = rect.bottom;
            state.steamCmdMenuRight = Math.max(4, rect.right);
          }
          state.isSteamCmdMenuOpen = true;
        }
        renderAll();
        break;
      }

      case 'steamcmd-start': {
        state.isSteamCmdMenuOpen = false;
        renderAll();
        if (isSteamTaskBusy()) {
          await window.SctModal.alert(
            'SteamCMD is already busy with another task.',
            'SteamCMD Busy',
            '⚠️'
          );
          break;
        }
        try {
          const started = await window.sctStartSteamCmd(
            state.settings.serverLibraryPath || ''
          );
          if (!started) {
            const confirm = await window.SctModal.confirm(
              'SteamCMD does not exist in your server directory.\nWould you like to download and setup SteamCMD now?',
              'SteamCMD Missing'
            );
            if (confirm) {
              await window.sctDownloadSteamCmd(
                state.settings.serverLibraryPath || ''
              );
              await refreshServerLikeClicked(srv ? srv.id : null);
              await window.sctStartSteamCmd(
                state.settings.serverLibraryPath || ''
              );
            }
          }
        } catch (err) {
          await refreshServerLikeClicked(srv ? srv.id : null);
          await window.SctModal.alert(
            'Failed to start SteamCMD: ' + (err && err.message ? err.message : String(err)),
            'SteamCMD Error'
          );
        }
        break;
      }

      case 'steamcmd-clear-cache':
        state.isSteamCmdMenuOpen = false;
        renderAll();
        try {
          await window.sctClearSteamCmdCache(state.settings.serverLibraryPath || '');
          await window.SctModal.alert(
            'SteamCMD cache cleared successfully.',
            'Done'
          );
        } catch {
          await window.SctModal.alert('SteamCMD cache cleared.', 'Done');
        }
        break;

      case 'open-planned-features':
        await window.sctOpenUrl('https://trello.com/b/9dsN6TIT/server-creation-tool');
        break;

      case 'open-faq':
        await window.sctOpenFaqWindow();
        break;

      case 'open-port-forward-guide':
        await window.sctOpenUrl(
          'https://steamcommunity.com/sharedfiles/filedetails/?id=2432446663'
        );
        break;

      case 'open-discord':
        await window.sctOpenUrl('https://discord.gg/79eKCs5fW8');
        break;

      case 'open-github':
        await window.sctOpenUrl('https://github.com/Zeromix9/Server-Creation-Tool');
        break;

      case 'check-for-updates':
        await checkForUpdates(true);
        break;

      case 'close-context-menus':
        closeContextMenus();
        renderAll();
        break;
    }
  }

  async function executeCustomControlAction(srv, ctrl, labelOverride) {
    if (!srv || !ctrl) return;
    if (
      ctrl.isSystemGuideBtn ||
      ctrl.buttonFuncName === 'OpenInstallationGuide'
    ) {
      await handleAction('open-guide');
      return;
    }

    const label =
      labelOverride ||
      getLocalizedText(ctrl.texts, ctrl.text || 'Control');

    if (ctrl.actionType === 'simple_action' && ctrl.simpleActionId) {
      const act = (srv.simpleActions || []).find(function (a) {
        return a && a.id === ctrl.simpleActionId;
      });
      if (!act) {
        await window.SctModal.alert(
          "Simple action '" + ctrl.simpleActionId + "' was not found in server metadata.",
          label,
          '⚠️'
        );
        return;
      }
      const actTitle = getLocalizedText(act.title, act.label || act.id);
      appendTerminalLine(
        srv,
        'Triggered action: ' + actTitle + ' [' + act.actionType + ']'
      );
      await window.SctModal.alert(
        'Action: ' +
          actTitle +
          '\nType: ' +
          act.actionType,
        label,
        '⚡'
      );
      return;
    }

    if (ctrl.buttonFuncName) {
      appendTerminalLine(
        srv,
        'Triggered function: ' + ctrl.buttonFuncName + '()'
      );
      await window.SctModal.alert(
        'Function: ' + ctrl.buttonFuncName + '()',
        label,
        '⚡'
      );
      return;
    }

    await window.SctModal.alert(
      'No action or function is assigned to "' + label + '".',
      label,
      'ℹ️'
    );
  }

  function setupListeners() {
    const searchInput = document.getElementById('sct-search-input');
    if (searchInput) {
      searchInput.addEventListener('input', function () {
        state.searchQuery = searchInput.value;
        renderFrameAndLeftColumn();
      });
    }

    document.addEventListener('click', async function (e) {
      if (
        state.openCustomDropdownIdx !== undefined &&
        state.openCustomDropdownIdx !== null &&
        !e.target.closest('.sct-custom-dropdown-wrap')
      ) {
        state.openCustomDropdownIdx = null;
        renderRightColumn();
      }

      const srvBtn = e.target.closest('[data-srv-id]');
      if (srvBtn) {
        const srvId = srvBtn.getAttribute('data-srv-id');
        await refreshServers({ preserveSelection: true });
        const found = state.servers.find(function (s) {
          return s.id === srvId;
        });
        if (found) {
          state.selectedServer = found;
          state.isWelcomeActive = false;
          state.isInstanceMenuOpen = false;
          state.openCustomDropdownIdx = null;
          renderAll();
        }
        return;
      }

      const tabBtn = e.target.closest('[data-tab]');
      if (tabBtn) {
        state.activeTab = tabBtn.getAttribute('data-tab');
        state.openCustomDropdownIdx = null;
        renderRightColumn();
        return;
      }

      const instBtn = e.target.closest('[data-switch-instance]');
      if (instBtn && state.selectedServer) {
        const nextInst = instBtn.getAttribute('data-switch-instance');
        state.selectedServer.currentInstance = nextInst;
        const instInfo = getActiveInstanceInfo(state.selectedServer);
        if (instInfo) {
          state.selectedServer.isInstalled = Boolean(instInfo.isInstalled);
          state.selectedServer.hasIncompleteInstall = Boolean(
            instInfo.hasIncompleteInstall
          );
          state.selectedServer.size = instInfo.size || '-';
          state.selectedServer.sizeBytes = instInfo.sizeBytes || 0;
        }
        state.isInstanceMenuOpen = false;
        renderAll();
        return;
      }

      const langBtn = e.target.closest('[data-select-lang]');
      if (langBtn) {
        const lang = langBtn.getAttribute('data-select-lang');
        state.currentLang = lang;
        state.isLangMenuOpen = false;
        state.settings.language = lang === 'de' ? 'German' : 'English';
        state.servers.forEach(applyLanguageToServer);
        saveSettings();
        renderAll();
        await window.SctModal.alert(
          'Language changed to ' + state.settings.language + '.',
          'Language'
        );
        return;
      }

      const cfgBtn = e.target.closest('[data-edit-config]');
      if (cfgBtn && state.selectedServer) {
        const srv = state.selectedServer;
        const rawFile = cfgBtn.getAttribute('data-edit-config') || '';
        const relFile = rawFile.replace(/^[\\/]+/, '');
        if (!srv.isInstalled) {
          await window.SctModal.alert(
            'Server is not installed yet.\nInstall ' +
              srv.name +
              ' first to edit ' +
              relFile +
              '.',
            'Config File Not Found',
            '⚠️'
          );
          return;
        }
        const activeInst = getActiveInstanceInfo(srv);
        const baseDir =
          (activeInst && activeInst.fullPath) || getServerInstallPath(srv);
        const filePath = baseDir + '\\' + relFile.replace(/\//g, '\\');
        try {
          await window.sctOpenFile(filePath);
        } catch (err) {
          const msg = err && err.message ? err.message : String(err);
          await window.SctModal.alert(
            "Could not open '" + filePath + "':\n" + msg,
            'Edit Config File',
            '⚠️'
          );
        }
        return;
      }

      const subCtrlBtn = e.target.closest('[data-custom-ctrl-sub]');
      if (subCtrlBtn && state.selectedServer) {
        const srv = state.selectedServer;
        const raw = subCtrlBtn.getAttribute('data-custom-ctrl-sub') || '';
        const parts = raw.split(':');
        const ctrlIdx = parseInt(parts[0], 10);
        const subIdx = parseInt(parts[1], 10);
        state.openCustomDropdownIdx = null;
        renderRightColumn();
        const ctrl =
          srv.customControls && srv.customControls[ctrlIdx];
        const sub =
          ctrl &&
          Array.isArray(ctrl.submenuButtons) &&
          ctrl.submenuButtons[subIdx];
        if (sub) {
          await executeCustomControlAction(
            srv,
            sub,
            getLocalizedText(sub.texts, sub.text || 'Submenu Option')
          );
        }
        return;
      }

      const customCtrlBtn = e.target.closest('[data-custom-ctrl-idx]');
      if (customCtrlBtn && state.selectedServer) {
        const srv = state.selectedServer;
        const ctrlIdx = parseInt(
          customCtrlBtn.getAttribute('data-custom-ctrl-idx') || '-1',
          10
        );
        const ctrl =
          srv.customControls && srv.customControls[ctrlIdx];
        if (!ctrl) return;

        if ((ctrl.controlType || '').toLowerCase() === 'dropdownbutton') {
          state.openCustomDropdownIdx =
            state.openCustomDropdownIdx === ctrlIdx ? null : ctrlIdx;
          renderRightColumn();
          return;
        }

        await executeCustomControlAction(srv, ctrl);
        return;
      }

      const actionBtn = e.target.closest('[data-action]');
      if (actionBtn) {
        const action = actionBtn.getAttribute('data-action');
        await handleAction(action);
      }
    });

    document.addEventListener('change', async function (e) {
      if (!state.selectedServer) return;
      const srv = state.selectedServer;

      const customCheck = e.target.closest('[data-custom-ctrl-check]');
      if (customCheck) {
        const ctrlIdx = parseInt(
          customCheck.getAttribute('data-custom-ctrl-check') || '-1',
          10
        );
        if (!srv.customCheckStates) {
          srv.customCheckStates = {};
        }
        srv.customCheckStates[ctrlIdx] = customCheck.checked;
        const ctrl = srv.customControls && srv.customControls[ctrlIdx];
        if (ctrl) {
          await executeCustomControlAction(srv, ctrl);
        }
        return;
      }

      const propCb = e.target.closest('[data-srv-prop]');
      if (propCb) {
        const prop = propCb.getAttribute('data-srv-prop');
        srv[prop] = propCb.checked;
        if (prop === 'autoRestartOnConfigChange') {
          renderRightColumn();
        }
        return;
      }

      const numInput = e.target.closest('[data-srv-num]');
      if (numInput) {
        const prop = numInput.getAttribute('data-srv-num');
        const parsed = parseInt(numInput.value, 10);
        if (!Number.isNaN(parsed)) {
          srv[prop] = parsed;
        }
        return;
      }

      const sel = e.target.closest('[data-srv-select]');
      if (sel) {
        const prop = sel.getAttribute('data-srv-select');
        srv[prop] = sel.value;
      }
    });
  }

  document.addEventListener('DOMContentLoaded', async function () {
    loadSettings();
    loadFavorites();
    setupListeners();

    await window.sctListenServerLogs(function (payload) {
      if (!payload || !payload.serverId || !payload.line) return;
      const target = state.servers.find(function (s) {
        return s.id === payload.serverId;
      });
      if (target) {
        appendTerminalLine(target, payload.line);
      }
    });

    await window.sctListenTaskState(function (payload) {
      if (!payload) return;
      state.task = {
        active: Boolean(payload.active),
        serverId: payload.serverId || '',
        message: payload.message || '',
        knownProgress: Boolean(payload.knownProgress),
        progress: Number(payload.progress) || 0,
        extraInfo: payload.extraInfo || '',
        cancelling: Boolean(payload.cancelling)
      };
      renderTaskBar();
    });

    await refreshServers({ preserveSelection: false });
    renderAll();

    await ensureValidLibraryLocationOnStartup();

    if (state.settings.autoCheckUpdates) {
      checkForUpdates(false);
    }
  });
})();
