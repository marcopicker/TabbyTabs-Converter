using System.Windows;

namespace BardicTabConverter.App.Converters;

public class BooleanToVisibilityConverter() : BooleanConverter<Visibility>(Visibility.Visible, Visibility.Collapsed);