using Box2D.NET.Common.Primitives;
using Box2D.NET.Unit.Helpers;
using NUnit.Framework;

namespace Box2D.NET.Unit.Primitives;

[TestFixture]
public class Vector2Unit
{
    [Test]
    public void Constructor()
    {
        Vector2 vector = new Vector2(3, 4);
        Generics.AreEqual(vector.X, 3);
        Generics.AreEqual(vector.Y, 4);
    }

    [Test]
    public void Length()
    {
        Vector2 vector = new Vector2(3, 4);
        Generics.AreEqual(vector.Length(), 5);
    }

    [Test]
    public void Normalizer()
    {
        Vector2 vector = new Vector2(3, 4);
        float length = vector.Normalize();
        Generics.AreEqual(vector.Length(), 1);
        Generics.AreEqual(length, 5);
    }

    [Test]
    public void AddOperator()
    {
        Vector2 v1 = new Vector2(1, 2);
        Vector2 v2 = new Vector2(3, 4);
        Vector2 result = v1 + v2;
        Generics.AreEqual(result, new Vector2(4, 6));
    }

    [Test]
    public void SubstractOperator()
    {
        Vector2 v1 = new Vector2(5, 7);
        Vector2 v2 = new Vector2(2, 3);
        Vector2 result = v1 - v2;
        Generics.AreEqual(result, new Vector2(3, 4));
    }

    [Test]
    public void Dot()
    {
        Vector2 v1 = new Vector2(2, 3);
        Vector2 v2 = new Vector2(4, 5);
        float result = Vector2.Dot(v1, v2);
        Generics.AreEqual(result, 23);
    }

    [Test]
    public void Cross()
    {
        Vector2 v1 = new Vector2(2, 3);
        Vector2 v2 = new Vector2(4, 5);
        float result = Vector2.Cross(v1, v2);
        Generics.AreEqual(result, -2);
    }
}