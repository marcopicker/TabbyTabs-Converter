using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.GuildWars.Instruments;

public class InstrumentNoteMap
{
    private readonly List<Dictionary<MidiNoteNumber, GuildWarsNote>> _maps;

    public InstrumentNoteMap(IEnumerable<Dictionary<MidiNoteNumber, GuildWarsNote>> maps)
    {
        ArgumentNullException.ThrowIfNull(maps);
        List<Dictionary<MidiNoteNumber, GuildWarsNote>> mapsList = maps.ToList();
        if (mapsList.Count == 0) throw new ArgumentException("Maps must not be empty.", nameof(maps));
        _maps = mapsList;
    }

    public bool IsNoteMapped(MidiNoteNumber midiNote)
    {
        return _maps.Any(m => m.ContainsKey(midiNote));
    }

    public GuildWarsNote[] MapMidiToNotes(MidiNoteNumber midiNote)
    {
        if (!IsNoteMapped(midiNote)) throw new ArgumentOutOfRangeException(nameof(midiNote), "The given MidiNoteNumber is not mapped to anything.");
        GuildWarsNote[] notes = _maps.Where(m => m.ContainsKey(midiNote))
                                     .Select(m => m[midiNote])
                                     .ToArray();
        return notes;
    }

    public static InstrumentNoteMap FromFile(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException("The given file does not exist.", path);
        throw new NotImplementedException();
    }
}