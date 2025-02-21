using System;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Collisions.RayCasts;

/// <summary>
/// Contains input data for a ray-cast operation.
/// The ray extends from <see cref="Start" /> to <see cref="Start" /> + <see cref="MaxFraction" /> * (<see cref="End" /> - <see cref="Start" />).
/// </summary>
public struct RayCastInput : IEquatable<RayCastInput>
{
    /// <summary>
    /// The starting point of the ray.
    /// </summary>
    public Vector2 Start;

    /// <summary>
    /// The ending point of the ray.
    /// </summary>
    public Vector2 End;

    /// <summary>
    /// The maximum fraction of the ray length to consider for intersections.
    /// </summary>
    public float MaxFraction;

    /// <summary>
    /// Initializes a new instance of the <see cref="RayCastInput" /> struct.
    /// </summary>
    /// <param name="start">The starting point of the ray.</param>
    /// <param name="end">The ending point of the ray.</param>
    /// <param name="maxFraction">The maximum fraction of the ray length.</param>
    public RayCastInput(in Vector2 start, in Vector2 end, float maxFraction)
    {
        Start = start;
        End = end;
        MaxFraction = maxFraction;
    }

    /// <inheritdoc />
    public bool Equals(RayCastInput other) => Start.Equals(other.Start) && End.Equals(other.End) && MaxFraction.Equals(other.MaxFraction);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is RayCastInput other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Start, End, MaxFraction);

    /// <inheritdoc />
    public override string ToString() => $"(P1: {Start}, P2: {End}, MaxFraction: {MaxFraction})";

    /// <summary>
    /// Checks if two <see cref="RayCastInput" /> instances are equal.
    /// </summary>
    public static bool operator ==(in RayCastInput left, in RayCastInput right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="RayCastInput" /> instances are not equal.
    /// </summary>
    public static bool operator !=(in RayCastInput left, in RayCastInput right) => !left.Equals(right);
}