using System;

namespace GW2MidiConverter.GuildWars.Instruments;

public record Instrument
{
    public Instrument(string name, InstrumentNoteMap noteMap)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("The name must not be empty.", nameof(name));
        ArgumentNullException.ThrowIfNull(noteMap);

        Name = name;
        NoteMap = noteMap;
    }

    public string Name { get; init; }
    public InstrumentNoteMap NoteMap { get; init; }

    public void Deconstruct(out string name, out InstrumentNoteMap noteMap)
    {
        name = this.Name;
        noteMap = this.NoteMap;
    }

    public override string ToString() => Name;

    public static Instrument Piano => new Instrument("Piano", InstrumentNoteMaps.TripleOctaveChromatic);
    public static Instrument Harp => new Instrument("Harp", InstrumentNoteMaps.TripleOctaveCMajor);
    public static Instrument Lute => new Instrument("Lute", InstrumentNoteMaps.TripleOctaveCMajor);
    public static Instrument Minstrel => new Instrument("Minstrel", InstrumentNoteMaps.TripleOctaveCMajor);
    public static Instrument Bass => new Instrument("Bass", InstrumentNoteMaps.LowerOctaveCMajor);
}