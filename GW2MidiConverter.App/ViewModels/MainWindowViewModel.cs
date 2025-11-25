using System;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GW2MidiConverter.App.Resources;
using GW2MidiConverter.Tab;
using Microsoft.Extensions.Logging;

namespace GW2MidiConverter.App.ViewModels;

internal class MainWindowViewModel : ObservableObject
{
    private object? _currentViewModel;

    private readonly FileLoaderViewModel _fileLoaderViewModel;
    private readonly TabRendererViewModel _tabRendererViewModel;
    private readonly ILogger<MainWindowViewModel> _logger;

    public string ApplicationTitle => $"{ApplicationResources.ApplicationName} ({Assembly.GetExecutingAssembly().GetName().Version?.ToString(3)})";

    public object? CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public ICommand LoadedCommand { get; }

    public MainWindowViewModel(FileLoaderViewModel fileLoaderViewModel,
                               TabRendererViewModel tabRendererViewModel,
                               ILogger<MainWindowViewModel> logger)
    {
        _logger = logger;
        _fileLoaderViewModel = fileLoaderViewModel;
        _fileLoaderViewModel.TabDocumentLoaded += HandleTabDocumentLoaded;

        _tabRendererViewModel = tabRendererViewModel;
        _tabRendererViewModel.ReturnRequested += HandleReturnRequested;

        CurrentViewModel = _fileLoaderViewModel;

        LoadedCommand = new RelayCommand(OnLoaded);
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

    private void OnLoaded()
    {
        _fileLoaderViewModel.PrepareInstruments();
    }
}