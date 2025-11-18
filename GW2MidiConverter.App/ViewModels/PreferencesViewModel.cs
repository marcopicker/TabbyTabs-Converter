using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        LicenseAttributions = GetLicenseAttributions().OrderBy(l => l.LicenseName).ToList();
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
            new LicenseAttribution("Newtonsoft Json.NET", "13.0.4", "https://www.newtonsoft.com/json", "MIT License", "https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md")
        ];
    }
}