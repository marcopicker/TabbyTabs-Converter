using BardicTabConverter.Midi;
using BardicTabConverter.Tab;
using NUnit.Framework;

namespace BardicTabConverter.Tests.Tab;

[TestFixture]
public class TabElementLengthTests
{
    private const int TICKS_PER_QUARTER_NOTE = 960;
    
    [TestCase(1.0, 4, 4, 1.0)]
    [TestCase(2.0, 4, 4, 2.0)]
    [TestCase(0.5, 4, 4, 0.5)]
    [TestCase(1.5, 4, 4, 1.5)]
    [TestCase(1.0, 8, 8, 2.0)]
    [TestCase(2.0, 8, 8, 4.0)]
    [TestCase(0.5, 8, 8, 1.0)]
    public void FromMidiData_VariousLengthsAndTimeSignatures(decimal quarterNotes, int numerator, int denominator, decimal expectedBeats)
    {
        long ticks = (long)(TICKS_PER_QUARTER_NOTE * quarterNotes);
        AssertFromMidiData(ticks, new TimeSignature(numerator, denominator), expectedBeats);
    }

    private static void AssertFromMidiData(long ticks, TimeSignature timeSignature, decimal expectedBeats)
    {
        MidiTimeSpan timeSpan = new MidiTimeSpan(0, ticks);
        TabElementLength length = TabElementLength.FromMidiData(timeSpan, TICKS_PER_QUARTER_NOTE, timeSignature);
        Assert.That(length.Beats, Is.EqualTo(expectedBeats));
    }

    [Test]
    public void Equals_EqualLengths()
    {
        TabElementLength length1 = new TabElementLength(1.5m);
        TabElementLength length2 = new TabElementLength(1.5m);
        bool result = length1.Equals(length2);
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void Equals_Operator()
    {
        TabElementLength length1 = new TabElementLength(1.5m);
        TabElementLength length2 = new TabElementLength(1.5m);
        bool result = length1 == length2;
        Assert.That(result, Is.True);
    }
}