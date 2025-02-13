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
    public Vector3B Ex;

    /// <summary>
    /// The second column vector of the matrix.
    /// </summary>
    public Vector3B Ey;

    /// <summary>
    /// The third column vector of the matrix.
    /// </summary>
    public Vector3B Ez;

    /// <summary>
    /// Default constructor initializes the matrix to zero.
    /// </summary>
    public Matrix3x3()
    {
        Ex = new Vector3B();
        Ey = new Vector3B();
        Ez = new Vector3B();
    }

    /// <summary>
    /// Constructs a matrix using three column vectors.
    /// </summary>
    /// <param name="c1">The first column vector.</param>
    /// <param name="c2">The second column vector.</param>
    /// <param name="c3">The third column vector.</param>
    public Matrix3x3(in Vector3B c1, in Vector3B c2, in Vector3B c3)
    {
        Ex = c1;
        Ey = c2;
        Ez = c3;
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
    public readonly Vector3B Solve33(Vector3B vector)
    {
        Vector3B crossEyEz = Vector3B.Cross(Ey, Ez);
        Vector3B crossExEz = Vector3B.Cross(Ex, Ez);
        Vector3B crossExEy = Vector3B.Cross(Ex, Ey);

        float det = Vector3B.Dot(Ex, crossEyEz);
        if (det != 0f)
            det = 1f / det;

        float x = det * Vector3B.Dot(vector, crossEyEz);
        float y = det * Vector3B.Dot(Ex, crossExEz);
        float z = det * Vector3B.Dot(Ex, crossExEy);

        return new Vector3B(x, y, z);
    }

    /// <summary>
    /// Solves the equation A * x = b, where b is a column vector.
    /// This is more efficient than computing the inverse in one-shot cases.
    /// Solves only the upper 2x2 matrix equation.
    /// </summary>
    /// <param name="vector">The column vector b.</param>
    /// <returns>The solution vector x.</returns>
    public readonly Vector2B Solve22(Vector2B vector)
    {
        float a11 = Ex.X, a12 = Ey.X, a21 = Ex.Y, a22 = Ey.Y;
        float det = a11 * a22 - a12 * a21;

        if (det != 0f)
            det = 1f / det;

        float x = det * (a22 * vector.X - a12 * vector.Y);
        float y = det * (a11 * vector.Y - a21 * vector.X);

        return new Vector2B(x, y);
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
            det = 1f / det;

        matrix = new Matrix3x3(
            new Vector3B(det * d, -det * c, 0f),
            new Vector3B(-det * b, det * a, 0f),
            new Vector3B(0f, 0f, 0f)
        );
    }

    /// <summary>
    /// Gets the symmetric inverse of this matrix as a 3x3 matrix.
    /// Returns the zero matrix if singular.
    /// </summary>
    /// <param name="matrix">The output symmetric inverse matrix.</param>
    public readonly void GetSymInverse33(out Matrix3x3 matrix)
    {
        float det = Vector3B.Dot(Ex, Vector3B.Cross(Ey, Ez));
        if (det != 0f)
            det = 1f / det;

        float a11 = Ex.X, a12 = Ey.X, a13 = Ez.X;
        float a22 = Ey.Y, a23 = Ez.Y;
        float a33 = Ez.Z;

        matrix = new Matrix3x3(
            new Vector3B(
                det * (a22 * a33 - a23 * a23),
                det * (a13 * a23 - a12 * a33),
                det * (a12 * a23 - a13 * a22)
            ),
            new Vector3B(
                det * (a13 * a23 - a12 * a33),
                det * (a11 * a33 - a13 * a13),
                det * (a13 * a12 - a11 * a23)
            ),
            new Vector3B(
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
    /// Multiplies a 3x3 matrix by a 3D vector.
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <param name="vector">The vector.</param>
    /// <returns>The resulting 3D vector after multiplication.</returns>
    public static Vector3B Multiply(in Matrix3x3 matrix, in Vector3B vector) => vector.X * matrix.Ex + vector.Y * matrix.Ey + vector.Z * matrix.Ez;

    /// <summary>
    /// Multiplies the upper-left 2x2 portion of a 3x3 matrix by a 2D vector.
    /// </summary>
    /// <param name="matrix">The 3x3 matrix.</param>
    /// <param name="vector">The 2D vector.</param>
    /// <returns>The resulting 2D vector after multiplication.</returns>
    public static Vector2B Multiply22(in Matrix3x3 matrix, in Vector2B vector) => new Vector2B(
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