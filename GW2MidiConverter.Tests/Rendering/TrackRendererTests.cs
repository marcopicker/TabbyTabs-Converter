using GW2MidiConverter.General;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Rendering;

[TestFixture]
public class TrackRendererTests
{
    [Test]
    public void Render_DefaultSettings()
    {
        TabTrack track = new TabTrack(new TrackInfo(1, "Piano"), [
            new TabMeasure(1, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1))], new TabElementLength(1)),
            ]),
            new TabMeasure(2, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0))], new TabElementLength(1)),
            ]),
            new TabMeasure(1, new TimeSignature(3, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                // new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
            ]),
            new TabMeasure(2, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0))], new TabElementLength(1)),
            ]),
            new TabMeasure(1, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
            ]),
            new TabMeasure(2, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0))], new TabElementLength(1)),
            ]),
            new TabMeasure(1, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
            ]),
            new TabMeasure(2, new TimeSignature(4, 4), [
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0))], new TabElementLength(1)),
                new NoteGroupToken([new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0))], new TabElementLength(1)),
            ]),            
        ]);

        TrackRenderer renderer = new TrackRenderer(new TabRendererSettings());
        string output = renderer.Render(track);
    }
}