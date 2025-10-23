using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Octaves;
using GW2MidiConverter.Tests.Resources;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Rendering;

[TestFixture]
// [Ignore("Currently these are only or manual testing. Meow!")]
public class TabRendererTests
{
    [Test]
    public void Test1()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile("""C:\Users\pocra\Downloads\Corpse Bride - The Piano Duet (GW2) Duet.mid""");
        TabDocument tabDocument = TabDocument.FromMidiDocument(midiDocument, new LookAheadOctaveCalculator());

        tabDocument.EnableTrack(0, Instrument.Piano);
        tabDocument.EnableTrack(1, Instrument.Piano);
        // tabDocument.EnableTrack(2, Instrument.Bass);

        TabRenderer renderer = new TabRenderer(tabDocument);
        string tab = renderer.Render();
        
        // TabCsvRenderer renderer = new TabCsvRenderer(tabDocument);
        // renderer.ToCsv("OUTPUT.csv");
    }
}