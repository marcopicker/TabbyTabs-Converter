using CommunityToolkit.Mvvm.ComponentModel;
using GW2MidiConverter.General;
using GW2MidiConverter.GuildWars.Instruments;

namespace GW2MidiConverter.App.ViewModels;

internal class TrackConfigItemViewModel(TrackInfo trackInfo) : ObservableObject
{
    private Instrument? _selectedInstrument;
    private bool _isChecked = true;

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
}