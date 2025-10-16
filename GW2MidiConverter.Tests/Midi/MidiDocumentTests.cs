using System;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Shared;
using GW2MidiConverter.Tests.Resources;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Midi;

[TestFixture]
public class MidiDocumentTests
{
    #region Loading

    [Test]
    public void LoadFromMidiFile_ValidFile_ReturnsDocument()
    {
        Assert.DoesNotThrow(() => MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass));
    }

    [Test]
    public void LoadFromMidiFile_NotAMidiFile_Throws()
    {
        Assert.Throws<FormatException>(() => MidiDocument.LoadFromMidiFile(MidiFiles.NotAMidiFile));
    }

    [Test]
    public void LoadFromMidiFile_NullFile_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => MidiDocument.LoadFromMidiFile(null!));
    }

    [Test]
    public void LoadFromMidiFile_PathEmpty_Throws()
    {
        Assert.Throws<ArgumentException>(() => MidiDocument.LoadFromMidiFile(string.Empty));
    }

    #endregion

    #region Track Info

    [Test]
    public void GetInstrumentCount_TwoInstrumentMidiFile_ReturnsCorrectNumber()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        Assert.That(midiDocument.InstrumentCount, Is.EqualTo(2));
    }

    [Test]
    public void GetInstrumentCount_EmptyMidiFile_ReturnsZero()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.EmptyMidiFile);
        Assert.That(midiDocument.InstrumentCount, Is.EqualTo(0));
    }

    [Test]
    public void InstrumentTrackIndices_TwoInstrumentMidiFile_ReturnsCorrectIndexes()
    {
        // The track at index 0 of this file is used purely for meta-events.
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        Assert.That(midiDocument.InstrumentTrackIndexes, Has.Member(1).And.Member(2));
    }

    [Test]
    public void GetTrackInfo_TwoInstrumentMidiFile_ValidIndex_DoesNotThrow()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        Assert.DoesNotThrow(() => midiDocument.GetTrackInfo(1));
        Assert.DoesNotThrow(() => midiDocument.GetTrackInfo(2));
    }

    [Test]
    public void GetTrackInfo_TwoInstrumentMidiFile_InvalidIndex_Throws()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        Assert.Throws<ArgumentOutOfRangeException>(() => midiDocument.GetTrackInfo(0)); // Meta-event track.
        Assert.Throws<ArgumentOutOfRangeException>(() => midiDocument.GetTrackInfo(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => midiDocument.GetTrackInfo(3));
    }

    [Test]
    public void GetTrackInfo_TwoInstrumentMidiFile_HasCorrectInfo()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        TrackInfo trackInfo = midiDocument.GetTrackInfo(1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(trackInfo.TrackIndex, Is.EqualTo(1));
            Assert.That(trackInfo.InstrumentName, Is.EqualTo("Piano"));
        }
    }

    #endregion
}