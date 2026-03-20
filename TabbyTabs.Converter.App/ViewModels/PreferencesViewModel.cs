using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TabbyTabs.Converter.App.Resources;
using TabbyTabs.Converter.App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyPrefs.Abstractions;
using Microsoft.Extensions.Logging;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace TabbyTabs.Converter.App.ViewModels;

internal class PreferencesViewModel : ObservableObject
{
    private readonly IApplicationVersionChecker _versionChecker;
    private readonly ISnackbarService _snackbarService;
    private readonly ILogger<PreferencesViewModel> _logger;
    private readonly IPreferences _preferences;
    private bool _autoCheckForUpdates;
    private VersionCheckState _versionCheckState = VersionCheckState.Unchecked;
    private VersionCheckInfo _updateInfo;
    private string _newVersion;

    public ICommand CheckForUpdateCommand { get; }

    public ICommand GetUpdateCommand { get; }

    public ICommand OpenLogsCommand { get; }

    public VersionCheckState VersionCheckState
    {
        get => _versionCheckState;
        set => SetProperty(ref _versionCheckState, value);
    }

    public bool AutoCheckForUpdates
    {
        get => _autoCheckForUpdates;
        set
        {
            if (!SetProperty(ref _autoCheckForUpdates, value)) return;
            _preferences.SetPreference("AutoUpdate", value);
        }
    }

    public IReadOnlyList<LicenseAttribution> LicenseAttributions { get; }

    public string NewVersion
    {
        get => _newVersion;
        private set => SetProperty(ref _newVersion, value);
    }

    public string CurrentVersion { get; }
    
    public string CopyrightNotice => $"Copyright © {DateTime.Now.Year}";

    public PreferencesViewModel(IApplicationVersionChecker versionChecker,
                                ISnackbarService snackbarService,
                                ILogger<PreferencesViewModel> logger,
                                IPreferences preferences)
    {
        _updateInfo = new VersionCheckInfo(false, new Version(), string.Empty, string.Empty);
        _versionChecker = versionChecker;
        _snackbarService = snackbarService;
        _logger = logger;
        _preferences = preferences;
        CheckForUpdateCommand = new AsyncRelayCommand(CheckForUpdates);
        GetUpdateCommand = new RelayCommand(GetUpdate);
        OpenLogsCommand = new RelayCommand(OpenLogDirectory);

        CurrentVersion = VersioningHelper.CurrentVersionText ?? string.Empty;
        _newVersion = string.Empty;

        LicenseAttributions = GetLicenseAttributions().OrderBy(l => l.SoftwareName).ToList();

        LoadPreferences();
    }

    private void LoadPreferences()
    {
        AutoCheckForUpdates = _preferences.GetPreference<bool>(PreferenceKeys.AUTO_UPDATE);
    }

    private async Task CheckForUpdates()
    {
        using IDisposable? _ = _logger.BeginScope(new { Operation = "CheckForUpdates" });
        _logger.LogInformation("Manual update check...");

        try
        {
            VersionCheckInfo result = await _versionChecker.IsNewVersionAvailableAsync();
            VersionCheckState = result.IsNewVersionAvailable ? VersionCheckState.NewVersionAvailable : VersionCheckState.NoVersionAvailable;

            _updateInfo = result;
            NewVersion = result.LatestVersion.ToString();
            _logger.LogInformation("Update check complete. New version available: {IsNewVersionAvailable}", result.IsNewVersionAvailable);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to check for updates.");
            VersionCheckState = VersionCheckState.NoVersionAvailable;
        }
    }

    private void GetUpdate()
    {
        if (!_updateInfo.IsNewVersionAvailable) return;

        try
        {
            string url = _updateInfo.DownloadUrl;
            Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to open update URL.");
            _snackbarService.Show("Unexpected Error", $"{ApplicationResources.ApplicationName} was unable to retrieve the update.", ControlAppearance.Danger, null, _snackbarService.DefaultTimeOut);
        }
    }

