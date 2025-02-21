using System;
using Box2D.NET.Collisions.RayCasts;

namespace Box2D.NET.Common.Primitives;

/// <summary>
/// Represents an axis-aligned bounding box (AABB).
/// </summary>
public struct AABB : IEquatable<AABB>
{
    /// <summary>
    /// The lower vertex of the bounding box.
    /// </summary>
    public Vector2 LowerBound;

    /// <summary>
    /// The upper vertex of the bounding box.
    /// </summary>
    public Vector2 UpperBound;

    /// <summary>
    /// Initializes a new instance of the <see cref="AABB" /> struct.
    /// </summary>
    /// <param name="lowerBound">The lower bound of the bounding box.</param>
    /// <param name="upperBound">The upper bound of the bounding box.</param>
    public AABB(in Vector2 lowerBound, in Vector2 upperBound)
    {
        LowerBound = lowerBound;
        UpperBound = upperBound;
    }

    /// <summary>
    /// Verifies that the bounds are sorted.
    /// </summary>
    /// <returns>True if the AABB is valid; otherwise, false.</returns>
    public bool IsValid
    {
        get
        {
            Vector2 vector = UpperBound - LowerBound;
            bool valid = vector is { X: >= 0f, Y: >= 0f };
            return valid && LowerBound.IsValid && UpperBound.IsValid;
        }
    }

    /// <summary>
    /// Gets the center of the AABB.
    /// </summary>
    public Vector2 Center => 0.5f * (LowerBound + UpperBound);

    /// <summary>
    /// Gets the extents (half-widths) of the AABB.
    /// </summary>
    public Vector2 Extents => 0.5f * (UpperBound - LowerBound);

    /// <summary>
    /// Gets the perimeter length of the AABB.
    /// </summary>
    /// <returns>The perimeter length.</returns>
    public float Perimeter
    {
        get
        {
            float width = UpperBound.X - LowerBound.X;
            float height = UpperBound.Y - LowerBound.Y;
            return 2f * (width + height);
        }
    }

    /// <summary>
    /// Expands this AABB to include another AABB.
    /// </summary>
    /// <param name="aabb">The AABB to combine with this one.</param>
    public void Combine(in AABB aabb)
    {
        LowerBound = Vector2.Min(LowerBound, aabb.LowerBound);
        UpperBound = Vector2.Max(UpperBound, aabb.UpperBound);
    }

    /// <summary>
    /// Combines two AABBs into this AABB.
    /// </summary>
    /// <param name="a">The first AABB.</param>
    /// <param name="b">The second AABB.</param>
    public void Combine(in AABB a, in AABB b)
    {
        LowerBound = Vector2.Min(a.LowerBound, b.LowerBound);
        UpperBound = Vector2.Max(a.UpperBound, b.UpperBound);
    }

    /// <summary>
    /// Checks if this AABB fully contains another AABB.
    /// </summary>
    /// <param name="aabb">The AABB to check containment for.</param>
    /// <returns>True if this AABB contains the provided AABB; otherwise, false.</returns>
    public bool Contains(in AABB aabb) =>
        LowerBound.X <= aabb.LowerBound.X &&
        LowerBound.Y <= aabb.LowerBound.Y &&
        aabb.UpperBound.X <= UpperBound.X &&
        aabb.UpperBound.Y <= UpperBound.Y;

    /// <summary>
    /// Tests if a ray intersects this AABB.
    /// </summary>
    /// <param name="output">The ray-cast output results.</param>
    /// <param name="input">The ray-cast input parameters.</param>
    /// <returns>True if the ray intersects the AABB, otherwise false.</returns>
    public bool RayCast(out RayCastOutput output, in RayCastInput input)
    {
        float tMin = -float.MaxValue;
        float tMax = float.MaxValue;

        Vector2 d = input.End - input.Start;
        Vector2 absD = Vector2.Abs(d);

        Vector2 normal = Vector2.Zero;

        for (int i = 0; i < 2; ++i)
        {
            if (absD[i] < float.Epsilon)
            {
                // Parallel.
                if (input.Start[i] < LowerBound[i] || UpperBound[i] < input.Start[i])
                {
                    output = default;
                    return false;
                }
            }
            else
            {
                float invD = 1f / d[i];
                float t1 = (LowerBound[i] - input.Start[i]) * invD;
                float t2 = (UpperBound[i] - input.Start[i]) * invD;

                // Sign of the normal vector.
                float s = -1f;

                if (t1 > t2)
                {
                    (t1, t2) = (t2, t1); // Swap t1 and t2
                    s = 1f;
                }

                // Push the min up
                if (t1 > tMin)
                {
                    normal.SetZero();
                    normal[i] = s;
                    tMin = t1;
                }

                // Pull the max down
                tMax = Math.Min(tMax, t2);

                if (tMin > tMax)
                {
                    output = default;
                    return false;
                }
            }
        }

        // Does the ray start inside the box?
        // Does the ray intersect beyond the max fraction?
        if (tMin < 0f || input.MaxFraction < tMin)
        {
            output = default;
            return false;
        }

        // Intersection.
        output = new RayCastOutput(normal, tMin);
        return true;
    }

    /// <inheritdoc />
    public bool Equals(AABB other) => LowerBound.Equals(other.LowerBound) && UpperBound.Equals(other.UpperBound);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is AABB other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(LowerBound, UpperBound);

    /// <inheritdoc />
    public override string ToString() => $"(LowerBound: {LowerBound}, UpperBound: {UpperBound})";

    /// <summary>
    /// Checks if two <see cref="AABB" /> instances are equal.
    /// </summary>
    public static bool operator ==(in AABB left, in AABB right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="AABB" /> instances are not equal.
    /// </summary>
    public static bool operator !=(in AABB left, in AABB right) => !left.Equals(right);

    /// <summary>
    /// Tests if two axis-aligned bounding boxes (AABBs) overlap.
    /// </summary>
    /// <param name="a">The first AABB.</param>
    /// <param name="b">The second AABB.</param>
    /// <returns><c>true</c> if the AABBs overlap; otherwise, <c>false</c>.</returns>
    public static bool TestOverlap(in AABB a, in AABB b)
    {
        Vector2 d1 = b.LowerBound - a.UpperBound;
        Vector2 d2 = a.LowerBound - b.UpperBound;

        // If either d1.x or d1.y is positive, the AABBs do not overlap.
        if (d1.X > 0f || d1.Y > 0f)
        {
            return false;
        }

        // If either d2.x or d2.y is positive, the AABBs do not overlap.
        if (d2.X > 0f || d2.Y > 0f)
        {
            return false;
        }

        // Otherwise, the AABBs overlap.
        return true;
    }
}