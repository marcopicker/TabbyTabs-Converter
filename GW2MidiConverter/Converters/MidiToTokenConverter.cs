using System;
using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tokens;
using NAudio.Midi;

namespace GW2MidiConverter;

public class MidiToTokenConverter
{
    private readonly TokenConverterSettings _settings;
    private readonly MidiDocument _midiDocument;
    private readonly MidiTimingMap _timingMap;

    public MidiToTokenConverter(MidiDocument midiDocument, TokenConverterSettings? settings = null)
    {
        ArgumentNullException.ThrowIfNull(midiDocument);
        _midiDocument = midiDocument;
        _timingMap = MidiTimingMap.FromMidiFile(_midiDocument.GetRawMidiFile());
        _settings = settings ?? new TokenConverterSettings();
    }

    public Dictionary<int, List<Token>> ConvertTracks(params int[] trackIndexes)
    {
        if(trackIndexes.Length == 0) throw new ArgumentException("At least one track index must be specified.", nameof(trackIndexes));
        List<int> tracks = trackIndexes.Where(i => _midiDocument.InstrumentTrackIndexes.Contains(i)).ToList();
        return tracks.ToDictionary(track => track, ConvertTrack);
    }

    private List<Token> ConvertTrack(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= _midiDocument.GetRawMidiFile().Tracks) throw new IndexOutOfRangeException(nameof(trackIndex));
        List<MidiEvent>? events = _midiDocument.GetRawMidiFile().Events?.GetTrackEvents(trackIndex).ToList();
        if (events == null) throw new Exception("Could not get events for track.");

        // We really only care about NoteOn events.
        List<MidiEvent> relevantEvents = events.Where(e => e is NoteOnEvent).ToList();
        
        List<Token> tokens = [];
        foreach (MidiMeasureSpan measure in _timingMap.Measures)
        {
            tokens.AddRange(ConvertMeasure(measure, relevantEvents));
        }

        tokens.Add(CreateClosingMeasureToken());
        tokens = AddRests(tokens);
        return tokens;
    }

    private List<Token> ConvertMeasure(MidiMeasureSpan midiMeasureSpan, List<MidiEvent> events)
    {
        List<long> eventTimes = events.Where(e => e.AbsoluteTime >= midiMeasureSpan.StartTick && e.AbsoluteTime < midiMeasureSpan.EndTick)
                                      .Select(e => e.AbsoluteTime)
                                      .Distinct().Order().ToList();
        List<Token> tokens =
        [
            // Always start with a measure token.
            new MeasureToken(midiMeasureSpan.StartTick, midiMeasureSpan.StartTick, midiMeasureSpan.TimeSignature)
        ];
        
        foreach (long time in eventTimes)
        {
            Token? token = CreateTokenForTick(time, events.Where(e => e.AbsoluteTime == time).ToList());
            if (token == null) continue;
            tokens.Add(token);
        }

        return tokens;
    }

    private Token? CreateTokenForTick(long tick, List<MidiEvent> events)
    {
        List<NoteOnEvent> noteOnEvents = events.OfType<NoteOnEvent>().ToList();
        // Shouldn't happen for now, but I'm paranoid.
        if(noteOnEvents.Count == 0) return null;
        
        bool areNotesOfSameLength = noteOnEvents.All(e => e.NoteLength == noteOnEvents.First().NoteLength);
        if (!areNotesOfSameLength)
        {
            // TODO MP: Add warning.
        }
        
        // For chords, we can use any of the played notes for time information.
        NoteOnEvent noteOnEvent = noteOnEvents.First();
        int[] pitches = noteOnEvents.Select(e => e.NoteNumber).Distinct().Order().ToArray();
        NoteToken token = new NoteToken(noteOnEvent.AbsoluteTime, noteOnEvent.AbsoluteTime + noteOnEvent.NoteLength, pitches, NoteLength.FromQuarterNotes(1));
        return token;
    }

    private MeasureToken CreateClosingMeasureToken()
    {
        return new MeasureToken(_timingMap.Measures.Last().EndTick, _timingMap.Measures.Last().EndTick, null);
    }

    private List<Token> AddRests(List<Token> tokens)
    {
        List<Token> output = [];

        // The idea is to check if any token's end aligns with the start of the next one.
        // For the last token, we don't need to check.
        for (int i = 0; i < tokens.Count - 1; i++)
        {
            Token token = tokens[i];
            output.Add(token);
            
            Token nextToken = tokens[i + 1]; // Safe, since we ignore the last element.

            if (token.EndTime == nextToken.StartTime) continue;
            // TODO MP: Use actual time.
            RestToken restToken = new RestToken(token.EndTime, nextToken.StartTime);
            output.Add(restToken);
        }

        if (tokens.Count > 1) output.Add(tokens.Last());
        return output;
    }
}