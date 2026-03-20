using System;

namespace TabbyTabs.Converter.Converters;

public class MidiConverterSettings
{
    public int MinimumNoteDuration { get; set; } = 1;
    
    [Obsolete("This is not implemented yet.", false)]
    public int ChordTickTolerance { get; set; } = 1;
}