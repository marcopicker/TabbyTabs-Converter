using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GW2MidiConverter.App.ViewModels;

public class DictionaryEntryViewModel<TKey, TValue> : ObservableObject
{
    private TKey _key;
    private TValue _value;

    public TKey Key
    {
        get => _key;
        set
        {
            TryUnsubscribeFromKey();
            if (!SetProperty(ref _key, value)) return;
            TrySubscribeToKey();
        }
    }

    public TValue Value
    {
        get => _value;
        set
        {
            TryUnsubscribeFromValue();
            if (!SetProperty(ref _value, value)) return;
            TrySubscribeToValue();
        }
    }

    public DictionaryEntryViewModel(TKey key, TValue value)
    {
        _key = key;
        _value = value;

        TrySubscribeToValue();
    }

    private void TrySubscribeToKey()
    {
        if (_key is not INotifyPropertyChanged observableKey) return;
        observableKey.PropertyChanged += AnyPropertyChanged;
    }

    private void TryUnsubscribeFromKey()
    {
        if (_key is not INotifyPropertyChanged observableKey) return;
        observableKey.PropertyChanged -= AnyPropertyChanged;
    }

    private void TrySubscribeToValue()
    {
        if (_value is not INotifyPropertyChanged observableValue) return;
        observableValue.PropertyChanged += AnyPropertyChanged;
    }

    private void TryUnsubscribeFromValue()
    {
        if (_value is not INotifyPropertyChanged observableValue) return;
        observableValue.PropertyChanged -= AnyPropertyChanged;
    }

    private void AnyPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(e);
    }
}