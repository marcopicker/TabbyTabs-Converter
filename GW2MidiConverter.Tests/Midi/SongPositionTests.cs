using GW2MidiConverter.Midi;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Midi;

[TestFixture]
public class SongPositionTests
{
    [Test]
    public void ComparisonTests()
    {
        Assert.That(new SongPosition(1, 1, 0), Is.EqualTo(new SongPosition(1, 1, 0)));
        Assert.That(new SongPosition(2, 1, 0), Is.GreaterThan(new SongPosition(1, 0, 0)));
        Assert.That(new SongPosition(1, 2, 0), Is.GreaterThan(new SongPosition(1, 0, 0)));
        Assert.That(new SongPosition(1, 1, 1), Is.GreaterThan(new SongPosition(1, 0, 0)));
    }
}