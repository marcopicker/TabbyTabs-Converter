using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Octaves;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace GW2MidiConverter.App.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private readonly ISnackbarService _snackbarService;
    private readonly TabDocument _tabDocument;

    private object _currentViewModel;
    private string _tabText;
    private int _measureSpaces;
    private bool _showMeasureNumbers;

    public object ApplicationTitle => "GW2 Tab Converter";

    public object CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    public string TabText
    {
        get => _tabText;
        set => SetProperty(ref _tabText, value);
    }

    public int MeasureSpaces
    {
        get => _measureSpaces;
        set
        {
            if (!SetProperty(ref _measureSpaces, value)) return;
            RebuildTab();
        }
    }

    public bool ShowMeasureNumbers
    {
        get => _showMeasureNumbers;
        set
        {
            if (!SetProperty(ref _showMeasureNumbers, value)) return;
            RebuildTab();
        }
    }

    public MainWindowViewModel(IServiceProvider serviceProvider,
                               ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
        MeasureSpaces = 4;

        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile("H:\\Private\\Coding\\GW2 Midi Converter\\GW2MidiConverter.Tests\\Resources\\Coraline - Exploration (GW2) Merged parts.mid");
        _tabDocument = TabDocument.FromMidiDocument(midiDocument, new LookAheadOctaveCalculator());
        // _tabRenderer = new TabRenderer(_tabDocument);
        _tabDocument.EnableTrack(0, Instrument.Piano);

        RebuildTab();
    }

    private void RebuildTab()
    {
        if (_tabDocument == null!) return;
        TabRenderingSettings settings = BuildSettings();

        try
        {
            TabRenderer renderer = new TabRenderer(_tabDocument, settings);
            TabText = renderer.Render();
        }
        catch (Exception e)
        {
            bool hasSnackbarPresenter = _snackbarService.GetSnackbarPresenter() != null;
            if(hasSnackbarPresenter) _snackbarService.Show("Rendering Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(30));
            TabText = string.Empty;
            Console.WriteLine(e);
        }
    }

    private TabRenderingSettings BuildSettings()
    {
        MeasureRenderingSettings measureRenderingSettings = new MeasureRenderingSettings()
        {
            SpacesBetweenBeats = MeasureSpaces,
            ShowMeasureNumbers = ShowMeasureNumbers
        };
        
        return new TabRenderingSettings()
        {
            MeasureRenderingSettings = measureRenderingSettings
        };
    }
}