using System.Windows;

namespace GW2MidiConverter.App.Converters;

public class BooleanToVisibilityConverter() : BooleanConverter<Visibility>(Visibility.Visible, Visibility.Collapsed);