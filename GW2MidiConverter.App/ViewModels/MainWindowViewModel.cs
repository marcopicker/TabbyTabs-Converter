using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
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

    public MainWindowViewModel(ISnackbarService snackbar)
    {
        _fileLoaderViewModel = new FileLoaderViewModel(snackbar);
        _fileLoaderViewModel.TabDocumentLoaded += HandleTabDocumentLoaded;
        
        _tabRendererViewModel = new TabRendererViewModel(snackbar);
        
        CurrentViewModel = _fileLoaderViewModel;
    }

    private void HandleTabDocumentLoaded(object? sender, TabDocument document)
    {
        CurrentViewModel = _tabRendererViewModel;
        _tabRendererViewModel.TabDocument = document;
    }
}