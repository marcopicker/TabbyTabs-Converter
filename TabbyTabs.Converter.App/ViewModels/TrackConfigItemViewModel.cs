using TabbyTabs.Converter.App.Controls;
using TabbyTabs.Converter.General;
using TabbyTabs.Converter.GuildWars.Instruments;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TabbyTabs.Converter.App.ViewModels;

internal class TrackConfigItemViewModel(TrackInfo trackInfo) : ObservableObject
{
    private Instrument? _selectedInstrument;
    private bool _isChecked = true;
    private ValidityType _validity;
    private string? _invalidMessage = string.Empty;

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }

    public string TrackName => string.IsNullOrWhiteSpace(TrackInfo.InstrumentName) ? $"Track {TrackInfo.TrackIndex + 1}" : TrackInfo.InstrumentName;

    public TrackInfo TrackInfo { get; } = trackInfo;

    public Instrument? SelectedInstrument
    {
        get => _selectedInstrument;
        set => SetProperty(ref _selectedInstrument, value);
    }

    public ValidityType Validity
    {
        get => _validity;
        set => SetProperty(ref _validity, value);
    }

    public string? InvalidMessage
    {
        get => _invalidMessage;
        set => SetProperty(ref _invalidMessage, value);
    }
}