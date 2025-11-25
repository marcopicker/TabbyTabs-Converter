using System;
using GW2MidiConverter.Rendering.Settings;

namespace GW2MidiConverter.Rendering;

public class TabRenderingException : Exception
{
    public TabRenderingSettings? Settings { get; init; }
    
    public TabRenderingException()
    {
    }
    
    public TabRenderingException(string message) : base(message)
    {
    }
    
    public TabRenderingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}