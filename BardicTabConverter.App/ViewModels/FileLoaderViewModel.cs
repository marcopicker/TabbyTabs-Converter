using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BardicTabConverter.App.Controls;
using BardicTabConverter.App.Models;
using BardicTabConverter.App.Services;
using BardicTabConverter.General;
using BardicTabConverter.GuildWars.Instruments;
using BardicTabConverter.Midi;
using BardicTabConverter.Tab;
using BardicTabConverter.Tab.Octaves;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace BardicTabConverter.App.ViewModels;

internal class FileLoaderViewModel : ObservableObject
{
    private readonly ISnackbarService _snackbar;
    private readonly IInstrumentProvider _instrumentProvider;
    private readonly IEditDistanceCalculator _editDistanceCalculator;
    private readonly IContentDialogService _contentDialogService;
    private readonly ILogger<FileLoaderViewModel> _logger;
    private FullyObservableCollection<TrackConfigItemViewModel> _trackConfigItems = [];

    private MidiDocument? _loadedMidiDocument;
    private TabDocument? _loadedTabDocument;
    private FileDraggingInfoState _currentDragState;
    private bool _fileLoadingErrorProxy;
    private VersionCheckInfo? _updateInfo;
    private IReadOnlyList<Instrument> _possibleInstruments = [];

    public ICommand OnDragEnterCommand { get; }
    public ICommand OnDragLeaveCommand { get; }

    public ICommand OnDropFilesCommand { get; }

    public ICommand BrowseFilesCommand { get; }

    /// <summary>
    /// This is a dummy object used to trigger the shake effect when an invalid file is loaded.
    /// </summary>
    public bool FileLoadingErrorProxy
    {
        get => _fileLoadingErrorProxy;
        private set => SetProperty(ref _fileLoadingErrorProxy, value);
    }

    public FileDraggingInfoState CurrentDragState
    {
        get => _currentDragState;
        private set => SetProperty(ref _currentDragState, value);
    }

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

    public IReadOnlyList<Instrument> PossibleInstruments
    {
        get => _possibleInstruments;
        private set => SetProperty(ref _possibleInstruments, value);
    }

    public RelayCommand ConvertTabCommand { get; }

    public VersionCheckInfo? UpdateInfo
    {
        get => _updateInfo;
        set => SetProperty(ref _updateInfo, value);
    }

    public event EventHandler<TabDocument>? TabDocumentLoaded;

    public FileLoaderViewModel(ISnackbarService snackbar,
                               IInstrumentProvider instrumentProvider,
                               IEditDistanceCalculator editDistanceCalculator,
                               IContentDialogService contentDialogService,
                               ILogger<FileLoaderViewModel> logger)
    {
        _snackbar = snackbar;
        _instrumentProvider = instrumentProvider;
        _editDistanceCalculator = editDistanceCalculator;
        _contentDialogService = contentDialogService;
        _logger = logger;

        OnDragEnterCommand = new RelayCommand<DragEventArgs>(HandleDragEnter);
        OnDragLeaveCommand = new RelayCommand<DragEventArgs>(HandleDragLeave);
        OnDropFilesCommand = new RelayCommand<DragEventArgs>(HandleDropFiles);
        BrowseFilesCommand = new RelayCommand(BrowseFiles);
        ConvertTabCommand = new RelayCommand(ConvertTab, CanConvertTab);
        FileLoadingErrorProxy = false;
    }

    public void PrepareInstruments()
    {
        IReadOnlyCollection<InstrumentProviderResult> results = _instrumentProvider.GetInstruments();
        PossibleInstruments = results.Where(i => i is { Successful: true, Instrument: not null })
                                     .Select(i => i.Instrument!)
                                     .ToList();

        List<InstrumentProviderResult> invalidInstruments = results.Where(i => !i.Successful).ToList();
        if (invalidInstruments.Count == 0) return;

        _snackbar.Show("Instrument Warning",
                       $"{invalidInstruments.Capacity} instrument(s) could not be loaded, as their files are corrupted or in the wrong format. See the log for details.",
                       ControlAppearance.Caution,
                       null,
                       TimeSpan.FromSeconds(10));
    }

    private void HandleDragEnter(DragEventArgs? e)
    {
        if (e?.Data == null) return;
        bool isFileDrop = e.Data.GetDataPresent(DataFormats.FileDrop);
        if (!isFileDrop) return;

        if (e.Data.GetData(DataFormats.FileDrop) is not string[] files) return;
        CurrentDragState = files.Length == 1 ? FileDraggingInfoState.Valid : FileDraggingInfoState.Invalid;
    }

    private void HandleDragLeave(DragEventArgs? e)
    {
        CurrentDragState = FileDraggingInfoState.None;
    }

