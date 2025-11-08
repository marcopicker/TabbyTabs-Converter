using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GW2MidiConverter.App.Models;
using GW2MidiConverter.General;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Octaves;
using Microsoft.Win32;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace GW2MidiConverter.App.ViewModels;

internal class FileLoaderViewModel : ObservableObject
{
    private readonly ISnackbarService _snackbar;
    private FullyObservableCollection<TrackConfigItemViewModel> _trackConfigItems = [];

    private MidiDocument? _loadedMidiDocument;

    public ICommand OnDragEnterCommand { get; }

    public ICommand OnDropFilesCommand { get; }

    public ICommand BrowseFilesCommand { get; }

    public FullyObservableCollection<TrackConfigItemViewModel> TrackConfigItems
    {
        get => _trackConfigItems;
        private set
        {
            if (_trackConfigItems == value) return;
            _trackConfigItems.ItemPropertyChanged -= TrackConfigItemsOnItemPropertyChanged;
            _trackConfigItems = value;
            _trackConfigItems.ItemPropertyChanged += TrackConfigItemsOnItemPropertyChanged;
            OnPropertyChanged();
        }
    }

    public IReadOnlyList<Instrument> PossibleInstruments { get; }

    public RelayCommand ConvertTabCommand { get; }

    public event EventHandler<TabDocument>? TabDocumentLoaded;

    public FileLoaderViewModel(ISnackbarService snackbar)
    {
        _snackbar = snackbar;
        OnDragEnterCommand = new RelayCommand<DragEventArgs>(HandleDragEnter);
        OnDropFilesCommand = new RelayCommand<DragEventArgs>(HandleDropFiles);
        BrowseFilesCommand = new RelayCommand(BrowseFiles);
        ConvertTabCommand = new RelayCommand(ConvertTab, CanConvertTab);

        PossibleInstruments = new List<Instrument>()
        {
            // Instrument.Piano,
            // Instrument.Minstrel,
            // Instrument.Harp,
            // Instrument.Lute,
            // Instrument.Bass
        };
    }

    [SuppressMessage("ReSharper", "RedundantJumpStatement")]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    private void HandleDragEnter(DragEventArgs? e)
    {
        if (e?.Data == null) return;
        bool isFileDrop = e.Data.GetDataPresent(DataFormats.FileDrop);
        if (!isFileDrop) return;

        if (e.Data.GetData(DataFormats.FileDrop) is not string[] files) return;

        // TODO MP: Check if file is valid. :3
    }

    private void HandleDropFiles(DragEventArgs? e)
    {
        if (e?.Data == null) return;
        bool isFileDrop = e.Data.GetDataPresent(DataFormats.FileDrop);
        if (!isFileDrop) return;

        if (e.Data.GetData(DataFormats.FileDrop) is not string[] files) return;
        TryLoadFiles(files);
    }

    private void BrowseFiles()
    {
        OpenFileDialog dialog = new OpenFileDialog()
        {
            Filter = "MIDI Files (*.mid, *.midi)|*.mid;*.midi",
            Multiselect = false,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        string[] fileNames;
        try
        {
            bool? result = dialog.ShowDialog();
            if (result != true) return;
            fileNames = dialog.FileNames;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        TryLoadFiles(fileNames);
    }

    private void TryLoadFiles(string[] files)
    {
        switch (files.Length)
        {
            case 0:
                _snackbar.Show("No Files", "Please select a MIDI file first.", ControlAppearance.Danger, null, _snackbar.DefaultTimeOut);
                return;
            case > 1:
                _snackbar.Show("Multiple Files", "Please select only one MIDI file.", ControlAppearance.Danger, null, _snackbar.DefaultTimeOut);
                return;
        }

        string fileName = files[0];

        MidiDocument midiDocument;
        try
        {
            midiDocument = MidiDocument.LoadFromMidiFile(fileName);
        }
        catch (Exception e)
        {
            _snackbar.Show("File Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(10));
            return;
        }

        if (midiDocument.InstrumentTrackIndexes.Count == 0)
        {
            _snackbar.Show("No Tracks", "The selected MIDI file does not contain any tracks.", ControlAppearance.Danger, null, TimeSpan.FromSeconds(3));
            return;
        }

        _loadedMidiDocument = midiDocument;
        PrepareMidiDocument();
    }

    private void PrepareMidiDocument()
    {
        if (_loadedMidiDocument == null) return;

        List<TrackConfigItemViewModel> items = [];
        foreach (int index in _loadedMidiDocument.InstrumentTrackIndexes)
        {
            TrackInfo trackInfo = _loadedMidiDocument.GetTrackInfo(index);
            TrackConfigItemViewModel item = new TrackConfigItemViewModel(trackInfo);
            items.Add(item);
        }

        TrackConfigItems = new FullyObservableCollection<TrackConfigItemViewModel>(items);
    }

    private void ConvertTab()
    {
        if (_loadedMidiDocument == null) return;
        try
        {
            TabDocument tabDocument = TabDocument.FromMidiDocument(_loadedMidiDocument, new LookAheadOctaveCalculator());
            List<TabDocumentTrackConfig> config = TrackConfigItems.Select(i => new TabDocumentTrackConfig(i.TrackInfo.TrackIndex, i.SelectedInstrument))
                                                                  .ToList();
            tabDocument.DisableAllTracks();
            tabDocument.EnableTracks(config);

            OnTabDocumentLoaded(tabDocument);
        }
        catch (Exception e)
        {
            _snackbar.Show("Conversion Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(10));
        }
    }

    private bool CanConvertTab()
    {
        if (_loadedMidiDocument == null) return false;
        if (TrackConfigItems.Count == 0) return false;
        if (TrackConfigItems.All(t => !t.IsChecked)) return false;
        if (TrackConfigItems.Where(t => t.IsChecked).Any(t => t.SelectedInstrument == null)) return false;

        return true;
    }

    private void TrackConfigItemsOnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ConvertTabCommand.NotifyCanExecuteChanged();
    }

    private void OnTabDocumentLoaded(TabDocument e)
    {
        TabDocumentLoaded?.Invoke(this, e);
    }
}