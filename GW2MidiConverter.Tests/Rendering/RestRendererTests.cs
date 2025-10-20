using System.Collections.Generic;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Rendering;

[TestFixture]
public class RestRendererTests
{
    [TestCase(0, "")]
    [TestCase(1, "—")]
    [TestCase(1.5, "— -")]
    [TestCase(1.55, "— -")]
    [TestCase(2, "— —")]
    [TestCase(3, "— — —")]
    [TestCase(0.5, "-")]
    [TestCase(0.25, "∙")]
    [TestCase(0.125, "")]
    public void RenderBeats_DefaultSettings_ValidInputs(decimal length, string expected)
    {
        TabRendererSettings settings = new TabRendererSettings()
        {
            RestSymbolsPerBeat = new Dictionary<decimal, char>()
            {
                [1.00m] = '—',
                [0.50m] = '-',
                [0.25m] = '∙'
            }
        };
        
        RestRenderer renderer = new RestRenderer(settings);
        string output = renderer.RenderBeats(length);
        Assert.That(output, Is.EqualTo(expected));
    }
    
    [TestCase(0, "")]
    [TestCase(1, "")]
    [TestCase(1.5, "")]
    [TestCase(1.55, "")]
    [TestCase(2, "")]
    [TestCase(3, "")]
    [TestCase(0.5, "")]
    [TestCase(0.25, "")]
    [TestCase(0.125, "")]
    public void RenderBeats_NoSymbolsSet_ValidInputs(decimal length, string expected)
    {
        TabRendererSettings settings = new TabRendererSettings()
        {
            RestSymbolsPerBeat = new Dictionary<decimal, char>()
            {
                // Empty dictionary!
            }
        };
        
        RestRenderer renderer = new RestRenderer(settings);
        string output = renderer.RenderBeats(length);
        Assert.That(output, Is.EqualTo(expected));
    }

    [TestCase(0, "")]
    [TestCase(1, "—")]
    [TestCase(1.5, "— -")]
    [TestCase(1.55, "— -")]
    [TestCase(2, "— —")]
    [TestCase(3, "— — —")]
    [TestCase(0.5, "-")]
    [TestCase(0.25, "∙")]
    [TestCase(0.125, "")]
    public void RenderToken_DefaultSettings_ValidInputs(decimal length, string expected)
    {
        TabRendererSettings settings = new TabRendererSettings()
        {
            RestSymbolsPerBeat = new Dictionary<decimal, char>()
            {
                [1.00m] = '—',
                [0.50m] = '-',
                [0.25m] = '∙'
            }
        };

        RestToken token = new RestToken(new TabElementLength(length));
        
        RestRenderer renderer = new RestRenderer(settings);
        string output = renderer.RenderToken(token);
        Assert.That(output, Is.EqualTo(expected));
    }
}