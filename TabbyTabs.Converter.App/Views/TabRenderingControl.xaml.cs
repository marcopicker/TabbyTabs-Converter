using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TabbyTabs.Converter.App.Views;

internal partial class TabRenderingControl
{
    private bool _previousIsScrolled;
    private ScrollContentPresenter? _tabScrollPresenter;

    private readonly List<GradientStop> _scrollGradientStops = [];
    private readonly List<Color> _scrollGradientDefaultColors = [];

    public TabRenderingControl()
    {
        InitializeComponent();
    }

    private void TabScrollViewer_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not ScrollViewer scrollViewer) return;
        _tabScrollPresenter = scrollViewer.Template.FindName("PART_ScrollContentPresenter", scrollViewer) as ScrollContentPresenter;
        if (_tabScrollPresenter == null) return;

        if (_tabScrollPresenter.OpacityMask is not LinearGradientBrush opacityMask) return;

        foreach (GradientStop gradientStop in opacityMask.GradientStops.Take(opacityMask.GradientStops.Count))
        {
            _scrollGradientStops.Add(gradientStop);
            _scrollGradientDefaultColors.Add(gradientStop.Color);
        }
        
        AnimateScrollGradient(false, true);
    }

    private void TabScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs _)
    {
        if (sender is not ScrollViewer scrollViewer) return;
        if (_tabScrollPresenter == null) return;
        bool isScrolled = scrollViewer.VerticalOffset > 0;
        if (isScrolled == _previousIsScrolled)
        {
            _previousIsScrolled = isScrolled;
            return;
        }

        AnimateScrollGradient(isScrolled);
        _previousIsScrolled = isScrolled;
    }

    private void AnimateScrollGradient(bool visible, bool instant = false)
    {
        TimeSpan duration = instant ? TimeSpan.Zero : TimeSpan.FromMilliseconds(200);
        
        for (int i = 0; i < _scrollGradientStops.Count; i++)
        {
            ColorAnimation animation = new ColorAnimation()
            {
                From = _scrollGradientStops[i].Color,
                To = !visible ? Colors.White : _scrollGradientDefaultColors[i],
                Duration = duration
            };

            _scrollGradientStops[i].BeginAnimation(GradientStop.ColorProperty, animation);
        }
    }
}