    private void OpenLogDirectory()
    {
        try
        {
            DirectoryInfo logFolder = new DirectoryInfo(Logging.LogFolder);
            if (!logFolder.Exists) logFolder.Create();

            Process.Start(new ProcessStartInfo(Logging.LogFolder)
            {
                UseShellExecute = true
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to open log directory: {Directory}", Logging.LogFolder);
            _snackbarService.Show("Unexpected Error", $"{ApplicationResources.ApplicationName} was unable to open the log directory.", ControlAppearance.Danger, null, _snackbarService.DefaultTimeOut);
        }
    }

    private static List<LicenseAttribution> GetLicenseAttributions()
    {
        return
        [
            new LicenseAttribution("Coverlet", "6.0.2", "https://github.com/coverlet-coverage/coverlet", "MIT License", "https://github.com/coverlet-coverage/coverlet/blob/master/LICENSE"),
            new LicenseAttribution("Microsoft .NET", "9.0", "https://dotnet.microsoft.com/en-us/", "MIT License", "https://github.com/dotnet/dotnet/blob/main/LICENSE.TXT"),
            new LicenseAttribution("VSTest", "17.12.0", "https://github.com/microsoft/vstest", "MIT License", "https://github.com/microsoft/vstest/blob/main/LICENSE"),
            new LicenseAttribution("NUnit", "4.2.2", "https://nunit.org/", "MIT License", "https://nunit.org/nuget/nunit3-license.txt"),
            new LicenseAttribution("NUnit Analyzers", "4.4.0", "https://github.com/nunit/nunit.analyzers", "MIT License", "https://github.com/nunit/nunit.analyzers/blob/master/license.txt"),
            new LicenseAttribution("NUnit 3 VS Test Adapter", "4.6.0", "https://github.com/nunit/nunit3-vs-adapter", "MIT License", "https://github.com/nunit/nunit3-vs-adapter/blob/main/LICENSE"),
            new LicenseAttribution("WPF UI", "3.1.1", "https://github.com/lepoco/wpfui", "MIT License", "https://github.com/lepoco/wpfui/blob/main/LICENSE"),
            new LicenseAttribution(".NET Community Toolkit", "8.4.0", "https://github.com/CommunityToolkit/dotnet", "MIT License", "https://github.com/CommunityToolkit/dotnet/blob/main/License.md"),
            new LicenseAttribution("XamlBehaviors for WPF", "1.1.135", "https://github.com/Microsoft/XamlBehaviorsWpf", "MIT License", "https://github.com/microsoft/XamlBehaviorsWpf/blob/main/LICENSE"),
            new LicenseAttribution("NAudio", "2.2.1", "https://github.com/naudio/NAudio", "MIT License", "https://github.com/naudio/NAudio/blob/master/license.txt"),
            new LicenseAttribution("Newtonsoft Json.NET", "13.0.4", "https://www.newtonsoft.com/json", "MIT License", "https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md"),
            new LicenseAttribution("Serilog", "4.3.0", "https://serilog.net/", "Apache 2.0 License", "https://github.com/serilog/serilog/blob/dev/LICENSE"),
            new LicenseAttribution("Serilog.Formatting.Compact", "3.0.0", "https://github.com/serilog/serilog-formatting-compact", "Apache 2.0 License", "https://github.com/serilog/serilog-formatting-compact/blob/dev/LICENSE"),
            new LicenseAttribution("Serilog.Sinks.Console", "6.1.1", "https://github.com/serilog/serilog-sinks-console", "Apache 2.0 License", "https://github.com/serilog/serilog-sinks-console/blob/dev/LICENSE"),
            new LicenseAttribution("Serilog.Sinks.File", "7.0.0", "https://github.com/serilog/serilog-sinks-file", "Apache 2.0 License", "https://github.com/serilog/serilog-sinks-file/blob/dev/LICENSE"),
            new LicenseAttribution("EasyPrefs", "1.3.4", "https://github.com/marcopicker/EasyPrefs", "MIT License", "https://github.com/marcopicker/EasyPrefs/blob/main/LICENSE"),
            new LicenseAttribution("EasyPrefs.JSON", "1.3.4", "https://github.com/marcopicker/EasyPrefs", "MIT License", "https://github.com/marcopicker/EasyPrefs/blob/main/LICENSE"),
        ];
    }
}