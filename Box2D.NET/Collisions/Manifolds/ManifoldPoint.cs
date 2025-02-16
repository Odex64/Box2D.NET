using System;
using Box2D.NET.Collisions.Contacts;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Collisions.Manifolds;

/// <summary>
/// A manifold point is a contact point belonging to a contact manifold.
/// It holds details related to the geometry and dynamics of the contact points.
/// The local point usage depends on the manifold type:
/// - Circles: the local center of circleB
/// - FaceA: the local center of circleB or the clip point of polygonB
/// - FaceB: the clip point of polygonA
/// This structure is stored across time steps, so we keep it small.
/// Note: the impulses are used for internal caching and may not
/// provide reliable contact forces, especially for high-speed collisions.
/// </summary>
public struct ManifoldPoint : IEquatable<ManifoldPoint>
{
    /// <summary>
    /// Usage depends on manifold type.
    /// </summary>
    public Vector2 LocalPoint;

    /// <summary>
    /// The non-penetration impulse.
    /// </summary>
    public float NormalImpulse;

    /// <summary>
    /// The friction impulse.
    /// </summary>
    public float TangentImpulse;

    /// <summary>
    /// Uniquely identifies a contact point between two shapes.
    /// </summary>
    public ContactId Id;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManifoldPoint" /> struct.
    /// </summary>
    /// <param name="localPoint">Usage depends on manifold type.</param>
    /// <param name="normalImpulse">The non-penetration impulse.</param>
    /// <param name="tangentImpulse">The friction impulse.</param>
    /// <param name="id">Uniquely identifies a contact point between two shapes.</param>
    public ManifoldPoint(Vector2 localPoint, float normalImpulse, float tangentImpulse, ContactId id)
    {
        LocalPoint = localPoint;
        NormalImpulse = normalImpulse;
        TangentImpulse = tangentImpulse;
        Id = id;
    }

    /// <inheritdoc />
    public bool Equals(ManifoldPoint other) =>
        LocalPoint.Equals(other.LocalPoint) &&
        NormalImpulse.Equals(other.NormalImpulse) &&
        TangentImpulse.Equals(other.TangentImpulse) &&
        Id.Equals(other.Id);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ManifoldPoint other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(LocalPoint, NormalImpulse, TangentImpulse, Id);

    /// <inheritdoc />
    public override string ToString() =>
        $"(LocalPoint: {LocalPoint}, NormalImpulse: {NormalImpulse}, TangentImpulse: {TangentImpulse}, Id: {Id})";

    /// <summary>
    /// Checks if two ManifoldPoint are equal.
    /// </summary>
    public static bool operator ==(ManifoldPoint left, ManifoldPoint right) => left.Equals(right);

    /// <summary>
    /// Checks if two ManifoldPoint are not equal.
    /// </summary>
    public static bool operator !=(ManifoldPoint left, ManifoldPoint right) => !(left == right);
}