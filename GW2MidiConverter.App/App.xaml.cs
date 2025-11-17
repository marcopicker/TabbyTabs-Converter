using System.Windows;
using GW2MidiConverter.App.Models;
using GW2MidiConverter.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GW2MidiConverter.App.ViewModels;
using GW2MidiConverter.App.Views;
using Wpf.Ui;

namespace GW2MidiConverter.App;

public partial class App : Application
{
    private static readonly IHost _host;

    static App()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
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
        // Windows
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
        
        // Services
        services.AddSingleton<IInstrumentProvider, InstrumentFilesLoader>();
        services.AddSingleton<IEditDistanceCalculator, LevenshteinDistanceCalculator>();
        services.AddSingleton<IApplicationVersionChecker, GitHubHostedApplicationVersionChecker>();
    }

    public static T GetRequiredService<T>() where T : class
    {
        return _host.Services.GetRequiredService<T>();
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