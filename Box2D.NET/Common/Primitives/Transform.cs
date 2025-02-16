using System;

namespace Box2D.NET.Common.Primitives;

/// <summary>
/// Represents a 2D transform containing translation (position) and rotation.
/// It is used to represent the position and orientation of rigid frames.
/// </summary>
public struct Transform : IEquatable<Transform>
{
    /// <summary>
    /// The translation (position) component of the transform.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// The rotation component of the transform.
    /// </summary>
    public Rotation Rotation;

    /// <summary>
    /// Default constructor initializes the transform to identity (zero position, identity rotation).
    /// </summary>
    public Transform()
    {
        Position = Vector2.Zero;
        Rotation = new Rotation();
    }

    /// <summary>
    /// Initializes the transform using a position vector and a rotation.
    /// </summary>
    /// <param name="rotation">The rotation.</param>
    /// <param name="position">The position vector.</param>
    public Transform(in Rotation rotation, in Vector2 position)
    {
        Rotation = rotation;
        Position = position;
    }

    /// <summary>
    /// Sets this transform based on the position and angle.
    /// </summary>
    /// <param name="position">The position vector.</param>
    /// <param name="angle">The angle in radians.</param>
    public void Set(in Vector2 position, float angle)
    {
        Position = position;
        Rotation.Set(angle);
    }

    /// <summary>
    /// Sets this transform to the identity transform (zero position, identity rotation).
    /// </summary>
    public void SetIdentity()
    {
        Position.SetZero();
        Rotation.SetIdentity();
    }

    /// <inheritdoc />
    public readonly bool Equals(Transform other) => Position.Equals(other.Position) && Rotation.Equals(other.Rotation);

    /// <inheritdoc />
    public readonly override bool Equals(object? obj) => obj is Transform other && Equals(other);

    /// <inheritdoc />
    public readonly override int GetHashCode() => HashCode.Combine(Position, Rotation);

    /// <inheritdoc />
    public readonly override string ToString() => $"(Position: {Position}, Rotation: {Rotation})";

    /// <summary>
    /// Applies the transformation T to the vector v.
    /// </summary>
    /// <param name="transform">The transformation to apply (rotation and translation).</param>
    /// <param name="vector">The vector to transform.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector2 Multiply(in Transform transform, in Vector2 vector)
    {
        // Calculate the new x position: rotate the vector and then apply translation.
        float x = transform.Rotation.Cosine * vector.X - transform.Rotation.Sine * vector.Y + transform.Position.X;

        // Calculate the new y position: rotate the vector and then apply translation.
        float y = transform.Rotation.Sine * vector.X + transform.Rotation.Cosine * vector.Y + transform.Position.Y;

        // Return the new vector after transformation.
        return new Vector2(x, y);
    }

    /// <summary>
    /// Applies the inverse of transformation T to the vector v.
    /// </summary>
    /// <param name="transform">The transformation to invert and apply.</param>
    /// <param name="vector">The vector to transform.</param>
    /// <returns>The vector after applying the inverse transformation.</returns>
    public static Vector2 MultiplyTranspose(in Transform transform, in Vector2 vector)
    {
        // Subtract the position of the transform from the vector to get the relative vector.
        float px = vector.X - transform.Position.X;
        float py = vector.Y - transform.Position.Y;

        // Apply the inverse rotation to the relative vector.
        float x = transform.Rotation.Cosine * px + transform.Rotation.Sine * py;
        float y = -transform.Rotation.Sine * px + transform.Rotation.Cosine * py;

        // Return the transformed vector after applying the inverse rotation.
        return new Vector2(x, y);
    }


    /// <summary>
    /// Combines two transformations A and B into a single transformation.
    /// </summary>
    /// <param name="left">The first transformation.</param>
    /// <param name="right">The second transformation.</param>
    /// <returns>The resulting transformation after combining A and B.</returns>
    public static Transform Multiply(in Transform left, in Transform right) => new Transform(
        Rotation.Multiply(left.Rotation, right.Rotation),
        Rotation.Multiply(left.Rotation, right.Position) + left.Position
    );

    /// <summary>
    /// Combines the inverse of two transformations A and B into a single transformation.
    /// </summary>
    /// <param name="left">The first transformation.</param>
    /// <param name="right">The second transformation.</param>
    /// <returns>The resulting inverse transformation after combining A and B.</returns>
    public static Transform MultiplyTranspose(in Transform left, in Transform right) => new Transform(
        Rotation.MultiplyTranspose(left.Rotation, right.Rotation),
        Rotation.MultiplyTranspose(left.Rotation, right.Position - left.Position)
    );

    /// <summary>
    /// Checks if two transforms are equal.
    /// </summary>
    /// <param name="left">The first transform.</param>
    /// <param name="right">The second transform.</param>
    /// <returns>True if both transforms are equal, otherwise false.</returns>
    public static bool operator ==(in Transform left, in Transform right) => left.Equals(right);

    /// <summary>
    /// Checks if two transforms are not equal.
    /// </summary>
    /// <param name="left">The first transform.</param>
    /// <param name="right">The second transform.</param>
    /// <returns>True if the transforms are not equal, otherwise false.</returns>
    public static bool operator !=(in Transform left, in Transform right) => !left.Equals(right);
}