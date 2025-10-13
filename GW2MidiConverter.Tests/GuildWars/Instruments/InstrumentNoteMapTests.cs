using System;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.GuildWars.Instruments;

[TestFixture]
public class InstrumentNoteMapTests
{
    [Test]
    public void Constructor_MapsIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new InstrumentNoteMap(null!));
    }

    [Test]
    public void Constructor_MapsIsEmpty_Throws()
    {
        Assert.Throws<ArgumentException>(() => _ = new InstrumentNoteMap([]));
    }

    [Test]
    public void IsNoteMapped_CorrectReturnValues()
    {
        InstrumentNoteMap map = new InstrumentNoteMap(InstrumentNoteMaps.Piano);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(map.IsNoteMapped(new MidiNoteNumber(60)), Is.True);
            Assert.That(map.IsNoteMapped(new MidiNoteNumber(61)), Is.True);
            Assert.That(map.IsNoteMapped(new MidiNoteNumber(72)), Is.True);

            Assert.That(map.IsNoteMapped(new MidiNoteNumber(0)), Is.False);
            Assert.That(map.IsNoteMapped(new MidiNoteNumber(127)), Is.False);
        }
    }

    [Test]
    public void MapMidiToNotes_UnmappedNote_Throws()
    {
        InstrumentNoteMap map = new InstrumentNoteMap(InstrumentNoteMaps.Piano);

        using (Assert.EnterMultipleScope())
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => map.MapMidiToNotes(new MidiNoteNumber(0)));
            Assert.Throws<ArgumentOutOfRangeException>(() => map.MapMidiToNotes(new MidiNoteNumber(127)));
        }
    }

    [Test]
    public void MapMidiToNotes_SingleMappedNote_ReturnsKey()
    {
        InstrumentNoteMap map = new InstrumentNoteMap(InstrumentNoteMaps.Piano);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(map.MapMidiToNotes(new MidiNoteNumber(65)), Has.Length.EqualTo(1));

            GuildWarsNote note = new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(0));
            Assert.That(map.MapMidiToNotes(new MidiNoteNumber(65))[0], Is.EqualTo(note));
        }
    }

    [Test]
    public void MapMidiToNotes_DoubleMappedNote_ReturnsTwoKeys()
    {
        InstrumentNoteMap map = new InstrumentNoteMap(InstrumentNoteMaps.Piano);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(map.MapMidiToNotes(new MidiNoteNumber(72)), Has.Length.EqualTo(2));

            GuildWarsNote lowerNote = new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0));
            GuildWarsNote upperNote = new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1));

            Assert.That(map.MapMidiToNotes(new MidiNoteNumber(72)), Has.Member(lowerNote));
            Assert.That(map.MapMidiToNotes(new MidiNoteNumber(72)), Has.Member(upperNote));
        }
    }
}