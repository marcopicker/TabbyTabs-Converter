using System;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Converters.Tokens.Midi;

public class NoteMidiToken : MidiToken
{
    public readonly MidiNoteNumber[] MidiNotes;

    public NoteMidiToken(long startTime, long endTime, MidiNoteNumber[] midiNotes) : base(startTime, endTime)
    {
        if(midiNotes.Length == 0) throw new ArgumentException("Midi notes must not be empty", nameof(midiNotes));
        MidiNotes = midiNotes;
    }
}