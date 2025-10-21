using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Octaves;
using GW2MidiConverter.Tab.Tokens;
using GW2MidiConverter.Tests.Resources;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Tab;

[TestFixture]
public class TrackConverterTests
{
    [Test]
    public void Convert_ValidResults()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.ChordTestFile);
        MidiTrack midiTrack = midiDocument.GetTrack(1);
        
        TrackConverter trackConverter = new TrackConverter(midiTrack, midiDocument.TimingMap);
        TabTrack track = trackConverter.Convert(Instrument.Piano, new LookAheadOctaveCalculator());
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(track, Is.Not.Null);
            Assert.That(track.Measures, Has.Count.EqualTo(4));

            Assert.That(track.Measures[0].Tokens, Has.Count.EqualTo(4));
            Assert.That(track.Measures[0].Tokens[0], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[0].Tokens[0]).Length.Beats, Is.EqualTo(1));
            Assert.That(track.Measures[0].Tokens[1], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[0].Tokens[1]).Length.Beats, Is.EqualTo(1));
            Assert.That(track.Measures[0].Tokens[2], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[0].Tokens[2]).Length.Beats, Is.EqualTo(1));
            Assert.That(track.Measures[0].Tokens[3], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[0].Tokens[3]).Length.Beats, Is.EqualTo(1));
            
            Assert.That(track.Measures[1].Tokens, Has.Count.EqualTo(3));
            Assert.That(track.Measures[1].Tokens[0], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[1].Tokens[0]).Length.Beats, Is.EqualTo(1.5));
            Assert.That(track.Measures[1].Tokens[1], Is.TypeOf<RestToken>());
            Assert.That(((RestToken) track.Measures[1].Tokens[1]).Length.Beats, Is.EqualTo(1));
            
            Assert.That(track.Measures[2].Tokens, Has.Count.EqualTo(2));
            
            Assert.That(track.Measures[3].Tokens, Has.Count.EqualTo(3));
            Assert.That(track.Measures[3].Tokens[0], Is.TypeOf<NoteGroupToken>());
            Assert.That(((NoteGroupToken) track.Measures[3].Tokens[0]).Length.Beats, Is.EqualTo(2));
            Assert.That(((NoteGroupToken) track.Measures[3].Tokens[0]).Notes, Has.Count.EqualTo(3));
        }
    }
}