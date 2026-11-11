using System;
using System.Collections.Generic;

namespace ServerCreationTool.UI.Models;

public class ServerItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public string Size { get; set; } = "0 MB";
    public string LastRan { get; set; } = "Never";
    public string LastUpdated { get; set; } = "Never";
    public bool IsInstalled { get; set; }
    public bool IsFavorite { get; set; }
    public bool HasGuide { get; set; } = true;
    public string GuideContent { get; set; } = string.Empty;
    public bool HasCustomBatch { get; set; } = true;
    public bool HasCustomConfig { get; set; } = true;
    public List<string> Instances { get; set; } = new() { "Default" };
    public string CurrentInstance { get; set; } = "Default";
    public List<string> ConfigFiles { get; set; } = new() { "server.cfg" };
    public string LaunchArgs { get; set; } = "";
    public string TerminalOutput { get; set; } = string.Empty;
    public bool IsRunning { get; set; } = false;

    // Universal Automation & Watchdog
    public bool AutoRestartOnCrash { get; set; } = true;
    public bool AutoRestartOnConfigChange { get; set; } = false;
    public string WatchedConfigFile { get; set; } = "";
    public string ScheduledRestart { get; set; } = "Disabled";
    public bool UpdateOnLaunch { get; set; } = false;

    // Server Execution & Configuration Settings
    public int ServerPort { get; set; } = 27015;
    public int MaxPlayers { get; set; } = 16;
    public bool RunAsAdmin { get; set; } = false;
    public bool HighPriority { get; set; } = true;
    public bool EnableConsoleWindow { get; set; } = true;
    public bool AutoBackupBeforeLaunch { get; set; } = false;
}

public class AppSettings
{
    public string ServerLibraryPath { get; set; } = @"C:\Server_Library";
    public string Language { get; set; } = "English";
    public bool AutoCheckUpdates { get; set; } = true;
    public bool CloseToTray { get; set; } = false;
    public bool RunSteamCmdMinimized { get; set; } = true;
    public string DefaultTextEditor { get; set; } = "notepad.exe";
}

public class BoundingClientRect
{
    public double Top { get; set; }
    public double Bottom { get; set; }
    public double Left { get; set; }
    public double Right { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double WindowHeight { get; set; }
    public double WindowWidth { get; set; }
}
