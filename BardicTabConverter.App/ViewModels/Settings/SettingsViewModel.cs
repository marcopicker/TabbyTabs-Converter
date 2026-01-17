using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BardicTabConverter.App.ViewModels.Settings;

internal abstract class SettingsViewModel<TSettings> : ObservableObject, ISettingsViewModel
{
    public event EventHandler? SubSettingChanged;

    private bool _isVerifyingSubSettings;
    private readonly ObservableCollection<ISubSetting> _subSettings = [];
    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    protected IReadOnlyCollection<ISubSetting> SubSettings => _subSettings;
    
    public SettingsViewModel(TSettings setting, string name)
    {
        Name = name;
        _subSettings.CollectionChanged += SubSettingsOnCollectionChanged;
        
        ApplySettings(setting);
        RegisterSubSettings();
    }
    
    public abstract void ApplySettings(TSettings setting); 
    
    public abstract TSettings GetSettings();
    
    protected abstract void RegisterSubSettings(); 
    
    protected void RegisterSubSetting(ISubSetting subSetting)
    {
        _subSettings.Add(subSetting);
    }

    private void SubSettingsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (ISubSetting item in e.OldItems)
            {
                item.SettingChanged -= HandleSubSettingChanged;
            }
        }

        if (e.NewItems != null)
        {
            foreach (ISubSetting item in e.NewItems)
            {
                item.SettingChanged += HandleSubSettingChanged;
            }
        }
    }

    private bool VerifySubSettings()
    {
        if (_isVerifyingSubSettings) return false;
        _isVerifyingSubSettings = true;

        // We don't want to use Linq.All(...) because we want to iterate over all subsettings, regardless of failures. 
        bool areAllSubSettingsValid = true;
        foreach (ISubSetting setting in SubSettings)
        {
            if (!setting.VerifySetting()) areAllSubSettingsValid = false;
        }

        _isVerifyingSubSettings = false;
        return areAllSubSettingsValid;
    }

    private void HandleSubSettingChanged(object? sender, EventArgs e)
    {
        if (!VerifySubSettings()) return;
        SubSettingChanged?.Invoke(this, e);
    }
}