using TabbyTabs.Converter.Tab;

namespace TabbyTabs.Converter.App.ViewModels.Settings.NoteRenderingSubSettings;

internal class RestSymbolsPerBeatItemViewModel(TabElementLength length, char symbol) : DictionarySubSettingItemViewModel
{
    private decimal _length = length.ToDecimal();
    private char _symbol = symbol;

    public decimal Length
    {
        get => _length;
        set => SetProperty(ref _length, value);
    }

    public char Symbol
    {
        get => _symbol;
        set => SetProperty(ref _symbol, value);
    }
}