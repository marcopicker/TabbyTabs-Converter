using System.Collections.Generic;

namespace GW2MidiConverter.Helpers;

public class NestedDictionary<TKey1, TKey2, TValue> : Dictionary<TKey1, Dictionary<TKey2, TValue>> where TKey2 : notnull where TKey1 : notnull;