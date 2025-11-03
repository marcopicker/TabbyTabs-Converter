using System.Windows;
using System.Windows.Controls;

namespace GW2MidiConverter.App.Views;

internal partial class TabRenderingControl : UserControl
{
    private ScrollContentPresenter? _tabScrollPresenter;
    
    public TabRenderingControl()
    {
        InitializeComponent();
    }

    private void TabScrollViewer_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not ScrollViewer scrollViewer) return;
        _tabScrollPresenter = scrollViewer.Template.FindName("PART_ScrollContentPresenter", scrollViewer) as ScrollContentPresenter;
    }

    private void TabScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        // if (sender is not ScrollViewer scrollViewer) return;
        // if (_tabScrollPresenter == null) return;
        // bool isScrolled = scrollViewer.VerticalOffset > 0;
        // if (isScrolled == _previousIsScrolled)
        // {
        //     _previousIsScrolled = isScrolled;
        //     return;
        // }
        // _previousIsScrolled = isScrolled;
        //
        // DoubleAnimation animation = new DoubleAnimation
        // {
        //     To = isScrolled ? 1 : 0,
        //     Duration = TimeSpan.FromMilliseconds(125),
        //     EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
        // };
        //
        // if (_tabScrollPresenter.OpacityMask is not LinearGradientBrush brush) return;
        // //
        // brush.BeginAnimation(, animation);
    }
}