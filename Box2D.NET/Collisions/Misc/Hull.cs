using System;
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
    public FixedArray<Vector2> Points;

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

    /// <summary>
    /// Validate hull
    /// </summary>
    /// <returns>True if hull is valid; otherwise false.</returns>
    public static bool ValidateHull(in Hull hull)
    {
        if (hull.Points.Length is < 3 or > Constants.MaxPolygonVertices)
        {
            return false;
        }

        // test that every point is behind every edge
        for (int i = 0; i < hull.Points.Length; ++i)
        {
            // create an edge vector
            int i2 = i < hull.Points.Length - 1 ? i + 1 : 0;
            Vector2 e = hull.Points[i2] - hull.Points[i];
            e.Normalize();

            for (int j = 0; j < hull.Points.Length; ++j)
            {
                // skip points that subtend the current edge
                if (j == i || j == i2)
                {
                    continue;
                }

                float distance = Vector2.Cross(hull.Points[j] - hull.Points[i], e);
                if (distance >= 0.0f)
                {
                    return false;
                }
            }
        }

        // test for collinear points
        for (int i = 0; i < hull.Points.Length; ++i)
        {
            int i2 = (i + 1) % hull.Points.Length;
            int i3 = (i + 2) % hull.Points.Length;

            Vector2 e = hull.Points[i3] - hull.Points[i];
            e.Normalize();

            float distance = Vector2.Cross(hull.Points[i2] - hull.Points[i], e);
            if (distance <= Constants.LinearSlop)
            {
                // p1-p2-p3 are collinear
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Quick hull algorithm.
    /// Merges vertices based on LinearSlop.
    /// Removes collinear points using LinearSlop.
    /// </summary>
    /// <returns>Returns an empty hull if it fails.</returns>
    public static Hull ComputeHull(Vector2[] points, int count)
    {
        Hull hull = new Hull();

        if (count is < 3 or > Constants.MaxPolygonVertices)
        {
            return hull;
        }

        count = Math.Min(count, Constants.MaxPolygonVertices);

        AABB aabb = new AABB(new Vector2(float.MaxValue, float.MaxValue), new Vector2(-float.MaxValue, -float.MaxValue));

        // Perform aggressive point welding. First point always remains.
        // Also compute the bounding box for later.
        Vector2[] ps = new Vector2[Constants.MaxPolygonVertices];
        int n = 0;
        float tolSqr = 16.0f * Constants.LinearSlop * Constants.LinearSlop;
        for (int i = 0; i < count; ++i)
        {
            aabb.LowerBound = Vector2.Min(aabb.LowerBound, points[i]);
            aabb.UpperBound = Vector2.Max(aabb.UpperBound, points[i]);

            bool unique = true;
            for (int j = 0; j < i; ++j)
            {
                float distSqr = Vector2.DistanceSquared(points[i], points[j]);
                if (distSqr < tolSqr)
                {
                    unique = false;
                    break;
                }
            }

            if (unique)
            {
                ps[n++] = points[i];
            }
        }

        if (n < 3)
        {
            // all points very close together, check your data and check your scale
            return hull;
        }

        // Find an extreme point as the first point on the hull
        Vector2 c = aabb.Center;
        int i1 = 0;
        float dsq1 = Vector2.DistanceSquared(c, ps[i1]);
        for (int i = 1; i < n; ++i)
        {
            float dsq = Vector2.DistanceSquared(c, ps[i]);
            if (dsq > dsq1)
            {
                i1 = i;
                dsq1 = dsq;
            }
        }

        // remove p1 from working set
        Vector2 p1 = ps[i1];
        ps[i1] = ps[n - 1];
        n -= 1;

        int i2 = 0;
        float dsq2 = Vector2.DistanceSquared(p1, ps[i2]);
        for (int i = 1; i < n; ++i)
        {
            float dsq = Vector2.DistanceSquared(p1, ps[i]);
            if (dsq > dsq2)
            {
                i2 = i;
                dsq2 = dsq;
            }
        }

        // remove p2 from working set
        Vector2 p2 = ps[i2];
        ps[i2] = ps[n - 1];
        n -= 1;

        // split the points into points that are left and right of the line p1-p2.
        Vector2[] rightPoints = new Vector2[Constants.MaxPolygonVertices - 2];
        int rightCount = 0;

        Vector2[] leftPoints = new Vector2[Constants.MaxPolygonVertices - 2];
        int leftCount = 0;

        Vector2 e = p2 - p1;
        e.Normalize();

        for (int i = 0; i < n; ++i)
        {
            float d = Vector2.Cross(ps[i] - p1, e);

            // slop used here to skip points that are very close to the line p1-p2
            if (d >= 2.0f * Constants.LinearSlop)
            {
                rightPoints[rightCount++] = ps[i];
            }
            else if (d <= -2.0f * Constants.LinearSlop)
            {
                leftPoints[leftCount++] = ps[i];
            }
        }

        // compute hulls on right and left
        Hull hull1 = RecurseHull(p1, p2, rightPoints, rightCount);
        Hull hull2 = RecurseHull(p2, p1, leftPoints, leftCount);

        if (hull1.Points.Length == 0 && hull2.Points.Length == 0)
        {
            // all points collinear
            return hull;
        }

        // stitch hulls together, preserving CCW winding order
        hull.Points[hull.Points.Length++] = p1;

        for (int i = 0; i < hull1.Points.Length; ++i)
        {
            hull.Points[hull.Points.Length++] = hull1.Points[i];
        }

        hull.Points[hull.Points.Length++] = p2;

        for (int i = 0; i < hull2.Points.Length; ++i)
        {
            hull.Points[hull.Points.Length++] = hull2.Points[i];
        }

        // merge collinear
        bool searching = true;
        while (searching && hull.Points.Length > 2)
        {
            searching = false;

            for (int i = 0; i < hull.Points.Length; ++i)
            {
                i1 = i;
                i2 = (i + 1) % hull.Points.Length;
                int i3 = (i + 2) % hull.Points.Length;


                e = hull.Points[i3] - hull.Points[i1];
                e.Normalize();

                float distance = Vector2.Cross(hull.Points[i2] - hull.Points[i1], e);
                if (distance <= 2.0f * Constants.LinearSlop)
                {
                    // remove midpoint from hull
                    for (int j = i2; j < hull.Points.Length - 1; ++j)
                    {
                        hull.Points[j] = hull.Points[j + 1];
                    }

                    hull.Points.Length -= 1;

                    // continue searching for collinear points
                    searching = true;

                    break;
                }
            }
        }

        if (hull.Points.Length < 3)
        {
            // all points collinear, shouldn't be reached since this was validated above
            hull.Points.Length = 0;
        }

        return hull;
    }
}