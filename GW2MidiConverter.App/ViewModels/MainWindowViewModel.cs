using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GW2MidiConverter.App.ViewModels.Settings;
using GW2MidiConverter.App.ViewModels.Settings.NoteRenderingSettings;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Octaves;
using Microsoft.Win32;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace GW2MidiConverter.App.ViewModels;

internal class MainWindowViewModel : ObservableObject
{
    private readonly ISnackbarService _snackbarService;
    private TabDocument? _tabDocument;

    private SettingsViewModel? _selectedSettingsViewModel;
    private readonly NoteRendererSettingsViewModel _noteRenderingSettingsViewModel;
    private string _tabText = "";

    public object ApplicationTitle => "GW2 Tab Converter";

    public List<SettingsViewModel> Settings { get; } = [];

    public SettingsViewModel? SelectedSettingsViewModel
    {
        get => _selectedSettingsViewModel;
        set => SetProperty(ref _selectedSettingsViewModel, value);
    }

    public TabDocument? TabDocument
    {
        get => _tabDocument;
        set
        {
            _tabDocument = value;
            RebuildTab();
        }
    }

    public string TabText
    {
        get => _tabText;
        private set => SetProperty(ref _tabText, value);
    }

    public ICommand ExportCommand { get; }

    public MainWindowViewModel(ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
        ExportCommand = new RelayCommand(ExportTab);
        
        _noteRenderingSettingsViewModel = new NoteRendererSettingsViewModel(new NoteRenderingSettings());
        RegisterSettingsViewModel(_noteRenderingSettingsViewModel);
        
        SelectedSettingsViewModel = Settings[0];
        
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile("H:\\Private\\Coding\\GW2 Midi Converter\\GW2MidiConverter.Tests\\Resources\\Coraline - Exploration (GW2) Merged parts.mid");
        TabDocument tabDocument = TabDocument.FromMidiDocument(midiDocument, new LookAheadOctaveCalculator());
        tabDocument.EnableTrack(0, Instrument.Piano);
        TabDocument = tabDocument;
    }

    private void RebuildTab()
    {
        if (_tabDocument == null!) return;
        
        try
        {
            TabRenderingSettings settings = BuildRenderingSettings();
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

    private TabRenderingSettings BuildRenderingSettings()
    {
        return new TabRenderingSettings
        {
            NoteRenderingSettings = _noteRenderingSettingsViewModel.CreateSettings()
        };
    }

    private void RegisterSettingsViewModel(SettingsViewModel settings)
    {
        Settings.Add(settings);
        settings.SubSettingChanged += (_, _) => RebuildTab();
    }
    
    private void ExportTab()
    {
        if (TabDocument == null)
        {
            _snackbarService.Show("No Tab", "Please load a MIDI file first.", ControlAppearance.Danger, null, TimeSpan.FromSeconds(4));
            return;
        }
        
        if (!CheckSettingValidity())
        {
            _snackbarService.Show("Invalid Settings", "Please check your settings and try again.", ControlAppearance.Danger, null, TimeSpan.FromSeconds(4));
            return;
        }
        
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "Text File (*.txt)|*.txt",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        string fileName = PrepareFileName();
        saveFileDialog.FileName = fileName;


        try
        {
            if (saveFileDialog.ShowDialog() != true) return;
            File.WriteAllText(saveFileDialog.FileName, TabText);
        }
        catch (Exception e)
        {
            _snackbarService.Show("Export Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(10));
            throw;
        }
    }

    private bool CheckSettingValidity()
    {
        return true;
    }

    private string PrepareFileName()
    {
        return "Tab.txt";
    }
}