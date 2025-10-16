using GW2MidiConverter.Midi;
using GW2MidiConverter.Tab;
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
        Assert.DoesNotThrow(() => TabDocument.FromMidi(midiDocument));
    }
    
    [Ignore("Currently not implemented. Very likely to get refactored.")]
    [Test]
    public void FromMidi_ValidMidiFile_CorrectTrackCount()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.ChordTestFile);
        TabDocument tabDocument = TabDocument.FromMidi(midiDocument);

        Assert.That(tabDocument.Tracks, Has.Length.EqualTo(1));
    }
}