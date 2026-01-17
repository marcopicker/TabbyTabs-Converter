using BardicTabConverter.Midi;

namespace BardicTabConverter.GuildWars.Instruments;

public record InstrumentNoteMapping(MidiNoteNumber MidiNote, GuildWarsNote InstrumentNote);