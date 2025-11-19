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
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace GW2MidiConverter.App.ViewModels;

internal class TabRendererViewModel : ObservableObject
{
    private readonly ISnackbarService _snackbarService;
    private readonly IContentDialogService _contentDialogService;
    private readonly ILogger<TabRendererViewModel> _logger;
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
                                IContentDialogService contentDialogService,
                                ILogger<TabRendererViewModel> logger)
    {
        _snackbarService = snackbarService;
        _contentDialogService = contentDialogService;
        _logger = logger;

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
        _logger.LogInformation("Applying settings: {@Settings}", settings);
        _noteRenderingSettingsViewModel.ApplySettings(settings.NoteRenderingSettings);
        _measureRenderingSettingsViewModel.ApplySettings(settings.MeasureRenderingSettings);
        _trackRenderingSettingsViewModel.ApplySettings(settings.TrackRenderingSettings);
        _timeSignatureRenderingSettingsViewModel.ApplySettings(settings.TimeSignatureRenderingSettings);
    }

    private void RebuildTab()
    {
        if (TabDocument == null!) return;

        using IDisposable? _ = _logger.BeginScope(new { Operation = "Rendering Tab" });
        TabRenderingSettings settings = BuildRenderingSettings();

        try
        {
            _logger.LogInformation("Rendering tab.");
            TabRenderer renderer = new TabRenderer(TabDocument, settings);
            TabText = renderer.Render();
            _logger.LogInformation("Tab rendered successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to render tab with settings: {Settings}", settings);
            bool hasSnackbarPresenter = _snackbarService.GetSnackbarPresenter() != null;
            if (hasSnackbarPresenter) _snackbarService.Show("Rendering Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(30));
            TabText = string.Empty;
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
        using IDisposable? _ = _logger.BeginScope(new { Operation = "Exporting Tab" });

        if (TabDocument == null)
        {
            _logger.LogError("Failed to export tab: No tab loaded.");
            _snackbarService.Show("No Tab", "Please load a MIDI file first.", ControlAppearance.Danger, null, TimeSpan.FromSeconds(4));
            return;
        }

        if (!CheckSettingValidity())
        {
            _logger.LogError("Failed to export tab: Invalid settings.");
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
            _logger.LogInformation("Tab exported successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to export tab.");
            _snackbarService.Show("Export Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(10));
            throw;
        }
    }

    private void ExecuteLoadStyleCommand()
    {
        using IDisposable? _ = _logger.BeginScope(new { Operation = "Loading Style" });
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

            _logger.LogInformation("Loading style '{FileName}'...", openFileDialog.FileName);

            TabRenderingSettings renderingSettings = TabRenderingSettings.FromFile(openFileDialog.FileName);
            ApplySettings(renderingSettings);

            _logger.LogInformation("Style loaded successfully.");
            _snackbarService.Show("Style Loaded", $"The style '{Path.GetFileName(openFileDialog.FileName)}' was successfully loaded.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Checkmark16), TimeSpan.FromSeconds(3));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to load style.");
            _snackbarService.Show("Load Error", e.Message, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ErrorCircle16), TimeSpan.FromSeconds(10));
        }
    }

    private void ExecuteSaveStyleCommand()
    {
        using IDisposable? _ = _logger.BeginScope(new { Operation = "Saving Style" });
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

            _logger.LogInformation("Saving style '{FileName}'...", saveFileDialog.FileName);

            TabRenderingSettings renderingSettings = BuildRenderingSettings();
            renderingSettings.SaveToFile(saveFileDialog.FileName);

            _logger.LogInformation("Style saved successfully.");
            _snackbarService.Show("Style Saved", "The style was saved successfully.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Checkmark16), TimeSpan.FromSeconds(3));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save style.");
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
        // TODO MP: Implement.
        return true;
    }

    private string PrepareFileName()
    {
        // TODO MP: Implement.
        return "Tab.txt";
    }
}