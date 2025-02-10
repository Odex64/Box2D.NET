namespace Box2D.NET.Common;

/// <summary>
/// Represents a 2D transform containing translation (position) and rotation.
/// It is used to represent the position and orientation of rigid frames.
/// </summary>
public struct Transform
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
}