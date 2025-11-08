using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Octaves;
using GW2MidiConverter.Tests.GuildWars.Instruments;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Rendering;

[TestFixture]
// [Ignore("Currently these are only or manual testing. Meow!")]
public class TabRendererTests
{
    [Test]
    public void Test1()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile("H:\\Private\\Coding\\GW2 Midi Converter\\GW2MidiConverter.Tests\\Resources\\Coraline - Exploration (GW2) Merged parts.mid");
        TabDocument tabDocument = TabDocument.FromMidiDocument(midiDocument, new LookAheadOctaveCalculator());

        tabDocument.EnableTrack(0, TestInstruments.Piano);
        // tabDocument.EnableTrack(2, Instrument.Bass);

        TabRenderer renderer = new TabRenderer(tabDocument);
        string tab = renderer.Render();
        
        // TabCsvRenderer renderer = new TabCsvRenderer(tabDocument);
        // renderer.ToCsv("OUTPUT.csv");
    }
}