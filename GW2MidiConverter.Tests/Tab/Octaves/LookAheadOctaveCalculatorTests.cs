using System.Collections.Generic;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.Tab.Octaves;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Tab.Octaves;

[TestFixture]
public class LookAheadOctaveCalculatorTests
{
    [Test]
    public void CalculateOctaves_PurelyAmbiguous_PrefersMiddleOctave()
    {
        List<Octave[]> input =
        [
            new[] { new Octave(0), new Octave(-1) },
            new[] { new Octave(0), new Octave(-1) },
            new[] { new Octave(0), new Octave(-1) },
        ];

        Octave[] expected =
        [
            new Octave(0),
            new Octave(0),
            new Octave(0)
        ];

        LookAheadOctaveCalculator calculator = new LookAheadOctaveCalculator();
        Octave[] output = calculator.CalculateOctaves(input);
        Assert.That(output, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void CalculateOctaves_PurelyAmbiguous_IncludesDifferentOctaves_PrefersMiddleOctave()
    {
        List<Octave[]> input =
        [
            new[] { new Octave(0), new Octave(-1) },
            new[] { new Octave(0), new Octave(1) },
            new[] { new Octave(0), new Octave(-1) },
        ];

        Octave[] expected =
        [
            new Octave(0),
            new Octave(0),
            new Octave(0)
        ];

        LookAheadOctaveCalculator calculator = new LookAheadOctaveCalculator();
        Octave[] output = calculator.CalculateOctaves(input);
        Assert.That(output, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void CalculateOctaves_PurelyDistinct_ReturnsGivenOctaves()
    {
        List<Octave[]> input =
        [
            new[] { new Octave(0) },
            new[] { new Octave(1) },
            new[] { new Octave(1) },
        ];

        Octave[] expected =
        [
            new Octave(0),
            new Octave(1),
            new Octave(1)
        ];

        LookAheadOctaveCalculator calculator = new LookAheadOctaveCalculator();
        Octave[] output = calculator.CalculateOctaves(input);
        Assert.That(output, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void CalculateOctaves_MixedOctaves()
    {
        List<Octave[]> input =
        [
            new[] { new Octave(0) },
            new[] { new Octave(0), new Octave(1) },
            new[] { new Octave(0), new Octave(1) },
            new[] { new Octave(0), new Octave(1) },
            new[] { new Octave(1) },
            new[] { new Octave(1) },
            new[] { new Octave(0) },
            new[] { new Octave(-1) },
        ];

        Octave[] expected =
        [
            new Octave(0),
            new Octave(1),
            new Octave(1),
            new Octave(1),
            new Octave(1),
            new Octave(1),
            new Octave(0),
            new Octave(-1),
        ];

        LookAheadOctaveCalculator calculator = new LookAheadOctaveCalculator();
        Octave[] output = calculator.CalculateOctaves(input);
        Assert.That(output, Is.EquivalentTo(expected));
    }
}