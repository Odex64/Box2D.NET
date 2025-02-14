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
}