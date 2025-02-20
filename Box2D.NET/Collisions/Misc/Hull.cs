using System;
using System.Linq;
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
    public readonly Vector2[] Points;

    /// <summary>
    /// Initializes a new instance of the <see cref="Hull" /> struct.
    /// </summary>
    /// <param name="points">The vertices of the hull.</param>
    /// <exception cref="ArgumentException">Thrown if the number of vertices exceeds <see cref="Constants.MaxPolygonVertices" />.</exception>
    public Hull(Vector2[] points)
    {
        if (points.Length != Constants.MaxPolygonVertices)
        {
            throw new ArgumentException($"The {nameof(points)} array has not {Constants.MaxManifoldPoints} elements.");
        }

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
    /// <returns>The computed convex hull.</returns>
    public static Hull RecurseHull(in Vector2 p1, in Vector2 p2, Vector2[] ps)
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


        // Compute hull to the right of p1-bestPoint
        Hull hull1 = RecurseHull(p1, ps[bestIndex], rightPoints.ToArray());

        // Compute hull to the right of bestPoint-p2
        Hull hull2 = RecurseHull(ps[bestIndex], p2, rightPoints.ToArray());

        int currentCount = hull.Points.Length;

        // Stitch together hulls
        foreach (Vector2 point in hull1.Points)
        {
            hull.Points[currentCount] = point;
        }

        hull.Points[currentCount] = ps[bestIndex];

        foreach (Vector2 point in hull2.Points)
        {
            hull.Points[currentCount] = point;
        }

        if (currentCount > Constants.MaxPolygonVertices)
        {
            throw new ArgumentException($"{nameof(currentCount)} is greater than {Constants.MaxManifoldPoints}.");
        }

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
            Vector2 e = hull.Points[i2] - hull.Points[i];
            _ = e.Normalize();

            for (int j = 0; j < count; ++j)
            {
                // Skip points that subtend the current edge
                if (j == i || j == i2)
                {
                    continue;
                }

                float distance = Vector2.Cross(hull.Points[j] - hull.Points[i], e);
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

            Vector2 e = hull.Points[i3] - hull.Points[i];
            _ = e.Normalize();

            float distance = Vector2.Cross(hull.Points[i2] - hull.Points[i], e);
            if (distance <= Constants.LinearSlop)
            {
                // p1-p2-p3 are collinear
                return false;
            }
        }

        return true;
    }

    // /// <summary>
    // /// Computes the convex hull using the QuickHull algorithm.
    // /// </summary>
    // /// <param name="points">The input points.</param>
    // /// <returns>The computed convex hull.</returns>
    // public static Hull ComputeHull(Vector2[] points)
    // {
    //     Hull hull = new Hull();
    //
    //     int count = points.Length;
    //
    //     if (count is < 3 or > Constants.MaxPolygonVertices)
    //     {
    //         return hull;
    //     }
    //
    //     count = Math.Min(count, Constants.MaxPolygonVertices);
    //
    //     AABB aabb = new AABB(new Vector2(float.MaxValue, float.MaxValue), new Vector2(-float.MaxValue, -float.MaxValue));
    //
    //     // Perform aggressive point welding and compute the bounding box
    //     List<Vector2> ps = new List<Vector2>();
    //     float tolSqr = 16.0f * Constants.LinearSlop * Constants.LinearSlop;
    //
    //     for(int i = 0; i < count; ++i)
    //     {
    //         aabb.LowerBound = Vector2.Min(aabb.LowerBound, points[i]);
    //         aabb.UpperBound = Vector2.Max(aabb.UpperBound, points[i]);
    //
    //         bool unique = true;
    //         for(int j = 0; j < i; ++j)
    //         {
    //             if (Vector2.DistanceSquared(points[i], points[j]) < tolSqr)
    //             {
    //                 unique = false;
    //                 break;
    //             }
    //         }
    //
    //         if (unique)
    //         {
    //             ps.Add(points[i]);
    //         }
    //     }
    //
    //     if (ps.Count < 3)
    //     {
    //         return hull;
    //     }
    //
    //     // Find an extreme point
    //     Vector2 center = aabb.Center;
    //     int i1 = 0;
    //     float maxDistSq = Vector2.DistanceSquared(center, ps[i1]);
    //
    //     for (int i = 1; i < ps.Count; i++)
    //     {
    //         float distSq = Vector2.DistanceSquared(center, ps[i]);
    //         if (distSq > maxDistSq)
    //         {
    //             i1 = i;
    //             maxDistSq = distSq;
    //         }
    //     }
    //
    //     ps.RemoveAt(i1);
    //
    //     int i2 = 0;
    //     maxDistSq = Vector2.DistanceSquared(ps[i1], ps[i2]);
    //
    //     for (int i = 1; i < ps.Count; i++)
    //     {
    //         float distSq = Vector2.DistanceSquared(ps[i1], ps[i]);
    //         if (distSq > maxDistSq)
    //         {
    //             i2 = i;
    //             maxDistSq = distSq;
    //         }
    //     }
    //
    //     ps.RemoveAt(i2);
    //
    //     List<Vector2> rightPoints = new List<Vector2>();
    //     List<Vector2> leftPoints = new List<Vector2>();
    //
    //     Vector2 edge = Vector2.Normalize(ps[i2] - ps[i1]);
    //
    //     foreach (Vector2 point in ps)
    //     {
    //         float d = Vector2.Cross(point - ps[i1], edge);
    //
    //         if (d >= 2.0f * LinearSlop)
    //         {
    //             rightPoints.Add(point);
    //         }
    //         else if (d <= -2.0f * LinearSlop)
    //         {
    //             leftPoints.Add(point);
    //         }
    //     }
    //
    //     // Compute hulls on right and left
    //     Hull hull1 = RecurseHull(ps[i1], ps[i2], rightPoints);
    //     Hull hull2 = RecurseHull(ps[i2], ps[i1], leftPoints);
    //
    //     if (hull1.Count == 0 && hull2.Count == 0)
    //     {
    //         return hull;
    //     }
    //
    //     // Stitch hulls together
    //     hull.Points.Add(ps[i1]);
    //     hull.Points.AddRange(hull1.Points);
    //     hull.Points.Add(ps[i2]);
    //     hull.Points.AddRange(hull2.Points);
    //
    //     // Merge collinear points
    //     bool searching = true;
    //     while (searching && hull.Count > 2)
    //     {
    //         searching = false;
    //
    //         for (int i = 0; i < hull.Count; i++)
    //         {
    //             int i1Idx = i;
    //             int i2Idx = (i + 1) % hull.Count;
    //             int i3Idx = (i + 2) % hull.Count;
    //
    //             Vector2 p1Hull = hull.Points[i1Idx];
    //             Vector2 p2Hull = hull.Points[i2Idx];
    //             Vector2 p3Hull = hull.Points[i3Idx];
    //
    //             Vector2 e = Vector2.Normalize(p3Hull - p1Hull);
    //             float distance = Vector2.Cross(p2Hull - p1Hull, e);
    //
    //             if (distance <= 2.0f * LinearSlop)
    //             {
    //                 hull.Points.RemoveAt(i2Idx);
    //                 searching = true;
    //                 break;
    //             }
    //         }
    //     }
    //
    //     if (hull.Count < 3)
    //     {
    //         hull.Points.Clear();
    //     }
    //
    //     return hull;
    // }
}