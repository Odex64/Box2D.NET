using System;
using System.Diagnostics;
using System.Linq;
using Box2D.NET.Common;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Collisions.Manifolds;

/// <summary>
/// A manifold for two touching convex shapes.
/// Box2D supports multiple types of contact:
/// - Clip point versus plane with radius
/// - Point versus point with radius (circles)
/// The local point usage depends on the manifold type:
/// - <see cref="ManifoldType.Circles" />: The local center of circleA.
/// - <see cref="ManifoldType.FaceA" />: The center of faceA.
/// - <see cref="ManifoldType.FaceB" />: The center of faceB.
/// Similarly, the local normal usage:
/// - <see cref="ManifoldType.Circles" />: Not used.
/// - <see cref="ManifoldType.FaceA" />: The normal on polygonA.
/// - <see cref="ManifoldType.FaceB" />: The normal on polygonB.
/// We store contacts in this way so that position correction can
/// account for movement, which is critical for continuous physics.
/// All contact scenarios must be expressed in one of these types.
/// This structure is stored across time steps, so we keep it small.
/// </summary>
public struct Manifold : IEquatable<Manifold>
{
    /// <summary>
    /// The points of contact.
    /// </summary>
    public ManifoldPoint[] Points;

    /// <summary>
    /// Not used for <see cref="ManifoldType.Circles" />.
    /// </summary>
    public Vector2 LocalNormal;

    /// <summary>
    /// Usage depends on manifold type.
    /// </summary>
    public Vector2 LocalPoint;

    /// <summary>
    /// The type of manifold.
    /// </summary>
    public ManifoldType Type;

    /// <summary>
    /// Initializes a new instance of the <see cref="Manifold" /> struct.
    /// </summary>
    /// <param name="points">The points of contact.</param>
    /// <param name="localNormal">The local normal.</param>
    /// <param name="localPoint">The local point.</param>
    /// <param name="type">The manifold type.</param>
    public Manifold(in ManifoldPoint[] points, in Vector2 localNormal, in Vector2 localPoint, in ManifoldType type)
    {
        Debug.Assert(points.Length <= Constants.MaxManifoldPoints);
        Points = points;
        LocalNormal = localNormal;
        LocalPoint = localPoint;
        Type = type;
    }

    public Manifold() => Points = new ManifoldPoint[Constants.MaxManifoldPoints];

    /// <inheritdoc />
    public bool Equals(Manifold other) =>
        Points.SequenceEqual(other.Points) &&
        LocalNormal.Equals(other.LocalNormal) &&
        LocalPoint.Equals(other.LocalPoint) &&
        Type == other.Type;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Manifold other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(LocalNormal);
        hash.Add(LocalPoint);
        hash.Add(Type);

        foreach (ManifoldPoint point in Points)
        {
            hash.Add(point);
        }

        return hash.ToHashCode();
    }

    /// <inheritdoc />
    public override string ToString() =>
        $"(Points: [{string.Join(", ", Points)}], LocalNormal: {LocalNormal}, LocalPoint: {LocalPoint}, Type: {Type})";

    /// <summary>
    /// Checks if two <see cref="Manifold" /> instances are equal.
    /// </summary>
    public static bool operator ==(in Manifold left, in Manifold right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="Manifold" /> instances are not equal.
    /// </summary>
    public static bool operator !=(in Manifold left, in Manifold right) => !(left == right);
}