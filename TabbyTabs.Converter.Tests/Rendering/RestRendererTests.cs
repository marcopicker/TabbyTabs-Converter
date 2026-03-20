using System.Collections.Generic;
using TabbyTabs.Converter.Rendering;
using TabbyTabs.Converter.Rendering.Settings;
using TabbyTabs.Converter.Tab;
using TabbyTabs.Converter.Tab.Tokens;
using NUnit.Framework;

namespace TabbyTabs.Converter.Tests.Rendering;

[TestFixture]
public class RestRendererTests
{
    [TestCase(0, "")]
    [TestCase(1, "—")]
    [TestCase(1.5, "—   -")]
    [TestCase(1.55, "—   -")]
    [TestCase(2, "—   —")]
    [TestCase(3, "—   —   —")]
    [TestCase(0.5, "-")]
    [TestCase(0.25, "∙")]
    [TestCase(0.125, "")]
    public void RenderBeats_DefaultSettings_ValidInputs(decimal length, string expected)
    {
        NoteRenderingSettings settings = new NoteRenderingSettings()
        {
            TrailingSpacesPerBeatLength = new Dictionary<TabElementLength, int>
            {
                [new TabElementLength(1.00m)] = 3,
                [new TabElementLength(0.50m)] = 2,
                [new TabElementLength(0.25m)] = 1,
            },
            RestSymbolsPerBeat = new Dictionary<TabElementLength, char>()
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
        NoteRenderingSettings settings = new NoteRenderingSettings()
        {
            RestSymbolsPerBeat = new Dictionary<TabElementLength, char>()
            {
                // Empty dictionary!
            }
        };

        RestRenderer renderer = new RestRenderer(settings);
        string output = renderer.RenderBeats(length);
        Assert.That(output, Is.EqualTo(expected));
    }
}