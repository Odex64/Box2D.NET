namespace Box2D.NET.Common;

/// <summary>
/// A 3D column vector with X, Y, and Z components.
/// </summary>
/// <remarks>
/// Constructs a vector with the specified coordinates.
/// </remarks>
/// <param name="x">The X-coordinate.</param>
/// <param name="y">The Y-coordinate.</param>
/// <param name="z">The Z-coordinate.</param>
public struct Vector3(float x, float y, float z)
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
    /// Sets this vector to all zeros.
    /// </summary>
    public void SetZero()
    {
        X = 0f;
        Y = 0f;
        Z = 0f;
    }

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
    /// Negates this vector.
    /// </summary>
    /// <returns>A new vector that is the negation of this vector.</returns>
    public static Vector3 operator -(in Vector3 v) => new(-v.X, -v.Y, -v.Z);

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>The resulting vector after addition.</returns>
    public static Vector3 operator +(in Vector3 a, in Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    /// <summary>
    /// Subtracts the second vector from the first vector.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>The resulting vector after subtraction.</returns>
    public static Vector3 operator -(in Vector3 a, in Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="v">The vector.</param>
    /// <param name="scalar">The scalar value.</param>
    /// <returns>The resulting vector after multiplication.</returns>
    public static Vector3 operator *(in Vector3 v, float scalar) => new(v.X * scalar, v.Y * scalar, v.Z * scalar);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="scalar">The scalar value.</param>
    /// <param name="v">The vector.</param>
    /// <returns>The resulting vector after multiplication.</returns>
    public static Vector3 operator *(float scalar, in Vector3 v) => new(v.X * scalar, v.Y * scalar, v.Z * scalar);

    /// <summary>
    /// Checks if this vector contains finite coordinates.
    /// </summary>
    /// <returns>True if both components are finite, otherwise false.</returns>
    public readonly bool IsValid => !float.IsFinite(X) && !float.IsFinite(Y) && !float.IsFinite(Z);

    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>The dot product of the two vectors.</returns>
    public static float Dot(in Vector3 a, in Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    /// <summary>
    /// Calculates the cross product of two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>A new vector that is the cross product of the two input vectors.</returns>
    public static Vector3 Cross(in Vector3 a, in Vector3 b) => new(
        a.Y * b.Z - a.Z * b.Y,
        a.Z * b.X - a.X * b.Z,
        a.X * b.Y - a.Y * b.X
    );
}