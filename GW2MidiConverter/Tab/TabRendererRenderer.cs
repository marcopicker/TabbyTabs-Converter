using System;
using System.Linq;

namespace GW2MidiConverter.Tab;

public class TabRendererRenderer
{
    private readonly TabDocument _tabDocument;
    private readonly TabSettings _settings;
    
    public TabRendererRenderer(TabDocument tabDocument, TabSettings? settings = null)
    {
        ArgumentNullException.ThrowIfNull(tabDocument);
        
        _tabDocument = tabDocument;
        _settings = settings ?? new TabSettings();
    }
    
    public string Render(int[] trackIndexes)
    {
        bool all = trackIndexes.All(i => _tabDocument.Tracks.Any(t => t.TrackInfo.TrackIndex == i));
        return "";
    }
}