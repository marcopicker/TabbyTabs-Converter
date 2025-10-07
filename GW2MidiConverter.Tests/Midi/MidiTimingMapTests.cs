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

        int beatDelta = midiFile.DeltaTicksPerQuarterNote * 4;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(map.GetTimeSignatureAtTick(0 * beatDelta), Is.EqualTo(new TimeSignature(4, 4)));
            Assert.That(map.GetTimeSignatureAtTick(1 * beatDelta), Is.EqualTo(new TimeSignature(3, 4)));
            Assert.That(map.GetTimeSignatureAtTick(2 * beatDelta), Is.EqualTo(new TimeSignature(4, 4)));
            Assert.That(map.GetTimeSignatureAtTick(3 * beatDelta), Is.EqualTo(new TimeSignature(4, 4)));
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

    [Test]
    public void GetBarAtTick_InvalidTick_Throws()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.TimingTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);
        Assert.Throws<ArgumentOutOfRangeException>(() => map.GetTimeAtTick(-1));
    }
    
    [Test]
    public void GetBarAtTick_ValidTests()
    {
        MidiFile midiFile = new MidiFile(MidiFiles.TimingTestFile);
        MidiTimingMap map = MidiTimingMap.FromMidiFile(midiFile);

        int quarterNote = midiFile.DeltaTicksPerQuarterNote;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(map.GetTimeAtTick(0 * quarterNote), Is.EqualTo(new SongTime(1, 1)));
            Assert.That(map.GetTimeAtTick(1 * quarterNote), Is.EqualTo(new SongTime(1, 2)));
            Assert.That(map.GetTimeAtTick(2 * quarterNote), Is.EqualTo(new SongTime(1, 3)));
            Assert.That(map.GetTimeAtTick((int)(2.5f * quarterNote)), Is.EqualTo(new SongTime(1, 3)));
            Assert.That(map.GetTimeAtTick(3 * quarterNote), Is.EqualTo(new SongTime(1, 4)));

            Assert.That(map.GetTimeAtTick(4 * quarterNote), Is.EqualTo(new SongTime(2, 1)));
            Assert.That(map.GetTimeAtTick(5 * quarterNote), Is.EqualTo(new SongTime(2, 2)));
            Assert.That(map.GetTimeAtTick(6 * quarterNote), Is.EqualTo(new SongTime(2, 3)));
            
            Assert.That(map.GetTimeAtTick(7 * quarterNote), Is.EqualTo(new SongTime(3, 1)));
        }
    }
}