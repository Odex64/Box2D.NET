using Box2D.NET.Common.Primitives;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common.Primitives;

[TestFixture]
public sealed class Matrix2x2Unit
{
    [Test]
    public void Constructors()
    {
        const float a11 = 1f;
        const float a12 = 2f;
        const float a21 = 3f;
        const float a22 = 4f;

        Vector2 ex = new Vector2(a11, a21);
        Vector2 ey = new Vector2(a12, a22);

        Matrix2x2 matrix = new Matrix2x2(a11, a12, a21, a22);
        Assert.Multiple(() =>
        {
            Assert.That(matrix.Ex, Is.EqualTo(ex));
            Assert.That(matrix.Ey, Is.EqualTo(ey));
        });

        matrix = new Matrix2x2(ex, ey);
        Assert.Multiple(() =>
        {
            Assert.That(matrix.Ex, Is.EqualTo(ex));
            Assert.That(matrix.Ey, Is.EqualTo(ey));
        });
    }

    [Test]
    public void DefaultConstructor()
    {
        Matrix2x2 matrix = new Matrix2x2();

        Assert.Multiple(() =>
        {
            Assert.That(matrix.Ex, Is.EqualTo(Vector2.Zero));
            Assert.That(matrix.Ey, Is.EqualTo(Vector2.Zero));
        });
    }

    [Test]
    public void SetIdentity()
    {
        Matrix2x2 matrix = new Matrix2x2();
        matrix.SetIdentity();

        Assert.That(matrix, Is.EqualTo(new Matrix2x2(1f, 0f, 0f, 1f)));
    }

    [Test]
    public void SetZero()
    {
        Matrix2x2 matrix = new Matrix2x2(1f, 2f, 3f, 4f);
        matrix.SetZero();

        Assert.That(matrix, Is.EqualTo(new Matrix2x2(0f, 0f, 0f, 0f)));
    }

    [Test]
    public void GetInverse()
    {
        Matrix2x2 matrix = new Matrix2x2(4f, 7f, 2f, 6f);
        Matrix2x2 inverse = matrix.GetInverse();

        Assert.That(inverse, Is.EqualTo(new Matrix2x2(0.6f, -0.7f, -0.2f, 0.4f)));
    }

    [Test]
    public void Solve()
    {
        Matrix2x2 matrix = new Matrix2x2(4f, 7f, 2f, 6f);
        Vector2 vector = new Vector2(5f, 9f);
        Vector2 result = matrix.Solve(vector);

        Assert.That(result, Is.EqualTo(new Vector2(-3.3f, 2.6f)));
    }

    [Test]
    public void MultiplyByVector()
    {
        Matrix2x2 matrix = new Matrix2x2(2f, 1f, 3f, 4f);
        Vector2 vector = new Vector2(1f, 2f);
        Vector2 result = Matrix2x2.Multiply(matrix, vector);

        Assert.That(result, Is.EqualTo(new Vector2(4f, 11f)));
    }

    [Test]
    public void MultiplyTransposeByVector()
    {
        Matrix2x2 matrix = new Matrix2x2(2f, 1f, 3f, 4f);
        Vector2 vector = new Vector2(1f, 2f);
        Vector2 result = Matrix2x2.MultiplyTranspose(matrix, vector);

        Assert.That(result, Is.EqualTo(new Vector2(8f, 9f)));
    }

    [Test]
    public void Multiply()
    {
        Matrix2x2 m1 = new Matrix2x2(1f, 2f, 3f, 4f);
        Matrix2x2 m2 = new Matrix2x2(2f, 0f, 1f, 2f);
        Matrix2x2 result = Matrix2x2.Multiply(m1, m2);

        Assert.That(result, Is.EqualTo(new Matrix2x2(4f, 4f, 10f, 8f)));
    }

    [Test]
    public void MultiplyTranspose()
    {
        Matrix2x2 m1 = new Matrix2x2(2f, 1f, 3f, 4f);
        Matrix2x2 m2 = new Matrix2x2(1f, 2f, 3f, 4f);
        Matrix2x2 result = Matrix2x2.MultiplyTranspose(m1, m2);

        Assert.That(result, Is.EqualTo(new Matrix2x2(11f, 16f, 13f, 18f)));
    }

    [Test]
    public void GetInverseSingular()
    {
        Matrix2x2 singularMatrix = new Matrix2x2(2f, 4f, 1f, 2f); // det = 2*2 - 4*1 = 0

        Matrix2x2 inverse = singularMatrix.GetInverse(); // Should not crash, but return zero matrix

        Assert.That(inverse, Is.EqualTo(new Matrix2x2(0f, 0f, 0f, 0f)));
    }

    [Test]
    public void MultiplyIdentity()
    {
        Matrix2x2 identity = new Matrix2x2();
        identity.SetIdentity();
        Matrix2x2 matrix = new Matrix2x2(1f, 2f, 3f, 4f);

        Matrix2x2 result = Matrix2x2.Multiply(matrix, identity);

        Assert.That(result, Is.EqualTo(matrix)); // Multiplying by identity should return the same matrix
    }

    [Test]
    public void MultiplyZero()
    {
        Matrix2x2 zeroMatrix = new Matrix2x2(0f, 0f, 0f, 0f);
        Matrix2x2 matrix = new Matrix2x2(1f, 2f, 3f, 4f);

        Matrix2x2 result = Matrix2x2.Multiply(matrix, zeroMatrix);

        Assert.That(result, Is.EqualTo(zeroMatrix)); // Multiplying by zero matrix should return zero matrix
    }

    [Test]
    public void MultiplyTransposeSymmetric()
    {
        Matrix2x2 symmetric = new Matrix2x2(1f, 2f, 2f, 3f); // Symmetric: Ex.X = Ey.Y, Ex.Y = Ey.X

        Matrix2x2 result = Matrix2x2.MultiplyTranspose(symmetric, symmetric);

        Assert.Multiple(() =>
        {
            Assert.That(result.Ex.X, Is.EqualTo(Vector2.Dot(symmetric.Ex, symmetric.Ex)));
            Assert.That(result.Ex.Y, Is.EqualTo(Vector2.Dot(symmetric.Ey, symmetric.Ex)));
            Assert.That(result.Ey.X, Is.EqualTo(Vector2.Dot(symmetric.Ex, symmetric.Ey)));
            Assert.That(result.Ey.Y, Is.EqualTo(Vector2.Dot(symmetric.Ey, symmetric.Ey)));
        });
    }

    [Test]
    public void SolveByZeroVector()
    {
        Matrix2x2 matrix = new Matrix2x2(4f, 7f, 2f, 6f);
        Vector2 zeroVector = Vector2.Zero;
        Vector2 result = matrix.Solve(zeroVector);

        Assert.That(result, Is.EqualTo(Vector2.Zero)); // Solving Ax = 0 should return x = 0
    }

    [Test]
    public void SolveNearSingularMatrix()
    {
        Matrix2x2 nearSingular = new Matrix2x2(1.00001f, 2f, 1f, 2f);
        Vector2 vector = new Vector2(3f, 4f);
        Vector2 result = nearSingular.Solve(vector);

        Assert.That(result.X, Is.Not.NaN);
        Assert.That(result.Y, Is.Not.NaN);
    }

    [Test]
    public void GetHashCodeConsistency()
    {
        Matrix2x2 matrix = new Matrix2x2(1f, 2f, 3f, 4f);
        int hash1 = matrix.GetHashCode();
        int hash2 = matrix.GetHashCode();

        Assert.That(hash1, Is.EqualTo(hash2)); // Hash should remain the same
    }
}