using System;

namespace Box2D.NET.Common;

/// <summary>
/// A 2D column vector.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Vector2" /> struct with the specified coordinates.
/// </remarks>
/// <param name="x">The X coordinate.</param>
/// <param name="y">The Y coordinate.</param>
public struct Vector2(float x, float y)
{
    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public float X = x;

    /// <summary>
    /// The Y component of the vector.
    /// </summary>
    public float Y = y;

    /// <summary>
    /// Set this vector to all zeros.
    /// </summary>
    public void SetZero()
    {
        X = 0f;
        Y = 0f;
    }

    /// <summary>
    /// Set this vector to the specified coordinates.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    public void Set(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Negates this vector.
    /// </summary>
    /// <param name="v">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    public static Vector2 operator -(in Vector2 v) => new(-v.X, -v.Y);

    /// <summary>
    /// Adds a vector to this vector.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector to add.</param>
    /// <returns>The result of adding the two vectors.</returns>
    public static Vector2 operator +(in Vector2 v1, in Vector2 v2) => new(v1.X + v2.X, v1.Y + v2.Y);

    /// <summary>
    /// Subtracts a vector from this vector.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector to subtract.</param>
    /// <returns>The result of subtracting the second vector from the first vector.</returns>
    public static Vector2 operator -(in Vector2 v1, in Vector2 v2) => new(v1.X - v2.X, v1.Y - v2.Y);

    /// <summary>
    /// Multiplies this vector by a scalar.
    /// </summary>
    /// <param name="v">The vector to multiply.</param>
    /// <param name="a">The scalar to multiply by.</param>
    /// <returns>The result of multiplying the vector by the scalar.</returns>
    public static Vector2 operator *(in Vector2 v, float a) => new(v.X * a, v.Y * a);

    /// <summary>
    /// Multiplies this vector by a scalar (scalar on the left side).
    /// </summary>
    /// <param name="a">The scalar to multiply by.</param>
    /// <param name="v">The vector to multiply.</param>
    /// <returns>The result of multiplying the scalar by the vector.</returns>
    public static Vector2 operator *(float a, in Vector2 v) => new(v.X * a, v.Y * a);

    /// <summary>
    /// Gets the length (magnitude) of this vector.
    /// </summary>
    /// <returns>The length of the vector.</returns>
    public readonly float Length() => MathF.Sqrt(X * X + Y * Y);

    /// <summary>
    /// Gets the squared length of this vector.
    /// </summary>
    /// <returns>The squared length of the vector.</returns>
    public readonly float LengthSquared() => X * X + Y * Y;

    /// <summary>
    /// Converts this vector into a unit vector and returns its original length.
    /// </summary>
    /// <returns>The length of the vector before normalization.</returns>
    public float Normalize()
    {
        float length = Length();
        if (length < float.Epsilon)
        {
            return 0f;
        }

        float invLength = 1f / length;
        X *= invLength;
        Y *= invLength;

        return length;
    }

    /// <summary>
    /// Checks if this vector contains finite coordinates.
    /// Note: in Box2D this is a method, but I transformed it to a property for convenience.
    /// </summary>
    /// <returns>True if both components are finite, otherwise false.</returns>
    public readonly bool IsValid => !float.IsFinite(X) && !float.IsFinite(Y);

    /// <summary>
    /// Gets the skew vector such that dot(skewVec, other) == cross(vec, other).
    /// </summary>
    /// <returns>The skew vector.</returns>
    public readonly Vector2 Skew() => new(-Y, X);

    /// <summary>
    /// Computes the dot product of two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>The dot product of the two vectors.</returns>
    public static float Dot(in Vector2 a, in Vector2 b) => a.X * b.X + a.Y * b.Y;

    /// <summary>
    /// Computes the cross product of two vectors. In 2D, this produces a scalar.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>The scalar cross product of the two vectors.</returns>
    public static float Cross(in Vector2 a, in Vector2 b) => a.X * b.Y - a.Y * b.X;

    /// <summary>
    /// Computes the cross product of a vector and a scalar. In 2D, this produces a vector.
    /// </summary>
    /// <param name="a">The vector.</param>
    /// <param name="s">The scalar.</param>
    /// <returns>The resulting vector.</returns>
    public static Vector2 Cross(in Vector2 a, float s) => new(s * a.Y, -s * a.X);

    /// <summary>
    /// Computes the cross product of a scalar and a vector. In 2D, this produces a vector.
    /// </summary>
    /// <param name="s">The scalar.</param>
    /// <param name="a">The vector.</param>
    /// <returns>The resulting vector.</returns>
    public static Vector2 Cross(float s, in Vector2 a) => new(-s * a.Y, s * a.X);

    /// <summary>
    /// Computes the distance between two points represented by vectors.
    /// </summary>
    /// <param name="a">The first point.</param>
    /// <param name="b">The second point.</param>
    /// <returns>The distance between the two points.</returns>
    public static float Distance(in Vector2 a, in Vector2 b)
    {
        Vector2 c = new(a.X - b.X, a.Y - b.Y);
        return c.Length();
    }

    /// <summary>
    /// Computes the squared distance between two points represented by vectors.
    /// This is faster than computing the distance and avoids a square root operation.
    /// </summary>
    /// <param name="a">The first point.</param>
    /// <param name="b">The second point.</param>
    /// <returns>The squared distance between the two points.</returns>
    public static float DistanceSquared(in Vector2 a, in Vector2 b)
    {
        Vector2 c = new(a.X - b.X, a.Y - b.Y);
        return Dot(c, c);
    }
}