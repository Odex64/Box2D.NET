using System;

namespace Box2D.NET.Common.Math;

/// <summary>
/// Represents a 2x2 matrix with two column vectors (ex and ey).
/// </summary>
public struct Matrix2x2 : IEquatable<Matrix2x2>
{
    /// <summary>
    /// The first column vector of the matrix.
    /// </summary>
    public Vector2 Ex;

    /// <summary>
    /// The second column vector of the matrix.
    /// </summary>
    public Vector2 Ey;

    /// <summary>
    /// Default constructor initializes the matrix to zero.
    /// </summary>
    public Matrix2x2()
    {
        Ex = new Vector2();
        Ey = new Vector2();
    }

    /// <summary>
    /// Constructs a matrix using two column vectors.
    /// </summary>
    /// <param name="c1">The first column vector.</param>
    /// <param name="c2">The second column vector.</param>
    public Matrix2x2(in Vector2 c1, in Vector2 c2)
    {
        Ex = c1;
        Ey = c2;
    }

    /// <summary>
    /// Constructs a matrix using scalar values.
    /// </summary>
    /// <param name="a11">The value at row 1, column 1.</param>
    /// <param name="a12">The value at row 1, column 2.</param>
    /// <param name="a21">The value at row 2, column 1.</param>
    /// <param name="a22">The value at row 2, column 2.</param>
    public Matrix2x2(float a11, float a12, float a21, float a22)
    {
        Ex = new Vector2(a11, a21);
        Ey = new Vector2(a12, a22);
    }

    /// <summary>
    /// Initializes this matrix using two column vectors.
    /// </summary>
    /// <param name="c1">The first column vector.</param>
    /// <param name="c2">The second column vector.</param>
    public void Set(in Vector2 c1, in Vector2 c2)
    {
        Ex = c1;
        Ey = c2;
    }

    /// <summary>
    /// Sets this matrix to the identity matrix.
    /// </summary>
    public void SetIdentity()
    {
        Ex.X = 1f;
        Ey.X = 0f;
        Ex.Y = 0f;
        Ey.Y = 1f;
    }

    /// <summary>
    /// Sets this matrix to all zeros.
    /// </summary>
    public void SetZero()
    {
        Ex.X = 0f;
        Ey.X = 0f;
        Ex.Y = 0f;
        Ey.Y = 0f;
    }

    /// <summary>
    /// Computes the inverse of this matrix.
    /// </summary>
    /// <returns>The inverse matrix.</returns>
    public readonly Matrix2x2 GetInverse()
    {
        float a = Ex.X, b = Ey.X, c = Ex.Y, d = Ey.Y;
        float det = a * d - b * c;
        if (det != 0f)
        {
            det = 1f / det;
        }

        Matrix2x2 inverse;
        inverse.Ex.X = det * d; inverse.Ey.X = -det * b;
        inverse.Ex.Y = -det * c; inverse.Ey.Y = det * a;
        return inverse;
    }

    /// <summary>
    /// Solves the equation A * x = b, where b is a column vector.
    /// This is more efficient than computing the inverse in one-shot cases.
    /// </summary>
    /// <param name="b">The column vector b.</param>
    /// <returns>The solution vector x.</returns>
    public readonly Vector2 Solve(in Vector2 b)
    {
        float a11 = Ex.X, a12 = Ey.X, a21 = Ex.Y, a22 = Ey.Y;
        float det = a11 * a22 - a12 * a21;
        if (det != 0.0f)
        {
            det = 1.0f / det;
        }

        Vector2 x;
        x.X = det * (a22 * b.X - a12 * b.Y);
        x.Y = det * (a11 * b.Y - a21 * b.X);
        return x;
    }

    /// <summary>
    /// Multiplies a matrix by a vector. If the matrix is a rotation matrix,
    /// this transforms the vector from one frame to another.
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <param name="vector">The vector.</param>
    /// <returns>The resulting vector.</returns>
    public static Vector2 Multiply(in Matrix2x2 matrix, in Vector2 vector) => new(
        matrix.Ex.X * vector.X + matrix.Ey.X * vector.Y,
        matrix.Ex.Y * vector.X + matrix.Ey.Y * vector.Y
    );

    /// <summary>
    /// Multiplies the transpose of a matrix by a vector. If the matrix is a rotation matrix,
    /// this transforms the vector from one frame to another (inverse transform).
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <param name="vector">The vector.</param>
    /// <returns>The resulting vector.</returns>
    public static Vector2 MultiplyTranspose(in Matrix2x2 matrix, in Vector2 vector) => new(
        Vector2.Dot(vector, matrix.Ex),
        Vector2.Dot(vector, matrix.Ey)
    );

    /// <summary>
    /// Multiplies two 2x2 matrices.
    /// </summary>
    /// <param name="left">The first matrix.</param>
    /// <param name="right">The second matrix.</param>
    /// <returns>The product of the two matrices.</returns>
    public static Matrix2x2 Multiply(in Matrix2x2 left, in Matrix2x2 right) => new(Multiply(left, right.Ex), Multiply(left, right.Ey));

    /// <summary>
    /// Multiplies the transpose of matrix A by matrix B (A^T * B).
    /// </summary>
    /// <param name="left">The first matrix.</param>
    /// <param name="right">The second matrix.</param>
    /// <returns>The resulting matrix after multiplying A^T by B.</returns>
    public static Matrix2x2 MultiplyTranspose(in Matrix2x2 left, in Matrix2x2 right)
    {
        Vector2 c1 = new(Vector2.Dot(left.Ex, right.Ex), Vector2.Dot(left.Ey, right.Ex));
        Vector2 c2 = new(Vector2.Dot(left.Ex, right.Ey), Vector2.Dot(left.Ey, right.Ey));
        return new Matrix2x2(c1, c2);
    }

    /// <summary>
    /// Checks whether this matrix is equal to another matrix.
    /// </summary>
    /// <param name="other">The matrix to compare with.</param>
    /// <returns>True if the matrices are equal, otherwise false.</returns>
    public readonly bool Equals(Matrix2x2 other) => Ex.Equals(other.Ex) && Ey.Equals(other.Ey);

    /// <summary>
    /// Determines whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns>True if the specified object is a Matrix2x2 and equal to this instance.</returns>
    public override readonly bool Equals(object? obj) => obj is Matrix2x2 other && Equals(other);

    /// <summary>
    /// Returns the hash code for this matrix.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override readonly int GetHashCode() => HashCode.Combine(Ex, Ey);

    /// <summary>
    /// Returns a string representation of this matrix.
    /// </summary>
    /// <returns>A string that represents the matrix.</returns>
    public override readonly string ToString() => $"(Ex: {Ex}, Ey: {Ey})";

    /// <summary>
    /// Checks if two matrices are equal.
    /// </summary>
    /// <param name="left">The first matrix.</param>
    /// <param name="right">The second matrix.</param>
    /// <returns>True if both matrices are equal, otherwise false.</returns>
    public static bool operator ==(in Matrix2x2 left, in Matrix2x2 right) => left.Equals(right);

    /// <summary>
    /// Checks if two matrices are not equal.
    /// </summary>
    /// <param name="left">The first matrix.</param>
    /// <param name="right">The second matrix.</param>
    /// <returns>True if the matrices are not equal, otherwise false.</returns>
    public static bool operator !=(in Matrix2x2 left, in Matrix2x2 right) => !left.Equals(right);
}