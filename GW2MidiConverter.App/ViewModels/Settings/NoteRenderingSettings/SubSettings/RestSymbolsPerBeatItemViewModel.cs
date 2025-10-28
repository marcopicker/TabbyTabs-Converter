using CommunityToolkit.Mvvm.ComponentModel;
using GW2MidiConverter.Tab;

namespace GW2MidiConverter.App.ViewModels.Settings.NoteRenderingSettings.SubSettings;

public class RestSymbolsPerBeatItemViewModel(TabElementLength length, char symbol) : ObservableObject
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