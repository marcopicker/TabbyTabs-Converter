using System;

namespace TabbyTabs.Converter.Midi;

public class InvalidMidiDocumentException : Exception
{
    public InvalidMidiDocumentException(string message) : base(message)
    {
    }

    public InvalidMidiDocumentException()
    {
    }

    public InvalidMidiDocumentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}