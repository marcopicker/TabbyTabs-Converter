using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GW2MidiConverter.App.Services;

namespace GW2MidiConverter.App.ViewModels;

internal class PreferencesViewModel : ObservableObject
{
    private readonly IApplicationVersionChecker _versionChecker;
    private bool _autoCheckForUpdates;
    private bool _isNewVersionAvailable;

    private VersionCheckInfo _updateInfo;
    private string _newVersion;

    public ICommand CheckForUpdateCommand { get; }

    public ICommand GetUpdateCommand { get; }

    public bool IsNewVersionAvailable
    {
        get => _isNewVersionAvailable;
        set => SetProperty(ref _isNewVersionAvailable, value);
    }
    

    public bool AutoCheckForUpdates
    {
        get => _autoCheckForUpdates;
        set => SetProperty(ref _autoCheckForUpdates, value);
    }

    public IReadOnlyList<LicenseAttribution> LicenseAttributions { get; }

    public string NewVersion
    {
        get => _newVersion;
        private set => SetProperty(ref _newVersion, value);
    }

    public string CurrentVersion { get; }

    public PreferencesViewModel(IApplicationVersionChecker versionChecker)
    {
        _updateInfo = new VersionCheckInfo(false, new Version(), string.Empty, string.Empty);
        _versionChecker = versionChecker;
        CheckForUpdateCommand = new AsyncRelayCommand(CheckForUpdates);
        GetUpdateCommand = new RelayCommand(GetUpdate);
        
        CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? string.Empty;
        _newVersion = string.Empty;

        LicenseAttributions = GetLicenseAttributions();
    }

    private async Task CheckForUpdates()
    {
        VersionCheckInfo result = await _versionChecker.IsNewVersionAvailableAsync();
        if (result.IsNewVersionAvailable) IsNewVersionAvailable = true;

        _updateInfo = result;
        NewVersion = result.LatestVersion.ToString();
    }

    private void GetUpdate()
    {
        if (!_updateInfo.IsNewVersionAvailable) return;

        string url = _updateInfo.DownloadUrl;
        Process.Start(new ProcessStartInfo(url)
        {
            UseShellExecute = true
        });
    }

    private static List<LicenseAttribution> GetLicenseAttributions()
    {
        return
        [
            new LicenseAttribution("FancyLibrary", "1.0.0", "https://google.com", "MIT License", "https://google.com"),
            new LicenseAttribution("OtherLibrary", "1.3.55", "https://google.com", "MIT License", "https://google.com")
        ];
    }
}