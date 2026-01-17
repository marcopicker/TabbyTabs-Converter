using System;
using BardicTabConverter.Rendering.Settings;

namespace BardicTabConverter.App.ViewModels.Settings;

internal class TrackRenderingSettingsViewModel(TrackRenderingSettings setting, string name) : SettingsViewModel<TrackRenderingSettings>(setting, name)
{
    public AtomicSubSettingViewModel<bool> ShowTrackName { get; } = new AtomicSubSettingViewModel<bool>();
    public AtomicSubSettingViewModel<int> LineBreaksAfterTrackName { get; } = new AtomicSubSettingViewModel<int>();
    public AtomicSubSettingViewModel<int> LineBreaksBetweenTracks { get; } = new AtomicSubSettingViewModel<int>();
    public AtomicSubSettingViewModel<int> MaxMeasureCountBeforeLineBreak { get; } = new AtomicSubSettingViewModel<int>();
    public AtomicSubSettingViewModel<int> LinesBeforeTimeSignature { get; } = new AtomicSubSettingViewModel<int>();
    public AtomicSubSettingViewModel<int> LinesAfterTimeSignature { get; } = new AtomicSubSettingViewModel<int>();
    public AtomicSubSettingViewModel<int> MeasuresPerLine { get; } = new AtomicSubSettingViewModel<int>();

    public override void ApplySettings(TrackRenderingSettings setting)
    {
        ShowTrackName.Value = setting.ShowTrackName;
        LineBreaksAfterTrackName.Value = setting.LineBreaksAfterTrackName;
        LineBreaksBetweenTracks.Value = setting.LineBreaksBetweenTracks;
        MaxMeasureCountBeforeLineBreak.Value = setting.MaxConsecutiveRowCount;
        LinesBeforeTimeSignature.Value = setting.LinesBeforeTimeSignature;
        LinesAfterTimeSignature.Value = setting.LinesAfterTimeSignature;
        MeasuresPerLine.Value = setting.MeasuresPerLine;
    }

    public override TrackRenderingSettings GetSettings()
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

    protected override void RegisterSubSettings()
    {
        RegisterSubSetting(ShowTrackName);
        RegisterSubSetting(LineBreaksAfterTrackName);
        RegisterSubSetting(LineBreaksBetweenTracks);
        RegisterSubSetting(MaxMeasureCountBeforeLineBreak);
        RegisterSubSetting(LinesBeforeTimeSignature);
        RegisterSubSetting(LinesAfterTimeSignature);
        RegisterSubSetting(MeasuresPerLine);
    }
}