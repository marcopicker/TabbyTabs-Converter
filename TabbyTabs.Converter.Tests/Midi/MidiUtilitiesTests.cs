using TabbyTabs.Converter.Midi;
using NUnit.Framework;

namespace TabbyTabs.Converter.Tests.Midi;

[TestFixture]
public class MidiUtilitiesTests
{
    [TestCase(4, 4, 480, 1920)]
    [TestCase(4, 4, 960, 3840)]
    [TestCase(3, 4, 480, 1440)]
    [TestCase(3, 4, 960, 2880)]
    [TestCase(4, 8, 480, 960)]
    [TestCase(4, 8, 960, 1920)]
    [TestCase(5, 8, 480, 1200)]
    [TestCase(5, 8, 960, 2400)]
    [TestCase(6, 8, 480, 1440)]
    [TestCase(6, 8, 960, 2880)]
    [TestCase(9, 8, 480, 2160)]
    [TestCase(9, 8, 960, 4320)]
    public void CalculateTicksForMeasure(int numerator, int denominator, long ticksPerQuarterBeat, long expected)
    {
        TimeSignature timeSignature = new TimeSignature(numerator, denominator);
        long actual = MidiUtilities.CalculateTicksForMeasure(timeSignature, ticksPerQuarterBeat);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(4, 4, 960, 960)]
    [TestCase(2, 4, 960, 960)]
    [TestCase(4, 8, 960, 480)]
    [TestCase(8, 8, 960, 480)]
    [TestCase(1, 2, 960, 1920)]
    [TestCase(4, 4, 480, 480)]
    [TestCase(2, 4, 480, 480)]
    [TestCase(4, 8, 480, 240)]
    [TestCase(8, 8, 480, 240)]
    [TestCase(1, 2, 480, 960)]    
    public void CalculateTicksPerBeat(int numerator, int denominator, long ticksPerQuarterBeat, long expected)
    {
        TimeSignature timeSignature = new TimeSignature(numerator, denominator);
        long actual = MidiUtilities.CalculateTicksPerBeat(timeSignature, ticksPerQuarterBeat);
        Assert.That(actual, Is.EqualTo(expected));
    }
}