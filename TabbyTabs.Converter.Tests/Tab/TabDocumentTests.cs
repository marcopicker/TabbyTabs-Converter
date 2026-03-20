using TabbyTabs.Converter.Midi;
using TabbyTabs.Converter.Tab;
using TabbyTabs.Converter.Tab.Octaves;
using TabbyTabs.Converter.Tests.GuildWars.Instruments;
using TabbyTabs.Converter.Tests.Resources;
using NUnit.Framework;

namespace TabbyTabs.Converter.Tests.Tab;

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