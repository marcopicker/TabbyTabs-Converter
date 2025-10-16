using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;
using GW2MidiConverter.Tests.GuildWars.Instruments;
using GW2MidiConverter.Tests.Resources;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Tab;

[TestFixture]
public class MidiDocumentConverterTests
{
    [Test]
    public void FromMidi_NullMidiDocument_Throws()
    {
        Assert.That(() => _ = new MidiDocumentConverter(null!), Throws.ArgumentNullException);
    }
    
    [Test]
    public void FromMidi_ValidMidiFile()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.ChordTestFile);
        MidiDocumentConverter converter = new MidiDocumentConverter(midiDocument);

        InstrumentNoteMap pianoMap = new InstrumentNoteMap(InstrumentNoteMaps.Piano);
        TabTrack track = converter.ConvertTrack(1, pianoMap);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(track, Is.Not.Null);
            Assert.That(track.Measures, Has.Length.EqualTo(4));

            Assert.That(track.Measures[0].Tokens, Has.Length.EqualTo(4));
            Assert.That(track.Measures[0].Tokens[0], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[0].Tokens[0]).Length.Beats, Is.EqualTo(1));
            Assert.That(track.Measures[0].Tokens[1], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[0].Tokens[1]).Length.Beats, Is.EqualTo(1));
            Assert.That(track.Measures[0].Tokens[2], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[0].Tokens[2]).Length.Beats, Is.EqualTo(1));
            Assert.That(track.Measures[0].Tokens[3], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[0].Tokens[3]).Length.Beats, Is.EqualTo(1));
            
            Assert.That(track.Measures[1].Tokens, Has.Length.EqualTo(3));
            Assert.That(track.Measures[1].Tokens[0], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[1].Tokens[0]).Length.Beats, Is.EqualTo(1.5));
            Assert.That(track.Measures[1].Tokens[1], Is.TypeOf<RestToken>());
            Assert.That(((RestToken) track.Measures[1].Tokens[1]).Length.Beats, Is.EqualTo(1));
            
            Assert.That(track.Measures[2].Tokens, Has.Length.EqualTo(2));
            
            Assert.That(track.Measures[3].Tokens, Has.Length.EqualTo(3));
            Assert.That(track.Measures[3].Tokens[0], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[3].Tokens[0]).Length.Beats, Is.EqualTo(2));
            Assert.That(((NoteGroupToken) track.Measures[3].Tokens[0]).Notes, Has.Length.EqualTo(3));
        }
    }
}