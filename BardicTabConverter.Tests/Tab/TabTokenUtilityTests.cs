using System.Collections.Generic;
using BardicTabConverter.Midi;
using BardicTabConverter.Tab;
using BardicTabConverter.Tab.Tokens;
using BardicTabConverter.GuildWars;
using NUnit.Framework;

namespace BardicTabConverter.Tests.Tab;

[TestFixture]
public class TabTokenUtilityTests
{
    private record TestToken(SongPosition Position, TabElementLength Length) : TabToken(Position, Length);

    [Test]
    public void TryCutTokenToFitBeat_TokenDoesNotTouchBeat()
    {
        TestToken input = new TestToken(new SongPosition(1, 1), new TabElementLength(1));
        bool result = TabTokenUtility.TryCutTokenToFitBeat(input, 4, out TestToken? cutToken);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.False);
            Assert.That(cutToken, Is.Null);
        }
    }

    [Test]
    public void TryCutTokenToFitBeat_EntireTokenIsInBeat_OutValueEqualToInput()
    {
        TestToken input = new TestToken(new SongPosition(1, 1), new TabElementLength(1));
        bool result = TabTokenUtility.TryCutTokenToFitBeat(input, 1, out TestToken? cutToken);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.True);
            Assert.That(cutToken, Is.EqualTo(input));
        }
    }
    
    [Test]
    public void TryCutTokenToFitBeat_TokenSpillIntoBeat()
    {
        TestToken input = new TestToken(new SongPosition(1, 1), new TabElementLength(1.5m));
        bool result = TabTokenUtility.TryCutTokenToFitBeat(input, 2, out TestToken? cutToken);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.True);
            Assert.That(cutToken, Is.Not.Null);
            Assert.That(cutToken.Position, Is.EqualTo(new SongPosition(1, 2)));
            Assert.That(cutToken.Length, Is.EqualTo(new TabElementLength(0.5m)));
        }
    }
    
    [Test]
    public void TryCutTokenToFitBeat_TokenSpansAcrossIntoBeat()
    {
        TestToken input = new TestToken(new SongPosition(1, 1), new TabElementLength(3));
        bool result = TabTokenUtility.TryCutTokenToFitBeat(input, 2, out TestToken? cutToken);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.True);
            Assert.That(cutToken, Is.Not.Null);
            Assert.That(cutToken.Position, Is.EqualTo(new SongPosition(1, 2)));
            Assert.That(cutToken.Length, Is.EqualTo(new TabElementLength(1)));
        }
    }
    
    [Test]
    public void TryCutTokenToFitBeat_TokenFlowsOutOfBeat()
    {
        TestToken input = new TestToken(new SongPosition(1, 1, 0.5m), new TabElementLength(2));
        bool result = TabTokenUtility.TryCutTokenToFitBeat(input, 1, out TestToken? cutToken);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.True);
            Assert.That(cutToken, Is.Not.Null);
            Assert.That(cutToken.Position, Is.EqualTo(new SongPosition(1, 1, 0.5m)));
            Assert.That(cutToken.Length, Is.EqualTo(new TabElementLength(0.5m)));
        }
    }

    [Test]
    public void TryCutTokenToFitBeat_TokenFromTwoBeatsPriorThatFillsOutBar()
    {
        TestToken token = new TestToken(new SongPosition(1, 2), new TabElementLength(2));
        bool result = TabTokenUtility.TryCutTokenToFitBeat(token, 3, out TestToken? cutToken);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.True);
            Assert.That(cutToken, Is.Not.Null);
            Assert.That(cutToken.Position, Is.EqualTo(new SongPosition(1, 3)));
            Assert.That(cutToken.Length, Is.EqualTo(new TabElementLength(1)));
        }
    }
}