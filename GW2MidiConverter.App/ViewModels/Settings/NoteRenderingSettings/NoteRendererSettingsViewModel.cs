using GW2MidiConverter.App.ViewModels.Settings.NoteRenderingSettings.SubSettings;

namespace GW2MidiConverter.App.ViewModels.Settings.NoteRenderingSettings;

internal class NoteRendererSettingsViewModel : SettingsViewModel
{
    private readonly BracketPairSubSettingViewModel _bracketPairSubSetting = null!;
    private readonly KeyNamesSubSettingViewModel _keyNamesSubSetting = null!;
    private readonly RestSymbolsPerBeatSubSettingViewModel _restSymbolsPerBeatSubSetting = null!;
    private readonly TrailingSpacesPerBeatSubSettingViewModel _trailingSpacesPerBeatSubSetting = null!;

    public KeyNamesSubSettingViewModel KeyNamesSubSetting
    {
        get => _keyNamesSubSetting;
        private init => SetProperty(ref _keyNamesSubSetting, value);
    }

    public BracketPairSubSettingViewModel BracketPairSubSetting
    {
        get => _bracketPairSubSetting;
        private init => SetProperty(ref _bracketPairSubSetting, value);
    }

    public RestSymbolsPerBeatSubSettingViewModel RestSymbolsPerBeatSubSetting
    {
        get => _restSymbolsPerBeatSubSetting;
        private init => SetProperty(ref _restSymbolsPerBeatSubSetting, value);
    }

    public TrailingSpacesPerBeatSubSettingViewModel TrailingSpacesPerBeatSubSetting
    {
        get => _trailingSpacesPerBeatSubSetting;
        private init => SetProperty(ref _trailingSpacesPerBeatSubSetting, value);
    }
    
    public NoteRendererSettingsViewModel(Rendering.Settings.NoteRenderingSettings settings) : base("Notes")
    {
        KeyNamesSubSetting = new KeyNamesSubSettingViewModel();
        KeyNamesSubSetting.ApplySubSettingObject(settings.KeyNames);
        RegisterSubSetting(KeyNamesSubSetting);

        BracketPairSubSetting = new BracketPairSubSettingViewModel();
        BracketPairSubSetting.ApplySubSettingObject(settings.BracketPairsPerOctave);
        RegisterSubSetting(BracketPairSubSetting);
        
        RestSymbolsPerBeatSubSetting = new RestSymbolsPerBeatSubSettingViewModel();
        RestSymbolsPerBeatSubSetting.ApplySubSettingObject(settings.RestSymbolsPerBeat);
        RegisterSubSetting(RestSymbolsPerBeatSubSetting);

        TrailingSpacesPerBeatSubSetting = new TrailingSpacesPerBeatSubSettingViewModel();
        TrailingSpacesPerBeatSubSetting.ApplySubSettingObject(settings.TrailingSpacesPerBeatLength);
        RegisterSubSetting(TrailingSpacesPerBeatSubSetting);
    }

    public Rendering.Settings.NoteRenderingSettings CreateSettings()
    {
        return new Rendering.Settings.NoteRenderingSettings
        {
            KeyNames = KeyNamesSubSetting.GetSubSettingObject(),
            BracketPairsPerOctave = BracketPairSubSetting.GetSubSettingObject(),
            RestSymbolsPerBeat = RestSymbolsPerBeatSubSetting.GetSubSettingObject(),
            TrailingSpacesPerBeatLength = TrailingSpacesPerBeatSubSetting.GetSubSettingObject()
        };
    }
}