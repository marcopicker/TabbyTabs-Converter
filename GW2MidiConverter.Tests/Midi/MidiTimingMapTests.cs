using System;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tests.Resources;
using NAudio.Midi;
using NUnit.Framework;

namespace GW2MidiConverter.Tests.Midi;

[TestFixture]
public class MidiTimingMapTests
{
    #region Loading

    [Test]
    public void FromMidiFile_ValidFile_ReturnsMap()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.TimingTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);
        Assert.That(map, Is.Not.Null);
    }

    [Test]
    public void FromMidiFile_FileIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => MidiTimingMap.FromMidiFile(null!));
    }

    #endregion

    #region Time Signatures

    [Test]
    public void GetTimeSignatureAtTick_ReturnsTimeSignature()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.TimingTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);

        int resolution = midiFile.DeltaTicksPerQuarterNote;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(map.GetTimeSignatureAtTick(0 * resolution), Is.EqualTo(new TimeSignature(4, 4)));
            Assert.That(map.GetTimeSignatureAtTick(4 * resolution), Is.EqualTo(new TimeSignature(3, 4)));
            Assert.That(map.GetTimeSignatureAtTick(7 * resolution), Is.EqualTo(new TimeSignature(4, 4)));
            Assert.That(map.GetTimeSignatureAtTick(11 * resolution), Is.EqualTo(new TimeSignature(4, 4)));
        }
    }

    [Test]
    public void GetTimeSignatureAtTick_InvalidTick_Throws()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.TimingTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);
        Assert.Throws<ArgumentOutOfRangeException>(() => map.GetTimeSignatureAtTick(-1));
    }

    #endregion

    #region Tempo

    [Test]
    public void GetTempoAtTick_ReturnsTempo()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.TimingTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);

        int beatDelta = midiFile.DeltaTicksPerQuarterNote;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(map.GetTempoAtTick(0 * beatDelta), Is.EqualTo(120).Within(0.005d));
            Assert.That(map.GetTempoAtTick(4 * beatDelta), Is.EqualTo(120).Within(0.005d));
            Assert.That(map.GetTempoAtTick(7 * beatDelta), Is.EqualTo(120).Within(0.005d));
            Assert.That(map.GetTempoAtTick(11 * beatDelta), Is.EqualTo(90).Within(0.005d));
        }
    }

    [Test]
    public void GetTempoAtTick_InvalidTick_Throws()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.TimingTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);
        Assert.Throws<ArgumentOutOfRangeException>(() => map.GetTempoAtTick(-1));
    }

    #endregion

    #region GetPositionAtTick

    [Test]
    public void GetPositionAtTick_InvalidTick_Throws()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.TimingTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);
        Assert.Throws<ArgumentOutOfRangeException>(() => map.GetPositionAtTick(-1));
    }

    [Test]
    public void GetPositionAtTick_ValidTests()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.TimingTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);

        int quarterNote = midiFile.DeltaTicksPerQuarterNote;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(map.GetPositionAtTick(0 * quarterNote), Is.EqualTo(new SongPosition(1, 1, 0)));
            Assert.That(map.GetPositionAtTick(1 * quarterNote), Is.EqualTo(new SongPosition(1, 2, 0)));
            Assert.That(map.GetPositionAtTick(2 * quarterNote), Is.EqualTo(new SongPosition(1, 3, 0)));
            Assert.That(map.GetPositionAtTick((int)(2.5f * quarterNote)), Is.EqualTo(new SongPosition(1, 3, quarterNote / 2)));
            Assert.That(map.GetPositionAtTick(3 * quarterNote), Is.EqualTo(new SongPosition(1, 4, 0)));

            Assert.That(map.GetPositionAtTick(4 * quarterNote), Is.EqualTo(new SongPosition(2, 1, 0)));
            Assert.That(map.GetPositionAtTick(5 * quarterNote), Is.EqualTo(new SongPosition(2, 2, 0)));
            Assert.That(map.GetPositionAtTick(6 * quarterNote), Is.EqualTo(new SongPosition(2, 3, 0)));

            Assert.That(map.GetPositionAtTick(7 * quarterNote), Is.EqualTo(new SongPosition(3, 1, 0)));
        }
    }

    #endregion

    [Test]
    public void Measures_MidiIsEmpty_MeasuresIsEmpty()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.EmptyMidiFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);
        Assert.That(map.Measures, Is.Empty);
    }

    [Test]
    public void Measures_MidiIsNotEmpty_HasCorrectAmountOfElements()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.LengthTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);
        Assert.That(map.Measures, Has.Length.EqualTo(4));
    }

    [Test]
    public void Measures_CorrectMeasures()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.TimingTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);

        long resolution = midiFile.DeltaTicksPerQuarterNote;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(map.Measures, Has.Length.EqualTo(4));
            Assert.That(map.Measures[0], Is.EqualTo(new MidiMeasureInfo(resolution * 00, resolution * 04, 0, new TimeSignature(4, 4))));
            Assert.That(map.Measures[1], Is.EqualTo(new MidiMeasureInfo(resolution * 04, resolution * 07, 1, new TimeSignature(3, 4))));
            Assert.That(map.Measures[2], Is.EqualTo(new MidiMeasureInfo(resolution * 07, resolution * 11, 2, new TimeSignature(4, 4))));
            Assert.That(map.Measures[3], Is.EqualTo(new MidiMeasureInfo(resolution * 11, resolution * 15, 3, new TimeSignature(4, 4))));
        }
    }
}