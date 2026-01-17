using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BardicTabConverter.App.Controls;

internal class SubSettingContentControl : ContentControl
{
    static SubSettingContentControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SubSettingContentControl),
                                                 new FrameworkPropertyMetadata(typeof(SubSettingContentControl)));
    }


    public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(nameof(HeaderText), typeof(string),
                                                                                               typeof(SubSettingContentControl),
                                                                                               new PropertyMetadata(default(string)));

    public static readonly DependencyProperty DescriptionTextProperty = DependencyProperty.Register(nameof(DescriptionText),
                                                                                                    typeof(string),
                                                                                                    typeof(SubSettingContentControl),
                                                                                                    new PropertyMetadata(default(string)));


    public static readonly DependencyProperty ErrorSymbolVisibilityProperty = DependencyProperty.Register(nameof(ErrorSymbolVisibility),
                                                                                                          typeof(Visibility),
                                                                                                          typeof(SubSettingContentControl),
                                                                                                          new PropertyMetadata(default(Visibility)));

    public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register(nameof(ErrorMessage),
                                                                                                 typeof(string),
                                                                                                 typeof(SubSettingContentControl),
                                                                                                 new PropertyMetadata(default(string)));

    public static readonly DependencyProperty ShowBorderProperty = DependencyProperty.Register(nameof(ShowBorder), 
                                                                                               typeof(bool), 
                                                                                               typeof(SubSettingContentControl), 
                                                                                               new PropertyMetadata(true));

    public bool ShowBorder
    {
        get => (bool)GetValue(ShowBorderProperty);
        set => SetValue(ShowBorderProperty, value);
    }

    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public string? DescriptionText
    {
        get => (string)GetValue(DescriptionTextProperty);
        set => SetValue(DescriptionTextProperty, value);
    }

    public Visibility ErrorSymbolVisibility
    {
        get => (Visibility)GetValue(ErrorSymbolVisibilityProperty);
        set => SetValue(ErrorSymbolVisibilityProperty, value);
    }

    public string ErrorMessage
    {
        get => (string)GetValue(ErrorMessageProperty);
        set => SetValue(ErrorMessageProperty, value);
    }
}