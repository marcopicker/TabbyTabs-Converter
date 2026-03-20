using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TabbyTabs.Converter.App.ViewModels.Settings;
using TabbyTabs.Converter.Rendering;
using TabbyTabs.Converter.Rendering.Settings;
using TabbyTabs.Converter.Tab;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace TabbyTabs.Converter.App.ViewModels;

internal class TabRendererViewModel : ObservableObject
{
    private static readonly TimeSpan MaximumExpectedRenderTimeBeforeShowingProgressIndicator = TimeSpan.FromMilliseconds(200);

    private readonly ISnackbarService _snackbarService;
    private readonly IContentDialogService _contentDialogService;
    private readonly ILogger<TabRendererViewModel> _logger;
    private string _tabText = "";
    private ISettingsViewModel? _selectedSettingsViewModel;

    private readonly NoteRendererSettingsViewModel _noteRenderingSettingsViewModel;
    private readonly MeasureRenderingSettingsViewModel _measureRenderingSettingsViewModel;
    private readonly TrackRenderingSettingsViewModel _trackRenderingSettingsViewModel;
    private readonly TimeSignatureRenderingSettingsViewModel _timeSignatureRenderingSettingsViewModel;
    private bool _isRenderingTakingLongerThanExpected;

    private int _activeRendererCount;
    private CancellationTokenSource? _renderCancellationTokenSource;

    public event EventHandler? ReturnRequested;

    public List<ISettingsViewModel> Settings { get; } = [];

    public ISettingsViewModel? SelectedSettingsViewModel
    {
        get => _selectedSettingsViewModel;
        set => SetProperty(ref _selectedSettingsViewModel, value);
    }

    public TabDocument? TabDocument { get; set; }

    public string TabText
    {
        get => _tabText;
        private set => SetProperty(ref _tabText, value);
    }

    public bool IsRenderingTakingLongerThanExpected
    {
        get => _isRenderingTakingLongerThanExpected;
        private set => SetProperty(ref _isRenderingTakingLongerThanExpected, value);
    }

    public ICommand CopyCommand { get; }
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

        CopyCommand = new RelayCommand(CopyTab);
        ExportCommand = new RelayCommand(ExportTab);
        ReturnCommand = new RelayCommand(RequestReturn);
        LoadStyleCommand = new AsyncRelayCommand(ExecuteLoadStyleCommandAsync);
        SaveStyleCommand = new AsyncRelayCommand(ExecuteSaveStyleCommandAsync);
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

