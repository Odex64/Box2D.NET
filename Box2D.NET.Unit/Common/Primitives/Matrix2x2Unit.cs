using Box2D.NET.Common.Primitives;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common.Primitives;

[TestFixture]
public sealed class Matrix2X2Unit
{
    [Test]
    public void Constructor()
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
    public void SetIdentity()
    {
        Matrix2x2 matrix = new Matrix2x2();
        matrix.SetIdentity();

        Assert.That(matrix, Is.EqualTo(new Matrix2x2(1f, 0f, 0f, 1f)));
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
    public void Multiply()
    {
        Matrix2x2 m1 = new Matrix2x2(1f, 2f, 3f, 4f);
        Matrix2x2 m2 = new Matrix2x2(2f, 0f, 1f, 2f);
        Matrix2x2 result = Matrix2x2.Multiply(m1, m2);

        Assert.That(result, Is.EqualTo(new Matrix2x2(4f, 4f, 10f, 8f)));
    }
}