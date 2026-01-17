using System;
using System.Collections.Generic;
using System.Linq;
using BardicTabConverter.Rendering.Settings;
using BardicTabConverter.Tab;

namespace BardicTabConverter.Rendering;

public static class NoteRenderingUtilities
{
    public static string GetTrailingSpacesForBeatLength(this NoteRenderingSettings settings, TabElementLength beatLength)
    {
        ArgumentNullException.ThrowIfNull(settings);
        if (settings.TrailingSpacesPerBeatLength.Count == 0) return string.Empty;
        
        List<TabElementLength> lengths = settings.TrailingSpacesPerBeatLength.Keys.Where(length => length.Beats <= beatLength.Beats).ToList();
        if(lengths.Count == 0) return string.Empty;

        TabElementLength fittingLength = lengths.OrderByDescending(length => length.Beats).MaxBy(l => l);
        int spaceCount = settings.TrailingSpacesPerBeatLength[fittingLength];
        return new string(' ', spaceCount);
    }
}