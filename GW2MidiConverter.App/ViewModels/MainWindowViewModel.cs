using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyPrefs.Abstractions;
using GW2MidiConverter.App.Resources;
using GW2MidiConverter.App.Services;
using GW2MidiConverter.Tab;
using Microsoft.Extensions.Logging;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace GW2MidiConverter.App.ViewModels;

internal class MainWindowViewModel : ObservableObject
{
    private object? _currentViewModel;

    private readonly FileLoaderViewModel _fileLoaderViewModel;
    private readonly TabRendererViewModel _tabRendererViewModel;
    private readonly IApplicationVersionChecker _versionChecker;
    private readonly IPreferencesReader _preferencesReader;
    private readonly ISnackbarService _snackbarService;
    private readonly ILogger<MainWindowViewModel> _logger;
    private bool _isUpdateAvailable;
    private VersionCheckInfo? _updateInfo;

    public string ApplicationTitle => $"{ApplicationResources.ApplicationName} ({VersioningHelper.CurrentVersionText})";

    public object? CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public bool IsUpdateAvailable
    {
        get => _isUpdateAvailable;
        private set => SetProperty(ref _isUpdateAvailable, value);
    }

    public ICommand LoadedCommand { get; }

    public ICommand GetUpdateCommand { get; }

    public MainWindowViewModel(FileLoaderViewModel fileLoaderViewModel,
                               TabRendererViewModel tabRendererViewModel,
                               IApplicationVersionChecker versionChecker,
                               IPreferencesReader preferencesReader,
                               ISnackbarService snackbarService,
                               ILogger<MainWindowViewModel> logger)
    {
        _versionChecker = versionChecker;
        _preferencesReader = preferencesReader;
        _snackbarService = snackbarService;
        _logger = logger;
        _fileLoaderViewModel = fileLoaderViewModel;
        _tabRendererViewModel = tabRendererViewModel;

        _fileLoaderViewModel.TabDocumentLoaded += HandleTabDocumentLoaded;
        _tabRendererViewModel.ReturnRequested += HandleReturnRequested;
        
        CurrentViewModel = _fileLoaderViewModel;

        LoadedCommand = new AsyncRelayCommand(OnLoadedAsync);
        GetUpdateCommand = new RelayCommand(GetUpdate);
    }

    private async void HandleTabDocumentLoaded(object? sender, TabDocument document)
    {
        try
        {
            CurrentViewModel = _tabRendererViewModel;
            _tabRendererViewModel.TabDocument = document;

            await _tabRendererViewModel.RenderTabAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to render tab.");
        }
    }

    private void HandleReturnRequested(object? sender, EventArgs e)
    {
        CurrentViewModel = _fileLoaderViewModel;
    }

    private async Task OnLoadedAsync()
    {
        _fileLoaderViewModel.PrepareInstruments();
        await AutoCheckForNewVersionAsync();
    }

    private async Task AutoCheckForNewVersionAsync()
    {
        bool autoCheckForUpdates = _preferencesReader.GetPreference<bool>(PreferenceKeys.AUTO_UPDATE);
        if (!autoCheckForUpdates) return;

        VersionCheckInfo versionInfo = await _versionChecker.IsNewVersionAvailableAsync();
        if (!versionInfo.IsNewVersionAvailable) return;

        _updateInfo = versionInfo;
        IsUpdateAvailable = true;
    }

    private void GetUpdate()
    {
        if (_updateInfo == null) return;

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
}