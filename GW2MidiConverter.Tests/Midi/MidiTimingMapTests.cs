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
}