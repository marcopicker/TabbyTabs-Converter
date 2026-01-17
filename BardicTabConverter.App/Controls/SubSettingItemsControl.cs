using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BardicTabConverter.App.Controls;

internal class SubSettingItemsControl : ItemsControl
{
    static SubSettingItemsControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SubSettingItemsControl),
                                                 new FrameworkPropertyMetadata(typeof(SubSettingItemsControl)));
    }

    public static readonly DependencyProperty AddCommandProperty = DependencyProperty.Register(nameof(AddCommand),
                                                                                               typeof(ICommand),
                                                                                               typeof(SubSettingItemsControl));

    public static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register(nameof(RemoveCommand),
                                                                                                  typeof(ICommand),
                                                                                                  typeof(SubSettingItemsControl));

    public static readonly DependencyProperty EmptyContentProperty = DependencyProperty.Register(nameof(EmptyContent),
                                                                                                 typeof(object),
                                                                                                 typeof(SubSettingItemsControl),
                                                                                                 new PropertyMetadata(default(object)));

    public static readonly DependencyProperty EmptyContentTemplateProperty = DependencyProperty.Register(nameof(EmptyContentTemplate),
                                                                                                         typeof(DataTemplate),
                                                                                                         typeof(SubSettingItemsControl),
                                                                                                         new PropertyMetadata(default(DataTemplate)));

    public static readonly DependencyProperty AddButtonVisibilityProperty = DependencyProperty.Register(nameof(AddButtonVisibility),
                                                                                                        typeof(Visibility),
                                                                                                        typeof(SubSettingItemsControl),
                                                                                                        new PropertyMetadata(default(Visibility)));

    public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(nameof(HeaderText), typeof(string),
                                                                                               typeof(SubSettingItemsControl),
                                                                                               new PropertyMetadata(default(string)));

    public static readonly DependencyProperty InternalHeaderContentProperty = DependencyProperty.Register(nameof(InternalHeaderContent),
                                                                                                          typeof(object),
                                                                                                          typeof(SubSettingItemsControl),
                                                                                                          new PropertyMetadata(default(object)));

    public static readonly DependencyProperty InternalHeaderTemplateProperty = DependencyProperty.Register(nameof(InternalHeaderTemplate),
                                                                                                           typeof(DataTemplate),
                                                                                                           typeof(SubSettingItemsControl),
                                                                                                           new PropertyMetadata(default(DataTemplate)));

    public static readonly DependencyProperty DescriptionTextProperty = DependencyProperty.Register(nameof(DescriptionText),
                                                                                                    typeof(string),
                                                                                                    typeof(SubSettingItemsControl),
                                                                                                    new PropertyMetadata(default(string)));

    public DataTemplate InternalHeaderTemplate
    {
        get => (DataTemplate)GetValue(InternalHeaderTemplateProperty);
        set => SetValue(InternalHeaderTemplateProperty, value);
    }

    public object InternalHeaderContent
    {
        get => GetValue(InternalHeaderContentProperty);
        set => SetValue(InternalHeaderContentProperty, value);
    }

    public string? HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public string? DescriptionText
    {
        get => (string)GetValue(DescriptionTextProperty);
        set => SetValue(DescriptionTextProperty, value);
    }

    public ICommand AddCommand
    {
        get => (ICommand)GetValue(AddCommandProperty);
        set => SetValue(AddCommandProperty, value);
    }

    public ICommand RemoveCommand
    {
        get => (ICommand)GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }

    public object EmptyContent
    {
        get => GetValue(EmptyContentProperty);
        set => SetValue(EmptyContentProperty, value);
    }

    public DataTemplate EmptyContentTemplate
    {
        get => (DataTemplate)GetValue(EmptyContentTemplateProperty);
        set => SetValue(EmptyContentTemplateProperty, value);
    }

    public Visibility AddButtonVisibility
    {
        get => (Visibility)GetValue(AddButtonVisibilityProperty);
        set => SetValue(AddButtonVisibilityProperty, value);
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
        return new ContentControl();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is not SubSettingContentControl && item is ContentControl;
    }
}