    public async Task RenderTabAsync(CancellationToken cancellationToken = default)
    {
        if (TabDocument == null!) return;

        _activeRendererCount++;

        using IDisposable? _ = _logger.BeginScope(new { Operation = "Rendering Tab", ActiveRenderers = _activeRendererCount });
        _logger.LogInformation("Rendering tab (Currently active: {Count})", _activeRendererCount);

        Stopwatch renderStopwatch = Stopwatch.StartNew();

        try
        {
            Task<string> renderingTask = GetRenderedTabAsync(cancellationToken);
            Task maxExpectedRuntimeTask = Task.Delay(MaximumExpectedRenderTimeBeforeShowingProgressIndicator, cancellationToken);

            Task completedTask = await Task.WhenAny(renderingTask, maxExpectedRuntimeTask);
            if (completedTask == maxExpectedRuntimeTask)
            {
                IsRenderingTakingLongerThanExpected = true;
                await renderingTask;
            }

            TabText = await renderingTask;
            _logger.LogInformation("Tab rendered successfully after {ElapsedTime}ms.", renderStopwatch.ElapsedMilliseconds);
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation("Rendering tab cancelled.");
        }
        catch (TabRenderingException e)
        {
            _logger.LogError(e, "Failed to render tab with settings: {Settings}", e.Settings);
            bool hasSnackbarPresenter = _snackbarService.GetSnackbarPresenter() != null;
            if (hasSnackbarPresenter) _snackbarService.Show("Tab Rendering Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(10));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to render tab.");
            bool hasSnackbarPresenter = _snackbarService.GetSnackbarPresenter() != null;
            if (hasSnackbarPresenter) _snackbarService.Show("Tab Rendering Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(10));
            TabText = string.Empty;
        }
        finally
        {
            _activeRendererCount--;
            if (_activeRendererCount == 0)
            {
                IsRenderingTakingLongerThanExpected = false;
            }
        }
    }

    private async Task<string> GetRenderedTabAsync(CancellationToken cancellationToken = default)
    {
        if (TabDocument == null!) return string.Empty;

        TabRenderingSettings settings = await BuildRenderingSettingsAsync();

        return await Task.Run(() =>
        {
            TabRenderer renderer = new TabRenderer(TabDocument, settings);
            return renderer.Render(cancellationToken);
        }, cancellationToken);
    }

    private async Task<TabRenderingSettings> BuildRenderingSettingsAsync()
    {
        return await Task.Run(() => new TabRenderingSettings
        {
            NoteRenderingSettings = _noteRenderingSettingsViewModel.GetSettings(),
            MeasureRenderingSettings = _measureRenderingSettingsViewModel.GetSettings(),
            TrackRenderingSettings = _trackRenderingSettingsViewModel.GetSettings(),
            TimeSignatureRenderingSettings = _timeSignatureRenderingSettingsViewModel.GetSettings()
        });
    }

    private void RegisterSettingsViewModel(ISettingsViewModel settingsViewModel)
    {
        Settings.Add(settingsViewModel);
        settingsViewModel.SubSettingChanged += HandleSubSettingsChanged;
    }

    private async void HandleSubSettingsChanged(object? sender, EventArgs eventArgs)
    {
        try
        {
            _renderCancellationTokenSource?.Cancel();
            _renderCancellationTokenSource?.Dispose();

            _renderCancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _renderCancellationTokenSource.Token;

            await RenderTabAsync(token);
        }
        catch (TaskCanceledException)
        {
            // Nothing; this is expected.
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to rebuild tab after sub-setting change.");
        }
    }

    private void RequestReturn()
    {
        ReturnRequested?.Invoke(this, EventArgs.Empty);
    }

    private void CopyTab()
    {
        Clipboard.SetText(TabText);
        _snackbarService.Show("Tab Copied", "The tab was copied to the clipboard.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Clipboard3Day24), _snackbarService.DefaultTimeOut);
    }

    private void ExportTab()
    {
        using IDisposable? _ = _logger.BeginScope(new { Operation = "Exporting Tab" });

        if (TabDocument == null)
        {
            _logger.LogError("Failed to export tab: No tab loaded.");
            _snackbarService.Show("No Tab", "Please load a MIDI file first.", ControlAppearance.Danger, null, _snackbarService.DefaultTimeOut);
            return;
        }

        if (!CheckSettingValidity())
        {
            _logger.LogError("Failed to export tab: Invalid settings.");
            _snackbarService.Show("Invalid Settings", "Please check your settings and try again.", ControlAppearance.Danger, null, _snackbarService.DefaultTimeOut);
            return;
        }

        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "Text File (*.txt)|*.txt",
            AddToRecent = true
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
            _snackbarService.Show("Export Error", e.Message, ControlAppearance.Danger, null, TimeSpan.FromSeconds(60));
            throw;
        }
    }

    private async Task ExecuteLoadStyleCommandAsync()
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

            await Task.Run(() =>
            {
                TabRenderingSettings renderingSettings = TabRenderingSettings.FromFile(openFileDialog.FileName);
                ApplySettings(renderingSettings);
            });

            _logger.LogInformation("Style loaded successfully.");
            _snackbarService.Show("Style Loaded", $"The style '{Path.GetFileName(openFileDialog.FileName)}' was successfully loaded.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Checkmark16), _snackbarService.DefaultTimeOut);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to load style.");
            _snackbarService.Show("Load Error", e.Message, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ErrorCircle16), TimeSpan.FromSeconds(60));
        }
    }

    private async Task ExecuteSaveStyleCommandAsync()
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

            TabRenderingSettings renderingSettings = await BuildRenderingSettingsAsync();
            await Task.Run(() => renderingSettings.SaveToFile(saveFileDialog.FileName));

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
        return true;
    }

    private string PrepareFileName()
    {
        // TODO MP: Implement.
        return "Tab.txt";
    }
}