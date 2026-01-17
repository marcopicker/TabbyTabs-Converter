using System;
using BardicTabConverter.Rendering.Settings;

namespace BardicTabConverter.App.ViewModels.Settings;

internal class MeasureRenderingSettingsViewModel : SettingsViewModel<MeasureRenderingSettings>
{
    public AtomicSubSettingViewModel<bool> ShowMeasureNumbers { get; } = new AtomicSubSettingViewModel<bool>();
    public AtomicSubSettingViewModel<bool> DrawMeasureDelimiters { get; } = new AtomicSubSettingViewModel<bool>();
    public AtomicSubSettingViewModel<char> MeasureDelimiter { get; } = new AtomicSubSettingViewModel<char>();
    public AtomicSubSettingViewModel<int> MeasureDelimiterPadding { get; } = new AtomicSubSettingViewModel<int>();
    public AtomicSubSettingViewModel<int> SpacesBetweenBeats { get; } = new AtomicSubSettingViewModel<int>();

    public MeasureRenderingSettingsViewModel(MeasureRenderingSettings settings, string name) : base(settings, name)
    {
        SpacesBetweenBeats.VerifySettingFunc = () =>
        {
            if (SpacesBetweenBeats.Value > 0) return true;
            SpacesBetweenBeats.ErrorMessage = "Must be greater than 0";
            return false;
        };
    }

    public override void ApplySettings(MeasureRenderingSettings setting)
    {
        ShowMeasureNumbers.Value = setting.ShowMeasureNumbers;
        DrawMeasureDelimiters.Value = setting.DrawMeasureDelimiters;
        MeasureDelimiter.Value = setting.MeasureDelimiter;
        MeasureDelimiterPadding.Value = setting.MeasureDelimiterPadding;
        SpacesBetweenBeats.Value = setting.SpacesBetweenBeats;
    }

    public override MeasureRenderingSettings GetSettings()
    {
        return new MeasureRenderingSettings
        {
            ShowMeasureNumbers = ShowMeasureNumbers.Value,
            DrawMeasureDelimiters = DrawMeasureDelimiters.Value,
            MeasureDelimiter = MeasureDelimiter.Value,
            MeasureDelimiterPadding = Math.Max(0, MeasureDelimiterPadding.Value),
            SpacesBetweenBeats = Math.Max(1, SpacesBetweenBeats.Value)
        };
    }

    protected override void RegisterSubSettings()
    {
        RegisterSubSetting(ShowMeasureNumbers);
        RegisterSubSetting(DrawMeasureDelimiters);
        RegisterSubSetting(MeasureDelimiter);
        RegisterSubSetting(MeasureDelimiterPadding);
        RegisterSubSetting(SpacesBetweenBeats);
    }
}