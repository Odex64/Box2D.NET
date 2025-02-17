using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Box2D.NET.Common;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Collisions.Manifolds;

/// <summary>
/// This is used to compute the current state of a contact manifold.
/// </summary>
public struct WorldManifold : IEquatable<WorldManifold>
{
    /// <summary>
    /// World vector pointing from A to B.
    /// </summary>
    public Vector2 Normal;

    /// <summary>
    /// World contact points (points of intersection).
    /// </summary>
    public Vector2[] Points;

    /// <summary>
    /// A negative value indicates overlap, in meters.
    /// </summary>
    public float[] Separations;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldManifold" /> struct.
    /// </summary>
    /// <param name="normal">World vector pointing from A to B.</param>
    /// <param name="points">World contact points.</param>
    /// <param name="separations">Separation values.</param>
    public WorldManifold(in Vector2 normal, in Vector2[] points, float[] separations)
    {
        Debug.Assert(points.Length <= Constants.MaxManifoldPoints && separations.Length <= Constants.MaxManifoldPoints);
        Normal = normal;
        Points = points;
        Separations = separations;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldManifold" /> struct with default values.
    /// </summary>
    public WorldManifold()
    {
        Points = new Vector2[Constants.MaxManifoldPoints];
        Separations = new float[Constants.MaxManifoldPoints];
    }

    /// <summary>
    /// Evaluates the manifold with supplied transforms.
    /// This assumes modest motion from the original state.
    /// This does not change the point count, impulses, etc.
    /// The radii must come from the shapes that generated the manifold.
    /// </summary>
    /// <param name="manifold">The manifold to evaluate.</param>
    /// <param name="xfA">The transform of shape A.</param>
    /// <param name="radiusA">The radius of shape A.</param>
    /// <param name="xfB">The transform of shape B.</param>
    /// <param name="radiusB">The radius of shape B.</param>
    /// <exception cref="InvalidEnumArgumentException">Thrown when the manifold type is invalid.</exception>
    public void Initialize(in Manifold manifold, in Transform xfA, float radiusA, in Transform xfB, float radiusB)
    {
        int pointsCount = manifold.Points.Length;
        if (pointsCount == 0)
        {
            return;
        }

        switch (manifold.Type)
        {
            case ManifoldType.Circles:
                {
                    Normal = new Vector2(1f, 0f);
                    Vector2 pointA = Transform.Multiply(xfA, manifold.LocalPoint);
                    Vector2 pointB = Transform.Multiply(xfB, manifold.Points[0].LocalPoint);

                    if ((pointB - pointA).LengthSquared() > float.Epsilon * float.Epsilon)
                    {
                        Normal = pointB - pointA;
                        _ = Normal.Normalize();
                    }

                    Vector2 cA = pointA + radiusA * Normal;
                    Vector2 cB = pointB - radiusB * Normal;
                    Points[0] = 0.5f * (cA + cB);
                    Separations[0] = Vector2.Dot(cB - cA, Normal);
                }
                break;

            case ManifoldType.FaceA:
                {
                    Normal = Rotation.Multiply(xfA.Rotation, manifold.LocalNormal);
                    Vector2 planePoint = Transform.Multiply(xfA, manifold.LocalPoint);

                    for (int i = 0; i < pointsCount; ++i)
                    {
                        Vector2 clipPoint = Transform.Multiply(xfB, manifold.Points[i].LocalPoint);
                        Vector2 cA = clipPoint + (radiusA - Vector2.Dot(clipPoint - planePoint, Normal)) * Normal;
                        Vector2 cB = clipPoint - radiusB * Normal;
                        Points[i] = 0.5f * (cA + cB);
                        Separations[i] = Vector2.Dot(cB - cA, Normal);
                    }
                }
                break;

            case ManifoldType.FaceB:
                {
                    Normal = Rotation.Multiply(xfB.Rotation, manifold.LocalNormal);
                    Vector2 planePoint = Transform.Multiply(xfB, manifold.LocalPoint);

                    for (int i = 0; i < pointsCount; ++i)
                    {
                        Vector2 clipPoint = Transform.Multiply(xfA, manifold.Points[i].LocalPoint);
                        Vector2 cB = clipPoint + (radiusB - Vector2.Dot(clipPoint - planePoint, Normal)) * Normal;
                        Vector2 cA = clipPoint - radiusA * Normal;
                        Points[i] = 0.5f * (cA + cB);
                        Separations[i] = Vector2.Dot(cA - cB, Normal);
                    }

                    // Ensure normal points from A to B.
                    Normal = -Normal;
                }
                break;

            default:
                throw new InvalidEnumArgumentException($"{nameof(manifold.Type)} has an incorrect value.");
        }
    }

    /// <inheritdoc />
    public bool Equals(WorldManifold other) =>
        Normal.Equals(other.Normal) &&
        Points.SequenceEqual(other.Points) &&
        Separations.SequenceEqual(other.Separations);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is WorldManifold other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Normal);
        foreach (Vector2 point in Points)
        {
            hash.Add(point);
        }

        foreach (float separation in Separations)
        {
            hash.Add(separation);
        }

        return hash.ToHashCode();
    }

    /// <inheritdoc />
    public override string ToString() =>
        $"(Normal: {Normal}, Points: [{string.Join(", ", Points)}], Separations: [{string.Join(", ", Separations)}])";

    /// <summary>
    /// Checks if two <see cref="WorldManifold" /> instances are equal.
    /// </summary>
    public static bool operator ==(in WorldManifold left, in WorldManifold right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="WorldManifold" /> instances are not equal.
    /// </summary>
    public static bool operator !=(in WorldManifold left, in WorldManifold right) => !(left == right);
}