using System;

namespace Box2D.NET.Common.Primitives;

/// <summary>
/// Represents a 3x3 matrix with three column vectors (ex, ey, ez).
/// Stored in column-major order.
/// </summary>
public struct Matrix3x3 : IEquatable<Matrix3x3>
{
    /// <summary>
    /// The first column vector of the matrix.
    /// </summary>
    public Vector3 Ex;

    /// <summary>
    /// The second column vector of the matrix.
    /// </summary>
    public Vector3 Ey;

    /// <summary>
    /// The third column vector of the matrix.
    /// </summary>
    public Vector3 Ez;

    /// <summary>
    /// Default constructor initializes the matrix to zero.
    /// </summary>
    public Matrix3x3()
    {
        Ex = new Vector3();
        Ey = new Vector3();
        Ez = new Vector3();
    }

    /// <summary>
    /// Constructs a matrix using three column vectors.
    /// </summary>
    /// <param name="c1">The first column vector.</param>
    /// <param name="c2">The second column vector.</param>
    /// <param name="c3">The third column vector.</param>
    public Matrix3x3(in Vector3 c1, in Vector3 c2, in Vector3 c3)
    {
        Ex = c1;
        Ey = c2;
        Ez = c3;
    }

    /// <summary>
    /// Constructs a matrix using the elements of a 3x3 matrix in row-major order.
    /// </summary>
    /// <param name="a11">The element in row 1, column 1.</param>
    /// <param name="a12">The element in row 1, column 2.</param>
    /// <param name="a13">The element in row 1, column 3.</param>
    /// <param name="a21">The element in row 2, column 1.</param>
    /// <param name="a22">The element in row 2, column 2.</param>
    /// <param name="a23">The element in row 2, column 3.</param>
    /// <param name="a31">The element in row 3, column 1.</param>
    /// <param name="a32">The element in row 3, column 2.</param>
    /// <param name="a33">The element in row 3, column 3.</param>
    public Matrix3x3(
        float a11, float a12, float a13,
        float a21, float a22, float a23,
        float a31, float a32, float a33)
    {
        Ex = new Vector3(a11, a21, a31);
        Ey = new Vector3(a12, a22, a32);
        Ez = new Vector3(a13, a23, a33);
    }

    /// <summary>
    /// Sets this matrix to all zeros.
    /// </summary>
    public void SetZero()
    {
        Ex.SetZero();
        Ey.SetZero();
        Ez.SetZero();
    }

