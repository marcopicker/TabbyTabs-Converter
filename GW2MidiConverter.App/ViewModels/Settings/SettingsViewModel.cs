using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GW2MidiConverter.App.ViewModels.Settings;

internal abstract class SettingsViewModel : ObservableObject, ISetting
{
    public event EventHandler? SubSettingChanged;

    private readonly ObservableCollection<ISubSetting> _subSettings = [];

    protected IReadOnlyCollection<ISubSetting> SubSettings => _subSettings;

    protected SettingsViewModel()
    {
        _subSettings.CollectionChanged += SubSettingsOnCollectionChanged;
    }

    protected void RegisterSubSetting(ISubSetting subSetting)
    {
        _subSettings.Add(subSetting);
    }

    protected virtual bool VerifySubSettings()
    {
        return _subSettings.All(s => s.VerifySetting());
    }

    private void SubSettingsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (ISubSetting item in e.OldItems)
            {
                item.SettingChanged -= SubSettingItemChanged;
            }
        }

        if (e.NewItems != null)
        {
            foreach (ISubSetting item in e.NewItems)
            {
                item.SettingChanged += SubSettingItemChanged;
            }
        }
    }

    private void SubSettingItemChanged(object? sender, EventArgs e)
    {
        if (!VerifySubSettings()) return;
        OnSubSettingChanged();
    }

    private void OnSubSettingChanged()
    {
        SubSettingChanged?.Invoke(this, EventArgs.Empty);
    }
}