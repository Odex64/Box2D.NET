using System;
using System.Diagnostics;
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
    /// The maximum number of manifold points.
    /// </summary>
    public const int MaxManifoldPoints = 2;

    /// <summary>
    /// The points of contact.
    /// </summary>
    public readonly ManifoldPoint[] Points;

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
    /// The number of manifold points.
    /// </summary>
    public int PointCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="Manifold" /> struct.
    /// </summary>
    /// <param name="points">The points of contact.</param>
    /// <param name="localNormal">The local normal.</param>
    /// <param name="localPoint">The local point.</param>
    /// <param name="type">The manifold type.</param>
    /// <param name="pointCount">The number of manifold points.</param>
    public Manifold(ManifoldPoint[] points, Vector2 localNormal, Vector2 localPoint, ManifoldType type, int pointCount)
    {
        Debug.Assert(points.Length <= MaxManifoldPoints);
        Points = points;
        LocalNormal = localNormal;
        LocalPoint = localPoint;
        Type = type;
        PointCount = pointCount;
    }

    public Manifold() => Points = new ManifoldPoint[MaxManifoldPoints];

    /// <inheritdoc />
    public bool Equals(Manifold other) =>
        Points.AsSpan().SequenceEqual(other.Points) &&
        LocalNormal.Equals(other.LocalNormal) &&
        LocalPoint.Equals(other.LocalPoint) &&
        Type == other.Type &&
        PointCount == other.PointCount;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Manifold other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(LocalNormal, LocalPoint, Type, PointCount, HashCode.Combine(Points[0], Points[1]));

    /// <inheritdoc />
    public override string ToString() =>
        $"(Points: [{string.Join(", ", Points)}], LocalNormal: {LocalNormal}, LocalPoint: {LocalPoint}, Type: {Type}, PointCount: {PointCount})";

    /// <summary>
    /// Checks if two <see cref="Manifold" /> instances are equal.
    /// </summary>
    public static bool operator ==(Manifold left, Manifold right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="Manifold" /> instances are not equal.
    /// </summary>
    public static bool operator !=(Manifold left, Manifold right) => !(left == right);
}