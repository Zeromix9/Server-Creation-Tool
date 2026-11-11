using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerCreationTool.UI.Models;

namespace ServerCreationTool.UI.Services;

public class ModalService
{
    // Alert state
    internal bool AlertVisible { get; private set; }
    internal string AlertTitle { get; private set; } = string.Empty;
    internal string AlertMessage { get; private set; } = string.Empty;
    internal string AlertIcon { get; private set; } = "ℹ️";
    private TaskCompletionSource<bool>? _alertTcs;

    // Confirm state
    internal bool ConfirmVisible { get; private set; }
    internal string ConfirmTitle { get; private set; } = string.Empty;
    internal string ConfirmMessage { get; private set; } = string.Empty;
    internal string ConfirmIcon { get; private set; } = "❓";
    internal string ConfirmOkLabel { get; private set; } = "Yes";
    internal string ConfirmCancelLabel { get; private set; } = "No";
    private TaskCompletionSource<bool>? _confirmTcs;

    // Settings Modal
    internal bool SettingsVisible { get; private set; }
    internal AppSettings? TempSettings { get; private set; }
    private TaskCompletionSource<AppSettings?>? _settingsTcs;

    // About Modal
    internal bool AboutVisible { get; private set; }

    // FAQ Modal
    internal bool FaqVisible { get; private set; }

    // Guide Modal
    internal bool GuideVisible { get; private set; }
    internal string GuideTitle { get; private set; } = string.Empty;
    internal string GuideText { get; private set; } = string.Empty;

    // SteamCMD Options Modal
    internal bool SteamCmdOptionsVisible { get; private set; }

    // Edit Settings / Config Modal
    internal bool EditSettingsVisible { get; private set; }
    internal string EditSettingsGame { get; private set; } = string.Empty;
    internal List<string> EditSettingsFiles { get; private set; } = new();

    public event Action? OnChange;

    public bool IsOpen => AlertVisible || ConfirmVisible || SettingsVisible || AboutVisible || FaqVisible || GuideVisible || SteamCmdOptionsVisible || EditSettingsVisible;

    public Task AlertAsync(string message, string title = "Information", string icon = "ℹ️")
    {
        AlertTitle = title;
        AlertMessage = message;
        AlertIcon = icon;
        AlertVisible = true;
        _alertTcs = new TaskCompletionSource<bool>();
        NotifyChange();
        return _alertTcs.Task;
    }

    public void CloseAlert()
    {
        AlertVisible = false;
        _alertTcs?.TrySetResult(true);
        NotifyChange();
    }

    public Task<bool> ConfirmAsync(
        string message,
        string title = "Confirm",
        string icon = "❓",
        string okLabel = "Yes",
        string cancelLabel = "No")
    {
        ConfirmTitle = title;
        ConfirmMessage = message;
        ConfirmIcon = icon;
        ConfirmOkLabel = okLabel;
        ConfirmCancelLabel = cancelLabel;
        ConfirmVisible = true;
        _confirmTcs = new TaskCompletionSource<bool>();
        NotifyChange();
        return _confirmTcs.Task;
    }

    public void CloseConfirm(bool result)
    {
        ConfirmVisible = false;
        _confirmTcs?.TrySetResult(result);
        NotifyChange();
    }

    public Task<AppSettings?> ShowSettingsAsync(AppSettings current)
    {
        TempSettings = new AppSettings
        {
            ServerLibraryPath = current.ServerLibraryPath,
            Language = current.Language,
            AutoCheckUpdates = current.AutoCheckUpdates,
            CloseToTray = current.CloseToTray,
            RunSteamCmdMinimized = current.RunSteamCmdMinimized
        };
        SettingsVisible = true;
        _settingsTcs = new TaskCompletionSource<AppSettings?>();
        NotifyChange();
        return _settingsTcs.Task;
    }

    public void CloseSettings(bool save)
    {
        SettingsVisible = false;
        _settingsTcs?.TrySetResult(save ? TempSettings : null);
        NotifyChange();
    }

    public void ShowAbout()
    {
        AboutVisible = true;
        NotifyChange();
    }

    public void CloseAbout()
    {
        AboutVisible = false;
        NotifyChange();
    }

    public void ShowGuide(string gameTitle, string guideContent)
    {
        GuideTitle = gameTitle;
        GuideText = guideContent;
        GuideVisible = true;
        NotifyChange();
    }

    public void CloseGuide()
    {
        GuideVisible = false;
        NotifyChange();
    }

    public void ShowSteamCmdOptions()
    {
        SteamCmdOptionsVisible = true;
        NotifyChange();
    }

    public void CloseSteamCmdOptions()
    {
        SteamCmdOptionsVisible = false;
        NotifyChange();
    }

    public void ShowEditSettings(string gameTitle, List<string> files)
    {
        EditSettingsGame = gameTitle;
        EditSettingsFiles = files;
        EditSettingsVisible = true;
        NotifyChange();
    }

    public void CloseEditSettings()
    {
        EditSettingsVisible = false;
        NotifyChange();
    }

    public void ShowFaq()
    {
        FaqVisible = true;
        NotifyChange();
    }

    public void CloseFaq()
    {
        FaqVisible = false;
        NotifyChange();
    }

    private void NotifyChange() => OnChange?.Invoke();
}
