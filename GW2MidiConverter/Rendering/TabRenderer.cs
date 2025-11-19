using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;

namespace GW2MidiConverter.Rendering;

public class TabRenderer
{
    private readonly TabDocument _tabDocument;
    private readonly TabRenderingSettings _settings;
    private readonly TabTrackRenderer _trackRenderer;

    private readonly StringBuilder _stringBuilder = new StringBuilder();

    public TabRenderer(TabDocument tabDocument, TabRenderingSettings? settings = null)
    {
        ArgumentNullException.ThrowIfNull(tabDocument);

        _tabDocument = tabDocument;
        _settings = settings ?? new TabRenderingSettings();
        _trackRenderer = new TabTrackRenderer(_settings.TrackRenderingSettings,
                                              _settings.MeasureRenderingSettings,
                                              _settings.NoteRenderingSettings,
                                              _settings.TimeSignatureRenderingSettings);
    }

    public string Render()
    {
        if (_tabDocument.Tracks.Count == 0) return string.Empty;
        _stringBuilder.Clear();

        _stringBuilder.Append(RenderHeader());
        for (int i = 0; i < _tabDocument.Tracks.Count; i++)
        {
            TabTrack track = _tabDocument.Tracks[i];
            RenderTrack(track);

            bool isLastTrack = i == _tabDocument.Tracks.Count - 1;
            if (!isLastTrack)
            {
                _stringBuilder.Append(RenderPostTrackMargin());
            }
        }

        return _stringBuilder.ToString();
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "This will be changed in the future, don't worry.")]
    private string RenderHeader()
    {
        return "Hello, I am a header!\n\n";
    }
    
    private void RenderTrack(TabTrack track)
    {
        _stringBuilder.AppendLine(_trackRenderer.Render(track));
    }

    private string RenderPostTrackMargin()
    {
        return new string('\n', _settings.TrackRenderingSettings.LineBreaksBetweenTracks);
    }
}