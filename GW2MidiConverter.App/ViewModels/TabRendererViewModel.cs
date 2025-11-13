using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GW2MidiConverter.App.ViewModels.Settings;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using Microsoft.Win32;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace GW2MidiConverter.App.ViewModels;

internal class TabRendererViewModel : ObservableObject
{
    private readonly ISnackbarService _snackbarService;
    private readonly IContentDialogService _contentDialogService;
    private TabDocument? _tabDocument;
    private string _tabText = "";
    private ISettingsViewModel? _selectedSettingsViewModel;

    private readonly NoteRendererSettingsViewModel _noteRenderingSettingsViewModel;
    private readonly MeasureRenderingSettingsViewModel _measureRenderingSettingsViewModel;
    private readonly TrackRenderingSettingsViewModel _trackRenderingSettingsViewModel;
    private readonly TimeSignatureRenderingSettingsViewModel _timeSignatureRenderingSettingsViewModel;

    public event EventHandler? ReturnRequested;

    public List<ISettingsViewModel> Settings { get; } = [];

    public ISettingsViewModel? SelectedSettingsViewModel
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

    public ICommand ReturnCommand { get; }
    public ICommand LoadStyleCommand { get; }
    public ICommand SaveStyleCommand { get; }
    public ICommand ResetStyleCommand { get; }


    public TabRendererViewModel(ISnackbarService snackbarService,
                                IContentDialogService contentDialogService)
    {
        _snackbarService = snackbarService;
        _contentDialogService = contentDialogService;

        ExportCommand = new RelayCommand(ExportTab);
        ReturnCommand = new RelayCommand(RequestReturn);
        LoadStyleCommand = new RelayCommand(ExecuteLoadStyleCommand);
        SaveStyleCommand = new RelayCommand(ExecuteSaveStyleCommand);
        ResetStyleCommand = new AsyncRelayCommand(ExecuteResetSettingsCommand);

        _noteRenderingSettingsViewModel = new NoteRendererSettingsViewModel(new NoteRenderingSettings(), "Notes");
        _measureRenderingSettingsViewModel = new MeasureRenderingSettingsViewModel(new MeasureRenderingSettings(), "Measures");
        _trackRenderingSettingsViewModel = new TrackRenderingSettingsViewModel(new TrackRenderingSettings(), "Tracks");
        _timeSignatureRenderingSettingsViewModel = new TimeSignatureRenderingSettingsViewModel(new TimeSignatureRenderingSettings(), "Time Signatures");

        RegisterSettingsViewModel(_noteRenderingSettingsViewModel);
        RegisterSettingsViewModel(_measureRenderingSettingsViewModel);
        RegisterSettingsViewModel(_trackRenderingSettingsViewModel);
        RegisterSettingsViewModel(_timeSignatureRenderingSettingsViewModel);

        SelectedSettingsViewModel = Settings[0];
    }

    private void ApplySettings(TabRenderingSettings settings)
    {
        _noteRenderingSettingsViewModel.ApplySettings(settings.NoteRenderingSettings);
        _measureRenderingSettingsViewModel.ApplySettings(settings.MeasureRenderingSettings);
        _trackRenderingSettingsViewModel.ApplySettings(settings.TrackRenderingSettings);
        _timeSignatureRenderingSettingsViewModel.ApplySettings(settings.TimeSignatureRenderingSettings);
    }

    private void RebuildTab()
    {
        if (TabDocument == null!) return;

        try
        {
            TabRenderingSettings settings = BuildRenderingSettings();
            TabRenderer renderer = new TabRenderer(TabDocument, settings);
            TabText = renderer.Render();
        }
        catch (Exception e)
        {
            bool hasSnackbarPresenter = _snackbarService.GetSnackbarPresenter() != null;
            if (hasSnackbarPresenter) _snackbarService.Show("Rendering Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(30));
            TabText = string.Empty;
            Console.WriteLine(e);
        }
    }

    private TabRenderingSettings BuildRenderingSettings()
    {
        return new TabRenderingSettings
        {
            NoteRenderingSettings = _noteRenderingSettingsViewModel.GetSettings(),
            MeasureRenderingSettings = _measureRenderingSettingsViewModel.GetSettings(),
            TrackRenderingSettings = _trackRenderingSettingsViewModel.GetSettings(),
            TimeSignatureRenderingSettings = _timeSignatureRenderingSettingsViewModel.GetSettings()
        };
    }

    private void RegisterSettingsViewModel(ISettingsViewModel settingsViewModel)
    {
        Settings.Add(settingsViewModel);
        settingsViewModel.SubSettingChanged += (_, _) => RebuildTab();
    }

    private void RequestReturn()
    {
        ReturnRequested?.Invoke(this, EventArgs.Empty);
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

    private void ExecuteLoadStyleCommand()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "GW2TabStyle File (*.gw2tabstyle)|*.gw2tabstyle",
            Multiselect = false,
            AddToRecent = true
        };

        try
        {
            bool? dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != true) return;

            TabRenderingSettings renderingSettings = TabRenderingSettings.FromFile(openFileDialog.FileName);
            ApplySettings(renderingSettings);

            _snackbarService.Show("Style Loaded", $"The style '{Path.GetFileName(openFileDialog.FileName)}' was successfully loaded.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Checkmark16), TimeSpan.FromSeconds(3));
        }
        catch (Exception e)
        {
            _snackbarService.Show("Load Error", e.Message, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ErrorCircle16), TimeSpan.FromSeconds(10));
        }
    }

    private void ExecuteSaveStyleCommand()
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "GW2TabStyle File (*.gw2tabstyle)|*.gw2tabstyle",
            CreatePrompt = true,
            AddToRecent = true
        };

        try
        {
            bool? dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != true) return;

            TabRenderingSettings renderingSettings = BuildRenderingSettings();
            renderingSettings.SaveToFile(saveFileDialog.FileName);

            _snackbarService.Show("Style Saved", "The style was saved successfully.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Checkmark16), TimeSpan.FromSeconds(3));
        }
        catch (Exception e)
        {
            _snackbarService.Show("Load Error", e.Message, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ErrorCircle16), TimeSpan.FromSeconds(10));
        }
    }

    private async Task ExecuteResetSettingsCommand()
    {
        ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
            new SimpleContentDialogCreateOptions
            {
                Title = "Reset the Tab Styles?",
                Content = "All of your unsaved changes will be lost. Are you sure you want to reset the tab styles?",
                PrimaryButtonText = "Reset",
                SecondaryButtonText = "Don't Reset",
                CloseButtonText = "Cancel",
            }
        );

        if (result == ContentDialogResult.Primary)
        {
            ApplySettings(new TabRenderingSettings());
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