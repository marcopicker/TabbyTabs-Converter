namespace GW2MidiConverter.Converters;

public class MidiConverterSettings
{
    public int MinimumNoteDuration { get; set; } = 1;
    public int ChordTickTolerance { get; set; } = 1;
}