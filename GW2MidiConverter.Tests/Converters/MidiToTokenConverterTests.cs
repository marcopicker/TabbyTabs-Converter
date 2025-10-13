using System;
using System.Collections.Generic;
using GW2MidiConverter.Converters;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tests.Resources;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Converters;

[TestFixture]
public class MidiToTokenConverterTests
{
    [Test]
    public void ConvertTracks_DocumentIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new MidiToTokenConverter(null!));
    }

    [Test]
    public void ConvertTracks_NoIndexesSpecified_Throws()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);
        Assert.Throws<ArgumentException>(() => converter.ConvertTracks([]));
    }

    [Test]
    public void ConvertTracks_DocumentHasNoInstruments_ReturnsEmpty()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.EmptyMidiFile);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);

        Dictionary<int, MidiTrack> output = converter.ConvertTracks([1]);
        Assert.That(output, Is.Empty);
    }

    [Test]
    public void ConvertTracks_TwoTracks_IndexesOutOfRange_Throws()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);
        Assert.Throws<ArgumentOutOfRangeException>(() => converter.ConvertTracks([-1]));
    }

    [Test]
    public void ConvertTracks_IndexIsNotMusicTrack_SkipsIndex()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);

        Dictionary<int, MidiTrack> result = converter.ConvertTracks([0, 1, 2]);

        Assert.That(result.Keys, Has.Count.EqualTo(2));
        Assert.That(result.Keys, Has.No.Member(0));
        Assert.That(result.Keys, Has.Member(1).And.Member(2));
    }

    [Test]
    public void ConvertTracks_TwoTracks_ReturnsTwoTracks()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);

        Dictionary<int, MidiTrack> result = converter.ConvertTracks([1, 2]);

        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public void ConvertTrack_TrackInfoIsCorrect()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.LengthTestFile);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);

        Dictionary<int, MidiTrack> result = converter.ConvertTracks([1]);
        MidiTrack track = result[1];

        using (Assert.EnterMultipleScope())
        {
            Assert.That(track, Is.Not.Null);
            Assert.That(track.Length, Is.EqualTo(4));
            Assert.That(track.Info.TrackIndex, Is.EqualTo(1));
            Assert.That(track.Info.InstrumentName, Is.Empty);
        }
    }

    [Test]
    public void ConvertTrack_MeasuresAreCorrect()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.LengthTestFile);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);

        Dictionary<int, MidiTrack> result = converter.ConvertTracks([1]);
        MidiTrack track = result[1];

        using (Assert.EnterMultipleScope())
        {
            Assert.That(track.Measures[0].MeasureInfo.TimeSignature, Is.EqualTo(new TimeSignature(4, 4)));
            Assert.That(track.Measures[0].NoteGroups[0].Notes, Has.Length.EqualTo(1));
            Assert.That(track.Measures[0].NoteGroups[0].Notes, Has.Member(new MidiNoteNumber(60)));
            Assert.That(track.Measures[0].NoteGroups[0].Position, Is.EqualTo(new SongPosition(1, 1, 0)));
            Assert.That(track.Measures[0].NoteGroups[1].Notes, Has.Length.EqualTo(1));
            Assert.That(track.Measures[0].NoteGroups[1].Notes, Has.Member(new MidiNoteNumber(62)));
            Assert.That(track.Measures[0].NoteGroups[2].Notes, Has.Length.EqualTo(1));
            Assert.That(track.Measures[0].NoteGroups[2].Notes, Has.Member(new MidiNoteNumber(64)));
            Assert.That(track.Measures[0].NoteGroups[3].Notes, Has.Length.EqualTo(1));
            Assert.That(track.Measures[0].NoteGroups[3].Notes, Has.Member(new MidiNoteNumber(67)));

            Assert.That(track.Measures[1].MeasureInfo.TimeSignature, Is.EqualTo(new TimeSignature(3, 4)));
            Assert.That(track.Measures[1].NoteGroups[0].Notes, Has.Member(new MidiNoteNumber(60)));
            Assert.That(track.Measures[1].NoteGroups[1].Notes, Has.Member(new MidiNoteNumber(62)));

            Assert.That(track.Measures[2].MeasureInfo.TimeSignature, Is.EqualTo(new TimeSignature(3, 4)));
            Assert.That(track.Measures[2].NoteGroups[0].Notes, Has.Member(new MidiNoteNumber(60)));

            Assert.That(track.Measures[3].MeasureInfo.TimeSignature, Is.EqualTo(new TimeSignature(4, 4)));
            Assert.That(track.Measures[3].NoteGroups[0].Notes, Has.Member(new MidiNoteNumber(60)));
        }
    }
}