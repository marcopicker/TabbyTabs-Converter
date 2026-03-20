using System.Collections.Generic;
using TabbyTabs.Converter.GuildWars.Instruments;

namespace TabbyTabs.Converter.App.Models;

internal interface IInstrumentProvider
{
    IReadOnlyCollection<InstrumentProviderResult> GetInstruments();
}