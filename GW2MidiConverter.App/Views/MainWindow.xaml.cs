using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GW2MidiConverter.App.ViewModels;
using Wpf.Ui;

namespace GW2MidiConverter.App.Views;

public partial class MainWindow
{
    private Rectangle? _tabGradientRectangle;
    private bool _previousIsScrolled;

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

    private void TabScrollViewer_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not ScrollViewer scrollViewer) return;
        _tabGradientRectangle = scrollViewer.Template.FindName("TabGradient", scrollViewer) as Rectangle;
    }

    private void TabScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (sender is not ScrollViewer scrollViewer) return;
        if (_tabGradientRectangle == null) return;
        bool isScrolled = scrollViewer.VerticalOffset > 0;
        if (isScrolled == _previousIsScrolled)
        {
            _previousIsScrolled = isScrolled;
            return;
        }
        _previousIsScrolled = isScrolled;

        DoubleAnimation animation = new DoubleAnimation
        {
            To = isScrolled ? 1.0 : 0.0,
            Duration = TimeSpan.FromMilliseconds(125),
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
        };
        
        _tabGradientRectangle.BeginAnimation(OpacityProperty, animation);
    }
}