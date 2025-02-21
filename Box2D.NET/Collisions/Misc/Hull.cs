using System;
using Box2D.NET.Common;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Collisions.Misc;

/// <summary>
/// Represents a convex hull defined by a set of vertices.
/// </summary>
public readonly struct Hull : IEquatable<Hull>
{
    /// <summary>
    /// The vertices of the hull.
    /// </summary>
    public readonly FixedArray<Vector2> Points;

    /// <summary>
    /// Initialize empty Hull
    /// </summary>
    public Hull() => Points = new FixedArray<Vector2>(Constants.MaxPolygonVertices);

    /// <inheritdoc />
    public bool Equals(Hull other) => Points.SequenceEqual(other.Points);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Hull other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        for (int i = 0; i < Points.Length; i++)
        {
            hash.Add(Points[i]);
        }

        return hash.ToHashCode();
    }

    /// <summary>
    /// Checks if two <see cref="Hull" /> instances are equal.
    /// </summary>
    public static bool operator ==(in Hull left, in Hull right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="Hull" /> instances are not equal.
    /// </summary>
    public static bool operator !=(in Hull left, in Hull right) => !(left == right);

    /// <summary>
    /// Returns a string representation of the hull.
    /// </summary>
    /// <returns>A string representation of the hull.</returns>
    public override string ToString() => $"Hull (Points: [{string.Join(", ", Points)}])";

    /// <summary>
    /// Performs recursive QuickHull to compute the convex hull of a set of points.
    /// </summary>
    /// <returns>The computed convex hull.</returns>
    public static Hull RecurseHull(in Vector2 p1, in Vector2 p2, Vector2[] ps, int count)
    {
        Hull hull = new Hull();

        if (count == 0)
        {
            return hull;
        }

        // create an edge vector pointing from p1 to p2
        Vector2 e = p2 - p1;
        e.Normalize();

        // discard points left of e and find point furthest to the right of e
        Vector2[] rightPoints = new Vector2[Constants.MaxPolygonVertices];
        int rightCount = 0;

        int bestIndex = 0;
        float bestDistance = Vector2.Cross(ps[bestIndex] - p1, e);
        if (bestDistance > 0f)
        {
            rightPoints[rightCount++] = ps[bestIndex];
        }

        for (int i = 1; i < count; ++i)
        {
            float distance = Vector2.Cross(ps[i] - p1, e);
            if (distance > bestDistance)
            {
                bestIndex = i;
                bestDistance = distance;
            }

            if (distance > 0f)
            {
                rightPoints[rightCount++] = ps[i];
            }
        }

        if (bestDistance < 2f * Constants.LinearSlop)
        {
            return hull;
        }


        // compute hull to the right of p1-bestPoint
        Hull hull1 = RecurseHull(p1, ps[bestIndex], rightPoints, rightCount);

        // compute hull to the right of bestPoint-p2
        Hull hull2 = RecurseHull(ps[bestIndex], p2, rightPoints, rightCount);

        // stitch together hulls
        for (int i = 0; i < hull1.Points.Length; ++i)
        {
            hull.Points[hull.Points.Length++] = hull1.Points[i];
        }

        hull.Points[hull.Points.Length++] = ps[bestIndex];

        for (int i = 0; i < hull2.Points.Length; ++i)
        {
            hull.Points[hull.Points.Length++] = hull2.Points[i];
        }

        return hull;
    }
}