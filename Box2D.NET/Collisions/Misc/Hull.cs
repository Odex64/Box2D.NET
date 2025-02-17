using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Box2D.NET.Common;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Collisions.Misc;

/// <summary>
/// Represents a convex hull defined by a set of vertices.
/// </summary>
public struct Hull : IEquatable<Hull>
{
    /// <summary>
    /// The vertices of the hull.
    /// </summary>
    public readonly Vector2[] Points;

    /// <summary>
    /// Initializes a new instance of the <see cref="Hull" /> struct.
    /// </summary>
    /// <param name="points">The vertices of the hull.</param>
    /// <exception cref="ArgumentException">Thrown if the number of vertices exceeds <see cref="Constants.MaxPolygonVertices" />.</exception>
    public Hull(in Vector2[] points)
    {
        Debug.Assert(points.Length <= Constants.MaxPolygonVertices);
        Points = points;
    }

    public Hull() => Points = new Vector2[Constants.MaxPolygonVertices];

    /// <inheritdoc />
    public bool Equals(Hull other) => Points.SequenceEqual(other.Points);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Hull other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        foreach (Vector2 point in Points)
        {
            hash.Add(point);
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
    public override string ToString() => $"Hull (Points: [{string.Join(", ", Points[..Points.Length])}])";

    /// <summary>
    /// Performs recursive QuickHull to compute the convex hull of a set of points.
    /// </summary>
    /// <param name="p1">The first point defining the edge of the hull.</param>
    /// <param name="p2">The second point defining the edge of the hull.</param>
    /// <param name="ps">The array of points to be considered for the hull.</param>
    /// <param name="count">The number of points in the array to consider for the hull.</param>
    /// <returns>The computed convex hull.</returns>
    public static Hull RecurseHull(in Vector2 p1, in Vector2 p2, in Vector2[] ps)
    {
        Hull hull = new Hull();

        int count = ps.Length;
        if (count == 0)
        {
            return hull;
        }

        // Create an edge vector pointing from p1 to p2
        Vector2 e = p2 - p1;
        e.Normalize();

        // Discard points to the left of e and find the point furthest to the right of e
        List<Vector2> rightPoints = new List<Vector2>();

        int bestIndex = 0;
        float bestDistance = Vector2.Cross(ps[bestIndex] - p1, e);
        if (bestDistance > 0f)
        {
            rightPoints.Add(ps[bestIndex]);
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
                rightPoints.Add(ps[i]);
            }
        }

        if (bestDistance < 2f * Constants.LinearSlop)
        {
            return hull;
        }

        Vector2 bestPoint = ps[bestIndex];

        // Compute hull to the right of p1-bestPoint
        Hull hull1 = RecurseHull(p1, bestPoint, rightPoints.ToArray());

        // Compute hull to the right of bestPoint-p2
        Hull hull2 = RecurseHull(bestPoint, p2, rightPoints.ToArray());

        int currentCount = hull.Points.Length;

        // Stitch together hulls
        foreach (Vector2 point in hull1.Points)
        {
            hull.Points[currentCount] = point;
        }

        hull.Points[currentCount] = bestPoint;

        foreach (Vector2 point in hull2.Points)
        {
            hull.Points[currentCount] = point;
        }

        Debug.Assert(currentCount < Constants.MaxPolygonVertices);

        return hull;
    }

    /// <summary>
    /// Validates a convex hull.
    /// </summary>
    /// <param name="hull">The hull to validate.</param>
    /// <returns>True if the hull is valid, otherwise false.</returns>
    public static bool ValidateHull(in Hull hull)
    {
        int count = hull.Points.Length;
        // Check if the number of vertices is within the valid range
        if (count is < 3 or > Constants.MaxPolygonVertices)
        {
            return false;
        }

        // Test that every point is behind every edge
        for (int i = 0; i < count; ++i)
        {
            // Create an edge vector
            int i2 = i < count - 1 ? i + 1 : 0;
            Vector2 p = hull.Points[i];
            Vector2 e = hull.Points[i2] - p;
            _ = e.Normalize();

            for (int j = 0; j < count; ++j)
            {
                // Skip points that subtend the current edge
                if (j == i || j == i2)
                {
                    continue;
                }

                float distance = Vector2.Cross(hull.Points[j] - p, e);
                if (distance >= 0f)
                {
                    return false;
                }
            }
        }

        // Test for collinear points
        for (int i = 0; i < count; ++i)
        {
            int i2 = (i + 1) % count;
            int i3 = (i + 2) % count;

            Vector2 p1 = hull.Points[i];
            Vector2 p2 = hull.Points[i2];
            Vector2 p3 = hull.Points[i3];

            Vector2 e = p3 - p1;
            _ = e.Normalize();

            float distance = Vector2.Cross(p2 - p1, e);
            if (distance <= Constants.LinearSlop)
            {
                // p1-p2-p3 are collinear
                return false;
            }
        }

        return true;
    }
}