// SPDX-FileCopyrightText: 2023 Erin Catto
// SPDX-FileCopyrightText: 2025 Carmine Pietroluongo
// SPDX-License-Identifier: MIT

using System;
using System.Numerics;
using Box2D.NET.Extensions;

namespace Box2D.NET;

/// <summary>
/// Axis-aligned bounding box
/// </summary>
public struct AABB
{
    public Vector2 LowerBound;
    public Vector2 UpperBound;

    /// <summary>
    /// Get surface area of an AABB (the perimeter length)
    /// </summary>
    public readonly float Perimeter
    {
        get
        {
            float wx = UpperBound.X - LowerBound.X;
            float wy = UpperBound.Y - LowerBound.Y;
            return 2.0f * (wx + wy);
        }
    }


    public bool IsValid
    {
        get
        {
            Vector2 dimensions = UpperBound - LowerBound; // Vector2 has built-in subtraction
            bool dimensionsValid = dimensions.X >= 0.0f && dimensions.Y >= 0.0f;
            return dimensionsValid && LowerBound.IsValid && UpperBound.IsValid;
        }
    }

    public AABB(in Vector2 lowerBound, in Vector2 upperBound)
    {
        LowerBound = lowerBound;
        UpperBound = upperBound;
    }

    /// <summary>
    /// Enlarge a to contain b
    /// </summary>
    /// <returns>true if the AABB grew</returns>
    public bool Enlarge(in AABB other)
    {
        bool changed = false;

        if (other.LowerBound.X < LowerBound.X)
        {
            LowerBound.X = other.LowerBound.X;
            changed = true;
        }

        if (other.LowerBound.Y < LowerBound.Y)
        {
            LowerBound.Y = other.LowerBound.Y;
            changed = true;
        }

        if (UpperBound.X < other.UpperBound.X)
        {
            UpperBound.X = other.UpperBound.X;
            changed = true;
        }

        if (UpperBound.Y < other.UpperBound.Y)
        {
            UpperBound.Y = other.UpperBound.Y;
            changed = true;
        }

        return changed;
    }

    /// <summary>
    /// Do a and b overlap
    /// </summary>
    public static bool Overlaps(in AABB a, in AABB b)
    {
        return !(b.LowerBound.X > a.UpperBound.X ||
                b.LowerBound.Y > a.UpperBound.Y ||
                a.LowerBound.X > b.UpperBound.X ||
                a.LowerBound.Y > b.UpperBound.Y);
    }

    /// <summary>
    /// From Real-time Collision Detection, p179.
    /// </summary>
    public CastOutput RayCast(in Vector2 p1, in Vector2 p2)
    {
        // Radius not handled
        CastOutput output = default;

        float tmin = float.MinValue;
        float tmax = float.MaxValue;

        ref readonly Vector2 p = ref p1;
        Vector2 d = Vector2.Subtract(p2, p1);
        Vector2 absD = Vector2.Abs(d);

        Vector2 normal = Vector2.Zero;

        // x-coordinate
        if (absD.X < float.Epsilon)
        {
            // parallel
            if (p.X < LowerBound.X || UpperBound.X < p.X)
            {
                return output;
            }
        }
        else
        {
            float inv_d = 1.0f / d.X;
            float t1 = (LowerBound.X - p.X) * inv_d;
            float t2 = (UpperBound.X - p.X) * inv_d;

            // Sign of the normal vector
            float s = -1.0f;

            if (t1 > t2)
            {
                (t1, t2) = (t2, t1);
                s = 1.0f;
            }

            if (t1 > tmin)
            {
                normal.Y = 0.0f;
                normal.X = s;
                tmin = t1;
            }

            tmax = Math.Min(tmax, t2);

            if (tmin > tmax)
            {
                return output;
            }
        }

        // y-coordinate
        if (absD.Y < float.Epsilon)
        {
            // parallel
            if (p.Y < LowerBound.Y || UpperBound.Y < p.Y)
            {
                return output;
            }
        }
        else
        {
            float inv_d = 1.0f / d.Y;
            float t1 = (LowerBound.Y - p.Y) * inv_d;
            float t2 = (UpperBound.Y - p.Y) * inv_d;

            // Sign of the normal vector
            float s = -1.0f;

            if (t1 > t2)
            {
                (t1, t2) = (t2, t1);
                s = 1.0f;
            }

            if (t1 > tmin)
            {
                normal.X = 0.0f;
                normal.Y = s;
                tmin = t1;
            }

            tmax = Math.Min(tmax, t2);

            if (tmin > tmax)
            {
                return output;
            }
        }

        // Does the ray start inside the box?
        // Does the ray intersect beyond the max fraction?
        if (tmin is < 0.0f or > 1.0f)
        {
            return output;
        }

        // Intersection
        output.Fraction = tmin;
        output.Normal = normal;
        output.Point = Vector2.Lerp(p1, p2, tmin);
        output.Hit = true;
        return output;
    }
}