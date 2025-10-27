using System.Collections.Generic;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Rendering.Settings;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Rendering;

[TestFixture]
public class NoteRendererTests
{
    [TestCase(GuildWarsNoteKey.Note1, 0, "1")]
    [TestCase(GuildWarsNoteKey.Note1, -1, "[1]")]
    [TestCase(GuildWarsNoteKey.Note1, 1, "(1)")]
    [TestCase(GuildWarsNoteKey.Note2Sharp, 0, "#2")]
    [TestCase(GuildWarsNoteKey.Note2Sharp, -1, "[#2]")]
    [TestCase(GuildWarsNoteKey.Note2Sharp, 1, "(#2)")]
    public void RenderToken_SingleNotes_DefaultSettings_ValidInputs(GuildWarsNoteKey key, int octave, string expected)
    {
        NoteRenderingSettings settings = new NoteRenderingSettings()
        {
            BracketPairsPerOctave = new Dictionary<Octave, BracketPair>
            {
                [new Octave(-1)] = new BracketPair('[', ']'),
                [new Octave(1)] = new BracketPair('(', ')'),
            }
        };
        
        GuildWarsNote note = new GuildWarsNote(key, new Octave(octave));
        
        NoteRenderer renderer = new NoteRenderer(settings);
        string output = renderer.RenderNotes([note]);
        Assert.That(output, Is.EqualTo(expected));
    }
    
    [Test]
    public void RenderToken_Chord_DefaultSettings_ValidInput()
    {
        GuildWarsNote[] input = [
            new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0)),
            new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)),
            new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0)),
        ];
        const string expected = "1/3/5";
        
        NoteRenderingSettings settings = new NoteRenderingSettings()
        {
            BracketPairsPerOctave = new Dictionary<Octave, BracketPair>
            {
                [new Octave(-1)] = new BracketPair('[', ']'),
                [new Octave(1)] = new BracketPair('(', ')'),
            }
        };
        
        NoteRenderer renderer = new NoteRenderer(settings);
        string output = renderer.RenderNotes(input);
        Assert.That(output, Is.EqualTo(expected));
    }
    
    [Test]
    public void RenderToken_MultiOctaveChord_DefaultSettings_ValidInput()
    {
        GuildWarsNote[] input = [
            new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1)),
            new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)),
            new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1)),
        ];
        const string expected = "[1]/3/(5)";
        
        NoteRenderingSettings settings = new NoteRenderingSettings()
        {
            BracketPairsPerOctave = new Dictionary<Octave, BracketPair>
            {
                [new Octave(-1)] = new BracketPair('[', ']'),
                [new Octave(1)] = new BracketPair('(', ')'),
            }
        };
        
        NoteRenderer renderer = new NoteRenderer(settings);
        string output = renderer.RenderNotes(input);
        Assert.That(output, Is.EqualTo(expected));
    }  
    
    [Test]
    public void RenderToken_UpperOctaveChord_DefaultSettings_ValidInput()
    {
        GuildWarsNote[] input = [
            new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1)),
            new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(1)),
            new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1)),
        ];
        const string expected = "(1/3/5)";
        
        NoteRenderingSettings settings = new NoteRenderingSettings()
        {
            BracketPairsPerOctave = new Dictionary<Octave, BracketPair>
            {
                [new Octave(-1)] = new BracketPair('[', ']'),
                [new Octave(1)] = new BracketPair('(', ')'),
            }
        };
        
        NoteRenderer renderer = new NoteRenderer(settings);
        string output = renderer.RenderNotes(input);
        Assert.That(output, Is.EqualTo(expected));
    }  
}