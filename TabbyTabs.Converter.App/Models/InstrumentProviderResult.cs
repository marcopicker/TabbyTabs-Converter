using TabbyTabs.Converter.GuildWars.Instruments;

namespace TabbyTabs.Converter.App.Models;

public record InstrumentProviderResult(Instrument? Instrument, string Path, bool Successful = true, string ErrorMessage = "");