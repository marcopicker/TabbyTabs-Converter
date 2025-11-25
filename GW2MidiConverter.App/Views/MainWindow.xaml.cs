using System;
using System.Windows;
using System.Windows.Input;
using GW2MidiConverter.App.ViewModels;
using Wpf.Ui;

namespace GW2MidiConverter.App.Views;

internal partial class MainWindow
{
    public MainWindow(MainWindowViewModel viewModel,
                      ISnackbarService snackbarService,
                      IContentDialogService contentDialogService,
                      PreferencesViewModel preferencesViewModel)
    {
        DataContext = viewModel;

        InitializeComponent();
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        contentDialogService.SetDialogHost(RootContentDialog);

        PreferencesView.DataContext = preferencesViewModel;
        // Loaded += (_, _) => snackbarService.Show("MEOW!", "Cat cat cat purr", ControlAppearance.Primary, null, TimeSpan.FromSeconds(5));
    }

    private void PreferencesButton_OnClick(object sender, RoutedEventArgs e)
    {
        Visibility newVisibility = PreferencesBackdrop.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        PreferencesBackdrop.Visibility = newVisibility;
    }

    private void PreferencesBackdrop_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        return;
        PreferencesBackdrop.Visibility = Visibility.Collapsed;
    }

    private void PreferencesView_OnCloseRequested(object? sender, EventArgs e)
    {
        PreferencesBackdrop.Visibility = Visibility.Collapsed;
    }

    private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (PreferencesBackdrop.Visibility != Visibility.Visible ||
            e.Key != Key.Escape)
        {
            e.Handled = false;
            return;
        }
        
        PreferencesBackdrop.Visibility = Visibility.Collapsed;
        e.Handled = true;
    }
}