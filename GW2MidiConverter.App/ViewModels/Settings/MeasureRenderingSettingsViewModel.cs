namespace GW2MidiConverter.App.ViewModels.Settings;

internal class MeasureRenderingSettingsViewModel : SettingsViewModel
{
    public AtomicSubSettingViewModel<bool> ShowMeasureNumbers { get; }
    public AtomicSubSettingViewModel<bool> DrawMeasureDelimiters { get; }
    public AtomicSubSettingViewModel<char> MeasureDelimiter { get; }
    public AtomicSubSettingViewModel<int> SpacesBetweenBeats { get; }
    
    public MeasureRenderingSettingsViewModel(Rendering.Settings.MeasureRenderingSettings settings) : base("Measures")
    {
        ShowMeasureNumbers = new AtomicSubSettingViewModel<bool>(settings.ShowMeasureNumbers);
        RegisterSubSetting(ShowMeasureNumbers);
        
        DrawMeasureDelimiters = new AtomicSubSettingViewModel<bool>(settings.DrawMeasureDelimiters);
        RegisterSubSetting(DrawMeasureDelimiters);
        
        MeasureDelimiter = new AtomicSubSettingViewModel<char>(settings.MeasureDelimiter);
        RegisterSubSetting(MeasureDelimiter);
        
        SpacesBetweenBeats = new AtomicSubSettingViewModel<int>(settings.SpacesBetweenBeats);
        SpacesBetweenBeats.VerifySettingFunc = () =>
        {
            if (SpacesBetweenBeats.Value > 0) return true;
            SpacesBetweenBeats.ErrorMessage = "Must be greater than 0";
            return false;
        };
        RegisterSubSetting(SpacesBetweenBeats);
    }
    
    public Rendering.Settings.MeasureRenderingSettings CreateSettings()
    {
        return new Rendering.Settings.MeasureRenderingSettings
        {
            ShowMeasureNumbers = ShowMeasureNumbers.Value,
            DrawMeasureDelimiters = DrawMeasureDelimiters.Value,
            MeasureDelimiter = MeasureDelimiter.Value,
            SpacesBetweenBeats = SpacesBetweenBeats.Value
        };
    }
}