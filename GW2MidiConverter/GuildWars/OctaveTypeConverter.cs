using System;
using System.ComponentModel;
using System.Globalization;

namespace GW2MidiConverter.GuildWars;

internal class OctaveTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string valueString) return null;

        bool isInt = int.TryParse(valueString, out int octaveValue);
        return isInt ? new Octave(octaveValue) : null;
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is Octave octave)
        {
            return octave.Value.ToString(CultureInfo.InvariantCulture);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}