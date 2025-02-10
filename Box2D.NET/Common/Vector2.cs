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
}