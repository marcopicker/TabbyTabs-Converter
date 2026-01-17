using BardicTabConverter.GuildWars.Instruments;

namespace BardicTabConverter.App.Models;

public record InstrumentProviderResult(Instrument? Instrument, string Path, bool Successful = true, string ErrorMessage = "");