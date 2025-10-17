using GW2MidiConverter.Midi;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Octaves;
using GW2MidiConverter.Tests.Resources;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Tab;

[TestFixture]
public class TabDocumentTests
{
    [Test]
    public void FromMidi_ValidMidiFile_DoesNotThrow()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.ChordTestFile);
        Assert.DoesNotThrow(() => TabDocument.FromMidiDocument(midiDocument, new LookAheadOctaveCalculator()));
    }
    
    [Ignore("Currently not implemented. Very likely to get refactored.")]
    [Test]
    public void FromMidi_ValidMidiFile_CorrectTrackCount()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.ChordTestFile);
        TabDocument tabDocument = TabDocument.FromMidiDocument(midiDocument, new LookAheadOctaveCalculator());

        Assert.That(tabDocument.Tracks, Has.Length.EqualTo(1));
    }
}