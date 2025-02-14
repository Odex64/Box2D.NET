using Box2D.NET.Common.Primitives;
using NUnit.Framework;

namespace Box2D.NET.Unit.Primitives;

[TestFixture]
public sealed class Vector2Unit
{
    [Test]
    public void Constructor()
    {
        Vector2 vector = new Vector2(3f, 4f);

        Assert.Multiple(() =>
        {
            Assert.That(vector.X, Is.EqualTo(3f));
            Assert.That(vector.Y, Is.EqualTo(4f));
        });
    }

    [Test]
    public void Length()
    {
        Vector2 vector = new Vector2(3f, 4f);
        float length = vector.Length();

        Assert.That(length, Is.EqualTo(5f));
    }

    [Test]
    public void Normalize()
    {
        Vector2 vector = new Vector2(3f, 4f);
        float length = vector.Normalize();

        Assert.Multiple(() =>
        {
            Assert.That(length, Is.EqualTo(5f));
            Assert.That(vector, Is.EqualTo(new Vector2(0.6f, 0.8f)));
        });
    }

    [Test]
    public void AddOperator()
    {
        Vector2 v1 = new Vector2(1f, 2f);
        Vector2 v2 = new Vector2(3f, 4f);
        Vector2 result = v1 + v2;

        Assert.That(result, Is.EqualTo(new Vector2(4f, 6f)));
    }

    [Test]
    public void SubtractOperator()
    {
        Vector2 v1 = new Vector2(5f, 7f);
        Vector2 v2 = new Vector2(2f, 3f);
        Vector2 result = v1 - v2;

        Assert.That(result, Is.EqualTo(new Vector2(3f, 4f)));
    }

    [Test]
    public void Dot()
    {
        Vector2 v1 = new Vector2(2f, 3f);
        Vector2 v2 = new Vector2(4f, 5f);
        float result = Vector2.Dot(v1, v2);

        Assert.That(result, Is.EqualTo(23f));
    }

    [Test]
    public void Cross()
    {
        Vector2 v1 = new Vector2(2f, 3f);
        Vector2 v2 = new Vector2(4f, 5f);
        float result = Vector2.Cross(v1, v2);

        Assert.That(result, Is.EqualTo(-2f));
    }
}