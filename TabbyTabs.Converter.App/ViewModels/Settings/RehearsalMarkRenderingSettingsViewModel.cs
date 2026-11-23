using TabbyTabs.Converter.Rendering.Settings;

namespace TabbyTabs.Converter.App.ViewModels.Settings;

internal class RehearsalMarkRenderingSettingsViewModel : SettingsViewModel<RehearsalMarkRenderingSettings>
{
    public AtomicSubSettingViewModel<bool> DrawRehearsalMarks { get; } = new AtomicSubSettingViewModel<bool>();
    public AtomicSubSettingViewModel<string> Format { get; } = new AtomicSubSettingViewModel<string>();
    public AtomicSubSettingViewModel<int> LinesBeforeRehearsalMark { get; } = new AtomicSubSettingViewModel<int>();
    public AtomicSubSettingViewModel<int> LinesAfterRehearsalMark { get; } = new AtomicSubSettingViewModel<int>();

    public RehearsalMarkRenderingSettingsViewModel(RehearsalMarkRenderingSettings setting, string name) : base(setting, name)
    {
        Format.VerifySettingFunc = () =>
        {
            if (!string.IsNullOrWhiteSpace(Format.Value)) return true;
            
            Format.ErrorMessage = "Format cannot be empty";
            return false;
        };
    }
    
    public override void ApplySettings(RehearsalMarkRenderingSettings setting)
    {
        DrawRehearsalMarks.Value = setting.DrawRehearsalMarks;
        Format.Value = setting.Format;
        LinesBeforeRehearsalMark.Value = setting.LinesBeforeRehearsalMark;
        LinesAfterRehearsalMark.Value = setting.LinesAfterRehearsalMark;
    }

    public override RehearsalMarkRenderingSettings GetSettings()
    {
        return new RehearsalMarkRenderingSettings
        {
            DrawRehearsalMarks = DrawRehearsalMarks.Value,
            Format = Format.Value ?? string.Empty,
            LinesBeforeRehearsalMark = LinesBeforeRehearsalMark.Value,
            LinesAfterRehearsalMark = LinesAfterRehearsalMark.Value
        };
    }

    protected override void RegisterSubSettings()
    {
        RegisterSubSetting(DrawRehearsalMarks);
        RegisterSubSetting(LinesBeforeRehearsalMark);
        RegisterSubSetting(LinesAfterRehearsalMark);
        RegisterSubSetting(Format);
    }
}