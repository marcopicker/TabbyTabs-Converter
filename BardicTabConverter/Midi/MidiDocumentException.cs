using System;

namespace BardicTabConverter.Midi;

public class MidiDocumentException : Exception
{
    public MidiDocumentException(string message) : base(message)
    {
    }

    public MidiDocumentException()
    {
    }

    public MidiDocumentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}