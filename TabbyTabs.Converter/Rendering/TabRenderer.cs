using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using TabbyTabs.Converter.Rendering.Settings;
using TabbyTabs.Converter.Tab;

namespace TabbyTabs.Converter.Rendering;

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

    public string Render(CancellationToken cancellationToken = default)
    {
        if (_tabDocument.Tracks.Count == 0) return string.Empty;
        _stringBuilder.Clear();

        try
        {
            _stringBuilder.Append(RenderHeader());
            for (int i = 0; i < _tabDocument.Tracks.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                TabTrack track = _tabDocument.Tracks[i];
                RenderTrack(track, cancellationToken);

                bool isLastTrack = i == _tabDocument.Tracks.Count - 1;
                if (!isLastTrack)
                {
                    _stringBuilder.Append(RenderPostTrackMargin());
                }
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new TabRenderingException("An unexpected error occurred while rendering the tab.", e)
            {
                Settings = _settings
            };
        }

        return _stringBuilder.ToString();
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "This will be changed in the future, don't worry.")]
    private string RenderHeader()
    {
        return "Hello, I am a header!\n\n";
    }

    private void RenderTrack(TabTrack track, CancellationToken cancellationToken)
    {
        string renderedTrack = _trackRenderer.Render(track, cancellationToken);
        _stringBuilder.AppendLine(renderedTrack);
    }

    private string RenderPostTrackMargin()
    {
        return new string('\n', _settings.TrackRenderingSettings.LineBreaksBetweenTracks);
    }
}