using Box2D.NET.Common;

namespace Box2D.NET.Dynamics.Bodies;

/// <summary>
/// A body definition holds all the data needed to construct a rigid body.
/// You can safely re-use body definitions. Shapes are added to a body after construction.
/// </summary>
public struct BodyDef
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BodyDef" /> struct with default values.
    /// </summary>
    public BodyDef()
    {
        Position = new Vector2(0.0f, 0.0f);
        Angle = 0.0f;
        LinearVelocity = new Vector2(0.0f, 0.0f);
        AngularVelocity = 0.0f;
        LinearDamping = 0.0f;
        AngularDamping = 0.0f;
        AllowSleep = true;
        IsAwake = true;
        IsFixedRotation = false;
        IsBullet = false;
        Type = BodyType.Static;
        IsEnabled = true;
        GravityScale = 1.0f;
    }

    /// <summary>
    /// The body type: static, kinematic, or dynamic.
    /// Note: if a dynamic body would have zero mass, the mass is set to one.
    /// </summary>
    public BodyType Type;

    /// <summary>
    /// The world position of the body. Avoid creating bodies at the origin
    /// since this can lead to many overlapping shapes.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// The world angle of the body in radians.
    /// </summary>
    public float Angle;

    /// <summary>
    /// The linear velocity of the body's origin in world coordinates.
    /// </summary>
    public Vector2 LinearVelocity;

    /// <summary>
    /// The angular velocity of the body.
    /// </summary>
    public float AngularVelocity;

    /// <summary>
    /// Linear damping is used to reduce the linear velocity. The damping parameter
    /// can be larger than 1.0f, but the damping effect becomes sensitive to the
    /// time step when the damping parameter is large.
    /// Units are 1/time.
    /// </summary>
    public float LinearDamping;

    /// <summary>
    /// Angular damping is used to reduce the angular velocity. The damping parameter
    /// can be larger than 1.0f, but the damping effect becomes sensitive to the
    /// time step when the damping parameter is large.
    /// Units are 1/time.
    /// </summary>
    public float AngularDamping;

    /// <summary>
    /// Set this flag to false if this body should never fall asleep. Note that
    /// this increases CPU usage.
    /// </summary>
    public bool AllowSleep;

    /// <summary>
    /// Is this body initially awake or sleeping
    /// Note: in Box2D this is called "awake".
    /// </summary>
    public bool IsAwake;

    /// <summary>
    /// Should this body be prevented from rotating? Useful for characters.
    /// Note: in Box2D this is called "fixedRotation".
    /// </summary>
    public bool IsFixedRotation;

    /// <summary>
    /// Is this a fast-moving body that should be prevented from tunneling through
    /// other moving bodies? Note that all bodies are prevented from tunneling through
    /// kinematic and static bodies. This setting is only considered on dynamic bodies.
    /// <para>Warning: Use this flag sparingly since it increases processing time.</para>
    /// Note: in Box2D this is called "bullet"
    /// </summary>
    public bool IsBullet;

    /// <summary>
    /// Does this body start out enabled.
    /// Note: in Box2D this is called "enabled".
    /// </summary>
    public bool IsEnabled;

    /// <summary>
    /// Use this to store application-specific body data.
    /// Note: in Box2D this is a b2BodyUserData type, which is a wrapper of a pointer.
    /// </summary>
    public object? UserData;

    /// <summary>
    /// Scale the gravity applied to this body.
    /// </summary>
    public float GravityScale;
}