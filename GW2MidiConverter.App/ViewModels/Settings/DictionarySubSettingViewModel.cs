using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GW2MidiConverter.App.Models;

namespace GW2MidiConverter.App.ViewModels.Settings;

internal abstract class DictionarySubSettingViewModel<TKey, TValue, TViewModel> : SubSettingViewModel<IReadOnlyDictionary<TKey, TValue>> where TViewModel : DictionarySubSettingItemViewModel
                                                                                                                                         where TKey : notnull
{
    private FullyObservableCollection<TViewModel> _items = [];
    private bool _isVerifyingSetting;

    public FullyObservableCollection<TViewModel> Items
    {
        get => _items;
        private set
        {
            if (!SetProperty(ref _items, value)) return;
            _items.ItemPropertyChanged += ItemsOnItemPropertyChanged;
            NotifySettingChanged();
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

    public sealed override bool VerifySetting()
    {
        bool itemsAreValid = VerifyAllItems();
        bool subSettingIsValid = VerifySubSettingInContext();
        return itemsAreValid && subSettingIsValid;
    }

    private bool VerifyAllItems()
    {
        bool allItemsAreValid = true;
        foreach (TViewModel item in Items)
        {
            bool itemIsValid = VerifyItem(item, out string errorMessage);
            if (itemIsValid)
            {
                item.SetValid();
                continue;
            }

            allItemsAreValid = false;
            item.SetInvalid(errorMessage);
        }

        return allItemsAreValid;
    }

    protected virtual bool VerifySubSettingInContext()
    {
        return true;
    }
    
    protected abstract bool VerifyItem(TViewModel item, out string errorMessage);

    private void ItemsOnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        NotifySettingChanged();
    }
}