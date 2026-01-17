using BardicTabConverter.App.ViewModels.Settings.NoteRenderingSubSettings;
using BardicTabConverter.Rendering.Settings;

namespace BardicTabConverter.App.ViewModels.Settings;

internal class NoteRendererSettingsViewModel(NoteRenderingSettings setting, string name) : SettingsViewModel<NoteRenderingSettings>(setting, name)
{
    public KeyNamesSubSettingViewModel KeyNamesSubSetting { get; } = new KeyNamesSubSettingViewModel();
    public BracketPairSubSettingViewModel BracketPairSubSetting { get; } = new BracketPairSubSettingViewModel();
    public RestSymbolsPerBeatSubSettingViewModel RestSymbolsPerBeatSubSetting { get; } = new RestSymbolsPerBeatSubSettingViewModel();
    public TrailingSpacesPerBeatSubSettingViewModel TrailingSpacesPerBeatSubSetting { get; } = new TrailingSpacesPerBeatSubSettingViewModel();

    public override void ApplySettings(NoteRenderingSettings setting)
    {
        KeyNamesSubSetting.ApplySubSettingObject(setting.KeyNames);
        BracketPairSubSetting.ApplySubSettingObject(setting.BracketPairsPerOctave);
        RestSymbolsPerBeatSubSetting.ApplySubSettingObject(setting.RestSymbolsPerBeat);
        TrailingSpacesPerBeatSubSetting.ApplySubSettingObject(setting.TrailingSpacesPerBeatLength);
    }

    protected override void RegisterSubSettings()
    {
        RegisterSubSetting(KeyNamesSubSetting);
        RegisterSubSetting(BracketPairSubSetting);
        RegisterSubSetting(RestSymbolsPerBeatSubSetting);
        RegisterSubSetting(TrailingSpacesPerBeatSubSetting);
    }

    public override NoteRenderingSettings GetSettings()
    {
        return new NoteRenderingSettings
        {
            KeyNames = KeyNamesSubSetting.GetSubSettingObject(),
            BracketPairsPerOctave = BracketPairSubSetting.GetSubSettingObject(),
            RestSymbolsPerBeat = RestSymbolsPerBeatSubSetting.GetSubSettingObject(),
            TrailingSpacesPerBeatLength = TrailingSpacesPerBeatSubSetting.GetSubSettingObject()
        };
    }
}