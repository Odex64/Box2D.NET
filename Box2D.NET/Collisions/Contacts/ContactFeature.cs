using System;

namespace Box2D.NET.Collisions.Contacts;

/// <summary>
/// The features that intersect to form the contact point.
/// This must be 4 bytes or less.
/// </summary>
public struct ContactFeature : IEquatable<ContactFeature>
{
    /// <summary>
    /// Feature index on shape A.
    /// </summary>
    public byte IndexA;

    /// <summary>
    /// Feature index on shape B.
    /// </summary>
    public byte IndexB;

    /// <summary>
    /// The feature type on shape A.
    /// </summary>
    public ContactFeatureType TypeA;

    /// <summary>
    /// The feature type on shape B.
    /// </summary>
    public ContactFeatureType TypeB;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactFeature" /> struct.
    /// </summary>
    /// <param name="indexA">Feature index on shape A.</param>
    /// <param name="indexB">Feature index on shape B.</param>
    /// <param name="typeA">The feature type on shape A.</param>
    /// <param name="typeB">The feature type on shape B.</param>
    public ContactFeature(byte indexA, byte indexB, ContactFeatureType typeA, ContactFeatureType typeB)
    {
        IndexA = indexA;
        IndexB = indexB;
        TypeA = typeA;
        TypeB = typeB;
    }

    /// <inheritdoc />
    public bool Equals(ContactFeature other) =>
        IndexA == other.IndexA &&
        IndexB == other.IndexB &&
        TypeA == other.TypeA &&
        TypeB == other.TypeB;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ContactFeature other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(IndexA, IndexB, TypeA, TypeB);

    /// <inheritdoc />
    public override string ToString() => $"(IndexA: {IndexA}, IndexB: {IndexB}, TypeA: {TypeA}, TypeB: {TypeB})";

    /// <summary>
    /// Check if two ContactFeature are equal.
    /// </summary>
    public static bool operator ==(in ContactFeature left, in ContactFeature right) => left.Equals(right);

    /// <summary>
    /// Checks if two ContactFeature are not equal.
    /// </summary>
    public static bool operator !=(in ContactFeature left, in ContactFeature right) => !(left == right);
}