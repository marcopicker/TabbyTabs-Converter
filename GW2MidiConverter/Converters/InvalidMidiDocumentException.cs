using System;

namespace GW2MidiConverter;

[Serializable]
public sealed class InvalidMidiDocumentException : Exception
{
    public InvalidMidiDocumentException() {}

    public InvalidMidiDocumentException(string message) : base(message) {}

    public InvalidMidiDocumentException(string message, Exception innerException) :base(message, innerException) {}
}