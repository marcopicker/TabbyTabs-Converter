using System.IO;
using System.Windows;
using TabbyTabs.Converter.App.Models;
using TabbyTabs.Converter.App.Resources;
using TabbyTabs.Converter.App.Services;
using TabbyTabs.Converter.App.ViewModels;
using TabbyTabs.Converter.App.Views;
using EasyPrefs;
using EasyPrefs.Abstractions;
using EasyPrefs.JSON;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using Wpf.Ui;

namespace TabbyTabs.Converter.App;

public partial class App
{
    private static readonly IHost Host;

    static App()
    {
        HostApplicationBuilder builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
        RegisterLogger(builder.Services);
        RegisterPreferences(builder.Services);
        RegisterWpfUiComponents(builder.Services);
        RegisterApplicationContent(builder.Services);
        Host = builder.Build();
    }

    private static void RegisterPreferences(IServiceCollection services)
    {
        JsonPreferenceDataProvider dataProvider = new JsonPreferenceDataProvider("preferences.json");

        BuildablePreferenceSchemaProvider schemaProvider = new BuildablePreferenceSchemaProvider();
        schemaProvider.AddSchema(new PreferenceSchema(PreferenceKeys.AUTO_UPDATE, typeof(bool), false));

        Preferences preferences = new Preferences(dataProvider, schemaProvider)
        {
            AutoApplyChanges = true
        };

        services.AddSingleton<IPreferences, Preferences>(_ => preferences);
        services.AddSingleton<IPreferencesReader, Preferences>(_ => preferences);
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
        string logFilePath = Path.Combine(Logging.LogFolder, "log.json");
        Log.Logger = new LoggerConfiguration().MinimumLevel.Warning()
                                              .Enrich.FromLogContext()
                                              .Enrich.WithProperty("Application", ApplicationResources.ApplicationName)
                                              .Enrich.WithProperty("Version", VersioningHelper.CurrentVersionText ?? string.Empty)
                                              .WriteTo.Console()
                                              .WriteTo.File(
                                                  formatter: new CompactJsonFormatter(),
                                                  path: logFilePath,
                                                  rollingInterval: RollingInterval.Day,
                                                  fileSizeLimitBytes: 10485760,
                                                  retainedFileCountLimit: 7,
                                                  rollOnFileSizeLimit: true
                                              )
                                              .CreateLogger();
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(Log.Logger, true);
        });
    }

    protected override void OnStartup(StartupEventArgs args)
    {
        Host.Start();

        Log.Information("Application starting. Version: {Version}", VersioningHelper.CurrentVersionText ?? "Unknown");

        MainWindow? mainWindow = Host.Services.GetService<MainWindow>();
        mainWindow?.Show();

        base.OnStartup(args);
    }

    protected override void OnExit(ExitEventArgs args)
    {
        Log.Information("Application shutting down.");

        Host.StopAsync().Wait();
        Host.Dispose();
        base.OnExit(args);
    }
}