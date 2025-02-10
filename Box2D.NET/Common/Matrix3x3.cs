namespace Box2D.NET.Common;

/// <summary>
/// Represents a 3x3 matrix with three column vectors (ex, ey, ez).
/// Stored in column-major order.
/// </summary>
public struct Matrix3x3
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
    /// <param name="b">The column vector b.</param>
    /// <returns>The solution vector x.</returns>
    public Vector3 Solve33(Vector3 b)
    {
        float det = Vector3.Dot(Ex, Vector3.Cross(Ey, Ez));
        if (det != 0f)
        {
            det = 1f / det;
        }

        Vector3 x;
        x.X = det * Vector3.Dot(b, Vector3.Cross(Ey, Ez));
        x.Y = det * Vector3.Dot(Ex, Vector3.Cross(b, Ez));
        x.Z = det * Vector3.Dot(Ex, Vector3.Cross(Ey, b));
        return x;
    }

    /// <summary>
    /// Solves the equation A * x = b, where b is a column vector.
    /// This is more efficient than computing the inverse in one-shot cases.
    /// Solves only the upper 2x2 matrix equation.
    /// </summary>
    /// <param name="b">The column vector b.</param>
    /// <returns>The solution vector x.</returns>
    public readonly Vector2 Solve22(Vector2 b)
    {
        float a11 = Ex.X, a12 = Ey.X, a21 = Ex.Y, a22 = Ey.Y;
        float det = a11 * a22 - a12 * a21;
        if (det != 0f)
        {
            det = 1f / det;
        }

        Vector2 x;
        x.X = det * (a22 * b.X - a12 * b.Y);
        x.Y = det * (a11 * b.Y - a21 * b.X);
        return x;
    }

    /// <summary>
    /// Gets the inverse of this matrix as a 2x2 matrix.
    /// Returns the zero matrix if singular.
    /// </summary>
    /// <param name="M">The output inverse matrix.</param>
    public void GetInverse22(out Matrix3x3 M)
    {
        float a = Ex.X, b = Ey.X, c = Ex.Y, d = Ey.Y;
        float det = a * d - b * c;
        if (det != 0f)
        {
            det = 1f / det;
        }

        M = new Matrix3x3
        {
            Ex = new Vector3(det * d, -det * c, 0f),
            Ey = new Vector3(-det * b, det * a, 0f),
            Ez = new Vector3(0f, 0f, 0f)
        };
    }

    /// <summary>
    /// Gets the symmetric inverse of this matrix as a 3x3 matrix.
    /// Returns the zero matrix if singular.
    /// </summary>
    /// <param name="M">The output symmetric inverse matrix.</param>
    public void GetSymInverse33(out Matrix3x3 M)
    {
        float det = Vector3.Dot(Ex, Vector3.Cross(Ey, Ez));
        if (det != 0f)
        {
            det = 1f / det;
        }

        float a11 = Ex.X, a12 = Ey.X, a13 = Ez.X;
        float a22 = Ey.Y, a23 = Ez.Y;
        float a33 = Ez.Z;

        M = new Matrix3x3
        {
            Ex = new Vector3(
                det * (a22 * a33 - a23 * a23),
                det * (a13 * a23 - a12 * a33),
                det * (a12 * a23 - a13 * a22)
            ),
            Ey = new Vector3(
                det * (a13 * a23 - a12 * a33),
                det * (a11 * a33 - a13 * a13),
                det * (a13 * a12 - a11 * a23)
            ),
            Ez = new Vector3(
                det * (a12 * a23 - a13 * a22),
                det * (a13 * a12 - a11 * a23),
                det * (a11 * a22 - a12 * a12)
            )
        };
    }

    /// <summary>
    /// Multiplies a 3x3 matrix by a 3D vector.
    /// </summary>
    /// <param name="A">The matrix.</param>
    /// <param name="v">The vector.</param>
    /// <returns>The resulting 3D vector after multiplication.</returns>
    public static Vector3 Multiply(in Matrix3x3 A, in Vector3 v) => v.X * A.Ex + v.Y * A.Ey + v.Z * A.Ez;

    /// <summary>
    /// Multiplies the upper-left 2x2 portion of a 3x3 matrix by a 2D vector.
    /// </summary>
    /// <param name="A">The 3x3 matrix.</param>
    /// <param name="v">The 2D vector.</param>
    /// <returns>The resulting 2D vector after multiplication.</returns>
    public static Vector2 Multiply22(in Matrix3x3 A, in Vector2 v) => new(
        A.Ex.X * v.X + A.Ey.X * v.Y,
        A.Ex.Y * v.X + A.Ey.Y * v.Y
    );
}