using GW2MidiConverter.GuildWars.Instruments;

namespace GW2MidiConverter.App.Models;

public record InstrumentProviderResult(Instrument? Instrument, string Path, bool Successful = true, string ErrorMessage = "");