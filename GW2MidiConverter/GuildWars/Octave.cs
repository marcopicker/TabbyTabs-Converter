namespace GW2MidiConverter.GuildWars;

public readonly record struct Octave(int Value)
{ 
    public static implicit operator int(Octave octave) => octave.Value;
    public static implicit operator Octave(int value) => new Octave(value);
}