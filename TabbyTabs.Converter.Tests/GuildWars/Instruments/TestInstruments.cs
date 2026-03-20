using TabbyTabs.Converter.GuildWars;
using TabbyTabs.Converter.GuildWars.Instruments;
using TabbyTabs.Converter.Midi;

namespace TabbyTabs.Converter.Tests.GuildWars.Instruments;

internal static class TestInstruments
{
    public static Instrument Piano => new Instrument("Piano", [
        new InstrumentNoteMapping(new MidiNoteNumber(48), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(49), new GuildWarsNote(GuildWarsNoteKey.Note1Sharp, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(50), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(51), new GuildWarsNote(GuildWarsNoteKey.Note2Sharp, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(52), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(53), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(54), new GuildWarsNote(GuildWarsNoteKey.Note4Sharp, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(55), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(56), new GuildWarsNote(GuildWarsNoteKey.Note5Sharp, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(57), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(58), new GuildWarsNote(GuildWarsNoteKey.Note6Sharp, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(59), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(-1))),
        new InstrumentNoteMapping(new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0))),

        new InstrumentNoteMapping(new MidiNoteNumber(61), new GuildWarsNote(GuildWarsNoteKey.Note1Sharp, new Octave(0))),
        new InstrumentNoteMapping(new MidiNoteNumber(62), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0))),
        new InstrumentNoteMapping(new MidiNoteNumber(63), new GuildWarsNote(GuildWarsNoteKey.Note2Sharp, new Octave(0))),
        new InstrumentNoteMapping(new MidiNoteNumber(64), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0))),
        new InstrumentNoteMapping(new MidiNoteNumber(65), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(0))),
        new InstrumentNoteMapping(new MidiNoteNumber(66), new GuildWarsNote(GuildWarsNoteKey.Note4Sharp, new Octave(0))),
        new InstrumentNoteMapping(new MidiNoteNumber(67), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0))),
        new InstrumentNoteMapping(new MidiNoteNumber(68), new GuildWarsNote(GuildWarsNoteKey.Note5Sharp, new Octave(0))),
        new InstrumentNoteMapping(new MidiNoteNumber(69), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(0))),
        new InstrumentNoteMapping(new MidiNoteNumber(70), new GuildWarsNote(GuildWarsNoteKey.Note6Sharp, new Octave(0))),
        new InstrumentNoteMapping(new MidiNoteNumber(71), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(0))),
        new InstrumentNoteMapping(new MidiNoteNumber(72), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0))),

        new InstrumentNoteMapping(new MidiNoteNumber(72), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(73), new GuildWarsNote(GuildWarsNoteKey.Note1Sharp, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(74), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(75), new GuildWarsNote(GuildWarsNoteKey.Note2Sharp, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(76), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(77), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(78), new GuildWarsNote(GuildWarsNoteKey.Note4Sharp, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(79), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(80), new GuildWarsNote(GuildWarsNoteKey.Note5Sharp, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(81), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(82), new GuildWarsNote(GuildWarsNoteKey.Note6Sharp, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(83), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(1))),
        new InstrumentNoteMapping(new MidiNoteNumber(84), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(1)))
    ]);
}