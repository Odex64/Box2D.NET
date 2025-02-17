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
    public AABB(Vector2 lowerBound, Vector2 upperBound)
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
            bool valid = vector.X >= 0f && vector.Y >= 0f;
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
    public float GetPerimeter()
    {
        float width = UpperBound.X - LowerBound.X;
        float height = UpperBound.Y - LowerBound.Y;
        return 2f * (width + height);
    }

    /// <summary>
    /// Expands this AABB to include another AABB.
    /// </summary>
    /// <param name="aabb">The AABB to combine with this one.</param>
    public void Combine(AABB aabb)
    {
        LowerBound = Vector2.Min(LowerBound, aabb.LowerBound);
        UpperBound = Vector2.Max(UpperBound, aabb.UpperBound);
    }

    /// <summary>
    /// Combines two AABBs into this AABB.
    /// </summary>
    /// <param name="aabb1">The first AABB.</param>
    /// <param name="aabb2">The second AABB.</param>
    public void Combine(AABB aabb1, AABB aabb2)
    {
        LowerBound = Vector2.Min(aabb1.LowerBound, aabb2.LowerBound);
        UpperBound = Vector2.Max(aabb1.UpperBound, aabb2.UpperBound);
    }

    /// <summary>
    /// Checks if this AABB fully contains another AABB.
    /// </summary>
    /// <param name="aabb">The AABB to check containment for.</param>
    /// <returns>True if this AABB contains the provided AABB; otherwise, false.</returns>
    public bool Contains(AABB aabb) =>
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

        Vector2 p = input.Start;
        Vector2 d = input.End - input.Start;
        Vector2 absD = Vector2.Abs(d);

        Vector2 normal = Vector2.Zero;

        for (int i = 0; i < 2; ++i)
        {
            if (absD[i] < float.Epsilon)
            {
                // Parallel.
                if (p[i] < LowerBound[i] || UpperBound[i] < p[i])
                {
                    output = default;
                    return false;
                }
            }
            else
            {
                float invD = 1f / d[i];
                float t1 = (LowerBound[i] - p[i]) * invD;
                float t2 = (UpperBound[i] - p[i]) * invD;

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
                    normal = Vector2.Zero;
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
    public bool Equals(AABB other) =>
        LowerBound.Equals(other.LowerBound) && UpperBound.Equals(other.UpperBound);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is AABB other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(LowerBound, UpperBound);

    /// <inheritdoc />
    public override string ToString() =>
        $"(LowerBound: {LowerBound}, UpperBound: {UpperBound})";

    /// <summary>
    /// Checks if two <see cref="AABB" /> instances are equal.
    /// </summary>
    public static bool operator ==(AABB left, AABB right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="AABB" /> instances are not equal.
    /// </summary>
    public static bool operator !=(AABB left, AABB right) => !(left == right);
}