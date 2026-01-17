using BardicTabConverter.Midi;
using BardicTabConverter.Tab;
using BardicTabConverter.Tab.Octaves;
using BardicTabConverter.Tests.GuildWars.Instruments;
using BardicTabConverter.Tests.Resources;
using NUnit.Framework;

namespace BardicTabConverter.Tests.Tab;

[TestFixture]
public class TabDocumentTests
{
    [Test]
    public void FromMidi_ValidMidiFile_DoesNotThrow()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.ChordTestFile);
        Assert.DoesNotThrow(() => TabDocument.FromMidiDocument(midiDocument, new LookAheadOctaveCalculator()));
    }
    
    [Test]
    public void FromMidi_ValidMidiFile_CorrectTrackCount()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.ChordTestFile);
        TabDocument tabDocument = TabDocument.FromMidiDocument(midiDocument, new LookAheadOctaveCalculator());

        tabDocument.EnableTrack(1, TestInstruments.Piano);
        
        Assert.That(tabDocument.Tracks, Has.Count.EqualTo(1));
    }
}