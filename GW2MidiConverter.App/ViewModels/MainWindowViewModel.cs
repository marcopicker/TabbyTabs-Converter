using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GW2MidiConverter.App.Models;
using GW2MidiConverter.App.Resources;
using GW2MidiConverter.App.Services;
using GW2MidiConverter.App.Views;
using GW2MidiConverter.Tab;
using Wpf.Ui;
using Wpf.Ui.Extensions;

namespace GW2MidiConverter.App.ViewModels;

internal class MainWindowViewModel : ObservableObject
{
    private readonly IContentDialogService _contentDialogService;
    private readonly IApplicationVersionChecker _applicationVersionChecker;
    private object? _currentViewModel;

    private readonly FileLoaderViewModel _fileLoaderViewModel;
    private readonly TabRendererViewModel _tabRendererViewModel;

    public string ApplicationTitle => $"{ApplicationResources.ApplicationName} ({Assembly.GetExecutingAssembly().GetName().Version?.ToString(3)})";

    public object? CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public ICommand LoadedCommand { get; }

    public MainWindowViewModel(ISnackbarService snackbar,
                               IContentDialogService contentDialogService,
                               IApplicationVersionChecker applicationVersionChecker,
                               FileLoaderViewModel fileLoaderViewModel,
                               TabRendererViewModel tabRendererViewModel)
    {
        _contentDialogService = contentDialogService;
        _applicationVersionChecker = applicationVersionChecker;
        _fileLoaderViewModel = fileLoaderViewModel;
        _fileLoaderViewModel.TabDocumentLoaded += HandleTabDocumentLoaded;

        _tabRendererViewModel = tabRendererViewModel;
        _tabRendererViewModel.ReturnRequested += HandleReturnRequested;

        CurrentViewModel = _fileLoaderViewModel;

        LoadedCommand = new AsyncRelayCommand(CheckForUpdates);
    }

    private void HandleTabDocumentLoaded(object? sender, TabDocument document)
    {
        CurrentViewModel = _tabRendererViewModel;
        _tabRendererViewModel.TabDocument = document;
    }

    private void HandleReturnRequested(object? sender, EventArgs e)
    {
        CurrentViewModel = _fileLoaderViewModel;
    }

    private async Task CheckForUpdates()
    {
        return;
        VersionCheckInfo versionInfo = await _applicationVersionChecker.IsNewVersionAvailableAsync();
        if (!versionInfo.IsNewVersionAvailable) return;
        _fileLoaderViewModel.UpdateInfo = versionInfo;
    }
}