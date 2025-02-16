using System.ComponentModel;
using System.Diagnostics;
using Box2D.NET.Common;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Collisions.Manifolds;

/// <summary>
/// This is used to compute the current state of a contact manifold.
/// </summary>
public struct WorldManifold
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
    /// <exception cref="InvalidEnumArgumentException"></exception>
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
                    Normal = new Vector2(1.0f, 0.0f);
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
                throw new InvalidEnumArgumentException($"{nameof(manifold)} has incorrect type!");
        }
    }


    /// <inheritdoc />
    public override string ToString() =>
        $"(Normal: {Normal}, Points: [{string.Join(", ", Points)}], Separations: [{string.Join(", ", Separations)}])";
}