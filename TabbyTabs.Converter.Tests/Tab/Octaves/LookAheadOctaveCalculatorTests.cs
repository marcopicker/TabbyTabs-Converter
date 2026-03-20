using System.Collections.Generic;
using TabbyTabs.Converter.GuildWars;
using TabbyTabs.Converter.Tab.Octaves;
using NUnit.Framework;

namespace TabbyTabs.Converter.Tests.Tab.Octaves;

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

        IList<Octave> expected =
        [
            new Octave(0),
            new Octave(0),
            new Octave(0)
        ];

        LookAheadOctaveCalculator calculator = new LookAheadOctaveCalculator();
        IReadOnlyList<Octave> output = calculator.CalculateOctaves(input);
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

        IList<Octave> expected =
        [
            new Octave(0),
            new Octave(0),
            new Octave(0)
        ];

        LookAheadOctaveCalculator calculator = new LookAheadOctaveCalculator();
        IReadOnlyList<Octave> output = calculator.CalculateOctaves(input);
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

        IList<Octave> expected =
        [
            new Octave(0),
            new Octave(1),
            new Octave(1)
        ];

        LookAheadOctaveCalculator calculator = new LookAheadOctaveCalculator();
        IReadOnlyList<Octave> output = calculator.CalculateOctaves(input);
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
            new[] { new Octave(1), new Octave(0) },
            new[] { new Octave(0) },
            new[] { new Octave(-1) },
        ];

        IList<Octave> expected =
        [
            new Octave(0),
            new Octave(1),
            new Octave(1),
            new Octave(1),
            new Octave(1),
            new Octave(0),
            new Octave(0),
            new Octave(-1),
        ];

        LookAheadOctaveCalculator calculator = new LookAheadOctaveCalculator();
        IReadOnlyList<Octave> output = calculator.CalculateOctaves(input);
        Assert.That(output, Is.EquivalentTo(expected));
    }
}