using System;
using Box2D.NET.Collisions.Contacts;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Collisions.Misc;

/// <summary>
/// Represents a vertex used for computing contact manifolds.
/// </summary>
public struct ClipVertex : IEquatable<ClipVertex>
{
    /// <summary>
    /// The vertex position.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// Contact ID associated with the vertex.
    /// </summary>
    public ContactId Id;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClipVertex" /> struct.
    /// </summary>
    /// <param name="position">The vertex position.</param>
    /// <param name="id">The contact ID associated with the vertex.</param>
    public ClipVertex(in Vector2 position, in ContactId id)
    {
        Position = position;
        Id = id;
    }

    /// <inheritdoc />
    public bool Equals(ClipVertex other) => Position.Equals(other.Position) && Id.Equals(other.Id);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ClipVertex other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Position, Id);

    /// <inheritdoc />
    public override string ToString() => $"(V: {Position}, Id: {Id})";

    /// <summary>
    /// Checks if two <see cref="ClipVertex" /> instances are equal.
    /// </summary>
    public static bool operator ==(in ClipVertex left, in ClipVertex right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="ClipVertex" /> instances are not equal.
    /// </summary>
    public static bool operator !=(in ClipVertex left, in ClipVertex right) => !(left == right);
}