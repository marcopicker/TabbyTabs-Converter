using System.Collections.Generic;
using GW2MidiConverter.Converters;
using GW2MidiConverter.Converters.Tokens;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tests.Resources;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Converters;

[TestFixture]
public class TokenToTabConverterTests
{
    [Test]
    public void Test1()
    {
        MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(MidiFiles.LengthTestFile);
        MidiToTokenConverter tokenConverter = new MidiToTokenConverter(midiDocument);
        TokenToTabConverter tabConverter = new TokenToTabConverter();
        
        Dictionary<int, List<Token>> tracks = tokenConverter.ConvertTracks([1]);
        List<Token> track = tracks[1];
        string tab = tabConverter.Convert(track);
    }
}