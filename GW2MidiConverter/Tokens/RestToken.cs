using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Tokens;

public class RestToken(long startTime, long endTime, int[] midiNotes, NoteLength length) : NoteToken(startTime, endTime, midiNotes, length)
{
    
}