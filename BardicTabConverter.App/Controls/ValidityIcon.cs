using System.Windows;
using System.Windows.Controls;

namespace BardicTabConverter.App.Controls;

internal class ValidityIcon : Control
{
    static ValidityIcon()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ValidityIcon), new FrameworkPropertyMetadata(typeof(ValidityIcon)));
    }

    public static readonly DependencyProperty ValidityTypeProperty = DependencyProperty.Register(nameof(ValidityType), typeof(ValidityType), typeof(ValidityIcon), new PropertyMetadata(default(ValidityType)));

    public static readonly DependencyProperty ValidityMessageProperty = DependencyProperty.Register(nameof(ValidityMessage), typeof(string), typeof(ValidityIcon), new PropertyMetadata(default(string)));

    public ValidityType ValidityType
    {
        get => (ValidityType)GetValue(ValidityTypeProperty);
        set => SetValue(ValidityTypeProperty, value);
    }


    public string ValidityMessage
    {
        get => (string)GetValue(ValidityMessageProperty);
        set => SetValue(ValidityMessageProperty, value);
    }
}