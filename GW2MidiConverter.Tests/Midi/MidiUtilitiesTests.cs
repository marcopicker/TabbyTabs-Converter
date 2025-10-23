using GW2MidiConverter.Midi;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Midi;

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
}