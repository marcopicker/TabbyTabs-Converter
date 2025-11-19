using System;
using System.IO;

namespace GW2MidiConverter.App.Services;

internal static class Logging
{
    public static readonly string LogFolder = Path.Combine(Environment.CurrentDirectory, "Logs");
}