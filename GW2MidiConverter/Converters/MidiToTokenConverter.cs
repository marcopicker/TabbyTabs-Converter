using System;
using System.Collections.Generic;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tokens;

namespace GW2MidiConverter;

public class MidiToTokenConverter
{
    private readonly MidiConverterSettings _settings;
    
    public MidiToTokenConverter(MidiConverterSettings? settings = null)
    {
        _settings = settings ?? new MidiConverterSettings();
    }

    public Dictionary<int, List<IToken>> ConvertTracks(MidiDocument midiDocument, params int[] trackIndices)
    {
        throw new NotImplementedException();
    }
}