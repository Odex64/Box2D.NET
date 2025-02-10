using System;

namespace Box2D.NET.Common;

/// <summary>
/// Represents a 2D rotation using sine and cosine values.
/// </summary>
/// <remarks>
/// Initializes the rotation using an angle in radians.
/// </remarks>
/// <param name="angle">The angle in radians.</param>
public struct Rotation(float angle)
{
    /// <summary>
    /// The sine component of the rotation.
    /// </summary>
    public float S = MathF.Sin(angle);

    /// <summary>
    /// The cosine component of the rotation.
    /// </summary>
    public float C = MathF.Cos(angle);

    /// <summary>
    /// Sets the rotation using an angle in radians.
    /// </summary>
    /// <param name="angle">The angle in radians.</param>
    public void Set(float angle)
    {
        S = MathF.Sin(angle);
        C = MathF.Cos(angle);
    }

    /// <summary>
    /// Sets the rotation to the identity rotation (0 radians).
    /// </summary>
    public void SetIdentity()
    {
        S = 0f;
        C = 1f;
    }

    /// <summary>
    /// Gets the angle in radians.
    /// </summary>
    /// <returns>The angle in radians.</returns>
    public readonly float GetAngle() => MathF.Atan2(S, C);

    /// <summary>
    /// Gets the x-axis of the rotation.
    /// </summary>
    /// <returns>The x-axis as a 2D vector.</returns>
    public readonly Vector2 GetXAxis() => new(C, S);

    /// <summary>
    /// Gets the y-axis of the rotation.
    /// </summary>
    /// <returns>The y-axis as a 2D vector.</returns>
    public readonly Vector2 GetYAxis() => new(-S, C);
}