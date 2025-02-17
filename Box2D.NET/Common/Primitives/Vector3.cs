using System;

namespace Box2D.NET.Common.Primitives;

/// <summary>
/// A 3D column vector with X, Y, and Z components.
/// </summary>
/// <remarks>
/// Constructs a vector with the specified coordinates.
/// </remarks>
/// <param name="x">The X-coordinate.</param>
/// <param name="y">The Y-coordinate.</param>
/// <param name="z">The Z-coordinate.</param>
public struct Vector3(float x, float y, float z) : IEquatable<Vector3>
{
    /// <summary>
    /// The X-coordinate of the vector.
    /// </summary>
    public float X = x;

    /// <summary>
    /// The Y-coordinate of the vector.
    /// </summary>
    public float Y = y;

    /// <summary>
    /// The Z-coordinate of the vector.
    /// </summary>
    public float Z = z;

    /// <summary>
    /// Get an empty vector.
    /// </summary>
    public static Vector3 Zero { get; } = new Vector3(0f, 0f, 0f);

    /// <summary>
    /// Get a vector with unit x.
    /// </summary>
    public static Vector3 UnitX { get; } = new Vector3(1f, 0f, 0f);

    /// <summary>
    /// Get a vector with unit y.
    /// </summary>
    public static Vector3 UnitY { get; } = new Vector3(0f, 1f, 0f);

    /// <summary>
    /// Get a vector with unit z.
    /// </summary>
    public static Vector3 UnitZ { get; } = new Vector3(0f, 0f, 1f);

    /// <summary>
    /// Checks if this vector contains finite coordinates.
    /// Note: in Box2D this is a method, but I transformed it to a property for convenience.
    /// </summary>
    /// <returns>True if both components are finite, otherwise false.</returns>
    public readonly bool IsValid => float.IsFinite(X) && float.IsFinite(Y) && float.IsFinite(Z);

