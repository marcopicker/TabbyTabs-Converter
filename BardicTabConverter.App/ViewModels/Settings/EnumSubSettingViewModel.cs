using System;
using System.Collections.Generic;
using System.Linq;

namespace BardicTabConverter.App.ViewModels.Settings;

internal class EnumSubSettingViewModel<TEnum> : AtomicSubSettingViewModel<TEnum> where TEnum : Enum
{
    public IEnumerable<TEnum> EnumTypeValues => Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
}