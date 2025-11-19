using System;
using System.Globalization;
using System.Windows.Data;
using GW2MidiConverter.GuildWars;

namespace GW2MidiConverter.App.Converters;

public class GuildWarsKeyToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is not GuildWarsNoteKey key) return value?.ToString() ?? string.Empty;
        return key switch
        {
            GuildWarsNoteKey.Note1 => "Note 1",
            GuildWarsNoteKey.Note1Sharp => "Note ♯1/♭2",
            GuildWarsNoteKey.Note2 => "Note 2",
            GuildWarsNoteKey.Note2Sharp => "Note ♯2/♭3",
            GuildWarsNoteKey.Note3 => "Note 3",
            GuildWarsNoteKey.Note4 => "Note 4",
            GuildWarsNoteKey.Note4Sharp => "Note ♯4/♭5",
            GuildWarsNoteKey.Note5 => "Note 5",
            GuildWarsNoteKey.Note5Sharp => "Note ♯5/♭6",
            GuildWarsNoteKey.Note6 => "Note 6",
            GuildWarsNoteKey.Note6Sharp => "Note ♯6/♭7",
            GuildWarsNoteKey.Note7 => "Note 7",
            GuildWarsNoteKey.Note8 => "Note 8",
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}