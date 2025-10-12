using System;
using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.GuildWars.Instruments;

public class InstrumentNoteMap(List<Dictionary<MidiNoteNumber, GuildWarsNote>> maps)
{
    private readonly List<Dictionary<MidiNoteNumber, GuildWarsNote>> _maps = maps;

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
        throw new NotImplementedException();
    }

    public static InstrumentNoteMap Piano = new InstrumentNoteMap(
        [
            new Dictionary<MidiNoteNumber, GuildWarsNote>()
            {
                { new MidiNoteNumber(48), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1)) },
                { new MidiNoteNumber(49), new GuildWarsNote(GuildWarsNoteKey.Note1Sharp, new Octave(-1)) },
                { new MidiNoteNumber(50), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(-1)) },
                { new MidiNoteNumber(51), new GuildWarsNote(GuildWarsNoteKey.Note2Sharp, new Octave(-1)) },
                { new MidiNoteNumber(52), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(-1)) },
                { new MidiNoteNumber(53), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(-1)) },
                { new MidiNoteNumber(54), new GuildWarsNote(GuildWarsNoteKey.Note4Sharp, new Octave(-1)) },
                { new MidiNoteNumber(55), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(-1)) },
                { new MidiNoteNumber(56), new GuildWarsNote(GuildWarsNoteKey.Note5Sharp, new Octave(-1)) },
                { new MidiNoteNumber(57), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(-1)) },
                { new MidiNoteNumber(58), new GuildWarsNote(GuildWarsNoteKey.Note6Sharp, new Octave(-1)) },
                { new MidiNoteNumber(59), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(-1)) },
                { new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1)) },
            },
            new Dictionary<MidiNoteNumber, GuildWarsNote>()
            {
                { new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0)) },
                { new MidiNoteNumber(61), new GuildWarsNote(GuildWarsNoteKey.Note1Sharp, new Octave(0)) },
                { new MidiNoteNumber(62), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0)) },
                { new MidiNoteNumber(63), new GuildWarsNote(GuildWarsNoteKey.Note2Sharp, new Octave(0)) },
                { new MidiNoteNumber(64), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)) },
                { new MidiNoteNumber(65), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(0)) },
                { new MidiNoteNumber(66), new GuildWarsNote(GuildWarsNoteKey.Note4Sharp, new Octave(0)) },
                { new MidiNoteNumber(67), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0)) },
                { new MidiNoteNumber(68), new GuildWarsNote(GuildWarsNoteKey.Note5Sharp, new Octave(0)) },
                { new MidiNoteNumber(69), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(0)) },
                { new MidiNoteNumber(70), new GuildWarsNote(GuildWarsNoteKey.Note6Sharp, new Octave(0)) },
                { new MidiNoteNumber(71), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(0)) },
                { new MidiNoteNumber(72), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0)) },
            },
            new Dictionary<MidiNoteNumber, GuildWarsNote>()
            {
                { new MidiNoteNumber(72), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1)) },
                { new MidiNoteNumber(73), new GuildWarsNote(GuildWarsNoteKey.Note1Sharp, new Octave(1)) },
                { new MidiNoteNumber(74), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(1)) },
                { new MidiNoteNumber(75), new GuildWarsNote(GuildWarsNoteKey.Note2Sharp, new Octave(1)) },
                { new MidiNoteNumber(76), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(1)) },
                { new MidiNoteNumber(77), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(1)) },
                { new MidiNoteNumber(78), new GuildWarsNote(GuildWarsNoteKey.Note4Sharp, new Octave(1)) },
                { new MidiNoteNumber(79), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1)) },
                { new MidiNoteNumber(80), new GuildWarsNote(GuildWarsNoteKey.Note5Sharp, new Octave(1)) },
                { new MidiNoteNumber(81), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(1)) },
                { new MidiNoteNumber(82), new GuildWarsNote(GuildWarsNoteKey.Note6Sharp, new Octave(1)) },
                { new MidiNoteNumber(83), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(1)) },
                { new MidiNoteNumber(84), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1)) },
            }
        ]
    );
}