namespace Box2D.NET.Dynamics.Bodies;

/// <summary>
/// The body type.
/// </summary>
public enum BodyType : byte
{
    /// <summary>
    /// Static body: zero mass, zero velocity, may be manually moved.
    /// </summary>
    Static,

    /// <summary>
    /// Kinematic body: zero mass, non-zero velocity set by user, moved by solver.
    /// </summary>
    Kinematic,

    /// <summary>
    /// Dynamic body: positive mass, non-zero velocity determined by forces, moved by solver.
    /// </summary>
    Dynamic
}