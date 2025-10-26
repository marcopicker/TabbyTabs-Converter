using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Octaves;

namespace GW2MidiConverter.App.ViewModels;

public class MainWindowViewModel : ObservableObject
{
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
            if(!SetProperty(ref _measureSpaces, value)) return;
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

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile("H:\\Private\\Coding\\GW2 Midi Converter\\GW2MidiConverter.Tests\\Resources\\The Nightmare Before Christmas - This is Halloween (GW2) Notedrop Ver.mid");
        _tabDocument = TabDocument.FromMidiDocument(midiDocument, new LookAheadOctaveCalculator());
        // _tabRenderer = new TabRenderer(_tabDocument);
        _tabDocument.EnableTrack(1, Instrument.Piano);
        
        RebuildTab();
    }

    private void RebuildTab()
    {
        TabRendererSettings settings = new TabRendererSettings()
        {
            SpacesBetweenBeats = MeasureSpaces,
            ShowMeasureNumbers = ShowMeasureNumbers
        };
        
        TabRenderer renderer = new TabRenderer(_tabDocument, settings);
        TabText = renderer.Render();
    }
}