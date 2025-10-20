namespace GW2MidiConverter.GuildWars.Instruments;

public class Instrument(string name, InstrumentNoteMap noteMap)
{
    public string Name { get; } = name;
    public InstrumentNoteMap NoteMap { get; } = noteMap;
}