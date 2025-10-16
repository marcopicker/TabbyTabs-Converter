using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tests.Resources;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Tab;

[TestFixture]
public class TabRendererTests
{
    [Test]
    public void Test1()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.ChordTestFile);
        TabDocument tabDocument = TabDocument.FromMidiDocument(midiDocument);
        tabDocument.EnableTrack(1, new Instrument("Piano", new InstrumentNoteMap(InstrumentNoteMaps.Piano)));
        
        TabRenderer renderer = new TabRenderer(tabDocument);
        string tab = renderer.Render();
    }
}