using System.Collections.Generic;
using BardicTabConverter.GuildWars.Instruments;

namespace BardicTabConverter.App.Models;

internal interface IInstrumentProvider
{
    IReadOnlyCollection<InstrumentProviderResult> GetInstruments();
}