    /// <summary>
    /// Solves the equation A * x = b, where b is a column vector.
    /// This is more efficient than computing the inverse in one-shot cases.
    /// </summary>
    /// <param name="vector">The column vector b.</param>
    /// <returns>The solution vector x.</returns>
    public readonly Vector3 Solve33(in Vector3 vector)
    {
        Vector3 crossEyEz = Vector3.Cross(Ey, Ez);
        float det = Vector3.Dot(Ex, crossEyEz);
        if (det != 0f)
        {
            det = 1f / det;
        }

        float x = det * Vector3.Dot(vector, crossEyEz);
        float y = det * Vector3.Dot(Ex, Vector3.Cross(vector, Ez));
        float z = det * Vector3.Dot(Ex, Vector3.Cross(Ey, vector));

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Solves the equation A * x = b, where b is a column vector.
    /// This is more efficient than computing the inverse in one-shot cases.
    /// Solves only the upper 2x2 matrix equation.
    /// </summary>
    /// <param name="vector">The column vector b.</param>
    /// <returns>The solution vector x.</returns>
    public readonly Vector2 Solve22(in Vector2 vector)
    {
        float a11 = Ex.X, a12 = Ey.X, a21 = Ex.Y, a22 = Ey.Y;
        float det = a11 * a22 - a12 * a21;

        if (det != 0f)
        {
            det = 1f / det;
        }

        float x = det * (a22 * vector.X - a12 * vector.Y);
        float y = det * (a11 * vector.Y - a21 * vector.X);

        return new Vector2(x, y);
    }

    /// <summary>
    /// Gets the inverse of this matrix as a 2x2 matrix.
    /// Returns the zero matrix if singular.
    /// </summary>
    /// <param name="matrix">The output inverse matrix.</param>
    public readonly void GetInverse22(out Matrix3x3 matrix)
    {
        float a = Ex.X, b = Ey.X, c = Ex.Y, d = Ey.Y;
        float det = a * d - b * c;
        if (det != 0f)
        {
            det = 1f / det;
        }

        matrix = new Matrix3x3(
            new Vector3(det * d, -det * c, 0f),
            new Vector3(-det * b, det * a, 0f),
            Vector3.Zero
        );
    }

    /// <summary>
    /// Gets the symmetric inverse of this matrix as a 3x3 matrix.
    /// Returns the zero matrix if singular.
    /// </summary>
    /// <param name="matrix">The output symmetric inverse matrix.</param>
    public readonly void GetSymmetricInverse33(out Matrix3x3 matrix)
    {
        float det = Vector3.Dot(Ex, Vector3.Cross(Ey, Ez));
        if (det != 0f)
        {
            det = 1f / det;
        }

        float a11 = Ex.X, a12 = Ey.X, a13 = Ez.X;
        float a22 = Ey.Y, a23 = Ez.Y;
        float a33 = Ez.Z;

        matrix = new Matrix3x3(
            new Vector3(
                det * (a22 * a33 - a23 * a23),
                det * (a13 * a23 - a12 * a33),
                det * (a12 * a23 - a13 * a22)
            ),
            new Vector3(
                det * (a13 * a23 - a12 * a33),
                det * (a11 * a33 - a13 * a13),
                det * (a13 * a12 - a11 * a23)
            ),
            new Vector3(
                det * (a12 * a23 - a13 * a22),
                det * (a13 * a12 - a11 * a23),
                det * (a11 * a22 - a12 * a12)
            )
        );
    }

    /// <inheritdoc />
    public readonly bool Equals(Matrix3x3 other) => Ex.Equals(other.Ex) && Ey.Equals(other.Ey) && Ez.Equals(other.Ez);

    /// <inheritdoc />
    public readonly override bool Equals(object? obj) => obj is Matrix3x3 other && Equals(other);

    /// <inheritdoc />
    public readonly override int GetHashCode() => HashCode.Combine(Ex, Ey, Ez);

    /// <inheritdoc />
    public readonly override string ToString() => $"(Ex: {Ex}, Ey: {Ey}, Ez: {Ez})";

    /// <summary>
    /// Returns a matrix where each component is the absolute value of the corresponding component in the input matrix.
    /// </summary>
    /// <param name="matrix">The input matrix.</param>
    /// <returns>A matrix with the absolute values of the input matrix components.</returns>
    public static Matrix3x3 Abs(in Matrix3x3 matrix) => new Matrix3x3(Vector3.Abs(matrix.Ex), Vector3.Abs(matrix.Ey), Vector3.Abs(matrix.Ez));

    /// <summary>
    /// Multiplies a 3x3 matrix by a 3D vector.
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <param name="vector">The vector.</param>
    /// <returns>The resulting 3D vector after multiplication.</returns>
    public static Vector3 Multiply(in Matrix3x3 matrix, in Vector3 vector) => vector.X * matrix.Ex + vector.Y * matrix.Ey + vector.Z * matrix.Ez;

    /// <summary>
    /// Multiplies two 3x3 matrices.
    /// </summary>
    /// <param name="a">The first matrix.</param>
    /// <param name="b">The second matrix.</param>
    /// <returns>The resulting matrix after multiplication.</returns>
    public static Matrix3x3 MultiplyMatrix(in Matrix3x3 a, in Matrix3x3 b) => new Matrix3x3(
        Multiply(a, b.Ex), // First column
        Multiply(a, b.Ey), // Second column
        Multiply(a, b.Ez) // Third column
    );

    /// <summary>
    /// Multiplies the upper-left 2x2 portion of a 3x3 matrix by a 2D vector.
    /// </summary>
    /// <param name="matrix">The 3x3 matrix.</param>
    /// <param name="vector">The 2D vector.</param>
    /// <returns>The resulting 2D vector after multiplication.</returns>
    public static Vector2 Multiply22(in Matrix3x3 matrix, in Vector2 vector) => new Vector2(
        matrix.Ex.X * vector.X + matrix.Ey.X * vector.Y,
        matrix.Ex.Y * vector.X + matrix.Ey.Y * vector.Y
    );

    /// <summary>
    /// Checks if two matrices are equal.
    /// </summary>
    /// <param name="left">The first matrix.</param>
    /// <param name="right">The second matrix.</param>
    /// <returns>True if both matrices are equal, otherwise false.</returns>
    public static bool operator ==(in Matrix3x3 left, in Matrix3x3 right) => left.Equals(right);

    /// <summary>
    /// Checks if two matrices are not equal.
    /// </summary>
    /// <param name="left">The first matrix.</param>
    /// <param name="right">The second matrix.</param>
    /// <returns>True if the matrices are not equal, otherwise false.</returns>
    public static bool operator !=(in Matrix3x3 left, in Matrix3x3 right) => !left.Equals(right);
}