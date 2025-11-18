using System.Windows;
using GW2MidiConverter.App.Models;
using GW2MidiConverter.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GW2MidiConverter.App.ViewModels;
using GW2MidiConverter.App.Views;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Wpf.Ui;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace GW2MidiConverter.App;

public partial class App : Application
{
    private static readonly IHost _host;

    static App()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        RegisterLogger(builder.Services);
        RegisterWpfUiComponents(builder.Services);
        RegisterApplicationContent(builder.Services);
        _host = builder.Build();
    }

    private static void RegisterWpfUiComponents(IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ISnackbarService, SnackbarService>();
        services.AddSingleton<IContentDialogService, ContentDialogService>();
    }

    private static void RegisterApplicationContent(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<PreferencesViewModel>();
        services.AddSingleton<FileLoaderViewModel>();
        services.AddSingleton<TabRendererViewModel>();

        services.AddSingleton<IInstrumentProvider, InstrumentFilesLoader>();
        services.AddSingleton<IEditDistanceCalculator, LevenshteinDistanceCalculator>();
        services.AddSingleton<IApplicationVersionChecker, GitHubHostedApplicationVersionChecker>();
    }

    private static void RegisterLogger(IServiceCollection services)
    {
        Logger logger = new LoggerConfiguration().MinimumLevel.Debug()
                                                 .Enrich.FromLogContext()
                                                 .WriteTo.Console()
                                                 .WriteTo.File(
                                                     formatter: new CompactJsonFormatter(),
                                                     path: "logs/log.json",
                                                     rollingInterval: RollingInterval.Day
                                                 )
                                                 .CreateLogger();
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(logger, true);
        });
    }

    protected override void OnStartup(StartupEventArgs args)
    {
        _host.Start();

        MainWindow? mainWindow = _host.Services.GetService<MainWindow>();
        mainWindow?.Show();

        base.OnStartup(args);
    }

    protected override void OnExit(ExitEventArgs args)
    {
        _host.StopAsync().Wait();
        _host.Dispose();
        base.OnExit(args);
    }
}