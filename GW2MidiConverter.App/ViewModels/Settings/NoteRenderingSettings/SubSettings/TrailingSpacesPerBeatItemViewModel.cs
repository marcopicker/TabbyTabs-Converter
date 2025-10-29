using GW2MidiConverter.Tab;

namespace GW2MidiConverter.App.ViewModels.Settings.NoteRenderingSettings.SubSettings;

internal class TrailingSpacesPerBeatItemViewModel(TabElementLength length, int spaceCount) : DictionarySubSettingItemViewModel
{
    private decimal _length = length.ToDecimal();
    private int _spaceCount = spaceCount;

    public decimal Length
    {
        get => _length;
        set => SetProperty(ref _length, value);
    }

    public int SpaceCount
    {
        get => _spaceCount;
        set => SetProperty(ref _spaceCount, value);
    }
}