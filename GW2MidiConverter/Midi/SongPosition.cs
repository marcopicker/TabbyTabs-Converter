using System;

namespace GW2MidiConverter.Midi;

/// <summary>
/// Represents a single distinct point in time within a musical piece with a <see cref="Measure"/>, <see cref="Beat"/> and <see cref="BeatFraction"/> property.
/// </summary>
public readonly struct SongPosition : IEquatable<SongPosition>, IComparable<SongPosition>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="SongPosition"/> struct.
    /// </summary>
    /// <param name="measure">The measure of the position within the piece.</param>
    /// <param name="beat">The beat within the <paramref name="measure"/>, starting at 1.</param>
    /// <param name="beatFraction">The fraction within the <paramref name="beat"/>, ranging from 0 to 0.99.</param>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="beat"/> is &lt; 1.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="beatFraction"/> is &lt; 0 or ≥ 1.</exception>
    public SongPosition(int measure, int beat, decimal beatFraction = 0m)
    {
        // Negative measures are allowed, but negative beats and fractions do not make sense.
        ArgumentOutOfRangeException.ThrowIfLessThan(beat, 1, nameof(beat));
        ArgumentOutOfRangeException.ThrowIfLessThan(beatFraction, 0, nameof(beatFraction));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(beatFraction, 1, nameof(beatFraction));

        Measure = measure;
        Beat = beat;
        BeatFraction = beatFraction;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SongPosition"/> struct.
    /// </summary>
    /// <param name="measure">The measure of the position within the piece.</param>
    /// <param name="beat">The beat within the <paramref name="measure"/>, starting at 1, including the beat-fraction.</param>
    public SongPosition(int measure, decimal beat)
    {
        // Negative measures are allowed, but negative beats and fractions do not make sense.
        ArgumentOutOfRangeException.ThrowIfLessThan(beat, 1, nameof(beat));
        
        Measure = measure;
        Beat = (int) beat;
        BeatFraction = beat - Beat;
    }

    /// <summary>
    /// The measure of the position in the piece.
    /// </summary>
    public int Measure { get; } = 0;

    /// <summary>
    /// The beat within the <see cref="Measure"/>.
    /// </summary>
    public int Beat { get; } = 1;

    /// <summary>
    /// The fraction within the <see cref="Beat"/>, ranging from 0 to 0.99.
    /// </summary>
    public decimal BeatFraction { get; } = 0;

    /// <summary>
    /// Transforms the <see cref="SongPosition"/> object into a readable string.
    /// </summary>
    /// <example><c>2:1:55</c></example>
    /// <returns>The <see cref="SongPosition"/> with the format "<see cref="Measure"/>:<see cref="Beat"/>:<see cref="BeatFraction"/>", where the fraction is represented by a double-digit integer like "00", "55", or "95".</returns>
    public override string ToString() => $"{Measure}:{Beat}:{BeatFraction*100:00}";
    
    #region Equality Comparison

    public override bool Equals(object? obj)
    {
        return obj is SongPosition other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Measure, Beat, BeatFraction);
    }

    public bool Equals(SongPosition other)
    {
        return Measure == other.Measure &&
               Beat == other.Beat &&
               BeatFraction == other.BeatFraction;
    }

    public int CompareTo(SongPosition other)
    {
        int result = Measure.CompareTo(other.Measure);
        if (result == 0) result = Beat.CompareTo(other.Beat);
        if (result == 0) result = BeatFraction.CompareTo(other.BeatFraction);
        return result;
    }

    public static bool operator ==(SongPosition left, SongPosition right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SongPosition left, SongPosition right)
    {
        return !(left == right);
    }

    public static bool operator <(SongPosition left, SongPosition right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(SongPosition left, SongPosition right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(SongPosition left, SongPosition right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(SongPosition left, SongPosition right)
    {
        return left.CompareTo(right) >= 0;
    }

    #endregion
}