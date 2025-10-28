using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GW2MidiConverter.App.Models;

namespace GW2MidiConverter.App.ViewModels.Settings;

public abstract class DictionarySubSettingViewModel<TKey, TValue, TViewModel> : SubSettingViewModel<IReadOnlyDictionary<TKey, TValue>> where TViewModel : INotifyPropertyChanged
                                                                                                                                           where TKey : notnull
{
    private FullyObservableCollection<TViewModel> _items = [];

    public FullyObservableCollection<TViewModel> Items
    {
        get => _items;
        private set
        {
            if (!SetProperty(ref _items, value)) return;
            _items.ItemPropertyChanged += ItemsOnItemPropertyChanged;
            OnSettingChanged();
        }
    }

    protected abstract TViewModel CreateItemViewModel(TKey key, TValue value);

    protected abstract KeyValuePair<TKey, TValue> CreateDictionaryEntryForItem(TViewModel item);

    public override void ApplySubSettingObject(IReadOnlyDictionary<TKey, TValue> setting)
    {
        List<TViewModel> items = setting.Select(pair => CreateItemViewModel(pair.Key, pair.Value)).ToList();
        Items = new FullyObservableCollection<TViewModel>(items);
    }

    public override IReadOnlyDictionary<TKey, TValue> GetSubSettingObject()
    {
        Dictionary<TKey, TValue> output = [];
        foreach (TViewModel item in Items)
        {
            KeyValuePair<TKey, TValue> entry = CreateDictionaryEntryForItem(item);
            output[entry.Key] = entry.Value;
        }

        return output;
    }

    public override bool VerifySetting()
    {
        return Items.All(VerifyItem); 
    }

    protected abstract bool VerifyItem(TViewModel item);
    
    private void ItemsOnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnSettingChanged();
    }
}