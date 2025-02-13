using Box2D.NET.Common.Primitives;
using Box2D.NET.Unit.Helpers;
using NUnit.Framework;

namespace Box2D.NET.Unit.Primitives;

[TestFixture]
public class Matrix2X2Unit
{
    [Test]
    public void Constructor()
    {
        Matrix2x2 m = new Matrix2x2(1, 2, 3, 4);
        Generics.AreEqual(m.Ex, new Vector2(1, 3));
        Generics.AreEqual(m.Ey, new Vector2(2, 4));
    }

    [Test]
    public void SetIdentity()
    {
        Matrix2x2 m = new Matrix2x2();
        m.SetIdentity();
        Generics.AreEqual(m.Ex, new Vector2(1, 0));
        Generics.AreEqual(m.Ey, new Vector2(0, 1));
    }

    [Test]
    public void Getinverse()
    {
        Matrix2x2 m = new Matrix2x2(4, 7, 2, 6);
        Matrix2x2 inverse = m.GetInverse();
        Generics.AreEqualWithin(inverse, new Matrix2x2(0.6f, -0.7f, -0.2f, 0.4f));
    }

    [Test]
    public void Solve()
    {
        Matrix2x2 m = new Matrix2x2(4, 7, 2, 6);
        Vector2 b = new Vector2(5, 9);
        Vector2 result = m.Solve(b);
        Generics.AreEqualWithin(result, new Vector2(-3.1f, 4.3f));
    }

    [Test]
    public void Multiply()
    {
        Matrix2x2 m1 = new Matrix2x2(1, 2, 3, 4);
        Matrix2x2 m2 = new Matrix2x2(2, 0, 1, 2);
        Matrix2x2 result = Matrix2x2.Multiply(m1, m2);
        Generics.AreEqual(result, new Matrix2x2(4, 4, 10, 8));
    }
}