namespace Box2D.NET.Common;

/// <summary>
/// A collection of utility methods for general-purpose operations.
/// </summary>
/// <remarks>
/// This class contains various helper methods, such as the ability to swap values,
/// which can be useful in a variety of programming scenarios.
/// </remarks>
public static class Utils
{
    /// <summary>
    /// Swaps the values of two variables.
    /// </summary>
    /// <typeparam name="T">The type of the variables to swap. This can be any type.</typeparam>
    /// <param name="left">The first value to swap.</param>
    /// <param name="right">The second value to swap.</param>
    /// <remarks>
    /// This method uses tuple deconstruction to swap the values of <paramref name="left" />
    /// and <paramref name="right" /> efficiently. Both values are passed by reference, so
    /// the changes are reflected outside the method.
    /// </remarks>
    public static void Swap<T>(ref T left, ref T right) => (right, left) = (left, right);
}