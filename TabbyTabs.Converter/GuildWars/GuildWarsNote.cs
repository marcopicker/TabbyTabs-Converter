namespace TabbyTabs.Converter.GuildWars;

public record GuildWarsNote(GuildWarsNoteKey Key, Octave Octave)
{
    public override string ToString()
    {
        if(Octave > 0)
        {
            return $"({Key.ToString()})";
        }
        else if (Octave < 0)
        {
            return $"[{Key.ToString()}]";
        }
        else
        {
            return $"{Key.ToString()}";
        }
    }
};