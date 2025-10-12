using System.Collections.Generic;
using System.Text;
using GW2MidiConverter.Converters.Tokens;
using GW2MidiConverter.Converters.Tokens.Midi;

namespace GW2MidiConverter.Converters;

public class TokenToTabConverter(MidiConverterSettings? settings = null)
{
    private readonly MidiConverterSettings _settings = settings ?? new MidiConverterSettings();

    public string Convert(List<MidiToken> tokens)
    {
        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < tokens.Count - 1; i++)
        {
            MidiToken token = tokens[i];
            MidiToken nextToken = tokens[i + 1];
            
            switch (token)
            {
                case MeasureMidiToken measureToken:
                {
                    if (measureToken.HasTimeSignatureChange)
                    {
                        if (i > 0)
                        {
                            builder.Append('|');
                            builder.AppendLine();
                        }
                        builder.AppendLine($"Time Signature: ({measureToken.NewTimeSignature?.ToString()})");
                    }

                    builder.Append('|');
                    break;
                }
                case NoteMidiToken noteToken:
                {
                    bool isChord = noteToken.MidiNotes.Length > 1;
                    if (isChord)
                    {
                        builder.Append(string.Join('/', noteToken.MidiNotes));
                    }
                    else
                    {
                        builder.Append(noteToken.MidiNotes[0]);
                    }

                    if (nextToken is NoteMidiToken)
                    {
                        builder.Append(' ');
                    }
                    break;
                }
                case RestMidiToken restToken:
                {
                    builder.Append('-');
                    break;
                }
            }
        }
        
        builder.Append('|');
        return builder.ToString();
    }
}