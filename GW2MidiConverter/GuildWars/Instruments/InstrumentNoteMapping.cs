using GW2MidiConverter.Midi;

namespace GW2MidiConverter.GuildWars.Instruments;

public record InstrumentNoteMapping(MidiNoteNumber MidiNote, GuildWarsNote InstrumentNote);