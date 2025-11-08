using System.Collections.Generic;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.GuildWars.Instruments;

internal static class InstrumentNoteMaps
{
    public static InstrumentNoteMap TripleOctaveChromatic
    {
        get
        {
            IReadOnlyList<IReadOnlyDictionary<MidiNoteNumber, GuildWarsNote>> mappings =
            [
                new Dictionary<MidiNoteNumber, GuildWarsNote>()
                {
                    { new MidiNoteNumber(48), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1)) },
                    { new MidiNoteNumber(49), new GuildWarsNote(GuildWarsNoteKey.Note1Sharp, new Octave(-1)) },
                    { new MidiNoteNumber(50), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(-1)) },
                    { new MidiNoteNumber(51), new GuildWarsNote(GuildWarsNoteKey.Note2Sharp, new Octave(-1)) },
                    { new MidiNoteNumber(52), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(-1)) },
                    { new MidiNoteNumber(53), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(-1)) },
                    { new MidiNoteNumber(54), new GuildWarsNote(GuildWarsNoteKey.Note4Sharp, new Octave(-1)) },
                    { new MidiNoteNumber(55), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(-1)) },
                    { new MidiNoteNumber(56), new GuildWarsNote(GuildWarsNoteKey.Note5Sharp, new Octave(-1)) },
                    { new MidiNoteNumber(57), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(-1)) },
                    { new MidiNoteNumber(58), new GuildWarsNote(GuildWarsNoteKey.Note6Sharp, new Octave(-1)) },
                    { new MidiNoteNumber(59), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(-1)) },
                    { new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(-1)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>()
                {
                    { new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0)) },
                    { new MidiNoteNumber(61), new GuildWarsNote(GuildWarsNoteKey.Note1Sharp, new Octave(0)) },
                    { new MidiNoteNumber(62), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0)) },
                    { new MidiNoteNumber(63), new GuildWarsNote(GuildWarsNoteKey.Note2Sharp, new Octave(0)) },
                    { new MidiNoteNumber(64), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)) },
                    { new MidiNoteNumber(65), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(0)) },
                    { new MidiNoteNumber(66), new GuildWarsNote(GuildWarsNoteKey.Note4Sharp, new Octave(0)) },
                    { new MidiNoteNumber(67), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0)) },
                    { new MidiNoteNumber(68), new GuildWarsNote(GuildWarsNoteKey.Note5Sharp, new Octave(0)) },
                    { new MidiNoteNumber(69), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(0)) },
                    { new MidiNoteNumber(70), new GuildWarsNote(GuildWarsNoteKey.Note6Sharp, new Octave(0)) },
                    { new MidiNoteNumber(71), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(0)) },
                    { new MidiNoteNumber(72), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>()
                {
                    { new MidiNoteNumber(72), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1)) },
                    { new MidiNoteNumber(73), new GuildWarsNote(GuildWarsNoteKey.Note1Sharp, new Octave(1)) },
                    { new MidiNoteNumber(74), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(1)) },
                    { new MidiNoteNumber(75), new GuildWarsNote(GuildWarsNoteKey.Note2Sharp, new Octave(1)) },
                    { new MidiNoteNumber(76), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(1)) },
                    { new MidiNoteNumber(77), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(1)) },
                    { new MidiNoteNumber(78), new GuildWarsNote(GuildWarsNoteKey.Note4Sharp, new Octave(1)) },
                    { new MidiNoteNumber(79), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1)) },
                    { new MidiNoteNumber(80), new GuildWarsNote(GuildWarsNoteKey.Note5Sharp, new Octave(1)) },
                    { new MidiNoteNumber(81), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(1)) },
                    { new MidiNoteNumber(82), new GuildWarsNote(GuildWarsNoteKey.Note6Sharp, new Octave(1)) },
                    { new MidiNoteNumber(83), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(1)) },
                    { new MidiNoteNumber(84), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(1)) },
                }
            ];
            return new InstrumentNoteMap(mappings);
        }
    }

    public static InstrumentNoteMap TripleOctaveCMajor
    {
        get
        {
            IReadOnlyList<IReadOnlyDictionary<MidiNoteNumber, GuildWarsNote>> mappings =
            [
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(48), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1)) },
                    { new MidiNoteNumber(50), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(-1)) },
                    { new MidiNoteNumber(52), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(-1)) },
                    { new MidiNoteNumber(53), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(-1)) },
                    { new MidiNoteNumber(55), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(-1)) },
                    { new MidiNoteNumber(57), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(-1)) },
                    { new MidiNoteNumber(59), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(-1)) },
                    { new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(-1)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0)) },
                    { new MidiNoteNumber(62), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0)) },
                    { new MidiNoteNumber(64), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)) },
                    { new MidiNoteNumber(65), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(0)) },
                    { new MidiNoteNumber(67), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0)) },
                    { new MidiNoteNumber(69), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(0)) },
                    { new MidiNoteNumber(71), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(0)) },
                    { new MidiNoteNumber(72), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(72), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1)) },
                    { new MidiNoteNumber(74), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(1)) },
                    { new MidiNoteNumber(76), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(1)) },
                    { new MidiNoteNumber(77), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(1)) },
                    { new MidiNoteNumber(79), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1)) },
                    { new MidiNoteNumber(81), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(1)) },
                    { new MidiNoteNumber(83), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(1)) },
                    { new MidiNoteNumber(84), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(1)) },
                }
            ];
            return new InstrumentNoteMap(mappings);
        }
    }

    public static InstrumentNoteMap DoubleOctaveCMajor
    {
        get
        {
            IReadOnlyList<IReadOnlyDictionary<MidiNoteNumber, GuildWarsNote>> mappings =
            [
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(62), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0)) },
                    { new MidiNoteNumber(64), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0)) },
                    { new MidiNoteNumber(65), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)) },
                    { new MidiNoteNumber(67), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(0)) },
                    { new MidiNoteNumber(69), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0)) },
                    { new MidiNoteNumber(71), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(0)) },
                    { new MidiNoteNumber(72), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(0)) },
                    { new MidiNoteNumber(74), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(74), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1)) },
                    { new MidiNoteNumber(76), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(1)) },
                    { new MidiNoteNumber(77), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(1)) },
                    { new MidiNoteNumber(79), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(1)) },
                    { new MidiNoteNumber(81), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1)) },
                    { new MidiNoteNumber(83), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(1)) },
                    { new MidiNoteNumber(84), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(1)) },
                    { new MidiNoteNumber(86), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(1)) },
                }
            ];
            return new InstrumentNoteMap(mappings);
        }
    }

    public static InstrumentNoteMap DoubleOctaveDDorian
    {
        get
        {
            IReadOnlyList<IReadOnlyDictionary<MidiNoteNumber, GuildWarsNote>> mappings =
            [
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(62), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0)) },
                    { new MidiNoteNumber(64), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0)) },
                    { new MidiNoteNumber(65), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)) },
                    { new MidiNoteNumber(67), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(0)) },
                    { new MidiNoteNumber(69), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0)) },
                    { new MidiNoteNumber(71), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(0)) },
                    { new MidiNoteNumber(72), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(0)) },
                    { new MidiNoteNumber(74), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(74), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1)) },
                    { new MidiNoteNumber(76), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(1)) },
                    { new MidiNoteNumber(77), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(1)) },
                    { new MidiNoteNumber(79), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(1)) },
                    { new MidiNoteNumber(81), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1)) },
                    { new MidiNoteNumber(83), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(1)) },
                    { new MidiNoteNumber(84), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(1)) },
                    { new MidiNoteNumber(86), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(1)) },
                }
            ];
            return new InstrumentNoteMap(mappings);
        }
    }

    public static InstrumentNoteMap LowerDoubleOctaveCMajor
    {
        get
        {
            IReadOnlyList<IReadOnlyDictionary<MidiNoteNumber, GuildWarsNote>> mappings =
            [
                new Dictionary<MidiNoteNumber, GuildWarsNote>()
                {
                    { new MidiNoteNumber(36), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1)) },
                    { new MidiNoteNumber(38), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(-1)) },
                    { new MidiNoteNumber(40), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(-1)) },
                    { new MidiNoteNumber(41), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(-1)) },
                    { new MidiNoteNumber(43), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(-1)) },
                    { new MidiNoteNumber(45), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(-1)) },
                    { new MidiNoteNumber(47), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(-1)) },
                    { new MidiNoteNumber(48), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(-1)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>()
                {
                    { new MidiNoteNumber(48), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0)) },
                    { new MidiNoteNumber(50), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0)) },
                    { new MidiNoteNumber(52), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)) },
                    { new MidiNoteNumber(53), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(0)) },
                    { new MidiNoteNumber(55), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0)) },
                    { new MidiNoteNumber(57), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(0)) },
                    { new MidiNoteNumber(59), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(0)) },
                    { new MidiNoteNumber(60), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0)) },
                }
            ];
            return new InstrumentNoteMap(mappings);
        }
    }

    public static InstrumentNoteMap LowerTripleOctaveCMajor
    {
        get
        {
            IReadOnlyList<IReadOnlyDictionary<MidiNoteNumber, GuildWarsNote>> mappings =
            [
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(48 - 12), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1)) },
                    { new MidiNoteNumber(50 - 12), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(-1)) },
                    { new MidiNoteNumber(52 - 12), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(-1)) },
                    { new MidiNoteNumber(53 - 12), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(-1)) },
                    { new MidiNoteNumber(55 - 12), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(-1)) },
                    { new MidiNoteNumber(57 - 12), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(-1)) },
                    { new MidiNoteNumber(59 - 12), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(-1)) },
                    { new MidiNoteNumber(60 - 12), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(-1)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(60 - 12), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0)) },
                    { new MidiNoteNumber(62 - 12), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0)) },
                    { new MidiNoteNumber(64 - 12), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)) },
                    { new MidiNoteNumber(65 - 12), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(0)) },
                    { new MidiNoteNumber(67 - 12), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0)) },
                    { new MidiNoteNumber(69 - 12), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(0)) },
                    { new MidiNoteNumber(71 - 12), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(0)) },
                    { new MidiNoteNumber(72 - 12), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(72 - 12), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1)) },
                    { new MidiNoteNumber(74 - 12), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(1)) },
                    { new MidiNoteNumber(76 - 12), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(1)) },
                    { new MidiNoteNumber(77 - 12), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(1)) },
                    { new MidiNoteNumber(79 - 12), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1)) },
                    { new MidiNoteNumber(81 - 12), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(1)) },
                    { new MidiNoteNumber(83 - 12), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(1)) },
                    { new MidiNoteNumber(84 - 12), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(1)) },
                }
            ];
            return new InstrumentNoteMap(mappings);
        }
    }

    public static InstrumentNoteMap DoubleOctaveEMajor
    {
        get
        {
            IReadOnlyList<IReadOnlyDictionary<MidiNoteNumber, GuildWarsNote>> mappings =
            [
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(62 + 4), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0)) },
                    { new MidiNoteNumber(64 + 4), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0)) },
                    { new MidiNoteNumber(65 + 4), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)) },
                    { new MidiNoteNumber(67 + 4), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(0)) },
                    { new MidiNoteNumber(69 + 4), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0)) },
                    { new MidiNoteNumber(71 + 4), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(0)) },
                    { new MidiNoteNumber(72 + 4), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(0)) },
                    { new MidiNoteNumber(74 + 4), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(74 + 4), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1)) },
                    { new MidiNoteNumber(76 + 4), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(1)) },
                    { new MidiNoteNumber(77 + 4), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(1)) },
                    { new MidiNoteNumber(79 + 4), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(1)) },
                    { new MidiNoteNumber(81 + 4), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1)) },
                    { new MidiNoteNumber(83 + 4), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(1)) },
                    { new MidiNoteNumber(84 + 4), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(1)) },
                    { new MidiNoteNumber(86 + 4), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(1)) },
                }
            ];
            return new InstrumentNoteMap(mappings);
        }
    }

    public static InstrumentNoteMap TripleOctaveEMajor
    {
        get
        {
            IReadOnlyList<IReadOnlyDictionary<MidiNoteNumber, GuildWarsNote>> mappings =
            [
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(48 + 4), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(-1)) },
                    { new MidiNoteNumber(50 + 4), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(-1)) },
                    { new MidiNoteNumber(52 + 4), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(-1)) },
                    { new MidiNoteNumber(53 + 4), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(-1)) },
                    { new MidiNoteNumber(55 + 4), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(-1)) },
                    { new MidiNoteNumber(57 + 4), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(-1)) },
                    { new MidiNoteNumber(59 + 4), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(-1)) },
                    { new MidiNoteNumber(60 + 4), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(-1)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(62 + 4), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(0)) },
                    { new MidiNoteNumber(64 + 4), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(0)) },
                    { new MidiNoteNumber(65 + 4), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(0)) },
                    { new MidiNoteNumber(67 + 4), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(0)) },
                    { new MidiNoteNumber(69 + 4), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(0)) },
                    { new MidiNoteNumber(71 + 4), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(0)) },
                    { new MidiNoteNumber(72 + 4), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(0)) },
                    { new MidiNoteNumber(74 + 4), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(0)) },
                },
                new Dictionary<MidiNoteNumber, GuildWarsNote>
                {
                    { new MidiNoteNumber(74 + 4), new GuildWarsNote(GuildWarsNoteKey.Note1, new Octave(1)) },
                    { new MidiNoteNumber(76 + 4), new GuildWarsNote(GuildWarsNoteKey.Note2, new Octave(1)) },
                    { new MidiNoteNumber(77 + 4), new GuildWarsNote(GuildWarsNoteKey.Note3, new Octave(1)) },
                    { new MidiNoteNumber(79 + 4), new GuildWarsNote(GuildWarsNoteKey.Note4, new Octave(1)) },
                    { new MidiNoteNumber(81 + 4), new GuildWarsNote(GuildWarsNoteKey.Note5, new Octave(1)) },
                    { new MidiNoteNumber(83 + 4), new GuildWarsNote(GuildWarsNoteKey.Note6, new Octave(1)) },
                    { new MidiNoteNumber(84 + 4), new GuildWarsNote(GuildWarsNoteKey.Note7, new Octave(1)) },
                    { new MidiNoteNumber(86 + 4), new GuildWarsNote(GuildWarsNoteKey.Note8, new Octave(1)) },
                }
            ];
            return new InstrumentNoteMap(mappings);
        }
    }
}