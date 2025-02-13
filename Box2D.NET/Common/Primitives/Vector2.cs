using System;

namespace Box2D.NET.Common.Primitives;

/// <summary>
/// A 2D column vector.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Vector2" /> struct with the specified coordinates.
/// </remarks>
/// <param name="x">The X coordinate.</param>
/// <param name="y">The Y coordinate.</param>
public struct Vector2(float x, float y) : IEquatable<Vector2>
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
    /// Get an empty vector.
    /// </summary>
    public static Vector2 Zero { get; } = new Vector2(0f, 0f);

    /// <summary>
    /// Get a vector with unit x.
    /// </summary>
    public static Vector2 UnitX { get; } = new Vector2(1f, 0f);

    /// <summary>
    /// Get a vector with unit y.
    /// </summary>
    public static Vector2 UnitY { get; } = new Vector2(0f, 1f);

    /// <summary>
    /// Checks if this vector contains finite coordinates.
    /// Note: in Box2D this is a method, but I transformed it to a property for convenience.
    /// </summary>
    /// <returns>True if both components are finite, otherwise false.</returns>
    public readonly bool IsValid => float.IsFinite(X) && float.IsFinite(Y);

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
    /// Set this vector to all zeros.
    /// </summary>
    public void SetZero()
    {
        X = 0f;
        Y = 0f;
    }

    /// <summary>
    /// Gets the skew vector such that dot(skewVec, other) == cross(vec, other).
    /// </summary>
    /// <returns>The skew vector.</returns>
    public readonly Vector2 Skew() => new Vector2(-Y, X);

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
            return 0f;

        float invLength = 1f / length;
        X *= invLength;
        Y *= invLength;

        return length;
    }

    /// <inheritdoc />
    public readonly bool Equals(Vector2 other) => X == other.X && Y == other.Y;

    /// <inheritdoc />
    public readonly override bool Equals(object? obj) => obj is Vector2 other && Equals(other);

    /// <inheritdoc />
    public readonly override int GetHashCode() => HashCode.Combine(X, Y);

    /// <inheritdoc />
    public readonly override string ToString() => $"(X: {X}, Y: {Y})";

    /// <summary>
    /// Computes the distance between two points represented by vectors.
    /// </summary>
    /// <param name="left">The first point.</param>
    /// <param name="right">The second point.</param>
    /// <returns>The distance between the two points.</returns>
    public static float Distance(in Vector2 left, in Vector2 right)
    {
        float dx = left.X - right.X;
        float dy = left.Y - right.Y;
        return MathF.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// Computes the squared distance between two points represented by vectors.
    /// This is faster than computing the distance and avoids a square root operation.
    /// </summary>
    /// <param name="left">The first point.</param>
    /// <param name="right">The second point.</param>
    /// <returns>The squared distance between the two points.</returns>
    public static float DistanceSquared(in Vector2 left, in Vector2 right)
    {
        float dx = left.X - right.X;
        float dy = left.Y - right.Y;
        return dx * dx + dy * dy;
    }

    /// <summary>
    /// Linearly interpolates between two vectors based on a given interpolation factor.
    /// </summary>
    /// <param name="start">The starting vector.</param>
    /// <param name="end">The target vector.</param>
    /// <param name="factor">The interpolation factor, typically between 0 and 1.</param>
    /// <returns>A new Vector2 representing the interpolated result.</returns>
    public static Vector2 Lerp(in Vector2 start, in Vector2 end, float factor)
    {
        // Ensures t is within the valid range
        factor = Math.Clamp(factor, 0f, 1f);

        // Perform linear interpolation for each component (X, Y)
        float x = start.X + (end.X - start.X) * factor;
        float y = start.Y + (end.Y - start.Y) * factor;

        return new Vector2(x, y);
    }

    /// <summary>
    /// Computes the dot product of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The dot product of the two vectors.</returns>
    public static float Dot(in Vector2 left, in Vector2 right) => left.X * right.X + left.Y * right.Y;

    /// <summary>
    /// Computes the cross product of two vectors. In 2D, this produces a scalar.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The scalar cross product of the two vectors.</returns>
    public static float Cross(in Vector2 left, in Vector2 right) => left.X * right.Y - left.Y * right.X;

    /// <summary>
    /// Computes the cross product of a vector and a scalar. In 2D, this produces a vector.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <param name="scalar">The scalar.</param>
    /// <returns>The resulting vector.</returns>
    public static Vector2 Cross(in Vector2 vector, float scalar) => new Vector2(scalar * vector.Y, -scalar * vector.X);

    /// <summary>
    /// Computes the cross product of a scalar and a vector. In 2D, this produces a vector.
    /// </summary>
    /// <param name="scalar">The scalar.</param>
    /// <param name="vector">The vector.</param>
    /// <returns>The resulting vector.</returns>
    public static Vector2 Cross(float scalar, in Vector2 vector) => new Vector2(-scalar * vector.Y, scalar * vector.X);

    /// <summary>
    /// Checks if two vectors are equal.
    /// </summary>
    public static bool operator ==(in Vector2 left, in Vector2 right) => left.Equals(right);

    /// <summary>
    /// Checks if two vectors are not equal.
    /// </summary>
    public static bool operator !=(in Vector2 left, in Vector2 right) => !left.Equals(right);

    /// <summary>
    /// Negates this vector.
    /// </summary>
    /// <param name="vector">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    public static Vector2 operator -(in Vector2 vector) => new Vector2(-vector.X, -vector.Y);

    /// <summary>
    /// Adds a vector to this vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector to add.</param>
    /// <returns>The result of adding the two vectors.</returns>
    public static Vector2 operator +(in Vector2 left, in Vector2 right) =>
        new Vector2(left.X + right.X, left.Y + right.Y);

    /// <summary>
    /// Subtracts a vector from this vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector to subtract.</param>
    /// <returns>The result of subtracting the second vector from the first vector.</returns>
    public static Vector2 operator -(in Vector2 left, in Vector2 right) =>
        new Vector2(left.X - right.X, left.Y - right.Y);

    /// <summary>
    /// Multiplies a vector from this vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of multiplying the second vector from the first vector.</returns>
    public static Vector2 operator *(in Vector2 left, in Vector2 right) =>
        new Vector2(left.X * right.X, left.Y * right.Y);

    /// <summary>
    /// Multiplies this vector by a scalar.
    /// </summary>
    /// <param name="vector">The vector to multiply.</param>
    /// <param name="scalar">The scalar to multiply by.</param>
    /// <returns>The result of multiplying the vector by the scalar.</returns>
    public static Vector2 operator *(in Vector2 vector, float scalar) =>
        new Vector2(vector.X * scalar, vector.Y * scalar);

    /// <summary>
    /// Multiplies this vector by a scalar (scalar on the left side).
    /// </summary>
    /// <param name="scalar">The scalar to multiply by.</param>
    /// <param name="vector">The vector to multiply.</param>
    /// <returns>The result of multiplying the scalar by the vector.</returns>
    public static Vector2 operator *(float scalar, in Vector2 vector) =>
        new Vector2(vector.X * scalar, vector.Y * scalar);

    /// <summary>
    /// Divides a vector by another vector component-wise.
    /// </summary>
    /// <param name="left">The numerator vector.</param>
    /// <param name="right">The denominator vector.</param>
    /// <returns>The result of component-wise division.</returns>
    /// <exception cref="DivideByZeroException">Thrown if any component of the divisor vector is zero.</exception>
    public static Vector2 operator /(in Vector2 left, in Vector2 right)
    {
        if (right.X == 0 || right.Y == 0)
            throw new DivideByZeroException("Cannot divide by a vector with zero components.");

        return new Vector2(left.X / right.X, left.Y / right.Y);
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="vector">The numerator vector.</param>
    /// <param name="scalar">The denominator scalar.</param>
    /// <returns>The result of dividing the vector by the scalar.</returns>
    /// <exception cref="DivideByZeroException">Thrown if scalar is zero.</exception>
    public static Vector2 operator /(in Vector2 vector, float scalar)
    {
        if (scalar == 0)
            throw new DivideByZeroException("Cannot divide by zero.");

        return new Vector2(vector.X / scalar, vector.Y / scalar);
    }

    /// <summary>
    /// Divides a scalar by a vector component-wise.
    /// </summary>
    /// <param name="scalar">The numerator scalar.</param>
    /// <param name="vector">The denominator vector.</param>
    /// <returns>The result of dividing the scalar by each component of the vector.</returns>
    /// <exception cref="DivideByZeroException">Thrown if any component of the vector is zero.</exception>
    public static Vector2 operator /(float scalar, in Vector2 vector)
    {
        if (vector.X == 0 || vector.Y == 0)
            throw new DivideByZeroException("Cannot divide by a vector with zero components.");

        return new Vector2(scalar / vector.X, scalar / vector.Y);
    }
}