using System;
using System.Windows;
using GW2MidiConverter.App.ViewModels;
using GW2MidiConverter.App.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace GW2MidiConverter.App.Views;

public partial class MainWindow
{
    public MainWindow(MainWindowViewModel viewModel,
                      INavigationService navigationService,
                      ISnackbarService snackbarService,
                      IServiceProvider serviceProvider,
                      IContentDialogService contentDialogService)
    {
        DataContext = viewModel;

        InitializeComponent();

        navigationService.SetNavigationControl(NavigationView);
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        contentDialogService.SetDialogHost(RootContentDialog);

        Loaded += (_, _) => NavigationView.Navigate(typeof(MyPage));

        NavigationView.SetServiceProvider(serviceProvider);
    }

    private bool _isUserClosedPane;
    private bool _isPaneOpenedOrClosedFromCode;

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_isUserClosedPane)
        {
            return;
        }

        _isPaneOpenedOrClosedFromCode = true;
        NavigationView.SetCurrentValue(NavigationView.IsPaneOpenProperty, e.NewSize.Width > 500);
        _isPaneOpenedOrClosedFromCode = false;
    }

    private void NavigationView_OnPaneOpened(NavigationView sender, RoutedEventArgs args)
    {
        if (_isPaneOpenedOrClosedFromCode)
        {
            return;
        }

        _isUserClosedPane = false;
    }

    private void NavigationView_OnPaneClosed(NavigationView sender, RoutedEventArgs args)
    {
        if (_isPaneOpenedOrClosedFromCode)
        {
            return;
        }

        _isUserClosedPane = true;
    }

    private void NavigationView_OnSelectionChanged(NavigationView navigationView, RoutedEventArgs args)
    {
        // You can make specific pages have no header!
        NavigationView.SetCurrentValue(NavigationView.HeaderVisibilityProperty,
            navigationView.SelectedItem?.TargetPageType != typeof(MyPage)
                ? Visibility.Visible
                : Visibility.Collapsed
        );
    }
}