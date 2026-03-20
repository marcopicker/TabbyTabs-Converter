using TabbyTabs.Converter.Rendering.Settings;

namespace TabbyTabs.Converter.App.ViewModels.Settings;

internal class TimeSignatureRenderingSettingsViewModel(TimeSignatureRenderingSettings setting, string name) : SettingsViewModel<TimeSignatureRenderingSettings>(setting, name)
{
    public EnumSubSettingViewModel<TimeSignatureVisibility> TimeSignatureVisibility { get; } = new EnumSubSettingViewModel<TimeSignatureVisibility>();
    public EnumSubSettingViewModel<TimeSignaturePositioning> TimeSignaturePositioning { get; } = new EnumSubSettingViewModel<TimeSignaturePositioning>();

    public override void ApplySettings(TimeSignatureRenderingSettings setting)
    {
        TimeSignatureVisibility.Value = setting.TimeSignatureVisibility;
        TimeSignaturePositioning.Value = setting.TimeSignaturePositioning;
    }

    public override TimeSignatureRenderingSettings GetSettings()
    {
        return new TimeSignatureRenderingSettings
        {
            TimeSignatureVisibility = TimeSignatureVisibility.Value,
            TimeSignaturePositioning = TimeSignaturePositioning.Value
        };
    }

    protected override void RegisterSubSettings()
    {
        RegisterSubSetting(TimeSignatureVisibility);
        RegisterSubSetting(TimeSignaturePositioning);
    }
}