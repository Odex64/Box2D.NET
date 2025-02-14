using System;
using System.Diagnostics;

namespace Box2D.NET.Common.Primitives;

/// <summary>
/// Describes the motion of a body/shape for time of impact (TOI) computation.
/// Shapes are defined with respect to the body origin, which may not coincide with the center of mass.
/// However, to support dynamics, we must interpolate the center of mass position.
/// </summary>
public struct Sweep : IEquatable<Sweep>
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
    /// Computes the transform for a given interpolation factor (beta), which is used to
    /// interpolate between two sweep states (initial and current state).
    /// </summary>
    /// <param name="transform">The transform to store the result.</param>
    /// <param name="beta">
    /// The interpolation factor, where 0f corresponds to the initial state (c0, a0) and 1f corresponds
    /// to the current state (c, a).
    /// </param>
    public readonly void GetTransform(ref Transform transform, float beta)
    {
        // Interpolate the position: (1 - beta) * c0 + beta * c
        transform.Position = (1f - beta) * C0 + beta * C;

        // Interpolate the angle: (1 - beta) * a0 + beta * a
        float angle = (1f - beta) * A0 + beta * A;

        // Set the rotation (angle) of the transform.
        transform.Rotation.Set(angle);

        // Shift the position to account for the local center of rotation.
        transform.Position -= Rotation.Multiply(transform.Rotation, LocalCenter);
    }

    /// <summary>
    /// Advances the sweep to a new value of alpha, interpolating the position and angle between
    /// the current and initial states based on the given alpha value.
    /// </summary>
    /// <param name="alpha">The new alpha value (0f to 1f), which represents the interpolation factor.</param>
    public void Advance(float alpha)
    {
        // Assert that alpha0 is less than 1f (the initial alpha value).
        Debug.Assert(Alpha0 < 1f);

        // Calculate the interpolation factor 'beta' based on the change in alpha.
        float beta = (alpha - Alpha0) / (1f - Alpha0);

        // Interpolate the position (c0 to c).
        C0 += beta * (C - C0);

        // Interpolate the angle (a0 to a).
        A0 += beta * (A - A0);

        // Update the Alpha0 value to the new alpha.
        Alpha0 = alpha;
    }

    /// <summary>
    /// Normalizes the angle 'a0' and 'a' to be between -π and π.
    /// This ensures the angles are always within a manageable range.
    /// </summary>
    public void Normalize()
    {
        // Define the constant 2π for angle normalization.
        const float twoPi = 2f * MathF.PI;

        // Compute the "rounding" value to bring 'a0' within the range [-π, π].
        float d = twoPi * MathF.Floor(A0 / twoPi);

        // Subtract the rounding value from 'a0' and 'a' to normalize them.
        A0 -= d;
        A -= d;
    }

    /// <inheritdoc />
    public readonly bool Equals(Sweep other) => LocalCenter.Equals(other.LocalCenter) && C0.Equals(other.C0) && C.Equals(other.C) && A0.ToleranceEquals(other.A0) && A.ToleranceEquals(other.A) && Alpha0.ToleranceEquals(other.Alpha0);

    /// <inheritdoc />
    public readonly override bool Equals(object? obj) => obj is Sweep other && Equals(other);

    /// <inheritdoc />
    public readonly override int GetHashCode() => HashCode.Combine(LocalCenter, C0, C, A0, A, Alpha0);

    /// <inheritdoc />
    public readonly override string ToString() => $"(LocalCenter: {LocalCenter}, C0: {C0}, C: {C}, A0: {A0}, A: {A}, Alpha0: {Alpha0})";

    /// <summary>
    /// Checks if two sweeps are equal.
    /// </summary>
    /// <param name="left">The first sweep.</param>
    /// <param name="right">The second sweep.</param>
    /// <returns>True if both sweeps are equal, otherwise false.</returns>
    public static bool operator ==(in Sweep left, in Sweep right) => left.Equals(right);

    /// <summary>
    /// Checks if two sweeps are not equal.
    /// </summary>
    /// <param name="left">The first sweep.</param>
    /// <param name="right">The second sweep.</param>
    /// <returns>True if the sweeps are not equal, otherwise false.</returns>
    public static bool operator !=(in Sweep left, in Sweep right) => !left.Equals(right);
}