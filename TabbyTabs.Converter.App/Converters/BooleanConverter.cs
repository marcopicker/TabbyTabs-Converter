using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace TabbyTabs.Converter.App.Converters;

public class BooleanConverter<T>(T trueValue, T falseValue) : IValueConverter
{
    public T? True { get; set; } = trueValue;
    public T? False { get; set; } = falseValue;

    public virtual object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool flag) return False;
        return flag ? True : False;
    }

    public virtual object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is T typedValue && EqualityComparer<T>.Default.Equals(typedValue, True);
    }
}