using Box2D.NET.Common.Primitives;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common.Primitives;

[TestFixture]
public sealed class Matrix3x3Unit
{
    [Test]
    public void Constructors()
    {
        const float a11 = 1f;
        const float a12 = 2f;
        const float a13 = 3f;
        const float a21 = 4f;
        const float a22 = 5f;
        const float a23 = 6f;
        const float a31 = 7f;
        const float a32 = 8f;
        const float a33 = 9f;

        Vector3 ex = new Vector3(a11, a21, a31);
        Vector3 ey = new Vector3(a12, a22, a32);
        Vector3 ez = new Vector3(a13, a23, a33);

        Matrix3x3 matrix = new Matrix3x3(a11, a12, a13, a21, a22, a23, a31, a32, a33);
        Assert.Multiple(() =>
        {
            Assert.That(matrix.Ex, Is.EqualTo(ex));
            Assert.That(matrix.Ey, Is.EqualTo(ey));
            Assert.That(matrix.Ez, Is.EqualTo(ez));
        });

        matrix = new Matrix3x3(ex, ey, ez);
        Assert.Multiple(() =>
        {
            Assert.That(matrix.Ex, Is.EqualTo(ex));
            Assert.That(matrix.Ey, Is.EqualTo(ey));
            Assert.That(matrix.Ez, Is.EqualTo(ez));
        });
    }

    [Test]
    public void SetZero()
    {
        Matrix3x3 matrix = new Matrix3x3(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f);
        matrix.SetZero();

        Assert.That(matrix, Is.EqualTo(new Matrix3x3(
            0f, 0f, 0f,
            0f, 0f, 0f,
            0f, 0f, 0f
        )));
    }

    [Test]
    public void Solve33()
    {
        Matrix3x3 matrix = new Matrix3x3(
            1f, 0f, 0f,
            0f, 1f, 0f,
            0f, 0f, 1f
        );

        Vector3 vector = new Vector3(1f, 2f, 3f);
        Vector3 result = matrix.Solve33(vector);

        Assert.That(result, Is.EqualTo(new Vector3(1f, 2f, 3f)));
    }

    [Test]
    public void Solve22()
    {
        Matrix3x3 matrix = new Matrix3x3(
            2f, 1f, 0f,
            1f, 2f, 0f,
            0f, 0f, 0f
        );

        Vector2 vector = new Vector2(4f, 5f);
        Vector2 result = matrix.Solve22(vector);

        Assert.That(result, Is.EqualTo(new Vector2(1f, 2f)));
    }

    [Test]
    public void GetInverse22()
    {
        Matrix3x3 matrix = new Matrix3x3(
            2f, 1f, 0f,
            1f, 2f, 0f,
            0f, 0f, 0f
        );

        matrix.GetInverse22(out Matrix3x3 inverse);

        Matrix3x3 expected = new Matrix3x3(
            2f / 3f, -1f / 3f, 0f,
            -1f / 3f, 2f / 3f, 0f,
            0f, 0f, 0f
        );
        Assert.That(inverse, Is.EqualTo(expected));
    }

    [Test]
    public void GetSymInverse33()
    {
        Matrix3x3 matrix = new Matrix3x3(
            2f, 0f, 0f,
            0f, 2f, 0f,
            0f, 0f, 2f
        );

        matrix.GetSymmetricInverse33(out Matrix3x3 inverse);

        Matrix3x3 expected = new Matrix3x3(
            0.5f, 0f, 0f,
            0f, 0.5f, 0f,
            0f, 0f, 0.5f
        );
        Assert.That(inverse, Is.EqualTo(expected));
    }

    [Test]
    public void MultiplyByVector()
    {
        Matrix3x3 matrix = new Matrix3x3(
            1f, 0f, 0f,
            0f, 1f, 0f,
            0f, 0f, 1f
        );

        Vector3 vector = new Vector3(1f, 2f, 3f);
        Vector3 result = Matrix3x3.Multiply(matrix, vector);

        Assert.That(result, Is.EqualTo(new Vector3(1f, 2f, 3f)));
    }

    [Test]
    public void Multiply22()
    {
        Matrix3x3 matrix = new Matrix3x3(
            1f, 0f, 0f,
            0f, 1f, 0f,
            0f, 0f, 0f
        );

        Vector2 vector = new Vector2(1f, 2f);
        Vector2 result = Matrix3x3.Multiply22(matrix, vector);

        Assert.That(result, Is.EqualTo(new Vector2(1f, 2f)));
    }

    [Test]
    public void DefaultConstructor_IsZero()
    {
        Matrix3x3 matrix = new Matrix3x3();

        Assert.Multiple(() =>
        {
            Assert.That(matrix.Ex, Is.EqualTo(Vector3.Zero));
            Assert.That(matrix.Ey, Is.EqualTo(Vector3.Zero));
            Assert.That(matrix.Ez, Is.EqualTo(Vector3.Zero));
        });
    }

