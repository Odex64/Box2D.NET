using System;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Collisions.RayCasts;

/// <summary>
/// Contains output data for a ray-cast operation.
/// The ray hits at <c>P1 + Fraction * (P2 - P1)</c>, where <c>P1</c> and <c>P2</c> come from <see cref="RayCastInput" />.
/// </summary>
public struct RayCastOutput : IEquatable<RayCastOutput>
{
    /// <summary>
    /// The normal of the surface at the contact point.
    /// </summary>
    public Vector2 Normal;

    /// <summary>
    /// The fraction of the ray length at which the contact occurred.
    /// </summary>
    public float Fraction;

    /// <summary>
    /// Initializes a new instance of the <see cref="RayCastOutput" /> struct.
    /// </summary>
    /// <param name="normal">The normal of the surface at the contact point.</param>
    /// <param name="fraction">The fraction of the ray length at which the contact occurred.</param>
    public RayCastOutput(in Vector2 normal, float fraction)
    {
        Normal = normal;
        Fraction = fraction;
    }

    /// <inheritdoc />
    public bool Equals(RayCastOutput other) => Normal.Equals(other.Normal) && Fraction.Equals(other.Fraction);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is RayCastOutput other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Normal, Fraction);

    /// <inheritdoc />
    public override string ToString() => $"(Normal: {Normal}, Fraction: {Fraction})";

    /// <summary>
    /// Checks if two <see cref="RayCastOutput" /> instances are equal.
    /// </summary>
    public static bool operator ==(in RayCastOutput left, in RayCastOutput right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="RayCastOutput" /> instances are not equal.
    /// </summary>
    public static bool operator !=(in RayCastOutput left, in RayCastOutput right) => !(left == right);
}