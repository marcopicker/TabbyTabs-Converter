using System.Windows;

namespace TabbyTabs.Converter.App.Converters;

public class BooleanToVisibilityConverter() : BooleanConverter<Visibility>(Visibility.Visible, Visibility.Collapsed);