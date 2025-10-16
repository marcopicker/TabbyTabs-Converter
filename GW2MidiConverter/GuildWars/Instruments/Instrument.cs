namespace GW2MidiConverter.GuildWars.Instruments;

public class Instrument(string name, InstrumentNoteMap noteMap)
{
    public readonly string Name = name;
    public readonly InstrumentNoteMap NoteMap = noteMap;
}