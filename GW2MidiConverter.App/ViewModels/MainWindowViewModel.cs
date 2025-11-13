using System;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using GW2MidiConverter.App.Models;
using GW2MidiConverter.Tab;
using Wpf.Ui;

namespace GW2MidiConverter.App.ViewModels;

internal class MainWindowViewModel : ObservableObject
{
    private object? _currentViewModel;
    
    private readonly FileLoaderViewModel _fileLoaderViewModel;
    private readonly TabRendererViewModel _tabRendererViewModel;
    
    public string ApplicationTitle => $"GW2 Tab Converter ({Assembly.GetExecutingAssembly().GetName().Version?.ToString(3)})";

    public object? CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public MainWindowViewModel(ISnackbarService snackbar,
                               IInstrumentProvider instrumentProvider,
                               IEditDistanceCalculator editDistanceCalculator,
                               IContentDialogService contentDialogService)
    {
        _fileLoaderViewModel = new FileLoaderViewModel(snackbar, instrumentProvider, editDistanceCalculator);
        _fileLoaderViewModel.TabDocumentLoaded += HandleTabDocumentLoaded;
        
        _tabRendererViewModel = new TabRendererViewModel(snackbar, contentDialogService);
        _tabRendererViewModel.ReturnRequested += HandleReturnRequested;
        
        CurrentViewModel = _fileLoaderViewModel;
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
}