using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.GuildWars.Instruments;

public class InstrumentNoteMap
{
    private readonly IReadOnlyList<IReadOnlyDictionary<MidiNoteNumber, GuildWarsNote>> _mappings;

    public InstrumentNoteMap(IReadOnlyList<IReadOnlyDictionary<MidiNoteNumber, GuildWarsNote>> mappings)
    {
        ArgumentNullException.ThrowIfNull(mappings);
        if (mappings.Count == 0) throw new ArgumentException("Maps must not be empty.", nameof(mappings));
        _mappings = mappings;
    }

    public bool IsNoteMapped(MidiNoteNumber midiNote)
    {
        return _mappings.Any(m => m.ContainsKey(midiNote));
    }

    public GuildWarsNote[] MapMidiToNotes(MidiNoteNumber midiNote)
    {
        if (!IsNoteMapped(midiNote)) throw new ArgumentOutOfRangeException(nameof(midiNote), "The given MidiNoteNumber is not mapped to anything.");
        GuildWarsNote[] notes = _mappings.Where(m => m.ContainsKey(midiNote))
                                     .Select(m => m[midiNote])
                                     .ToArray();
        return notes;
    }
}