using System;

namespace Box2D.NET.Common;

public static class MathExtensions
{
    /// <summary>
    /// Checks if two float values are equal within a specified tolerance.
    /// </summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <param name="tolerance">Allowed difference (default: 1e-6).</param>
    /// <returns>True if values are approximately equal, otherwise false.</returns>
    public static bool ToleranceEquals(this float a, float b, float tolerance = MathHelper.DefaultFloatTolerance) => Math.Abs(a - b) <= tolerance;

    /// <summary>
    /// Checks if two double values are equal within a specified tolerance.
    /// </summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <param name="tolerance">Allowed difference (default: 1e-9).</param>
    /// <returns>True if values are approximately equal, otherwise false.</returns>
    public static bool ToleranceEquals(this double a, double b, double tolerance = MathHelper.DefaultDoubleTolerance) => Math.Abs(a - b) <= tolerance;
}