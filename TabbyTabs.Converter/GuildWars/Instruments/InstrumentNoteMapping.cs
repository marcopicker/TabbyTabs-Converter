using TabbyTabs.Converter.Midi;

namespace TabbyTabs.Converter.GuildWars.Instruments;

public record InstrumentNoteMapping(MidiNoteNumber MidiNote, GuildWarsNote InstrumentNote);