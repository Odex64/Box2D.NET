using System;
using Box2D.NET.Collisions.Contacts;
using Box2D.NET.Collisions.Misc;
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
    public readonly FixedArray<ManifoldPoint> Points;

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
    /// Initializes a new instance of the <see cref="Manifold" /> struct with default values.
    /// </summary>
    public Manifold() => Points = new FixedArray<ManifoldPoint>(Constants.MaxManifoldPoints);

    /// <summary>
    /// Determines the state of contact points between two manifolds.
    /// </summary>
    /// <param name="state1">An array to store the state of points from the first manifold.</param>
    /// <param name="state2">An array to store the state of points from the second manifold.</param>
    /// <param name="manifold1">The first manifold.</param>
    /// <param name="manifold2">The second manifold.</param>
    public static void GetPointStates(PointState[] state1, PointState[] state2, in Manifold manifold1, in Manifold manifold2)
    {
        if (state1.Length != Constants.MaxManifoldPoints || state2.Length != Constants.MaxManifoldPoints)
        {
            throw new ArgumentException($"The state arrays must equal the {Constants.MaxManifoldPoints} elements.");
        }

        // Initialize all states to NullState.
        for (int i = 0; i < Constants.MaxManifoldPoints; ++i)
        {
            state1[i] = PointState.NullState;
            state2[i] = PointState.NullState;
        }

        // Detect persists and removes for manifold1.
        for (int i = 0; i < manifold1.Points.Length; ++i)
        {
            ContactId id = manifold1.Points[i].Id;
            state1[i] = PointState.RemoveState;

            for (int j = 0; j < manifold2.Points.Length; ++j)
            {
                if (manifold2.Points[j].Id.Key == id.Key)
                {
                    state1[i] = PointState.PersistState;
                    break;
                }
            }
        }

        // Detect persists and adds for manifold2.
        for (int i = 0; i < manifold2.Points.Length; ++i)
        {
            ContactId id = manifold2.Points[i].Id;
            state2[i] = PointState.AddState;

            for (int j = 0; j < manifold1.Points.Length; ++j)
            {
                if (manifold1.Points[j].Id.Key == id.Key)
                {
                    state2[i] = PointState.PersistState;
                    break;
                }
            }
        }
    }

    /// <inheritdoc />
    public bool Equals(Manifold other) => Points.SequenceEqual(other.Points) && LocalNormal.Equals(other.LocalNormal) && LocalPoint.Equals(other.LocalPoint) && Type == other.Type;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Manifold other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(LocalNormal);
        hash.Add(LocalPoint);
        hash.Add(Type);

        for (int i = 0; i < Points.Length; i++)
        {
            hash.Add(Points[i]);
        }

        return hash.ToHashCode();
    }

    /// <inheritdoc />
    public override string ToString() => $"(Points: [{string.Join(", ", Points)}], LocalNormal: {LocalNormal}, LocalPoint: {LocalPoint}, Type: {Type})";

    /// <summary>
    /// Checks if two <see cref="Manifold" /> instances are equal.
    /// </summary>
    public static bool operator ==(in Manifold left, in Manifold right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="Manifold" /> instances are not equal.
    /// </summary>
    public static bool operator !=(in Manifold left, in Manifold right) => !left.Equals(right);
}