using System.Collections.Generic;
using TabbyTabs.Converter.Rendering;
using NUnit.Framework;

namespace TabbyTabs.Converter.Tests.Rendering;

[TestFixture]
public class BracketSimplifierTests
{
    [Test]
    public void Constructor_BracketPairsIsNull_Throws()
    {
        Assert.That(() => _ = new BracketSimplifier(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void Constructor_NoBracketPairsSpecified_DoesNotThrow()
    {
        Assert.That(() => _ = new BracketSimplifier([]), Throws.Nothing);
    }

    [TestCase("", "")]
    [TestCase("  ", "  ")]
    [TestCase("1 2 3", "1 2 3")]
    [TestCase("1 2 3", "1 2 3")]
    [TestCase("[1 2 3]", "[1 2 3]")]
    [TestCase("(1 2 3)", "(1 2 3)")]
    [TestCase("[1] [2 3]", "[1 2 3]")]
    [TestCase("[1] [2] [3]", "[1 2 3]")]
    [TestCase("[1] [#2] [3]", "[1 #2 3]")]
    [TestCase("[1] - [3]", "[1 - 3]")]
    [TestCase("[1]/[2]/[3]", "[1/2/3]")]
    [TestCase("(1) (2 3)", "(1 2 3)")]
    [TestCase("[1] (2) [3] [4] 5 [6]", "[1] (2) [3 4] 5 [6]")]
    [TestCase("[1]/[2]/2/(3)", "[1/2]/2/(3)")]
    public void Simplify_StandardGuildWars2Brackets(string input, string expected)
    {
        List<BracketPair> bracketPairs =
        [
            new BracketPair('(', ')'),
            new BracketPair('[', ']')
        ];
        BracketSimplifier bracketSimplifier = new BracketSimplifier(bracketPairs);
        string output = bracketSimplifier.Simplify(input);
        Assert.That(output, Is.EqualTo(expected));
    }
}