using System.Collections.Generic;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Rendering;

[TestFixture]
public class NoteGroupTokenRendererTests
{
    [TestCase(GuildWarsNoteKey.Note1, 0, 1, "1")]
    [TestCase(GuildWarsNoteKey.Note1, -1, 1, "[1]")]
    [TestCase(GuildWarsNoteKey.Note1, 1, 1, "(1)")]
    [TestCase(GuildWarsNoteKey.Note2Sharp, 0, 1, "#2")]
    [TestCase(GuildWarsNoteKey.Note2Sharp, -1, 1, "[#2]")]
    [TestCase(GuildWarsNoteKey.Note2Sharp, 1, 1, "(#2)")]
    public void RenderToken_SingleNotes_NoRests(GuildWarsNoteKey key, int octave, decimal length, string expected)
    {
        GuildWarsNote note = new GuildWarsNote(key, new Octave(octave));
        NoteGroupToken token = new NoteGroupToken([note], new TabElementLength(length));

        TabRendererSettings settings = new TabRendererSettings()
        {
            BracketPairsPerOctave = new Dictionary<Octave, BracketPair>
            {
                [new Octave(-1)] = new BracketPair('[', ']'),
                [new Octave(1)] = new BracketPair('(', ')'),
            },
            RestSymbolsPerBeat = new Dictionary<decimal, char>
            {
                [1.00m] = '—',
                [0.50m] = '-',
            }
        };

        NoteGroupTokenRenderer renderer = new NoteGroupTokenRenderer(settings);
        string output = renderer.RenderToken(token);
        Assert.That(output, Is.EqualTo(expected));
    }

    [TestCase(GuildWarsNoteKey.Note1, 0, 1.5, "1 -")]
    [TestCase(GuildWarsNoteKey.Note1, 0, 1.6, "1 -")]
    [TestCase(GuildWarsNoteKey.Note1, 0, 2, "1 —")]
    [TestCase(GuildWarsNoteKey.Note1, 0, 3, "1 — —")]
    public void RenderToken_SingleNotes_LongerThanOneBeat(GuildWarsNoteKey key, int octave, decimal length, string expected)
    {
        GuildWarsNote note = new GuildWarsNote(key, new Octave(octave));
        NoteGroupToken token = new NoteGroupToken([note], new TabElementLength(length));

        TabRendererSettings settings = new TabRendererSettings()
        {
            BracketPairsPerOctave = new Dictionary<Octave, BracketPair>
            {
                [new Octave(-1)] = new BracketPair('[', ']'),
                [new Octave(1)] = new BracketPair('(', ')'),
            },
            RestSymbolsPerBeat = new Dictionary<decimal, char>
            {
                [1.00m] = '—',
                [0.50m] = '-',
            }
        };

        NoteGroupTokenRenderer renderer = new NoteGroupTokenRenderer(settings);
        string output = renderer.RenderToken(token);
        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void RenderToken_Chord_NoRests()
    {
        GuildWarsNote[] notes =
        [
            new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1)),
            new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)),
            new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1)),
        ];
        NoteGroupToken token = new NoteGroupToken(notes, new TabElementLength(1));
        const string expected = "[1]/3/(5)";

        TabRendererSettings settings = new TabRendererSettings()
        {
            BracketPairsPerOctave = new Dictionary<Octave, BracketPair>
            {
                [new Octave(-1)] = new BracketPair('[', ']'),
                [new Octave(1)] = new BracketPair('(', ')'),
            },
            RestSymbolsPerBeat = new Dictionary<decimal, char>
            {
                [1.00m] = '—',
                [0.50m] = '-',
            }
        };

        NoteGroupTokenRenderer renderer = new NoteGroupTokenRenderer(settings);
        string output = renderer.RenderToken(token);
        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void RenderToken_SameOctaveChord_NoRests()
    {
        GuildWarsNote[] notes =
        [
            new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1)),
            new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(-1)),
            new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(-1)),
        ];
        NoteGroupToken token = new NoteGroupToken(notes, new TabElementLength(1));
        const string expected = "[1/3/5]";

        TabRendererSettings settings = new TabRendererSettings()
        {
            BracketPairsPerOctave = new Dictionary<Octave, BracketPair>
            {
                [new Octave(-1)] = new BracketPair('[', ']'),
                [new Octave(1)] = new BracketPair('(', ')'),
            },
            RestSymbolsPerBeat = new Dictionary<decimal, char>
            {
                [1.00m] = '—',
                [0.50m] = '-',
            }
        };

        NoteGroupTokenRenderer renderer = new NoteGroupTokenRenderer(settings);
        string output = renderer.RenderToken(token);
        Assert.That(output, Is.EqualTo(expected));
    }

    [TestCase(1.5, "[1/3/5] -")]
    [TestCase(1.6, "[1/3/5] -")]
    [TestCase(2, "[1/3/5] —")]
    [TestCase(3, "[1/3/5] — —")]
    public void RenderToken_LowerOctave135Chord_DifferentLengths_ShowsRests(decimal length, string expected)
    {
        GuildWarsNote[] notes =
        [
            new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1)),
            new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(-1)),
            new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(-1)),
        ];
        NoteGroupToken token = new NoteGroupToken(notes, new TabElementLength(length));

        TabRendererSettings settings = new TabRendererSettings()
        {
            BracketPairsPerOctave = new Dictionary<Octave, BracketPair>
            {
                [new Octave(-1)] = new BracketPair('[', ']'),
                [new Octave(1)] = new BracketPair('(', ')'),
            },
            RestSymbolsPerBeat = new Dictionary<decimal, char>
            {
                [1.00m] = '—',
                [0.50m] = '-',
            }
        };

        NoteGroupTokenRenderer renderer = new NoteGroupTokenRenderer(settings);
        string output = renderer.RenderToken(token);
        Assert.That(output, Is.EqualTo(expected));
    }
}