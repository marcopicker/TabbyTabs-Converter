using System;
using GW2MidiConverter.Rendering.Settings;

namespace GW2MidiConverter.App.ViewModels.Settings;

internal class TrackRenderingSettingsViewModel : SettingsViewModel
{
    public AtomicSubSettingViewModel<bool> ShowTrackName { get; }
    public AtomicSubSettingViewModel<int> LineBreaksAfterTrackName { get; }
    public AtomicSubSettingViewModel<int> LineBreaksBetweenTracks { get; }
    public AtomicSubSettingViewModel<int> MaxMeasureCountBeforeLineBreak { get; }
    public AtomicSubSettingViewModel<int> LinesBeforeTimeSignature { get; }
    public AtomicSubSettingViewModel<int> LinesAfterTimeSignature { get; }
    public AtomicSubSettingViewModel<int> MeasuresPerLine { get; }

    public TrackRenderingSettingsViewModel(TrackRenderingSettings settings) : base("Tracks")
    {
        ShowTrackName = new AtomicSubSettingViewModel<bool>(true);
        RegisterSubSetting(ShowTrackName);

        LineBreaksAfterTrackName = new AtomicSubSettingViewModel<int>(settings.LineBreaksAfterTrackName);
        RegisterSubSetting(LineBreaksAfterTrackName);

        LineBreaksBetweenTracks = new AtomicSubSettingViewModel<int>(settings.LineBreaksBetweenTracks);
        RegisterSubSetting(LineBreaksBetweenTracks);

        MaxMeasureCountBeforeLineBreak = new AtomicSubSettingViewModel<int>(settings.MaxConsecutiveRowCount);
        RegisterSubSetting(MaxMeasureCountBeforeLineBreak);

        LinesBeforeTimeSignature = new AtomicSubSettingViewModel<int>(settings.LinesBeforeTimeSignature);
        RegisterSubSetting(LinesBeforeTimeSignature);

        LinesAfterTimeSignature = new AtomicSubSettingViewModel<int>(settings.LinesAfterTimeSignature);
        RegisterSubSetting(LinesAfterTimeSignature);

        MeasuresPerLine = new AtomicSubSettingViewModel<int>(settings.MeasuresPerLine);
        RegisterSubSetting(MeasuresPerLine);
    }

    public TrackRenderingSettings CreateSettings()
    {
        return new TrackRenderingSettings
        {
            ShowTrackName = ShowTrackName.Value,
            LineBreaksAfterTrackName = Math.Max(0, LineBreaksAfterTrackName.Value),
            LineBreaksBetweenTracks = Math.Max(0, LineBreaksBetweenTracks.Value),
            MaxConsecutiveRowCount = Math.Max(1, MaxMeasureCountBeforeLineBreak.Value),
            LinesBeforeTimeSignature = Math.Max(0, LinesBeforeTimeSignature.Value),
            LinesAfterTimeSignature = Math.Max(0, LinesAfterTimeSignature.Value),
            MeasuresPerLine = Math.Max(1, MeasuresPerLine.Value)
        };
    }
}