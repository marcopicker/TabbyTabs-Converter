using System.Collections.Generic;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Rendering;

[TestFixture]
public class TabMeasureRendererTests
{
    [Test]
    public void Render_MeasureIsEmpty_ReturnsEmptyString()
    {
        TabMeasure measure = new TabMeasure(0, TimeSignature.CommonTime, []);
        string expected = string.Empty;

        TabRendererSettings settings = new TabRendererSettings();
        TabMeasureRenderer renderer = new TabMeasureRenderer(settings);
        string output = renderer.RenderMeasure(measure);
        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void Render_SimpleTokens_0()
    {
        List<TabToken> tokens =
        [
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 1), new TabElementLength(1)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note2, 0)], new SongPosition(1, 2), new TabElementLength(1)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note3, 0)], new SongPosition(1, 3), new TabElementLength(1)),
            new RestToken(new SongPosition(1, 4), new TabElementLength(1))
        ];
        TabMeasure measure = new TabMeasure(0, TimeSignature.CommonTime, tokens);
        const string expected = "1    2    3    —";

        TabRendererSettings settings = new TabRendererSettings();
        TabMeasureRenderer renderer = new TabMeasureRenderer(settings);
        string output = renderer.RenderMeasure(measure);
        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void Render_SimpleTokens_1()
    {
        List<TabToken> tokens =
        [
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 1), new TabElementLength(1)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note2, 0)], new SongPosition(1, 2), new TabElementLength(1)),
            new RestToken(new SongPosition(1, 3), new TabElementLength(2))
        ];
        TabMeasure measure = new TabMeasure(0, TimeSignature.CommonTime, tokens);
        const string expected = "1    2    —    —";

        TabRendererSettings settings = new TabRendererSettings();
        TabMeasureRenderer renderer = new TabMeasureRenderer(settings);
        string output = renderer.RenderMeasure(measure);
        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void Render_LongNotes()
    {
        List<TabToken> tokens =
        [
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 1), new TabElementLength(2)),
            new RestToken(new SongPosition(1, 2), new TabElementLength(2))
        ];
        TabMeasure measure = new TabMeasure(0, TimeSignature.CommonTime, tokens);
        const string expected = "1    —    —    —";

        TabRendererSettings settings = new TabRendererSettings();
        TabMeasureRenderer renderer = new TabMeasureRenderer(settings);
        string output = renderer.RenderMeasure(measure);
        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void Render_FastNotes()
    {
        List<TabToken> tokens =
        [
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 1), new TabElementLength(0.5m)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 1, 0.5m), new TabElementLength(0.5m)),

            new RestToken(new SongPosition(1, 2), new TabElementLength(1)),

            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 3), new TabElementLength(0.25m)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 3, 0.25m), new TabElementLength(0.25m)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 3, 0.5m), new TabElementLength(0.25m)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 3, 0.75m), new TabElementLength(0.25m)),

            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 4), new TabElementLength(0.25m)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 4, 0.25m), new TabElementLength(0.25m)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 4, 0.5m), new TabElementLength(0.25m)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 4, 0.75m), new TabElementLength(0.25m))
        ];
        TabMeasure measure = new TabMeasure(0, TimeSignature.CommonTime, tokens);
        const string expected = "1  1    —    1 1 1 1    1 1 1 1";

        TabRendererSettings settings = new TabRendererSettings();
        TabMeasureRenderer renderer = new TabMeasureRenderer(settings);
        string output = renderer.RenderMeasure(measure);
        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void Render_LongNotesWithStartingRest()
    {
        List<TabToken> tokens =
        [
            new RestToken(new SongPosition(1, 1), new TabElementLength(2)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 3), new TabElementLength(2))
        ];
        TabMeasure measure = new TabMeasure(0, TimeSignature.CommonTime, tokens);
        const string expected = "—    —    1    —";

        TabRendererSettings settings = new TabRendererSettings();
        TabMeasureRenderer renderer = new TabMeasureRenderer(settings);
        string output = renderer.RenderMeasure(measure);
        Assert.That(output, Is.EqualTo(expected));
    }

    [TestCase(1, "1  1")]
    [TestCase(2, "—")]
    [TestCase(3, "1")]
    [TestCase(4, "—")]
    public void RenderBeat_CommonTimeMeasure_NormalLengths(int beat, string expected)
    {
        List<TabToken> tokens =
        [
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 1), new TabElementLength(0.5m)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 1, 0.5m), new TabElementLength(0.5m)),
            new RestToken(new SongPosition(1, 2), new TabElementLength(1)),
            new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, 0)], new SongPosition(1, 3), new TabElementLength(1m)),
            new RestToken(new SongPosition(1, 4), new TabElementLength(1)),
        ];
        TabMeasure measure = new TabMeasure(0, TimeSignature.CommonTime, tokens);

        TabRendererSettings settings = new TabRendererSettings();
        TabMeasureRenderer renderer = new TabMeasureRenderer(settings);
        string output = renderer.RenderBeat(measure, beat);
        Assert.That(output, Is.EqualTo(expected));
    }
}