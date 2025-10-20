using System;
using System.Linq;
using System.Text;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Rendering;

public class RestTokenRenderer(TabRendererSettings settings)
{
    private readonly TabRendererSettings _settings = settings;

    public string RenderToken(RestToken token)
    {
        ArgumentNullException.ThrowIfNull(token);
        return RenderBeats(token.Length);
    }

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
            builder.Append(' '); // Appending a space here is safe, as we trim the output anyway.
            remainingBeats -= maxFittingRest;
        }

        return builder.ToString().Trim();
    }
}