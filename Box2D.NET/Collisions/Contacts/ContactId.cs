using System;

namespace Box2D.NET.Collisions.Contacts;

/// <summary>
/// Contact ids to facilitate warm starting.
/// </summary>
public struct ContactId : IEquatable<ContactId>
{
    /// <summary>
    /// The contact feature.
    /// </summary>
    public ContactFeature Feature;

    /// <summary>
    /// Used to quickly compare contact ids.
    /// </summary>
    public uint Key;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactId" /> struct.
    /// </summary>
    public ContactId(in ContactFeature feature, uint key)
    {
        Feature = feature;
        Key = key;
    }

    /// <inheritdoc />
    public bool Equals(ContactId other) => Feature.Equals(other.Feature) && Key == other.Key;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ContactId other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Feature, Key);

    /// <inheritdoc />
    public override string ToString() => $"(Feature: {Feature}, Key: {Key})";

    /// <summary>
    /// Checks if two ContactId are equal.
    /// </summary>
    public static bool operator ==(in ContactId left, in ContactId right) => left.Equals(right);

    /// <summary>
    /// Checks if two ContactId are not equal.
    /// </summary>
    public static bool operator !=(in ContactId left, in ContactId right) => !left.Equals(right);
}