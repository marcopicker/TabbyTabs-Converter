using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GW2MidiConverter.App.Controls;

internal class EditableItemsControl : ItemsControl
{
    static EditableItemsControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableItemsControl),
                                                 new FrameworkPropertyMetadata(typeof(EditableItemsControl)));
    }

    public static readonly DependencyProperty AddCommandProperty = DependencyProperty.Register(nameof(AddCommand),
                                                                                               typeof(ICommand),
                                                                                               typeof(EditableItemsControl));

    public static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register(nameof(RemoveCommand),
                                                                                                  typeof(ICommand),
                                                                                                  typeof(EditableItemsControl));

    public static readonly DependencyProperty EmptyContentProperty = DependencyProperty.Register(nameof(EmptyContent),
                                                                                                 typeof(object),
                                                                                                 typeof(EditableItemsControl),
                                                                                                 new PropertyMetadata(default(object)));

    public static readonly DependencyProperty EmptyContentTemplateProperty = DependencyProperty.Register(nameof(EmptyContentTemplate),
                                                                                                         typeof(DataTemplate),
                                                                                                         typeof(EditableItemsControl),
                                                                                                         new PropertyMetadata(default(DataTemplate)));

    public static readonly DependencyProperty AddButtonVisibilityProperty = DependencyProperty.Register(
        nameof(AddButtonVisibility), typeof(Visibility), typeof(EditableItemsControl), new PropertyMetadata(default(Visibility)));

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
}