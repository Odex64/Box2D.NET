using System;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Shapes;

/// <summary>
/// This holds the mass data computed for a shape.
/// </summary>
public struct MassData : IEquatable<MassData>
{
    /// <summary>
    /// The mass of the shape, usually in kilograms.
    /// </summary>
    public float Mass;

    /// <summary>
    /// The position of the shape's centroid relative to the shape's origin.
    /// </summary>
    public Vector2 Center;

    /// <summary>
    /// The rotational inertia of the shape about the local origin.
    /// </summary>
    public float Inertia;

    /// <summary>
    /// Initializes a new instance of the <see cref="MassData"/> struct.
    /// </summary>
    /// <param name="mass">The mass of the shape, usually in kilograms.</param>
    /// <param name="center">The position of the shape's centroid relative to the shape's origin.</param>
    /// <param name="inertia">The rotational inertia of the shape about the local origin.</param>
    public MassData(float mass, in Vector2 center, float inertia)
    {
        Mass = mass;
        Center = center;
        Inertia = inertia;
    }

    /// <inheritdoc />
    public bool Equals(MassData other) => Mass.Equals(other.Mass) && Center.Equals(other.Center) && Inertia.Equals(other.Inertia);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is MassData other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Mass, Center, Inertia);

    /// <inheritdoc />
    public override string ToString() => $"(Mass: {Mass}, Center: {Center}, Inertia: {Inertia})";

    /// <summary>
    /// Checks if two MassData are equal.
    /// </summary>
    public static bool operator ==(in MassData left, in MassData right) => left.Equals(right);

    /// <summary>
    /// Checks if two MassData are not equal.
    /// </summary>
    public static bool operator !=(in MassData left, in MassData right) => !left.Equals(right);
}