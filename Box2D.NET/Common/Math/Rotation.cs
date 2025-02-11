using System;

namespace Box2D.NET.Common.Math;

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
    public readonly float GetAngle() => MathF.Atan2(Sine, Cosine);

    /// <summary>
    /// Gets the x-axis of the rotation.
    /// </summary>
    /// <returns>The x-axis as a 2D vector.</returns>
    public readonly Vector2 GetXAxis() => new(Cosine, Sine);

    /// <summary>
    /// Gets the y-axis of the rotation.
    /// </summary>
    /// <returns>The y-axis as a 2D vector.</returns>
    public readonly Vector2 GetYAxis() => new(-Sine, Cosine);

    /// <summary>
    /// Determines whether this instance is equal to another Rotation instance.
    /// </summary>
    /// <param name="other">The other Rotation instance to compare.</param>
    /// <returns>True if both instances are equal; otherwise, false.</returns>
    public readonly bool Equals(Rotation other) => Cosine == other.Cosine && Sine == other.Sine;

    /// <summary>
    /// Determines whether this instance is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns>True if the object is a Rotation and equal; otherwise, false.</returns>
    public override readonly bool Equals(object? obj) => obj is Rotation other && Equals(other);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance.</returns>
    public override readonly int GetHashCode() => HashCode.Combine(Sine, Cosine);

    /// <summary>
    /// Returns a string representation of this rotation.
    /// </summary>
    /// <returns>A string representing this rotation.</returns>
    public override readonly string ToString() => $"(Sin: {Sine}, Cos: {Cosine})";

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

    /// <summary>
    /// Multiplies two rotations: q * r, which combines two rotations into a single resulting rotation.
    /// </summary>
    /// <param name="q">The first rotation (q).</param>
    /// <param name="r">The second rotation (r).</param>
    /// <returns>The resulting rotation after applying q * r.</returns>
    public static Rotation Multiply(in Rotation q, in Rotation r)
    {
        Rotation qr = new()
        {
            Sine = q.Sine * r.Cosine + q.Cosine * r.Sine, // sine part of the resulting rotation
            Cosine = q.Cosine * r.Cosine - q.Sine * r.Sine // cosine part of the resulting rotation
        };
        return qr;
    }

    /// <summary>
    /// Multiplies two rotations in a transposed order: q^T * r, which combines the rotations in reverse order.
    /// </summary>
    /// <param name="q">The first rotation (q).</param>
    /// <param name="r">The second rotation (r).</param>
    /// <returns>The resulting rotation after applying q^T * r.</returns>
    public static Rotation MultiplyTranspose(in Rotation q, in Rotation r) => new(
        q.Cosine * r.Sine - q.Sine * r.Cosine, // cosine
        q.Cosine * r.Cosine + q.Sine * r.Sine // sine
    );

    /// <summary>
    /// Rotates a vector by a given rotation (q). This performs the transformation of vector v using the rotation q.
    /// </summary>
    /// <param name="q">The rotation to apply to the vector.</param>
    /// <param name="v">The vector to rotate.</param>
    /// <returns>The rotated vector.</returns>
    public static Vector2 Multiply(in Rotation q, in Vector2 v) => new(
        q.Cosine * v.X - q.Sine * v.Y,  // x-component after rotation
        q.Sine * v.X + q.Cosine * v.Y   // y-component after rotation
    );

    /// <summary>
    /// Inverse rotates a vector by a given rotation (q), effectively applying q^T (transpose of q) to the vector.
    /// </summary>
    /// <param name="q">The rotation to apply the inverse of.</param>
    /// <param name="v">The vector to rotate.</param>
    /// <returns>The inverse rotated vector.</returns>
    public static Vector2 MultiplyTranspose(in Rotation q, in Vector2 v) => new(
        q.Cosine * v.X + q.Sine * v.Y,  // x-component after inverse rotation
        -q.Sine * v.X + q.Cosine * v.Y  // y-component after inverse rotation
    );
}