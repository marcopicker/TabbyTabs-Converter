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
    public void GetPositionAtTick_Common_Time_ValidTests()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.TimingTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);

        int quarterNote = midiFile.DeltaTicksPerQuarterNote;

        using (Assert.EnterMultipleScope())
        {
            // 4/4 Time
            Assert.That(map.GetPositionAtTick(0 * quarterNote), Is.EqualTo(new SongPosition(1, 1, 0)));
            Assert.That(map.GetPositionAtTick(1 * quarterNote), Is.EqualTo(new SongPosition(1, 2, 0)));
            Assert.That(map.GetPositionAtTick(2 * quarterNote), Is.EqualTo(new SongPosition(1, 3, 0)));
            Assert.That(map.GetPositionAtTick((int)(2.5f * quarterNote)), Is.EqualTo(new SongPosition(1, 3, 0.5m)));
            Assert.That(map.GetPositionAtTick(3 * quarterNote), Is.EqualTo(new SongPosition(1, 4, 0)));

            // 3/3 Time
            Assert.That(map.GetPositionAtTick(4 * quarterNote), Is.EqualTo(new SongPosition(2, 1, 0)));
            Assert.That(map.GetPositionAtTick(5 * quarterNote), Is.EqualTo(new SongPosition(2, 2, 0)));
            Assert.That(map.GetPositionAtTick(6 * quarterNote), Is.EqualTo(new SongPosition(2, 3, 0)));

            // Back to 4/4
            Assert.That(map.GetPositionAtTick(7 * quarterNote), Is.EqualTo(new SongPosition(3, 1, 0)));
        }
    }

    // Going to be mega thorough.
    [TestCase(00, 1, 1)]
    [TestCase(01, 1, 2)]
    [TestCase(02, 1, 3)]
    [TestCase(03, 1, 4)]
    [TestCase(04, 1, 5)]
    [TestCase(05, 1, 6)]
    [TestCase(06, 1, 7)]
    [TestCase(07, 1, 8)]
    [TestCase(08, 1, 9)]
    [TestCase(09, 2, 1)]
    [TestCase(10, 2, 2)]
    [TestCase(11, 2, 3)]
    [TestCase(12, 2, 4)]
    [TestCase(13, 3, 1)]
    [TestCase(14, 3, 2)]
    [TestCase(15, 3, 3)]
    [TestCase(16, 3, 4)]
    [TestCase(17, 3, 5)]
    [TestCase(18, 3, 6)]
    [TestCase(19, 4, 1)]
    [TestCase(20, 4, 2)]
    [TestCase(21, 4, 3)]
    [TestCase(22, 4, 4)]
    [TestCase(23, 4, 5)]
    public void GetPositionAtTick_OddTimeSignatures(decimal eighths, int expectedMeasure, decimal expectedBeat)
    {
        MidiFile midiFile = new MidiFile(MidiFiles.OddTimeSignatures);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);

        SongPosition expectedPosition = new SongPosition(expectedMeasure, expectedBeat);

        long eighthNote = midiFile.DeltaTicksPerQuarterNote / 2;
        long ticks = (long)(eighthNote * eighths);

        Assert.That(map.GetPositionAtTick(ticks), Is.EqualTo(expectedPosition));
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

    [Test]
    public void Measures_OddTimeSignatures_CorrectMeasures()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.OddTimeSignatures);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(map.Measures, Has.Length.EqualTo(4));
            Assert.That(map.Measures[0].TimeSignature, Is.EqualTo(new TimeSignature(9, 8)));
            Assert.That(map.Measures[1].TimeSignature, Is.EqualTo(new TimeSignature(4, 8)));
            Assert.That(map.Measures[2].TimeSignature, Is.EqualTo(new TimeSignature(6, 8)));
            Assert.That(map.Measures[3].TimeSignature, Is.EqualTo(new TimeSignature(5, 8)));
        }
    }
}