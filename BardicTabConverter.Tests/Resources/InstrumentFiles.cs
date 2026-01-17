using System.IO;

namespace BardicTabConverter.Tests.Resources;

internal static class InstrumentFiles
{
    private const string RESOURCE_FOLDER = "Resources";

    public static string ValidInstrumentFile => Path.Combine(RESOURCE_FOLDER, "test_instrument.gw2instrument");
}