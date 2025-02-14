using System;
using Box2D.NET.Common.Primitives;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common.Primitives;

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
    public void IsValid()
    {
        // ReSharper disable once ArrangeMethodOrOperatorBody
        Assert.Multiple(() =>
        {
            Assert.That(new Vector2(float.NegativeInfinity, float.NegativeInfinity).IsValid, Is.False);
            Assert.That(new Vector2(float.PositiveInfinity, float.PositiveInfinity).IsValid, Is.False);
            Assert.That(new Vector2(float.NaN, float.NaN).IsValid, Is.False);
            Assert.That(new Vector2(3f, 4f).IsValid, Is.True);
        });
    }

    [Test]
    public void Skew()
    {
        Vector2 vector = new Vector2(3f, 4f);
        Vector2 skew = vector.Skew();

        Assert.That(skew, Is.EqualTo(new Vector2(-4f, 3f)));
    }

    [Test]
    public void Length()
    {
        Vector2 vector = new Vector2(3f, 4f);
        float length = vector.Length();

        Assert.That(length, Is.EqualTo(5f));
    }

    [Test]
    public void LengthSquared()
    {
        Vector2 vector = new Vector2(3f, 4f);
        float length = vector.LengthSquared();

        Assert.That(length, Is.EqualTo(25f));
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
    public void Distance()
    {
        Vector2 point1 = new Vector2(3f, 4f);
        Vector2 point2 = new Vector2(0f, 0f);
        float distance = Vector2.Distance(point1, point2);

        Assert.That(distance, Is.EqualTo(5f));
    }

    [Test]
    public void DistanceSquared()
    {
        Vector2 point1 = new Vector2(3f, 4f);
        Vector2 point2 = new Vector2(0f, 0f);
        float distanceSquared = Vector2.DistanceSquared(point1, point2);

        Assert.That(distanceSquared, Is.EqualTo(25f));
    }

    [Test]
    public void Lerp()
    {
        Vector2 start = new Vector2(0f, 0f);
        Vector2 end = new Vector2(10f, 10f);

        Assert.Multiple(() =>
        {
            Assert.That(Vector2.Lerp(start, end, 0f), Is.EqualTo(new Vector2(0f, 0f))); // Start
            Assert.That(Vector2.Lerp(start, end, 1f), Is.EqualTo(new Vector2(10f, 10f))); // End
            Assert.That(Vector2.Lerp(start, end, 0.5f), Is.EqualTo(new Vector2(5f, 5f))); // Midpoint
            Assert.That(Vector2.Lerp(start, end, -1f), Is.EqualTo(new Vector2(0f, 0f))); // Clamped to 0
            Assert.That(Vector2.Lerp(start, end, 2f), Is.EqualTo(new Vector2(10f, 10f))); // Clamped to 1
        });
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
    public void CrossVectors()
    {
        Vector2 v1 = new Vector2(2f, 3f);
        Vector2 v2 = new Vector2(4f, 5f);
        float result = Vector2.Cross(v1, v2);

        Assert.That(result, Is.EqualTo(-2f));
    }

    [Test]
    public void CrossVectorByScalar()
    {
        Vector2 vector = new Vector2(3f, 4f);
        Vector2 result = Vector2.Cross(vector, 2f);

        Assert.That(result, Is.EqualTo(new Vector2(8f, -6f)));
    }

    [Test]
    public void CrossScalarByVector()
    {
        Vector2 vector = new Vector2(3f, 4f);
        Vector2 result = Vector2.Cross(2f, vector);

        Assert.That(result, Is.EqualTo(new Vector2(-8f, 6f)));
    }

    [Test]
    public void NegateOperator()
    {
        Vector2 vector = new Vector2(1f, 2f);
        Vector2 result = -vector;

        Assert.That(result, Is.EqualTo(new Vector2(-1f, -2f)));
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
    public void MultiplyOperator()
    {
        Vector2 v1 = new Vector2(2f, 3f);
        Vector2 v2 = new Vector2(4f, 5f);
        Vector2 result = v1 * v2;

        Assert.That(result, Is.EqualTo(new Vector2(8f, 15f)));
    }

    [Test]
    public void MultiplyVectorByScalarOperator()
    {
        Vector2 vector = new Vector2(3f, 4f);
        Vector2 result = vector * 2f;

        Assert.That(result, Is.EqualTo(new Vector2(6f, 8f)));
    }

    [Test]
    public void MultiplyScalarByVectorOperator()
    {
        Vector2 vector = new Vector2(3f, 4f);
        Vector2 result = 2f * vector;

        Assert.That(result, Is.EqualTo(new Vector2(6f, 8f)));
    }

    [Test]
    public void DivideOperator()
    {
        Vector2 v1 = new Vector2(8f, 9f);
        Vector2 v2 = new Vector2(2f, 3f);

        Vector2 result = v1 / v2;

        Assert.That(result, Is.EqualTo(new Vector2(4f, 3f)));
    }

    [Test]
    public void DivideVectorByScalarOperator()
    {
        Vector2 vector = new Vector2(6f, 8f);
        Vector2 result = vector / 2f;

        Assert.That(result, Is.EqualTo(new Vector2(3f, 4f)));
    }

    [Test]
    public void DivideScalarByVectorOperator()
    {
        Vector2 vector = new Vector2(2f, 4f);
        Vector2 result = 8f / vector;

        Assert.That(result, Is.EqualTo(new Vector2(4f, 2f)));
    }

    [Test]
    public void DivideOperatorException()
    {
        Vector2 v1 = new Vector2(4f, 5f);
        Vector2 v2 = new Vector2(2f, 0f);

        Assert.Throws<DivideByZeroException>(() =>
        {
            _ = v1 / v2;
        });
    }

    [Test]
    public void DivideVectorByScalarOperatorException()
    {
        Vector2 vector = new Vector2(4f, 5f);
        Assert.Throws<DivideByZeroException>(() =>
        {
            _ = vector / 0f;
        });
    }

    [Test]
    public void DivideScalarByVectorOperatorException()
    {
        Vector2 vector = new Vector2(2f, 0f);

        Assert.Throws<DivideByZeroException>(() =>
        {
            _ = 10f / vector;
        });
    }
}