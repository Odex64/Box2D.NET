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
        Vector2 point2 = Vector2.Zero;
        float distance = Vector2.Distance(point1, point2);

        Assert.That(distance, Is.EqualTo(5f));
    }

    [Test]
    public void DistanceSquared()
    {
        Vector2 point1 = new Vector2(3f, 4f);
        Vector2 point2 = Vector2.Zero;
        float distanceSquared = Vector2.DistanceSquared(point1, point2);

        Assert.That(distanceSquared, Is.EqualTo(25f));
    }

    [Test]
    public void Lerp()
    {
        Vector2 start = Vector2.Zero;
        Vector2 end = new Vector2(10f, 10f);

        Assert.Multiple(() =>
        {
            Assert.That(Vector2.Lerp(start, end, 0f), Is.EqualTo(Vector2.Zero)); // Start
            Assert.That(Vector2.Lerp(start, end, 1f), Is.EqualTo(new Vector2(10f, 10f))); // End
            Assert.That(Vector2.Lerp(start, end, 0.5f), Is.EqualTo(new Vector2(5f, 5f))); // Midpoint
            Assert.That(Vector2.Lerp(start, end, -1f), Is.EqualTo(Vector2.Zero)); // Clamped to 0
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

    [Test]
    public void Normalize_ZeroVector()
    {
        Vector2 zeroVector = Vector2.Zero;
        float length = zeroVector.Normalize();

        Assert.Multiple(() =>
        {
            Assert.That(length, Is.EqualTo(0f));
            Assert.That(zeroVector, Is.EqualTo(Vector2.Zero)); // Should remain unchanged
        });
    }

    [Test]
    public void Equals_NearEqualVectors()
    {
        Vector2 v1 = new Vector2(1.000001f, 2.000001f);
        Vector2 v2 = new Vector2(1.000002f, 2.000002f);

        Assert.That(v1, Is.EqualTo(v2)); // Should be equal within precision
    }

    [Test]
    public void GetHashCode_Consistency()
    {
        Vector2 v1 = new Vector2(3f, 4f);
        int hash1 = v1.GetHashCode();
        int hash2 = v1.GetHashCode();

        Assert.That(hash1, Is.EqualTo(hash2)); // Hash should remain the same
    }

    [Test]
    public void Lerp_Clamping()
    {
        Vector2 start = Vector2.Zero;
        Vector2 end = new Vector2(10f, 10f);

        Assert.Multiple(() =>
        {
            Assert.That(Vector2.Lerp(start, end, -10f), Is.EqualTo(Vector2.Zero)); // Clamped to 0
            Assert.That(Vector2.Lerp(start, end, 10f), Is.EqualTo(new Vector2(10f, 10f))); // Clamped to 1
        });
    }

    [Test]
    public void Arithmetic_NaN_Infinity()
    {
        Vector2 nanVector = new Vector2(float.NaN, float.NaN);
        Vector2 infVector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

        Assert.Multiple(() =>
        {
            Assert.That(nanVector.IsValid, Is.False);
            Assert.That(infVector.IsValid, Is.False);
        });
    }

    [Test]
    public void StaticProperties_AreImmutable()
    {
        Vector2 originalZero = Vector2.Zero;
        Vector2 originalUnitX = Vector2.UnitX;
        Vector2 originalUnitY = Vector2.UnitY;

        Vector2.Zero.Set(1f, 1f);
        Vector2.UnitX.Set(2f, 2f);
        Vector2.UnitY.Set(3f, 3f);

        Assert.Multiple(() =>
        {
            Assert.That(Vector2.Zero, Is.EqualTo(originalZero));
            Assert.That(Vector2.UnitX, Is.EqualTo(originalUnitX));
            Assert.That(Vector2.UnitY, Is.EqualTo(originalUnitY));
        });
    }

    [Test]
    public void Distance_SamePoint_ShouldBeZero()
    {
        Vector2 point = new Vector2(5f, 5f);
        float distance = Vector2.Distance(point, point);

        Assert.That(distance, Is.EqualTo(0f));
    }

    [Test]
    public void DistanceSquared_SamePoint_ShouldBeZero()
    {
        Vector2 point = new Vector2(5f, 5f);
        float distanceSquared = Vector2.DistanceSquared(point, point);

        Assert.That(distanceSquared, Is.EqualTo(0f));
    }

    [Test]
    public void Skew_ZeroVector()
    {
        Vector2 zeroVector = Vector2.Zero;
        Vector2 skew = zeroVector.Skew();

        Assert.That(skew, Is.EqualTo(Vector2.Zero));
    }
}