    [Test]
    public void Solve33_SingularMatrix()
    {
        Matrix3x3 singularMatrix = new Matrix3x3(
            1f, 2f, 3f,
            2f, 4f, 6f,  // Row 2 is twice Row 1 → det = 0
            3f, 6f, 9f   // Row 3 is three times Row 1
        );

        Vector3 vector = new Vector3(1f, 2f, 3f);
        Vector3 result = singularMatrix.Solve33(vector); // Should not crash, but return zero vector

        Assert.That(result, Is.EqualTo(Vector3.Zero));
    }

    [Test]
    public void Solve22_NearSingularMatrix()
    {
        Matrix3x3 nearSingular = new Matrix3x3(
            1.00001f, 2f, 0f,
            1f, 2f, 0f,
            0f, 0f, 0f
        );

        Vector2 vector = new Vector2(3f, 4f);
        Vector2 result = nearSingular.Solve22(vector);

        Assert.That(result.X, Is.Not.NaN);
        Assert.That(result.Y, Is.Not.NaN);
    }

    [Test]
    public void SetZero_AfterModification()
    {
        Matrix3x3 matrix = new Matrix3x3(3f, 5f, 7f, 9f, 11f, 13f, 15f, 17f, 19f);
        matrix.SetZero();

        Assert.That(matrix, Is.EqualTo(new Matrix3x3(
            0f, 0f, 0f,
            0f, 0f, 0f,
            0f, 0f, 0f
        )));
    }

    [Test]
    public void GetInverse22_SingularMatrix()
    {
        Matrix3x3 singularMatrix = new Matrix3x3(
            1f, 2f, 0f,
            2f, 4f, 0f,  // det = 1*4 - 2*2 = 0
            0f, 0f, 0f
        );

        singularMatrix.GetInverse22(out Matrix3x3 inverse);

        Assert.That(inverse, Is.EqualTo(new Matrix3x3(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f)));
    }

    [Test]
    public void GetSymmetricInverse33_SingularMatrix()
    {
        Matrix3x3 singularMatrix = new Matrix3x3(
            1f, 2f, 3f,
            2f, 4f, 6f,  // Linearly dependent rows (det = 0)
            3f, 6f, 9f
        );

        singularMatrix.GetSymmetricInverse33(out Matrix3x3 inverse);

        Assert.That(inverse, Is.EqualTo(new Matrix3x3(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f)));
    }

    [Test]
    public void Multiply_MatrixByMatrix()
    {
        Matrix3x3 m1 = new Matrix3x3(
            1f, 2f, 3f,
            4f, 5f, 6f,
            7f, 8f, 9f
        );

        Matrix3x3 m2 = new Matrix3x3(
            9f, 8f, 7f,
            6f, 5f, 4f,
            3f, 2f, 1f
        );

        Matrix3x3 result = Matrix3x3.MultiplyMatrix(m1, m2);

        Assert.That(result, Is.EqualTo(new Matrix3x3(
            30f, 24f, 18f,
            84f, 69f, 54f,
            138f, 114f, 90f
        )));
    }

    [Test]
    public void Multiply_IdentityMatrixByMatrix()
    {
        Matrix3x3 identity = new Matrix3x3(
            1f, 0f, 0f,
            0f, 1f, 0f,
            0f, 0f, 1f
        );
        Matrix3x3 matrix = new Matrix3x3(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f);

        Matrix3x3 result = Matrix3x3.MultiplyMatrix(identity, matrix);

        Assert.That(result, Is.EqualTo(matrix)); // Multiplying by identity should return the same matrix
    }

    [Test]
    public void Multiply_ZeroMatrixByMatrix()
    {
        Matrix3x3 zeroMatrix = new Matrix3x3(
            0f, 0f, 0f,
            0f, 0f, 0f,
            0f, 0f, 0f
        );
        Matrix3x3 matrix = new Matrix3x3(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f);

        Matrix3x3 result = Matrix3x3.MultiplyMatrix(matrix, zeroMatrix);

        Assert.That(result, Is.EqualTo(zeroMatrix)); // Multiplying by zero matrix should return zero matrix
    }

    [Test]
    public void Multiply22_IdentityMatrix()
    {
        Matrix3x3 identity = new Matrix3x3(
            1f, 0f, 0f,
            0f, 1f, 0f,
            0f, 0f, 0f
        );
        Vector2 vector = new Vector2(1f, 2f);

        Vector2 result = Matrix3x3.Multiply22(identity, vector);

        Assert.That(result, Is.EqualTo(vector)); // Multiplying by identity should return the same vector
    }

    [Test]
    public void Multiply22_ZeroMatrix()
    {
        Matrix3x3 zeroMatrix = new Matrix3x3(
            0f, 0f, 0f,
            0f, 0f, 0f,
            0f, 0f, 0f
        );
        Vector2 vector = new Vector2(1f, 2f);

        Vector2 result = Matrix3x3.Multiply22(zeroMatrix, vector);

        Assert.That(result, Is.EqualTo(Vector2.Zero)); // Multiplying by zero matrix should return zero vector
    }

    [Test]
    public void GetHashCode_Consistency()
    {
        Matrix3x3 matrix = new Matrix3x3(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f);
        int hash1 = matrix.GetHashCode();
        int hash2 = matrix.GetHashCode();

        Assert.That(hash1, Is.EqualTo(hash2)); // Hash should remain the same
    }
}