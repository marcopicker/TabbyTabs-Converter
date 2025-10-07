using System;
using System.Collections.Generic;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tests.Resources;
using GW2MidiConverter.Tokens;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Converters;

[TestFixture]
public class MidiToTokenConverterTests
{
    [Test]
    public void ConvertTracks_DocumentIsNull_Throws()
    {
        MidiToTokenConverter converter = new MidiToTokenConverter();
        Assert.Throws<ArgumentNullException>(() => converter.ConvertTracks(null!, 1, 2, 3));
    }
    
    [Test]
    public void ConvertTracks_NoIndexesSpecified_Throws()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        MidiToTokenConverter converter = new MidiToTokenConverter();
        Assert.Throws<ArgumentException>(() => converter.ConvertTracks(midiDocument));
    }
    
    [Test]
    public void ConvertTracks_DocumentHasNoInstruments_Throws()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.EmptyMidiFile);
        MidiToTokenConverter converter = new MidiToTokenConverter();
        Assert.Throws<InvalidMidiDocumentException>(() => converter.ConvertTracks(midiDocument, 1));
    }
    
    [Test]
    public void ConvertTracks_TwoTracks_IndexesOutOfRange_Throws()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        MidiToTokenConverter converter = new MidiToTokenConverter();
        
        Assert.Throws<IndexOutOfRangeException>(() => converter.ConvertTracks(midiDocument, -1));
    }
    
    [Test]
    public void ConvertTracks_IndexIsNotMusicTrack_SkipsIndex()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        MidiToTokenConverter converter = new MidiToTokenConverter();
        
        Dictionary<int, List<IToken>> result = converter.ConvertTracks(midiDocument, 0, 1, 2);
        
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Keys, Has.No.Member(0));
        Assert.That(result.Keys, Has.Member(1).And.Member(2));
    }
    
    [Test]
    public void ConvertTracks_TwoTracks_ReturnsTwoTracks()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        MidiToTokenConverter converter = new MidiToTokenConverter();
        
        Dictionary<int, List<IToken>> result = converter.ConvertTracks(midiDocument, 1, 2);
        
        Assert.That(result, Has.Count.EqualTo(2));
    }
}