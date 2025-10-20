using System;
using System.Text;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;

namespace GW2MidiConverter.Rendering;

public class TabRenderer
{
    private readonly TabDocument _tabDocument;
    private readonly TabRendererSettings _settings;
    private readonly TrackRenderer _trackRenderer;
    
    private readonly StringBuilder _stringBuilder = new StringBuilder();

    public TabRenderer(TabDocument tabDocument, TabRendererSettings? settings = null)
    {
        ArgumentNullException.ThrowIfNull(tabDocument);

        _tabDocument = tabDocument;
        _settings = settings ?? new TabRendererSettings();
        _trackRenderer = new TrackRenderer(_settings);
    }

    public string Render()
    {
        if (_tabDocument.Tracks.Length == 0) return string.Empty;
        _stringBuilder.Clear();
        
        _stringBuilder.Append(RenderHeader());
        for (int i = 0; i < _tabDocument.Tracks.Length; i++)
        {
            TabTrack track = _tabDocument.Tracks[i];
            RenderTrack(track);

            bool isLastTrack = i == _tabDocument.Tracks.Length - 1;
            if (!isLastTrack)
            {
                _stringBuilder.Append(RenderPostTrackMargin());
            }
        }

        return _stringBuilder.ToString();
    }

    private string RenderHeader()
    {
        return "Hello, I am a header!\n\n";
    }

    private void RenderTrack(TabTrack track)
    {
        _stringBuilder.AppendLine($"{track.TrackInfo.InstrumentName}:");
        _stringBuilder.Append(new string('\n', _settings.LineBreaksAfterTrackName));
        
        _stringBuilder.AppendLine(_trackRenderer.Render(track));
    }

    private string RenderPostTrackMargin()
    {
        return new string('\n', _settings.LineBreaksBetweenTracks);
    }
}