using GW2MidiConverter.Rendering.Settings;

namespace GW2MidiConverter.App.ViewModels.Settings;

internal class TimeSignatureRenderingSettingsViewModel : SettingsViewModel
{
    public EnumSubSettingViewModel<TimeSignatureVisibility> TimeSignatureVisibility { get; }
    public EnumSubSettingViewModel<TimeSignaturePositioning> TimeSignaturePositioning { get; }
    
    public TimeSignatureRenderingSettingsViewModel(TimeSignatureRenderingSettings settings) : base("Time Signatures")
    {
        TimeSignatureVisibility = new EnumSubSettingViewModel<TimeSignatureVisibility>(settings.TimeSignatureVisibility);
        RegisterSubSetting(TimeSignatureVisibility);
        
        TimeSignaturePositioning = new EnumSubSettingViewModel<TimeSignaturePositioning>(settings.TimeSignaturePositioning);
        RegisterSubSetting(TimeSignaturePositioning);
    }

    public TimeSignatureRenderingSettings CreateSettings()
    {
        return new TimeSignatureRenderingSettings
        {
            TimeSignatureVisibility = TimeSignatureVisibility.Value,
            TimeSignaturePositioning = TimeSignaturePositioning.Value
        };
    }
}