using System;
using System.ComponentModel;
using System.Globalization;

namespace BardicTabConverter.Tab;

internal class TabElementLengthConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string valueString) return null;

        bool isDecimal = decimal.TryParse(valueString, out decimal decimalValue);
        return isDecimal ? new TabElementLength(decimalValue) : null;
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is TabElementLength tabElementLength)
        {
            return tabElementLength.Beats.ToString(CultureInfo.InvariantCulture);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}