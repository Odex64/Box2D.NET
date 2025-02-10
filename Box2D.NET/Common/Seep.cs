using System;

namespace Box2D.NET.Common;

/// <summary>
/// Describes the motion of a body/shape for time of impact (TOI) computation.
/// Shapes are defined with respect to the body origin, which may not coincide with the center of mass.
/// However, to support dynamics, we must interpolate the center of mass position.
/// </summary>
public struct Sweep
{
    /// <summary>
    /// Local center of mass position.
    /// </summary>
    public Vector2 LocalCenter;

    /// <summary>
    /// Center world positions at the initial time (alpha0).
    /// </summary>
    public Vector2 C0;

    /// <summary>
    /// Center world positions at the current time.
    /// </summary>
    public Vector2 C;

    /// <summary>
    /// World angles at the initial time (alpha0).
    /// </summary>
    public float A0;

    /// <summary>
    /// World angles at the current time.
    /// </summary>
    public float A;

    /// <summary>
    /// Fraction of the current time step in the range [0, 1].
    /// C0 and A0 are the positions at alpha0.
    /// </summary>
    public float Alpha0;

    /// <summary>
    /// Default constructor initializes all fields to their default values.
    /// </summary>
    public Sweep()
    {
        LocalCenter = new Vector2();
        C0 = new Vector2();
        C = new Vector2();
        A0 = 0f;
        A = 0f;
        Alpha0 = 0f;
    }

    /// <summary>
    /// Gets the interpolated transform at a specific time.
    /// </summary>
    /// <param name="transform">The output transform.</param>
    /// <param name="beta">A factor in [0, 1], where 0 indicates alpha0.</param>
    public void GetTransform(out Transform transform, float beta) => transform = new Transform
    {
        Position = (1f - beta) * C0 + beta * C,
        Rotation = new Rotation((1f - beta) * A0 + beta * A)
    };

    /// <summary>
    /// Advances the sweep forward, yielding a new initial state.
    /// </summary>
    /// <param name="alpha">The new initial time.</param>
    public void Advance(float alpha)
    {
        if (Alpha0 < 1f)
        {
            float beta = (alpha - Alpha0) / (1f - Alpha0);
            C0 += beta * (C - C0);
            A0 += beta * (A - A0);
            Alpha0 = alpha;
        }
    }

    /// <summary>
    /// Normalizes the angles to the range [-π, π].
    /// </summary>
    public void Normalize()
    {
        float twoPi = 2f * MathF.PI;
        float d = twoPi * MathF.Floor(A0 / twoPi);
        A0 -= d;
        A -= d;
    }
}