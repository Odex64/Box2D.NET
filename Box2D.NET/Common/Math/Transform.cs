using System;

namespace Box2D.NET.Common.Math;

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
        Position = new Vector2();
        Rotation = new Rotation();
    }

    /// <summary>
    /// Initializes the transform using a position vector and a rotation.
    /// </summary>
    /// <param name="position">The position vector.</param>
    /// <param name="rotation">The rotation.</param>
    public Transform(in Vector2 position, in Rotation rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    /// <summary>
    /// Sets this transform to the identity transform (zero position, identity rotation).
    /// </summary>
    public void SetIdentity()
    {
        Position.SetZero();
        Rotation.SetIdentity();
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
    /// Checks whether this transform is equal to another transform.
    /// </summary>
    /// <param name="other">The transform to compare with.</param>
    /// <returns>True if the transforms are equal, otherwise false.</returns>
    public readonly bool Equals(Transform other) => Position.Equals(other.Position) && Rotation.Equals(other.Rotation);

    /// <summary>
    /// Determines whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns>True if the specified object is a Transform and equal to this instance.</returns>
    public override readonly bool Equals(object? obj) => obj is Transform other && Equals(other);

    /// <summary>
    /// Returns the hash code for this transform.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override readonly int GetHashCode() => HashCode.Combine(Position, Rotation);

    /// <summary>
    /// Returns a string representation of this transform.
    /// </summary>
    /// <returns>A string that represents the transform.</returns>
    public override readonly string ToString() => $"(Position: {Position}, Rotation: {Rotation})";

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

    /// <summary>
    /// Applies the transformation T to the vector v.
    /// </summary>
    /// <param name="T">The transformation to apply (rotation and translation).</param>
    /// <param name="v">The vector to transform.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector2 Multiply(in Transform T, in Vector2 v)
    {
        // Calculate the new x position: rotate the vector and then apply translation.
        float x = T.Rotation.Cosine * v.X - T.Rotation.Sine * v.Y + T.Position.X;

        // Calculate the new y position: rotate the vector and then apply translation.
        float y = T.Rotation.Sine * v.X + T.Rotation.Cosine * v.Y + T.Position.Y;

        // Return the new vector after transformation.
        return new Vector2(x, y);
    }

    /// <summary>
    /// Applies the inverse of transformation T to the vector v.
    /// </summary>
    /// <param name="T">The transformation to invert and apply.</param>
    /// <param name="v">The vector to transform.</param>
    /// <returns>The vector after applying the inverse transformation.</returns>
    public static Vector2 MultiplyTranspose(in Transform T, in Vector2 v)
    {
        // Subtract the position of the transform from the vector to get the relative vector.
        float px = v.X - T.Position.X;
        float py = v.Y - T.Position.Y;

        // Apply the inverse rotation to the relative vector.
        float x = T.Rotation.Cosine * px + T.Rotation.Sine * py;
        float y = -T.Rotation.Sine * px + T.Rotation.Cosine * py;

        // Return the transformed vector after applying the inverse rotation.
        return new Vector2(x, y);
    }


    /// <summary>
    /// Combines two transformations A and B into a single transformation.
    /// </summary>
    /// <param name="A">The first transformation.</param>
    /// <param name="B">The second transformation.</param>
    /// <returns>The resulting transformation after combining A and B.</returns>
    public static Transform Multiply(in Transform A, in Transform B)
    {
        // Create a new transformation to store the result.
        Transform C = new()
        {
            // Combine the rotations of A and B (apply B's rotation then A's).
            Rotation = Rotation.Multiply(A.Rotation, B.Rotation),

            // Combine the positions: apply B's position then A's position.
            // Rotate B's position by A's rotation, then add A's position.
            Position = Rotation.Multiply(A.Rotation, B.Position) + A.Position
        };

        // Return the resulting combined transformation.
        return C;
    }


    /// <summary>
    /// Combines the inverse of two transformations A and B into a single transformation.
    /// </summary>
    /// <param name="A">The first transformation.</param>
    /// <param name="B">The second transformation.</param>
    /// <returns>The resulting inverse transformation after combining A and B.</returns>
    public static Transform MultiplyTranspose(in Transform A, in Transform B)
    {
        // Create a new transformation to store the result.
        Transform C = new()
        {
            // Combine the inverse rotations: apply B's rotation, then apply the inverse of A's rotation.
            Rotation = Rotation.MultiplyTranspose(A.Rotation, B.Rotation),

            // Combine the inverse positions: first subtract A's position from B's, then apply the inverse rotation of A.
            Position = Rotation.MultiplyTranspose(A.Rotation, B.Position - A.Position)
        };

        // Return the resulting inverse combined transformation.
        return C;
    }
}