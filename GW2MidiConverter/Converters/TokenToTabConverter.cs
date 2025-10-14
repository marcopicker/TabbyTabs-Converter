using System;
using System.Text;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Converters;

public class TokenToTabConverter(MidiConverterSettings? settings = null)
{
    private readonly MidiConverterSettings _settings = settings ?? new MidiConverterSettings();

    public string Convert(MidiTrack track, InstrumentNoteMap map)
    { 
        ArgumentNullException.ThrowIfNull(track);
        ArgumentNullException.ThrowIfNull(map);
        
        StringBuilder builder = new StringBuilder();
        return builder.ToString();
    }
}