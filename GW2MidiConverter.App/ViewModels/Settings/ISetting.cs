using System;

namespace GW2MidiConverter.App.ViewModels.Settings;

internal interface ISetting
{
    public event EventHandler? SubSettingChanged;
}