using System.Diagnostics.CodeAnalysis;
using TabbyTabs.Converter.Midi;
using NUnit.Framework;

namespace TabbyTabs.Converter.Tests.Midi;

[TestFixture]
public class SongPositionTests
{
    [Test]
    public void Constructor()
    {
        SongPosition position = new SongPosition(1, 2, 0.33m);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(position.Measure, Is.EqualTo(1));
            Assert.That(position.Beat, Is.EqualTo(2));
            Assert.That(position.BeatFraction, Is.EqualTo(0.33m));   
        }
    }
    
    [Test]
    public void Constructor_CombinedBeatAndFraction()
    {
        SongPosition position = new SongPosition(1, 2.33m);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(position.Measure, Is.EqualTo(1));
            Assert.That(position.Beat, Is.EqualTo(2));
            Assert.That(position.BeatFraction, Is.EqualTo(0.33m));   
        }
    }
    
    [Test]
    [SuppressMessage("Assertion", "NUnit2009:The same value has been provided as both the actual and the expected argument", Justification = "Of course they're the same. That's what the test is about.")]
    public void IsEqualTo_IsEqual()
    {
        Assert.That(new SongPosition(1, 1, 0), Is.EqualTo(new SongPosition(1, 1, 0)));
    }
    
    [TestCase(2, 1, 0)]
    [TestCase(1, 2, 0)]
    [TestCase(1, 1, 0.5)]
    public void IsEqualTo_IsNotEqual_1_1_00(int measure, int beat, decimal beatFraction)
    {
        SongPosition testPosition = new SongPosition(measure, beat, beatFraction);
        SongPosition other = new SongPosition(1, 1, 0);
        Assert.That(testPosition, Is.Not.EqualTo(other));
    }

    [TestCase(2, 1, 0)]
    [TestCase(1, 2, 0)]
    [TestCase(1, 1, 0.5)]
    public void IsGreaterThan_Position_1_1_00(int measure, int beat, decimal beatFraction)
    {
        SongPosition testPosition = new SongPosition(measure, beat, beatFraction);
        SongPosition other = new SongPosition(1, 1, 0);
        Assert.That(testPosition, Is.GreaterThan(other));
    }
    
    [TestCase(1, 2, 0.5)]
    [TestCase(2, 1, 0.5)]
    [TestCase(2, 2, 0)]
    public void IsLessThan_Position_2_2_05(int measure, int beat, decimal beatFraction)
    {
        SongPosition testPosition = new SongPosition(measure, beat, beatFraction);
        SongPosition other = new SongPosition(2, 2, 0.5m);
        Assert.That(testPosition, Is.LessThan(other));
    }

    [TestCase(1, 1, 0, "1:1:00")]
    [TestCase(1, 3, 0, "1:3:00")]
    [TestCase(2, 1, 0, "2:1:00")]
    [TestCase(1, 1, 0.25, "1:1:25")]
    public void ToStringTests(int measure, int beat, decimal beatFraction, string expected)
    {
        SongPosition position = new SongPosition(measure, beat, beatFraction);
        string actual = position.ToString();
        Assert.That(actual, Is.EqualTo(expected));
    }
}