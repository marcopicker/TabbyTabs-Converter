using System.Linq;
using System.Text;
using BardicTabConverter.Rendering.Settings;
using BardicTabConverter.Tab;

namespace BardicTabConverter.Rendering;

public class RestRenderer(NoteRenderingSettings settings)
{
    public string RenderBeats(decimal beats)
    {
        if (beats <= 0) return string.Empty;

        TabElementLength[] registeredLengths = settings.RestSymbolsPerBeat.Keys.OrderByDescending(d => d).ToArray();
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

            char restSymbol = settings.RestSymbolsPerBeat[maxFittingRest];
            builder.Append(restSymbol);
            builder.Append(settings.GetTrailingSpacesForBeatLength(new TabElementLength(maxFittingRest)));
            remainingBeats -= maxFittingRest;
        }

        return builder.ToString().Trim();
    }
}