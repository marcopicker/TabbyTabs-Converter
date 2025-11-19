using System;

namespace GW2MidiConverter.App.ViewModels.Settings;

internal interface ISettingsViewModel
{
    public event EventHandler? SubSettingChanged;
}