    /// <summary>
    /// Sets this vector to the specified coordinates.
    /// </summary>
    /// <param name="x">The X-coordinate.</param>
    /// <param name="y">The Y-coordinate.</param>
    /// <param name="z">The Z-coordinate.</param>
    public void Set(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Sets this vector to all zeros.
    /// </summary>
    public void SetZero()
    {
        X = 0f;
        Y = 0f;
        Z = 0f;
    }

    /// <summary>
    /// Gets or sets the component of the vector at the specified index.
    /// Index 0 corresponds to X, index 1 corresponds to Y, and index 2 corresponds to Z.
    /// </summary>
    /// <param name="index">The index of the component to access (0, 1, or 2).</param>
    /// <returns>The value of the component at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown if the index is not 0, 1, or 2.</exception>
    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0: return X;
                case 1: return Y;
                case 2: return Z;
                default:
                    throw new IndexOutOfRangeException("Index for Vector2 must be 0 (X) or 1 (Y).");
            }
        }
        set
        {
            switch (index)
            {
                case 0: X = value; break;
                case 1: Y = value; break;
                case 2: Z = value; break;
                default:
                    throw new IndexOutOfRangeException("Index for Vector2 must be 0 (X) or 1 (Y).");
            }
        }
    }

    /// <summary>
    /// Gets the length (magnitude) of the vector.
    /// </summary>
    public float Length() => MathF.Sqrt(LengthSquared());

    /// <summary>
    /// Gets the squared length (magnitude) of the vector.
    /// More efficient than Length() since it avoids a square root calculation.
    /// </summary>
    public float LengthSquared() => X * X + Y * Y + Z * Z;

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
        Z *= invLength;

        return length;
    }

    /// <inheritdoc />
    public readonly bool Equals(Vector3 other) => X.ToleranceEquals(other.X) && Y.ToleranceEquals(other.Y) && Z.ToleranceEquals(other.Z);

    /// <inheritdoc />
    public readonly override bool Equals(object? obj) => obj is Vector3 other && Equals(other);

    /// <inheritdoc />
    public readonly override int GetHashCode() => HashCode.Combine(X, Y, Z);

    /// <inheritdoc />
    public readonly override string ToString() => $"(X: {X}, Y: {Y}, Z: {Z})";

    /// <summary>
    /// Computes the distance between two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The distance between the vectors.</returns>
    public static float Distance(in Vector3 left, in Vector3 right)
    {
        float dx = left.X - right.X;
        float dy = left.Y - right.Y;
        float dz = left.Z - right.Z;
        return MathF.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    /// <summary>
    /// Computes the squared distance between two vectors.
    /// More efficient than Distance() since it avoids a square root calculation.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The squared distance between the vectors.</returns>
    public static float DistanceSquared(in Vector3 left, in Vector3 right)
    {
        float dx = left.X - right.X;
        float dy = left.Y - right.Y;
        float dz = left.Z - right.Z;
        return dx * dx + dy * dy + dz * dz;
    }

    /// <summary>
    /// Performs a linear interpolation between two vectors.
    /// </summary>
    /// <param name="start">The starting vector.</param>
    /// <param name="end">The ending vector.</param>
    /// <param name="factor">The interpolation factor, typically between 0 and 1.</param>
    /// <returns>A vector representing the linear interpolation between <paramref name="start" /> and <paramref name="end" />.</returns>
    public static Vector3 Lerp(in Vector3 start, in Vector3 end, float factor)
    {
        // Ensures t is within the valid range
        factor = Math.Clamp(factor, 0f, 1f);

        // Perform linear interpolation for each component (X, Y, Z)
        float x = start.X + (end.X - start.X) * factor;
        float y = start.Y + (end.Y - start.Y) * factor;
        float z = start.Z + (end.Z - start.Z) * factor;

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The dot product of the two vectors.</returns>
    public static float Dot(in Vector3 left, in Vector3 right) =>
        left.X * right.X + left.Y * right.Y + left.Z * right.Z;

    /// <summary>
    /// Calculates the cross product of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A new vector that is the cross product of the two input vectors.</returns>
    public static Vector3 Cross(in Vector3 left, in Vector3 right) => new Vector3(
        left.Y * right.Z - left.Z * right.Y,
        left.Z * right.X - left.X * right.Z,
        left.X * right.Y - left.Y * right.X
    );

    /// <summary>
    /// Returns a new <see cref="Vector3" /> containing the maximum values from two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A new <see cref="Vector3" /> where each component is the maximum of the corresponding components in <paramref name="left" /> and <paramref name="right" />.</returns>
    public static Vector3 Max(in Vector3 left, in Vector3 right) => new Vector3(MathF.Max(left.X, right.X), MathF.Max(left.Y, right.Y), MathF.Max(left.Z, right.Z));

    /// <summary>
    /// Returns a new <see cref="Vector3" /> containing the minimum values from two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A new <see cref="Vector3" /> where each component is the minimum of the corresponding components in <paramref name="left" /> and <paramref name="right" />.</returns>
    public static Vector3 Min(in Vector3 left, in Vector3 right) => new Vector3(MathF.Min(left.X, right.X), MathF.Min(left.Y, right.Y), MathF.Min(left.Z, right.Z));

    /// <summary>
    /// Returns a vector where each component is the absolute value of the corresponding component in the input vector.
    /// </summary>
    /// <param name="vector">The input vector.</param>
    /// <returns>A vector with the absolute values of the input vector components.</returns>
    public static Vector3 Abs(in Vector3 vector) => new Vector3(MathF.Abs(vector.X), MathF.Abs(vector.Y), MathF.Abs(vector.Z));

    /// <summary>
    /// Checks if two vectors are equal.
    /// </summary>
    public static bool operator ==(in Vector3 left, in Vector3 right) => left.Equals(right);

    /// <summary>
    /// Checks if two vectors are not equal.
    /// </summary>
    public static bool operator !=(in Vector3 left, in Vector3 right) => !left.Equals(right);

    /// <summary>
    /// Negates this vector.
    /// </summary>
    /// <returns>A new vector that is the negation of this vector.</returns>
    public static Vector3 operator -(in Vector3 vector) => new Vector3(-vector.X, -vector.Y, -vector.Z);

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The resulting vector after addition.</returns>
    public static Vector3 operator +(in Vector3 left, in Vector3 right) => new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    /// <summary>
    /// Subtracts the second vector from the first vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The resulting vector after subtraction.</returns>
    public static Vector3 operator -(in Vector3 left, in Vector3 right) => new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    /// <summary>
    /// Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A new vector where each component is the product of the corresponding components of the input vectors.</returns>
    public static Vector3 operator *(in Vector3 left, in Vector3 right) =>
        new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="v">The vector.</param>
    /// <param name="scalar">The scalar value.</param>
    /// <returns>The resulting vector after multiplication.</returns>
    public static Vector3 operator *(in Vector3 v, float scalar) =>
        new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="scalar">The scalar value.</param>
    /// <param name="v">The vector.</param>
    /// <returns>The resulting vector after multiplication.</returns>
    public static Vector3 operator *(float scalar, in Vector3 v) =>
        new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);

    /// <summary>
    /// Divides a vector by another vector component-wise.
    /// </summary>
    /// <exception cref="DivideByZeroException">Thrown if any component of the divisor vector is zero.</exception>
    public static Vector3 operator /(in Vector3 left, in Vector3 right)
    {
        if (right.X == 0 || right.Y == 0 || right.Z == 0)
        {
            throw new DivideByZeroException("Cannot divide by a vector with zero components.");
        }

        return new Vector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <exception cref="DivideByZeroException">Thrown if scalar is zero.</exception>
    public static Vector3 operator /(in Vector3 vector, float scalar)
    {
        if (scalar == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }

        return new Vector3(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);
    }

    /// <summary>
    /// Divides a scalar by a vector component-wise.
    /// </summary>
    /// <exception cref="DivideByZeroException">Thrown if any component of the vector is zero.</exception>
    public static Vector3 operator /(float scalar, in Vector3 vector)
    {
        if (vector.X == 0 || vector.Y == 0 || vector.Z == 0)
        {
            throw new DivideByZeroException("Cannot divide by a vector with zero components.");
        }

        return new Vector3(scalar / vector.X, scalar / vector.Y, scalar / vector.Z);
    }
}