using System;

namespace TabbyTabs.Converter.GuildWars.Instruments;

public class InstrumentException : Exception
{
    public InstrumentException()
    {
    }

    public InstrumentException(string message) : base(message)
    {
    }

    public InstrumentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}