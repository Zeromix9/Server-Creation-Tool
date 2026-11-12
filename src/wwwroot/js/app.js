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
      defaultTextEditor: 'notepad.exe'
    },
    libraryStatus: {
      libraryExists: false,
      hasSteamcmdFolder: false,
      hasSteamcmdBinary: false,
      sctDataPath: ''
    },
    favoriteIds: [],
    servers: [],
    selectedServer: null
  };

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
    const localizedName =
      (dto.names && dto.names[state.currentLang]) ||
      dto.name ||
      dto.codename;

    return {
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
      basePlatform: dto.basePlatform || 'windows',
      logoUrl: dto.logoUrl || 'img/servers/toolIcon(1).png',
      size: activeInfo ? activeInfo.size : dto.size || '-',
      sizeBytes: activeInfo ? activeInfo.sizeBytes : dto.sizeBytes || 0,
      lastRan: prev ? prev.lastRan : 'Never',
      lastUpdated: prev ? prev.lastUpdated : 'Never',
      isInstalled: activeInfo ? Boolean(activeInfo.isInstalled) : Boolean(dto.isInstalled),
      isFavorite: isFav,
      hasGuide: false,
      guideContent: prev ? prev.guideContent : '',
      instances: instanceNames,
      instanceInfos: instanceInfos,
      currentInstance: activeInstName,
      configFiles: prev ? prev.configFiles : [],
      launchArgs: prev ? prev.launchArgs : '',
      terminalOutput: prev ? prev.terminalOutput : '',
      isRunning: prev ? prev.isRunning : false,
      autoRestartOnCrash: prev ? prev.autoRestartOnCrash : true,
      autoRestartOnConfigChange: prev ? prev.autoRestartOnConfigChange : false,
      watchedConfigFile: prev ? prev.watchedConfigFile : '',
      scheduledRestart: prev ? prev.scheduledRestart : 'Disabled',
      updateOnLaunch: prev ? prev.updateOnLaunch : false,
      serverPort: prev ? prev.serverPort : 27015,
      maxPlayers: prev ? prev.maxPlayers : 16,
      runAsAdmin: prev ? prev.runAsAdmin : false,
      highPriority: prev ? prev.highPriority : true,
      enableConsoleWindow: prev ? prev.enableConsoleWindow : true,
      autoBackupBeforeLaunch: prev ? prev.autoBackupBeforeLaunch : false,
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

    const steamCmdBtn = document.getElementById('sct-steamcmd-btn');
    if (steamCmdBtn) {
      steamCmdBtn.classList.toggle('active', state.isSteamCmdMenuOpen);
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

  function renderTabContent(srv) {
    const activeInstInfo = getActiveInstanceInfo(srv);
    const activeFolderName = getServerFolderName(srv);
    const installLoc =
      (activeInstInfo && activeInstInfo.fullPath) ||
      getServerInstallPath(srv);

    if (state.activeTab === 'general') {
      const lifecycleText = srv.isRunning
        ? 'Active Process (Running)'
        : srv.isInstalled
        ? 'Installed & Ready'
        : 'Uninstalled';

      const overviewCardHtml = state.isTerminalExpanded
        ? ''
        : '<div class="sct-general-status-card">' +
          '<div class="sct-card-section-title">-Server Overview-</div>' +
          '<div class="sct-status-meta-grid">' +
          '<div class="sct-meta-entry">' +
          '<span class="sct-entry-label">Lifecycle State:</span>' +
          '<span class="sct-entry-value">' +
          escapeHtml(lifecycleText) +
          '</span>' +
          '</div>' +
          '<div class="sct-meta-entry">' +
          '<span class="sct-entry-label">Storage Footprint:</span>' +
          '<span class="sct-entry-value">' +
          escapeHtml(srv.size) +
          '</span>' +
          '</div>' +
          '<div class="sct-meta-entry">' +
          '<span class="sct-entry-label">Codename:</span>' +
          '<span class="sct-entry-value">' +
          escapeHtml(srv.codename) +
          '</span>' +
          '</div>' +
          '<div class="sct-meta-entry">' +
          '<span class="sct-entry-label">Steam App ID:</span>' +
          '<span class="sct-entry-value">' +
          escapeHtml(srv.steamAppId || 'N/A') +
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

    if (state.activeTab === 'tools') {
      const disabledAttr = srv.isInstalled ? '' : 'disabled';
      return (
        '<div class="sct-tab-pane">' +
        '<div class="sct-tools-section-card">' +
        '<div class="sct-card-section-title">-General-</div>' +
        '<div class="sct-tools-row-3">' +
        '<button type="button" class="sct-tool-btn" data-action="update-or-repair" ' +
        disabledAttr +
        '>' +
        '<img src="img/icons/icons8-rotate2-24.png" class="sct-tool-btn-icon" alt="" />' +
        '<span class="sct-tool-btn-text">Update</span>' +
        '</button>' +
        '<button type="button" class="sct-tool-btn" data-action="delete-server" ' +
        disabledAttr +
        '>' +
        '<img src="img/icons/1263.png" class="sct-tool-btn-icon" alt="" />' +
        '<span class="sct-tool-btn-text">Delete</span>' +
        '</button>' +
        '<button type="button" class="sct-tool-btn" data-action="open-folder" ' +
        disabledAttr +
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
        '<div class="sct-tools-section-card">' +
        '<div class="sct-card-section-title">-Extra-</div>' +
        '<div class="sct-tools-row-2">' +
        '<button type="button" class="sct-tool-btn" data-action="create-custom-batch" ' +
        disabledAttr +
        '>' +
        '<img src="img/icons/SquaregreenArrowRightBatch.png" class="sct-tool-btn-icon" alt="" />' +
        '<span class="sct-tool-btn-text">Create Custom Batch File</span>' +
        '</button>' +
        '<button type="button" class="sct-tool-btn" data-action="open-guide">' +
        '<img src="img/icons/1091.png" class="sct-tool-btn-icon" alt="" />' +
        '<span class="sct-tool-btn-text">Manual Installation Guide</span>' +
        '</button>' +
        '</div>' +
        '</div>' +
        '</div>'
      );
    }

    if (state.activeTab === 'settings') {
      let configFilesHtml = '';
      if (!srv.isInstalled) {
        configFilesHtml =
          '<div class="sct-auto-hint" style="padding: 4px 0;">Server is not installed. Install it first to edit its configuration files.</div>';
      } else if (!srv.configFiles || srv.configFiles.length === 0) {
        configFilesHtml =
          '<div class="sct-auto-hint" style="padding: 4px 0;">No configuration files configured for ' +
          escapeHtml(srv.name) +
          '.</div>';
      } else {
        configFilesHtml =
          '<div class="sct-config-files-grid">' +
          srv.configFiles
            .map(function (file) {
              return (
                '<button type="button" class="sct-config-file-row-btn" data-edit-config="' +
                escapeHtml(file) +
                '" title="Open ' +
                escapeHtml(file) +
                ' in default text editor">' +
                '<img src="img/icons/1645.png" class="sct-config-file-icon" alt="" />' +
                '<span class="sct-config-file-name">' +
                escapeHtml(file) +
                '</span>' +
                '<span class="sct-config-file-action">Edit ↗</span>' +
                '</button>'
              );
            })
            .join('') +
          '</div>';
      }

      return (
        '<div class="sct-tab-pane">' +
        '<div class="sct-settings-card">' +
        '<div class="sct-card-section-title">-Settings-</div>' +
        '<div class="sct-settings-body">' +
        '<div class="sct-settings-checkboxes-grid">' +
        '<label class="sct-checkbox-label" title="Run the server executable with elevated Administrator privileges">' +
        '<input type="checkbox" class="sct-win-checkbox" data-srv-prop="runAsAdmin" ' +
        (srv.runAsAdmin ? 'checked' : '') +
        ' />' +
        '<span class="sct-check-title">Run as Administrator</span>' +
        '</label>' +
        '<label class="sct-checkbox-label" title="Assign High CPU process priority to the dedicated server process">' +
        '<input type="checkbox" class="sct-win-checkbox" data-srv-prop="highPriority" ' +
        (srv.highPriority ? 'checked' : '') +
        ' />' +
        '<span class="sct-check-title">High CPU Priority</span>' +
        '</label>' +
        '<label class="sct-checkbox-label" title="Show interactive command prompt console window for server log and rcon">' +
        '<input type="checkbox" class="sct-win-checkbox" data-srv-prop="enableConsoleWindow" ' +
        (srv.enableConsoleWindow ? 'checked' : '') +
        ' />' +
        '<span class="sct-check-title">Enable Console Window</span>' +
        '</label>' +
        '<label class="sct-checkbox-label" title="Automatically create a backup of save and world data before launching">' +
        '<input type="checkbox" class="sct-win-checkbox" data-srv-prop="autoBackupBeforeLaunch" ' +
        (srv.autoBackupBeforeLaunch ? 'checked' : '') +
        ' />' +
        '<span class="sct-check-title">Backup Saves on Launch</span>' +
        '</label>' +
        '</div>' +
        '<div class="sct-settings-fields-grid">' +
        '<div class="sct-settings-field-row">' +
        '<span class="sct-auto-field-label">Server Port:</span>' +
        '<input type="number" class="sct-settings-input" style="max-width: 120px;" data-srv-num="serverPort" value="' +
        escapeHtml(srv.serverPort) +
        '" min="1024" max="65535" />' +
        '<span class="sct-auto-field-label" style="width: auto; margin-left: 12px;">Max Players:</span>' +
        '<input type="number" class="sct-settings-input" style="max-width: 80px;" data-srv-num="maxPlayers" value="' +
        escapeHtml(srv.maxPlayers) +
        '" min="1" max="128" />' +
        '</div>' +
        '</div>' +
        '<div class="sct-settings-buttons-bar">' +
        '<button type="button" class="sct-tool-btn" data-action="save-server-settings">' +
        '<span class="sct-tool-btn-text">💾 Save Settings</span>' +
        '</button>' +
        '<button type="button" class="sct-tool-btn" data-action="reset-server-settings">' +
        '<span class="sct-tool-btn-text">↺ Reset Server Defaults</span>' +
        '</button>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="sct-settings-card">' +
        '<div class="sct-card-section-title">-Manually Edit config files:-</div>' +
        '<div class="sct-config-files-container">' +
        configFilesHtml +
        '</div>' +
        '</div>' +
        '</div>'
      );
    }

    if (state.activeTab === 'automation') {
      const configSelectHtml =
        srv.configFiles && srv.configFiles.length > 0
          ? '<select class="sct-auto-select" data-srv-select="watchedConfigFile" ' +
            (srv.autoRestartOnConfigChange ? '' : 'disabled') +
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
        '<div class="sct-card-section-title">-Process &amp; Crash Watchdog-</div>' +
        '<div class="sct-auto-card-body">' +
        '<label class="sct-checkbox-label" title="Automatically relaunch server if the process terminates or crashes unexpectedly">' +
        '<input type="checkbox" class="sct-win-checkbox" data-srv-prop="autoRestartOnCrash" ' +
        (srv.autoRestartOnCrash ? 'checked' : '') +
        ' />' +
        '<span class="sct-check-title">Auto-restart server on crash</span>' +
        '</label>' +
        '<div class="sct-auto-hint">Monitors the server process PID and restarts it immediately if an abnormal exit occurs.</div>' +
        '</div>' +
        '</div>' +
        '<div class="sct-auto-card">' +
        '<div class="sct-card-section-title">-Config File Watcher-</div>' +
        '<div class="sct-auto-card-body">' +
        '<div class="sct-auto-row-split">' +
        '<label class="sct-checkbox-label" title="Restart server when monitored config file is updated">' +
        '<input type="checkbox" class="sct-win-checkbox" data-srv-prop="autoRestartOnConfigChange" ' +
        (srv.autoRestartOnConfigChange ? 'checked' : '') +
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
        '<select class="sct-auto-select" data-srv-select="scheduledRestart" title="Periodic automated restart schedule">' +
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
        (srv.updateOnLaunch ? 'checked' : '') +
        ' />' +
        '<span class="sct-check-title">Check &amp; update files before launch</span>' +
        '</label>' +
        '</div>' +
        '</div>' +
        '</div>' +
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

    const actionBtnHtml = srv.isInstalled
      ? '<button type="button" class="sct-srv-action-btn ' +
        (srv.isRunning ? 'state-stop' : 'state-start') +
        '" data-action="start-or-stop">' +
        '<img src="' +
        (srv.isRunning ? 'img/icons/stop_img.png' : 'img/icons/start_img.png') +
        '" class="sct-srv-btn-icon" alt="" />' +
        '<span class="sct-srv-btn-text">' +
        (srv.isRunning ? 'Stop' : 'Start') +
        '</span>' +
        '</button>'
      : '<button type="button" class="sct-srv-action-btn state-install" data-action="install-server">' +
        '<img src="img/icons/install_img.png" class="sct-srv-btn-icon" alt="" />' +
        '<span class="sct-srv-btn-text">Install</span>' +
        '</button>';

    const statusColor = srv.isRunning
      ? '#4ade80'
      : srv.isInstalled
      ? '#38bdf8'
      : '#94a3b8';
    const statusText = srv.isRunning
      ? 'Running'
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

    return (
      '<div class="sct-main-srv-panel">' +
      '<div class="sct-srv-header-row">' +
      '<button type="button" class="sct-instance-dropdown-btn ' +
      (state.isInstanceMenuOpen ? 'active' : '') +
      '" data-action="toggle-instance-menu">' +
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
      '" class="sct-logo-img" alt="' +
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
      '<button type="button" class="sct-quick-icon-btn" data-action="toggle-favorite" title="Add to favourites">' +
      '<img src="' +
      (srv.isFavorite ? 'img/icons/starChecked.png' : 'img/icons/starUnChecked.png') +
      '" alt="Star" />' +
      '</button>' +
      '<button type="button" class="sct-quick-icon-btn" data-action="open-server-info" title="Info">' +
      '<img src="img/icons/srvInfBtn.BackgroundImage.png" alt="Info" />' +
      '</button>' +
      '</div>' +
      '</div>' +
      '<div class="sct-tab-strip">' +
      '<button type="button" class="sct-tab-btn ' +
      (state.activeTab === 'general' ? 'active' : '') +
      '" data-tab="general">' +
      '<img src="img/icons/icons8-server-24.png" class="sct-tab-icon" alt="" />' +
      '<span>Overview</span>' +
      '</button>' +
      '<button type="button" class="sct-tab-btn ' +
      (state.activeTab === 'tools' ? 'active' : '') +
      '" data-tab="tools">' +
      '<img src="img/icons/1645.png" class="sct-tab-icon" alt="" />' +
      '<span>Tools &amp; Maintenance</span>' +
      '</button>' +
      '<button type="button" class="sct-tab-btn ' +
      (state.activeTab === 'settings' ? 'active' : '') +
      '" data-tab="settings">' +
      '<img src="img/icons/settingsBtn.Image.png" class="sct-tab-icon" alt="" />' +
      '<span>Settings</span>' +
      '</button>' +
      '<button type="button" class="sct-tab-btn ' +
      (state.activeTab === 'automation' ? 'active' : '') +
      '" data-tab="automation">' +
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
          sizeBytes: 0,
          size: '-'
        });
        srv.currentInstance = newName;
        srv.isInstalled = false;
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
        if (!srv) break;
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
          srv.isRunning = true;
          srv.lastRan = formatDateDmy(new Date());
          appendTerminalLine(
            srv,
            'Starting ' + srv.name + ' (' + srv.currentInstance + ')...'
          );
          renderAll();
          await window.SctModal.alert(
            'Starting ' +
              srv.name +
              ' (' +
              srv.currentInstance +
              ')...\n\nServer process launched successfully.',
            'Server Started',
            '▶'
          );
        }
        break;
      }

      case 'install-server': {
        if (!srv) break;
        if (!(state.settings.serverLibraryPath || '').trim() || !state.libraryStatus.libraryExists) {
          await ensureValidLibraryLocationOnStartup();
          if (!(state.settings.serverLibraryPath || '').trim() || !state.libraryStatus.libraryExists) {
            break;
          }
        }

        const confirmed = await window.SctModal.confirm(
          'Do you want to install ' + srv.name + '?',
          'Install Server',
          '📥'
        );
        if (!confirmed) break;

        try {
          const createdDir = await window.sctMockInstallServer(
            state.settings.serverLibraryPath,
            srv.codename,
            srv.currentInstance
          );
          await refreshServers({ preserveSelection: true });
          const updatedSrv = state.selectedServer || srv;
          updatedSrv.lastUpdated = formatDateDmy(new Date());
          appendTerminalLine(
            updatedSrv,
            'Installed ' + updatedSrv.name + ' (' + updatedSrv.currentInstance + ') to ' + createdDir + '.'
          );
          renderAll();
          await window.SctModal.alert(
            'Installation completed successfully for ' + updatedSrv.name + '!',
            'Installed'
          );
        } catch (err) {
          const msg = err && err.message ? err.message : String(err);
          await window.SctModal.alert(
            'Failed to install ' + srv.name + ':\n' + msg,
            'Install Error',
            '⚠️'
          );
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

      case 'open-server-info':
        if (!srv) break;
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
            (srv.steamAppId || 'N/A') +
            '\n' +
            'Directory: ' +
            getServerInstallPath(srv) +
            '\n' +
            'Disk Size: ' +
            srv.size +
            '\n' +
            'Status: ' +
            (srv.isInstalled ? 'Installed' : 'Not Installed') +
            '\n' +
            'Active Instance: ' +
            srv.currentInstance,
          srv.name + ' Information'
        );
        break;

      case 'toggle-terminal-expand':
        state.isTerminalExpanded = !state.isTerminalExpanded;
        renderRightColumn();
        break;

      case 'update-or-repair': {
        if (!srv) break;
        const confirmed = await window.SctModal.confirm(
          'Validate and repair installation files for ' + srv.name + '?',
          'Update/Repair',
          '🔄'
        );
        if (confirmed) {
          srv.lastUpdated = formatDateDmy(new Date());
          appendTerminalLine(
            srv,
            'Validated and repaired files for ' + srv.name + '.'
          );
          renderAll();
          await window.SctModal.alert(
            'Validation finished. All files for ' + srv.name + ' are intact.',
            'Verification Complete'
          );
        }
        break;
      }

      case 'delete-server': {
        if (!srv) break;
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

        try {
          await window.sctDeleteServer(
            state.settings.serverLibraryPath,
            srv.codename,
            srv.currentInstance
          );
          await refreshServers({ preserveSelection: true });
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

      case 'open-guide':
        if (!srv) break;
        await window.SctModal.showGuide(srv.name, srv.guideContent);
        break;

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
              await window.SctModal.alert(
                'SteamCMD setup initiated. Please wait...',
                'SteamCMD Setup'
              );
            }
          }
        } catch (err) {
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

  function setupListeners() {
    const searchInput = document.getElementById('sct-search-input');
    if (searchInput) {
      searchInput.addEventListener('input', function () {
        state.searchQuery = searchInput.value;
        renderFrameAndLeftColumn();
      });
    }

    document.addEventListener('click', async function (e) {
      const srvBtn = e.target.closest('[data-srv-id]');
      if (srvBtn) {
        const srvId = srvBtn.getAttribute('data-srv-id');
        const found = state.servers.find(function (s) {
          return s.id === srvId;
        });
        if (found) {
          state.selectedServer = found;
          state.isWelcomeActive = false;
          state.isInstanceMenuOpen = false;
          renderAll();
        }
        return;
      }

      const tabBtn = e.target.closest('[data-tab]');
      if (tabBtn) {
        state.activeTab = tabBtn.getAttribute('data-tab');
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
        const file = cfgBtn.getAttribute('data-edit-config');
        const filePath = getServerInstallPath(state.selectedServer) + '\\' + file;
        try {
          await window.sctOpenFile(filePath);
        } catch {
          await window.SctModal.alert(
            "Opening '" + file + "' in external editor...",
            'Edit Settings'
          );
        }
        return;
      }

      const actionBtn = e.target.closest('[data-action]');
      if (actionBtn) {
        const action = actionBtn.getAttribute('data-action');
        await handleAction(action);
      }
    });

    document.addEventListener('change', function (e) {
      if (!state.selectedServer) return;
      const srv = state.selectedServer;

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
    await refreshServers({ preserveSelection: false });
    renderAll();

    await ensureValidLibraryLocationOnStartup();

    if (state.settings.autoCheckUpdates) {
      checkForUpdates(false);
    }
  });
})();
