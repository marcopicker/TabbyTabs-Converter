using System;
using System.Collections.Generic;
using System.Linq;

namespace GW2MidiConverter.App.ViewModels.Settings;

internal class EnumSubSettingViewModel<TEnum>(TEnum initialValue) : AtomicSubSettingViewModel<TEnum>(initialValue) where TEnum : Enum
{
    public IEnumerable<TEnum> EnumTypeValues => Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
}