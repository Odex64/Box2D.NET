using System;
using System.Numerics;

namespace Box2D.NET.Common;

/// <summary>
/// Provides mathematical helper methods for numeric operations.
/// </summary>
/// <remarks>
/// This class contains utility methods for mathematical operations
/// that can work with various numeric types.
/// </remarks>
public static class MathHelper
{
    /// <summary>
    /// Determines if a number is a power of two.
    /// </summary>
    /// <typeparam name="T">The numeric type, must implement <see cref="INumber{T}" />.</typeparam>
    /// <param name="value">The number to check.</param>
    /// <returns>True if the number is a power of two; otherwise, false.</returns>
    /// <remarks>
    /// A number is a power of two if it has exactly one bit set in its binary representation.
    /// This method uses a bitwise operation to check if the number is a power of two.
    /// </remarks>
    public static bool IsPowerOfTwo<T>(T value) where T : INumber<T>
    {
        // Handle the special case where value is zero (not a power of two)
        if (value == T.Zero)
        {
            return false;
        }

        // Convert the value to an integer for bitwise operations
        int num = Convert.ToInt32(value);

        // A number is a power of two if it has exactly one bit set in its binary representation
        return (num & (num - 1)) == 0;
    }

    /// <summary>
    /// Returns the next power of two greater than the given number.
    /// </summary>
    /// <typeparam name="T">The numeric type, must implement <see cref="INumber{T}" />.</typeparam>
    /// <param name="value">The number to find the next power of two for.</param>
    /// <returns>The next power of two greater than or equal to <paramref name="value" />.</returns>
    /// <remarks>
    /// This method uses a bitwise trick to find the next power of two:
    /// The method progressively ORs the number with right-shifted versions of itself to fill in all bits
    /// to the right of the most significant bit with `1`s, and then adds `1` to find the next power of two.
    /// </remarks>
    public static T NextPowerOfTwo<T>(T value) where T : INumber<T>
    {
        // Handle the special case where value is zero (next power of two is 1)
        if (value == T.Zero)
        {
            return T.One;
        }

        // Convert to int for bitwise manipulation
        int num = Convert.ToInt32(value);

        num |= num >> 1;
        num |= num >> 2;
        num |= num >> 4;
        num |= num >> 8;
        num |= num >> 16;

        // Return the next power of two (x + 1)
        return (T)Convert.ChangeType(num + 1, typeof(T));
    }
}