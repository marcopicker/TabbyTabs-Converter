using BardicTabConverter.General;
using BardicTabConverter.GuildWars;
using BardicTabConverter.Midi;
using BardicTabConverter.Rendering;
using BardicTabConverter.Rendering.Settings;
using BardicTabConverter.Tab;
using BardicTabConverter.Tab.Tokens;
using NUnit.Framework;

namespace BardicTabConverter.Tests.Rendering;

[TestFixture]
public class TabTrackRendererTests
{
    [Test]
    public void Render_DefaultSettings()
    {
        TabTrack track = new TabTrack(new TrackInfo(1, "Piano"), [
            new TabMeasure(1, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1))], new SongPosition(1, 1), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1))], new SongPosition(1, 2), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1))], new SongPosition(1, 3), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1))], new SongPosition(1, 4), new TabElementLength(1)),
            ]),
            new TabMeasure(2, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(2, 1), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0))], new SongPosition(2, 2), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0))], new SongPosition(2, 3), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0))], new SongPosition(2, 4), new TabElementLength(1)),
            ]),
            new TabMeasure(3, new TimeSignature(3, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(3, 1), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(3, 2), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(3, 3), new TabElementLength(1)),
            ]),
            new TabMeasure(4, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(4, 1), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0))], new SongPosition(4, 2), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0))], new SongPosition(4, 3), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0))], new SongPosition(4, 4), new TabElementLength(1)),
            ]),
            new TabMeasure(5, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(5, 1), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(5, 2), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(5, 3), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(5, 4), new TabElementLength(1)),
            ]),
            new TabMeasure(6, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(6, 1), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0))], new SongPosition(6, 2), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0))], new SongPosition(6, 3), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0))], new SongPosition(6, 4), new TabElementLength(1)),
            ]),
            new TabMeasure(7, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(7, 1), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(7, 2), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(7, 3), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(7, 4), new TabElementLength(1)),
            ]),
            new TabMeasure(8, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new SongPosition(8, 1), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0))], new SongPosition(8, 2), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0))], new SongPosition(8, 3), new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0))], new SongPosition(8, 4), new TabElementLength(1)),
            ]),            
        ]);

        TabTrackRenderer renderer = new TabTrackRenderer(new TrackRenderingSettings(), new MeasureRenderingSettings(), new NoteRenderingSettings(), new TimeSignatureRenderingSettings());
        string output = renderer.Render(track);
    }
}