    private void HandleDropFiles(DragEventArgs? e)
    {
        CurrentDragState = FileDraggingInfoState.None;
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
        using IDisposable? _ = _logger.BeginScope(new
        {
            Operation = "Loading MIDI files",
            Files = files
        });

        switch (files.Length)
        {
            case 0:
                _logger.LogWarning("No files selected.");
                _snackbar.Show("No Files", "Please select a MIDI file first.", ControlAppearance.Danger, null, _snackbar.DefaultTimeOut);
                TriggerShakeAnimation();
                return;
            case > 1:
                _logger.LogWarning("Multiple files selected.");
                _snackbar.Show("Multiple Files", "Please select only one MIDI file.", ControlAppearance.Danger, null, _snackbar.DefaultTimeOut);
                TriggerShakeAnimation();
                return;
        }

        string fileName = files[0];
        _logger.LogInformation("Loading MIDI file '{FileName}'...", fileName);

        MidiDocument midiDocument;
        try
        {
            midiDocument = MidiDocument.LoadFromMidiFile(fileName);
        }
        catch (InvalidMidiDocumentException e)
        {
            _logger.LogError(e, "Failed to load MIDI file '{FileName}'.", fileName);
            _snackbar.Show("Invalid File", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(10));
            TriggerShakeAnimation();
            return;
        }
        catch (MidiDocumentException e)
        {
            _logger.LogError(e, "Failed to load MIDI file '{FileName}'.", fileName);
            _snackbar.Show("Invalid MIDI File", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(10));
            TriggerShakeAnimation();
            return;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to load MIDI file '{FileName}'.", fileName);
            _snackbar.Show("File Loading Error", "An unexpected error occurred while trying to load the provided file.", ControlAppearance.Danger, null, TimeSpan.FromSeconds(10));
            TriggerShakeAnimation();
            return;
        }

        if (midiDocument.InstrumentTrackIndexes.Count == 0)
        {
            _logger.LogWarning("MIDI file '{FileName}' does not contain any tracks.", fileName);
            _snackbar.Show("Invalid MIDI File", "The selected MIDI file does not contain any instrument tracks.", ControlAppearance.Danger, null, TimeSpan.FromSeconds(3));
            TriggerShakeAnimation();
            return;
        }

        _loadedMidiDocument = midiDocument;
        _logger.LogInformation("Successfully loaded MIDI file '{FileName}'.", fileName);
        PrepareMidiDocument();
    }

    private void PrepareMidiDocument()
    {
        if (_loadedMidiDocument == null) return;
        using IDisposable? _ = _logger.BeginScope(new { Operation = "Preparing MIDI document" });

        _logger.LogInformation("Preparing tab document...");
        _loadedTabDocument = TabDocument.FromMidiDocument(_loadedMidiDocument, new LookAheadOctaveCalculator());

        List<TrackConfigItemViewModel> items = [];
        foreach (int index in _loadedMidiDocument.InstrumentTrackIndexes)
        {
            TrackInfo trackInfo = _loadedMidiDocument.GetTrackInfo(index);
            _logger.LogDebug("Preparing track {TrackIndex}...", index);

            TrackConfigItemViewModel item = new TrackConfigItemViewModel(trackInfo);
            item.SelectedInstrument = FindFittingInstrumentForTrack(trackInfo);
            _logger.LogDebug("Assigned the track the instrument '{Instrument}'.", item.SelectedInstrument?.Name);

            item.IsChecked = item.SelectedInstrument != null;
            items.Add(item);
            
            VerifyTrackConfigItem(item);
            _logger.LogDebug("Prepared track {TrackIndex}.", index);
        }

        TrackConfigItems = new FullyObservableCollection<TrackConfigItemViewModel>(items);
        ConvertTabCommand.NotifyCanExecuteChanged();
        _logger.LogInformation("Successfully prepared tab document.");
    }

    private void ConvertTab()
    {
        if (_loadedTabDocument == null) return;
        try
        {
            List<TabDocumentTrackConfig> config = TrackConfigItems.Where(i => i.IsChecked)
                                                                  .Select(i => new TabDocumentTrackConfig(i.TrackInfo.TrackIndex, i.SelectedInstrument))
                                                                  .ToList();
            _loadedTabDocument.DisableAllTracks();
            _loadedTabDocument.EnableTracks(config);

            OnTabDocumentLoaded(_loadedTabDocument);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to convert MIDI file to tab document.");
            _snackbar.Show("Unexpected Conversion Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(10));
        }
    }

    private bool CanConvertTab()
    {
        if (_loadedMidiDocument == null) return false;
        if (_loadedTabDocument == null) return false;
        if (TrackConfigItems.Count == 0) return false;
        if (TrackConfigItems.All(t => !t.IsChecked)) return false;
        return TrackConfigItems.All(t => t.Validity != ValidityType.Invalid);
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
        _logger.LogInformation("Tab document loaded.");
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

    private void TriggerShakeAnimation()
    {
        FileLoadingErrorProxy = false;
        FileLoadingErrorProxy = true;

        // Set it back to false at the end to prevent the shake animation from replaying when the view is reloaded.
        FileLoadingErrorProxy = false;
    }
}