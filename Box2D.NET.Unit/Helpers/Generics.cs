using Box2D.NET.Common.Primitives;
using NUnit.Framework;

namespace Box2D.NET.Unit.Helpers;

public static class Generics
{
    /// <summary>
    /// Compares the equality of either matrix or a vector under the limit of 1e-1.
    /// </summary>
    private static void IsArrayWithin1e_1<T>(T a, T e) => Assert.That(a, Is.EqualTo(e).Within(1e-1));

    /// <summary>
    /// Compares the equality of a numeric value that can reach 1e-6.
    /// </summary>
    private static void IsNumericWithin1e_6<T>(T a, T e) => Assert.That(a, Is.EqualTo(e).Within(1e-6));

    /// <summary>
    /// Asserts that two values are equal under the NUnit Framework. For numeric values (double and float),
    /// it allows a small tolerance equal to 1e-6. For other types, it uses the default equality comparison.
    /// </summary>
    /// <typeparam name="T">The type of the values to compare.</typeparam>
    /// <param name="actual">The actual value to compare.</param>
    /// <param name="expected">The expected value to compare.</param>
    public static void AreEqual<T>(T actual, T expected)
    {
        switch (actual)
        {
            // Handle numeric values
            case double d1 when expected is double d2:
                IsNumericWithin1e_6(d1, d2);
                break;
            case float f1 when expected is float f2:
                IsNumericWithin1e_6(f1, f2);
                break;
            // Fallback to default comparison
            default:
                Assert.That(actual, Is.EqualTo(expected));
                break;
        }
    }

    /// <summary>
    /// Asserts that two Matrix2x2 values are equal within a tolerance of 1e-1.
    /// </summary>
    /// <param name="actual">The actual Matrix2x2 value to compare.</param>
    /// <param name="expected">The expected Matrix2x2 value to compare.</param>
    public static void AreEqualWithin(Matrix2x2 actual, Matrix2x2 expected) => IsArrayWithin1e_1(actual, expected);

    /// <summary>
    /// Asserts that two Vector2B values are equal within a tolerance of 1e-1.
    /// </summary>
    /// <param name="actual">The actual Vector2B value to compare.</param>
    /// <param name="expected">The expected Vector2B value to compare.</param>
    public static void AreEqualWithin(Vector2 actual, Vector2 expected) => IsArrayWithin1e_1(actual, expected);
}