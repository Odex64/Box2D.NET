using System;

namespace Box2D.NET.Common.Primitives;

/// <summary>
/// Represents a 2D rotation using sine and cosine values.
/// </summary>
public struct Rotation : IEquatable<Rotation>
{
    /// <summary>
    /// The sine component of the rotation.
    /// </summary>
    public float Sine;

    /// <summary>
    /// The cosine component of the rotation.
    /// </summary>
    public float Cosine;

    /// <summary>
    /// Constructor that initializes the rotation using an angle (in radians).
    /// Calculates the sine and cosine components of the angle.
    /// </summary>
    /// <param name="angle">The angle of rotation in radians.</param>
    public Rotation(float angle)
    {
        Sine = MathF.Sin(angle);
        Cosine = MathF.Cos(angle);
    }

    /// <summary>
    /// Constructor that initializes the rotation using pre-calculated sine and cosine values.
    /// </summary>
    /// <param name="sine">The sine component of the rotation.</param>
    /// <param name="cosine">The cosine component of the rotation.</param>
    public Rotation(float sine, float cosine)
    {
        Sine = sine;
        Cosine = cosine;
    }

    /// <summary>
    /// Sets the rotation using an angle in radians.
    /// </summary>
    /// <param name="angle">The angle in radians.</param>
    public void Set(float angle)
    {
        Sine = MathF.Sin(angle);
        Cosine = MathF.Cos(angle);
    }

    /// <summary>
    /// Sets the rotation to the identity rotation (0 radians).
    /// </summary>
    public void SetIdentity()
    {
        Sine = 0f;
        Cosine = 1f;
    }

    /// <summary>
    /// Gets the angle in radians.
    /// </summary>
    /// <returns>The angle in radians.</returns>
    public readonly float Angle => MathF.Atan2(Sine, Cosine);

    /// <summary>
    /// Gets the x-axis of the rotation.
    /// </summary>
    /// <returns>The x-axis as a 2D vector.</returns>
    public readonly Vector2 AxisX => new Vector2(Cosine, Sine);

    /// <summary>
    /// Gets the y-axis of the rotation.
    /// </summary>
    /// <returns>The y-axis as a 2D vector.</returns>
    public readonly Vector2 AxisY => new Vector2(-Sine, Cosine);

    /// <inheritdoc />
    public readonly bool Equals(Rotation other) => Cosine.ToleranceEquals(other.Cosine) && Sine.ToleranceEquals(other.Sine);

    /// <inheritdoc />
    public readonly override bool Equals(object? obj) => obj is Rotation other && Equals(other);

    /// <inheritdoc />
    public readonly override int GetHashCode() => HashCode.Combine(Sine, Cosine);

    /// <inheritdoc />
    public readonly override string ToString() => $"(Sine: {Sine}, Cosine: {Cosine})";


    /// <summary>
    /// Multiplies two rotations: q * r, which combines two rotations into a single resulting rotation.
    /// </summary>
    /// <param name="left">The first rotation (q).</param>
    /// <param name="right">The second rotation (r).</param>
    /// <returns>The resulting rotation after applying q * r.</returns>
    public static Rotation Multiply(in Rotation left, in Rotation right) => new Rotation(
        left.Sine * right.Cosine + left.Cosine * right.Sine, // sine
        left.Cosine * right.Cosine - left.Sine * right.Sine // cosine
    );

    /// <summary>
    /// Multiplies two rotations in a transposed order: q^T * r, which combines the rotations in reverse order.
    /// </summary>
    /// <param name="left">The first rotation (q).</param>
    /// <param name="right">The second rotation (r).</param>
    /// <returns>The resulting rotation after applying q^T * r.</returns>
    public static Rotation MultiplyTranspose(in Rotation left, in Rotation right) => new Rotation(
        left.Cosine * right.Sine - left.Sine * right.Cosine, // sine
        left.Cosine * right.Cosine + left.Sine * right.Sine // cosine
    );

    /// <summary>
    /// Rotates a vector by a given rotation (q). This performs the transformation of vector v using the rotation q.
    /// </summary>
    /// <param name="rotation">The rotation to apply to the vector.</param>
    /// <param name="vector">The vector to rotate.</param>
    /// <returns>The rotated vector.</returns>
    public static Vector2 Multiply(in Rotation rotation, in Vector2 vector) => new Vector2(
        rotation.Cosine * vector.X - rotation.Sine * vector.Y, // x-component after rotation
        rotation.Sine * vector.X + rotation.Cosine * vector.Y // y-component after rotation
    );

    /// <summary>
    /// Inverse rotates a vector by a given rotation (q), effectively applying q^T (transpose of q) to the vector.
    /// </summary>
    /// <param name="rotation">The rotation to apply the inverse of.</param>
    /// <param name="vector">The vector to rotate.</param>
    /// <returns>The inverse rotated vector.</returns>
    public static Vector2 MultiplyTranspose(in Rotation rotation, in Vector2 vector) => new Vector2(
        rotation.Cosine * vector.X + rotation.Sine * vector.Y, // x-component after inverse rotation
        -rotation.Sine * vector.X + rotation.Cosine * vector.Y // y-component after inverse rotation
    );

    /// <summary>
    /// Checks if two Rotation instances are equal.
    /// </summary>
    /// <param name="left">The first Rotation instance.</param>
    /// <param name="right">The second Rotation instance.</param>
    /// <returns>True if both instances are equal; otherwise, false.</returns>
    public static bool operator ==(in Rotation left, in Rotation right) => left.Equals(right);

    /// <summary>
    /// Checks if two Rotation instances are not equal.
    /// </summary>
    /// <param name="left">The first Rotation instance.</param>
    /// <param name="right">The second Rotation instance.</param>
    /// <returns>True if both instances are not equal; otherwise, false.</returns>
    public static bool operator !=(in Rotation left, in Rotation right) => !left.Equals(right);
}