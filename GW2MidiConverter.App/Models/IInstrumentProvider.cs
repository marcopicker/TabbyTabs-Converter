using System.Collections.Generic;
using GW2MidiConverter.GuildWars.Instruments;

namespace GW2MidiConverter.App.Models;

internal interface IInstrumentProvider
{
    IReadOnlyCollection<Instrument> GetInstruments();
}