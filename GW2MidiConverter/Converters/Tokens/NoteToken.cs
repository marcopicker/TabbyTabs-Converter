using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Converters.Tokens;

public class NoteToken(long startTime, long endTime, int[] midiNotes, NoteLength length) : Token(startTime, endTime)
{
    public readonly int[] MidiNotes = midiNotes;

    public readonly NoteLength Length = length;
}