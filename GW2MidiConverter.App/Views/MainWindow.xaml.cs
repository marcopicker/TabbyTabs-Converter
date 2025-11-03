using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GW2MidiConverter.App.ViewModels;
using Wpf.Ui;

namespace GW2MidiConverter.App.Views;

internal partial class MainWindow
{
    public MainWindow(MainWindowViewModel viewModel,
                      INavigationService navigationService,
                      ISnackbarService snackbarService,
                      IServiceProvider serviceProvider,
                      IContentDialogService contentDialogService)
    {
        DataContext = viewModel;

        InitializeComponent();
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        contentDialogService.SetDialogHost(RootContentDialog);

        // Loaded += (_, _) => snackbarService.Show("MEOW!", "Cat cat cat purr", ControlAppearance.Primary, null, TimeSpan.FromSeconds(5));
    }
}