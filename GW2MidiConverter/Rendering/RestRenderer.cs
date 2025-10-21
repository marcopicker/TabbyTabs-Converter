using System.Linq;
using System.Text;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;

namespace GW2MidiConverter.Rendering;

public class RestRenderer(TabRendererSettings settings)
{
    private readonly TabRendererSettings _settings = settings;

    public string RenderBeats(decimal beats)
    {
        if (beats <= 0) return string.Empty;

        decimal[] registeredLengths = _settings.RestSymbolsPerBeat.Keys.OrderByDescending(d => d).ToArray();
        StringBuilder builder = new StringBuilder();

        decimal remainingBeats = beats;
        while (remainingBeats > 0)
        {
            decimal maxFittingRest = registeredLengths.FirstOrDefault(d => d <= remainingBeats);
            if (maxFittingRest == 0)
            {
                // No registered rest duration exists that fits into the remaining beats. Our job here is done.
                break;
            }

            char restSymbol = _settings.RestSymbolsPerBeat[maxFittingRest];
            builder.Append(restSymbol);
            builder.Append(_settings.GetTrailingSpacesForBeatLength(new TabElementLength(maxFittingRest)));
            remainingBeats -= maxFittingRest;
        }

        return builder.ToString().Trim();
    }
}