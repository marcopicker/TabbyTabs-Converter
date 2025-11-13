using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GW2MidiConverter.App.Controls;
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
    private readonly IEditDistanceCalculator _editDistanceCalculator;
    private FullyObservableCollection<TrackConfigItemViewModel> _trackConfigItems = [];

    private MidiDocument? _loadedMidiDocument;
    private TabDocument? _loadedTabDocument;

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

    public FileLoaderViewModel(ISnackbarService snackbar,
                               IInstrumentProvider instrumentProvider,
                               IEditDistanceCalculator editDistanceCalculator)
    {
        _snackbar = snackbar;
        _editDistanceCalculator = editDistanceCalculator;

        OnDragEnterCommand = new RelayCommand<DragEventArgs>(HandleDragEnter);
        OnDropFilesCommand = new RelayCommand<DragEventArgs>(HandleDropFiles);
        BrowseFilesCommand = new RelayCommand(BrowseFiles);
        ConvertTabCommand = new RelayCommand(ConvertTab, CanConvertTab);

        PossibleInstruments = instrumentProvider.GetInstruments().ToList();
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
            AddToRecent = true,
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

        _loadedTabDocument = TabDocument.FromMidiDocument(_loadedMidiDocument, new LookAheadOctaveCalculator());

        List<TrackConfigItemViewModel> items = [];
        foreach (int index in _loadedMidiDocument.InstrumentTrackIndexes)
        {
            TrackInfo trackInfo = _loadedMidiDocument.GetTrackInfo(index);

            TrackConfigItemViewModel item = new TrackConfigItemViewModel(trackInfo);
            item.SelectedInstrument = FindFittingInstrumentForTrack(trackInfo);
            item.IsChecked = item.SelectedInstrument != null;

            items.Add(item);
        }

        TrackConfigItems = new FullyObservableCollection<TrackConfigItemViewModel>(items);
        ConvertTabCommand.NotifyCanExecuteChanged();
    }

    private void ConvertTab()
    {
        if (_loadedTabDocument == null) return;
        try
        {
            List<TabDocumentTrackConfig> config = TrackConfigItems.Select(i => new TabDocumentTrackConfig(i.TrackInfo.TrackIndex, i.SelectedInstrument))
                                                                  .ToList();
            _loadedTabDocument.DisableAllTracks();
            _loadedTabDocument.EnableTracks(config);

            OnTabDocumentLoaded(_loadedTabDocument);
        }
        catch (Exception e)
        {
            _snackbar.Show("Conversion Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(10));
        }
    }

    private bool CanConvertTab()
    {
        if (_loadedMidiDocument == null) return false;
        if (_loadedTabDocument == null) return false;
        if (TrackConfigItems.Count == 0) return false;
        if (TrackConfigItems.All(t => !t.IsChecked)) return false;
        return TrackConfigItems.All(t => t.Validity != ValidityType.Invalid );
    }

    private void TrackConfigItemsOnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not TrackConfigItemViewModel item) return;
        if (e.PropertyName is nameof(TrackConfigItemViewModel.Validity) or nameof(TrackConfigItemViewModel.InvalidMessage)) return;

        VerifyTrackConfigItem(item);
        ConvertTabCommand.NotifyCanExecuteChanged();
    }

    private void OnTabDocumentLoaded(TabDocument e)
    {
        TabDocumentLoaded?.Invoke(this, e);
    }

    private Instrument? FindFittingInstrumentForTrack(TrackInfo track)
    {
        IReadOnlyList<Instrument> fittingInstruments = PossibleInstruments.Where(i => i.Name.Contains(track.InstrumentName, StringComparison.InvariantCultureIgnoreCase))
                                                                          .ToList();
        if (fittingInstruments.Count == 0) fittingInstruments = PossibleInstruments;
        return fittingInstruments.MinBy(i => _editDistanceCalculator.CalculateDistance(i.Name, track.InstrumentName));
    }

    private void VerifyTrackConfigItem(TrackConfigItemViewModel item)
    {
        if (_loadedTabDocument == null) return;

        if (item is { IsChecked: true, SelectedInstrument: null })
        {
            item.Validity = ValidityType.Invalid;
            item.InvalidMessage = "No instrument selected!";
            return;
        }

        bool canPlayTrack = _loadedTabDocument.CanInstrumentPlayTrack(item.TrackInfo.TrackIndex, item.SelectedInstrument!);
        if (canPlayTrack)
        {
            item.Validity = ValidityType.Valid;
            item.InvalidMessage = string.Empty;
        }
        else
        {
            item.Validity = ValidityType.Warning;
            item.InvalidMessage = "At least one note of this track is outside of the selected instrument's range!";
        }
    }
}