using System.IO;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tests.Resources;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.GuildWars.Instruments;

[TestFixture]
public class InstrumentTests
{
    [Test]
    public void FromFile_FileMissing_Throws()
    {
        Assert.That(() => Instrument.FromFile("Meow meow meow, I do not exist!"), Throws.Exception.TypeOf<FileNotFoundException>());
    }

    [Test]
    public void FromFile_FileExistsButWrongType_Throws()
    {
        Assert.That(() => Instrument.FromFile(MidiFiles.ChordTestFile), Throws.Exception.TypeOf<InstrumentException>());
    }

    [Test]
    public void FromFile_ValidFile_DoesNotThrow()
    {
        Instrument instrument = Instrument.FromFile(InstrumentFiles.ValidInstrumentFile);

        Assert.That(instrument, Is.Not.Null);
        Assert.That(instrument.PlayableNotes, Is.Not.Null);
        Assert.That(instrument.PlayableNotes, Is.Not.Empty);
    }
    
    [Test]
    public void FromFile_ValidFile_NameIsCorrect()
    {
        Instrument instrument = Instrument.FromFile(InstrumentFiles.ValidInstrumentFile);

        Assert.That(instrument.Name, Is.EqualTo("Test Instrument"));
    }

    [Test]
    public void IsNoteMapped()
    {
        Instrument instrument = new Instrument("Test Instrument", [
            new InstrumentNoteMapping(new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))),
            new InstrumentNoteMapping(new MidiNoteNumber(62), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0)))
        ]);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(instrument.CanPlayNote(new MidiNoteNumber(60)), Is.True);
            Assert.That(instrument.CanPlayNote(new MidiNoteNumber(62)), Is.True);
            Assert.That(instrument.CanPlayNote(new MidiNoteNumber(123)), Is.False);
        }
    }

    [Test]
    public void MapMidiToNotes_MultipleMappings_ReturnsAll()
    {
        Instrument instrument = new Instrument("Test Instrument", [
            new InstrumentNoteMapping(new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(-1))),
            new InstrumentNoteMapping(new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))),
            new InstrumentNoteMapping(new MidiNoteNumber(62), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0))),
        ]);

        GuildWarsNote[] notes = instrument.MapMidiToNotes(new MidiNoteNumber(60));
        
        Assert.That(notes, Has.Length.EqualTo(2));
        Assert.That(notes, Contains.Item(new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(-1))));
        Assert.That(notes, Contains.Item(new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))));
    }
    
    [Test]
    public void MapMidiToNotes_OnlyOneMapping_ReturnsSingleNote()
    {
        Instrument instrument = new Instrument("Test Instrument", [
            new InstrumentNoteMapping(new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(-1))),
            new InstrumentNoteMapping(new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))),
            new InstrumentNoteMapping(new MidiNoteNumber(62), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0))),
        ]);

        GuildWarsNote[] notes = instrument.MapMidiToNotes(new MidiNoteNumber(62));
        
        Assert.That(notes, Has.Length.EqualTo(1));
        Assert.That(notes[0], Is.EqualTo(new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0))));
    }
}