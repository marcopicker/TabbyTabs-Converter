using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace GW2MidiConverter.App.Models;

public class FullyObservableCollection<TNotifiable> : ObservableCollection<TNotifiable> where TNotifiable : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? ItemPropertyChanged;

    public FullyObservableCollection()
    {
    }

    public FullyObservableCollection(IEnumerable<TNotifiable> collection) : base(collection)
    {
        // "This" contains all items.
        foreach (TNotifiable item in this)
        {
            item.PropertyChanged += Item_PropertyChanged;
        }
    }

    private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ItemPropertyChanged?.Invoke(sender, e);
    }


    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (TNotifiable item in e.OldItems)
            {
                item.PropertyChanged -= Item_PropertyChanged;
            }
        }
        
        if (e.NewItems != null)
        {
            foreach (TNotifiable item in e.NewItems)
            {
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        base.OnCollectionChanged(e);
    }
}