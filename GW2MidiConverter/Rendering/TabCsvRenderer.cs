using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Rendering;

public class TabCsvRenderer
{
    private readonly TabDocument _tabDocument;
    private readonly TabRenderingSettings _settings;
    private readonly TabMeasureRenderer _measureRenderer;

    public TabCsvRenderer(TabDocument tabDocument, TabRenderingSettings? settings = null)
    {
        ArgumentNullException.ThrowIfNull(tabDocument);

        _tabDocument = tabDocument;
        _settings = settings ?? new TabRenderingSettings();
        _measureRenderer = new TabMeasureRenderer(_settings.MeasureRenderingSettings, _settings.NoteRenderingSettings);
    }

    public void ToCsv(string outputPath)
    {
        if (_tabDocument.Tracks.Count == 0) return;

        foreach (TabTrack track in _tabDocument.Tracks)
        {
            string path = Path.Combine(outputPath, $"{track.TrackInfo.TrackIndex} - {track.TrackInfo.InstrumentName}.csv");
            
            using StreamWriter writer = new StreamWriter(path);
            using CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteField(track.TrackInfo.InstrumentName);
            csv.NextRecord();
            foreach (TabMeasure measure in track.Measures)
            {
                IReadOnlyList<string> beats = _measureRenderer.RenderBeats(measure);
                foreach (string beat in beats)
                {
                    csv.WriteField(beat);
                }
                csv.NextRecord();
            }
        }
    }
}