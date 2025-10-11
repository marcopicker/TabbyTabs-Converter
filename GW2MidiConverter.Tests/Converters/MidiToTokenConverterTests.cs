using System;
using System.Collections.Generic;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tests.Resources;
using GW2MidiConverter.Tokens;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

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
        Assert.Throws<ArgumentException>(() => converter.ConvertTracks());
    }
    
    [Test]
    public void ConvertTracks_DocumentHasNoInstruments_ReturnsEmpty()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.EmptyMidiFile);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);
        
        Dictionary<int, List<Token>> output = converter.ConvertTracks(1);
        Assert.That(output, Is.Empty);
    }
    
    [Test]
    public void ConvertTracks_TwoTracks_IndexesOutOfRange_Ignores()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);
        Assert.DoesNotThrow(() => converter.ConvertTracks(-1));
    }
    
    [Test]
    public void ConvertTracks_IndexIsNotMusicTrack_SkipsIndex()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);
        
        Dictionary<int, List<Token>> result = converter.ConvertTracks(0, 1, 2);
        
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Keys, Has.No.Member(0));
        Assert.That(result.Keys, Has.Member(1).And.Member(2));
    }
    
    [Test]
    public void ConvertTracks_TwoTracks_ReturnsTwoTracks()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.SimpleTwoTrackPiecePianoBass);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);
        
        Dictionary<int, List<Token>> result = converter.ConvertTracks(1, 2);
        
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public void ConvertTrack_TokenTypeTest()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.LengthTestFile);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);
        
        Dictionary<int, List<Token>> result = converter.ConvertTracks(1);
        List<Token> tokens = result[1];
        Assert.That(tokens, Is.Not.Null);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(tokens[0], Is.TypeOf<MeasureToken>());
            
            Assert.That(tokens[1], Is.TypeOf<NoteToken>());
            Assert.That(tokens[2], Is.TypeOf<NoteToken>());
            Assert.That(tokens[3], Is.TypeOf<NoteToken>());
            Assert.That(tokens[4], Is.TypeOf<NoteToken>());
            
            Assert.That(tokens[5], Is.TypeOf<MeasureToken>());
            
            Assert.That(tokens[6], Is.TypeOf<NoteToken>());
            Assert.That(tokens[7], Is.TypeOf<RestToken>());
            Assert.That(tokens[8], Is.TypeOf<NoteToken>());
            
            Assert.That(tokens[9], Is.TypeOf<MeasureToken>());
            
            Assert.That(tokens[10], Is.TypeOf<NoteToken>());
            Assert.That(tokens[11], Is.TypeOf<RestToken>());
            
            Assert.That(tokens[12], Is.TypeOf<MeasureToken>());
            
            Assert.That(tokens[13], Is.TypeOf<NoteToken>());
            
            Assert.That(tokens[14], Is.TypeOf<MeasureToken>());
        }
    }
    
    [Test]
    public void ConvertTrack_NoGapsBetweenTokens()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.LengthTestFile);
        MidiToTokenConverter converter = new MidiToTokenConverter(midiDocument);
        
        Dictionary<int, List<Token>> result = converter.ConvertTracks(1);
        List<Token> tokens = result[1];
        
        for (int i = 0; i < tokens.Count - 1; i++)
        {
            Token token = tokens[i];
            Token nextToken = tokens[i + 1];
            Assert.That(token.EndTime, Is.EqualTo(nextToken.StartTime));
        }
    }
}