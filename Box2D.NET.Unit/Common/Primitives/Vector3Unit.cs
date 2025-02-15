using System;
using Box2D.NET.Common.Primitives;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common.Primitives;

[TestFixture]
public sealed class Vector3Unit
{
    [Test]
    public void Constructor()
    {
        Vector3 vector = new Vector3(3f, 4f, 5f);

        Assert.Multiple(() =>
        {
            Assert.That(vector.X, Is.EqualTo(3f));
            Assert.That(vector.Y, Is.EqualTo(4f));
            Assert.That(vector.Z, Is.EqualTo(5f));
        });
    }

    [Test]
    public void IsValid()
    {
        // ReSharper disable once ArrangeMethodOrOperatorBody
        Assert.Multiple(() =>
        {
            Assert.That(new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity).IsValid, Is.False);
            Assert.That(new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).IsValid, Is.False);
            Assert.That(new Vector3(float.NaN, float.NaN, float.NaN).IsValid, Is.False);
            Assert.That(new Vector3(3f, 4f, 5f).IsValid, Is.True);
        });
    }

    [Test]
    public void Length()
    {
        Vector3 vector = new Vector3(3f, 4f, 5f);
        float length = vector.Length();

        Assert.That(length, Is.EqualTo(MathF.Sqrt(50f)));
    }

    [Test]
    public void LengthSquared()
    {
        Vector3 vector = new Vector3(3f, 4f, 5f);
        float length = vector.LengthSquared();

        Assert.That(length, Is.EqualTo(50f));
    }

    [Test]
    public void Normalize()
    {
        Vector3 vector = new Vector3(3f, 4f, 5f);
        float length = vector.Normalize();

        float expectedLength = MathF.Sqrt(50f);
        Assert.Multiple(() =>
        {
            Assert.That(length, Is.EqualTo(expectedLength));
            Assert.That(vector, Is.EqualTo(new Vector3(3f / expectedLength, 4f / expectedLength, 5f / expectedLength)));
        });
    }

    [Test]
    public void Distance()
    {
        Vector3 point1 = new Vector3(3f, 4f, 5f);
        Vector3 point2 = new Vector3(0f, 0f, 0f);
        float distance = Vector3.Distance(point1, point2);

        Assert.That(distance, Is.EqualTo(MathF.Sqrt(50f)));
    }

    [Test]
    public void DistanceSquared()
    {
        Vector3 point1 = new Vector3(3f, 4f, 5f);
        Vector3 point2 = new Vector3(0f, 0f, 0f);
        float distanceSquared = Vector3.DistanceSquared(point1, point2);

        Assert.That(distanceSquared, Is.EqualTo(50f));
    }

    [Test]
    public void Lerp()
    {
        Vector3 start = new Vector3(0f, 0f, 0f);
        Vector3 end = new Vector3(10f, 10f, 10f);

        Assert.Multiple(() =>
        {
            Assert.That(Vector3.Lerp(start, end, 0f), Is.EqualTo(new Vector3(0f, 0f, 0f))); // Start
            Assert.That(Vector3.Lerp(start, end, 1f), Is.EqualTo(new Vector3(10f, 10f, 10f))); // End
            Assert.That(Vector3.Lerp(start, end, 0.5f), Is.EqualTo(new Vector3(5f, 5f, 5f))); // Midpoint
            Assert.That(Vector3.Lerp(start, end, -1f), Is.EqualTo(new Vector3(0f, 0f, 0f))); // Clamped to 0
            Assert.That(Vector3.Lerp(start, end, 2f), Is.EqualTo(new Vector3(10f, 10f, 10f))); // Clamped to 1
        });
    }

    [Test]
    public void Dot()
    {
        Vector3 v1 = new Vector3(2f, 3f, 4f);
        Vector3 v2 = new Vector3(4f, 5f, 6f);
        float result = Vector3.Dot(v1, v2);

        Assert.That(result, Is.EqualTo(47f));
    }

    [Test]
    public void Cross()
    {
        Vector3 v1 = new Vector3(2f, 3f, 4f);
        Vector3 v2 = new Vector3(5f, 6f, 7f);
        Vector3 result = Vector3.Cross(v1, v2);

        Assert.That(result, Is.EqualTo(new Vector3(-3f, 6f, -3f)));
    }

    [Test]
    public void NegateOperator()
    {
        Vector3 vector = new Vector3(1f, 2f, 3f);
        Vector3 result = -vector;

        Assert.That(result, Is.EqualTo(new Vector3(-1f, -2f, -3f)));
    }

    [Test]
    public void AddOperator()
    {
        Vector3 v1 = new Vector3(1f, 2f, 3f);
        Vector3 v2 = new Vector3(4f, 5f, 6f);
        Vector3 result = v1 + v2;

        Assert.That(result, Is.EqualTo(new Vector3(5f, 7f, 9f)));
    }

    [Test]
    public void SubtractOperator()
    {
        Vector3 v1 = new Vector3(5f, 7f, 9f);
        Vector3 v2 = new Vector3(2f, 3f, 4f);
        Vector3 result = v1 - v2;

        Assert.That(result, Is.EqualTo(new Vector3(3f, 4f, 5f)));
    }

    [Test]
    public void MultiplyOperator()
    {
        Vector3 v1 = new Vector3(2f, 3f, 4f);
        Vector3 v2 = new Vector3(4f, 5f, 6f);
        Vector3 result = v1 * v2;

        Assert.That(result, Is.EqualTo(new Vector3(8f, 15f, 24f)));
    }

    [Test]
    public void MultiplyVectorByScalarOperator()
    {
        Vector3 vector = new Vector3(3f, 4f, 5f);
        Vector3 result = vector * 2f;

        Assert.That(result, Is.EqualTo(new Vector3(6f, 8f, 10f)));
    }

    [Test]
    public void MultiplyScalarByVectorOperator()
    {
        Vector3 vector = new Vector3(3f, 4f, 5f);
        Vector3 result = 2f * vector;

        Assert.That(result, Is.EqualTo(new Vector3(6f, 8f, 10f)));
    }

    [Test]
    public void DivideOperator()
    {
        Vector3 v1 = new Vector3(8f, 9f, 10f);
        Vector3 v2 = new Vector3(2f, 3f, 5f);

        Vector3 result = v1 / v2;

        Assert.That(result, Is.EqualTo(new Vector3(4f, 3f, 2f)));
    }

    [Test]
    public void DivideVectorByScalarOperator()
    {
        Vector3 vector = new Vector3(6f, 8f, 10f);
        Vector3 result = vector / 2f;

        Assert.That(result, Is.EqualTo(new Vector3(3f, 4f, 5f)));
    }

    [Test]
    public void DivideScalarByVectorOperator()
    {
        Vector3 vector = new Vector3(2f, 4f, 5f);
        Vector3 result = 8f / vector;

        Assert.That(result, Is.EqualTo(new Vector3(4f, 2f, 1.6f)));
    }

    [Test]
    public void DivideOperatorException()
    {
        Vector3 v1 = new Vector3(4f, 5f, 6f);
        Vector3 v2 = new Vector3(2f, 0f, 1f);

        Assert.Throws<DivideByZeroException>(() =>
        {
            _ = v1 / v2;
        });
    }

    [Test]
    public void DivideVectorByScalarOperatorException()
    {
        Vector3 vector = new Vector3(4f, 5f, 6f);
        Assert.Throws<DivideByZeroException>(() =>
        {
            _ = vector / 0f;
        });
    }

    [Test]
    public void DivideScalarByVectorOperatorException()
    {
        Vector3 vector = new Vector3(2f, 0f, 1f);

        Assert.Throws<DivideByZeroException>(() =>
        {
            _ = 10f / vector;
        });
    }

    [Test]
    public void Normalize_ZeroVector()
    {
        Vector3 zeroVector = new Vector3(0f, 0f, 0f);
        float length = zeroVector.Normalize();

        Assert.Multiple(() =>
        {
            Assert.That(length, Is.EqualTo(0f));
            Assert.That(zeroVector, Is.EqualTo(Vector3.Zero)); // Should remain unchanged
        });
    }

    [Test]
    public void Equals_NearEqualVectors()
    {
        Vector3 v1 = new Vector3(1.000001f, 2.000001f, 3.000001f);
        Vector3 v2 = new Vector3(1.000002f, 2.000002f, 3.000002f);

        Assert.That(v1, Is.EqualTo(v2)); // Should be equal within precision
    }

    [Test]
    public void GetHashCode_Consistency()
    {
        Vector3 v1 = new Vector3(3f, 4f, 5f);
        int hash1 = v1.GetHashCode();
        int hash2 = v1.GetHashCode();

        Assert.That(hash1, Is.EqualTo(hash2)); // Hash should remain the same
    }

    [Test]
    public void Lerp_Clamping()
    {
        Vector3 start = new Vector3(0f, 0f, 0f);
        Vector3 end = new Vector3(10f, 10f, 10f);

        Assert.Multiple(() =>
        {
            Assert.That(Vector3.Lerp(start, end, -10f), Is.EqualTo(new Vector3(0f, 0f, 0f))); // Clamped to 0
            Assert.That(Vector3.Lerp(start, end, 10f), Is.EqualTo(new Vector3(10f, 10f, 10f))); // Clamped to 1
        });
    }

    [Test]
    public void Arithmetic_NaN_Infinity()
    {
        Vector3 nanVector = new Vector3(float.NaN, float.NaN, float.NaN);
        Vector3 infVector = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        Assert.Multiple(() =>
        {
            Assert.That(nanVector.IsValid, Is.False);
            Assert.That(infVector.IsValid, Is.False);
        });
    }

    [Test]
    public void StaticProperties_AreImmutable()
    {
        Vector3 originalZero = Vector3.Zero;
        Vector3 originalUnitX = Vector3.UnitX;
        Vector3 originalUnitY = Vector3.UnitY;
        Vector3 originalUnitZ = Vector3.UnitZ;

        Vector3.Zero.Set(1f, 1f, 1f);
        Vector3.UnitX.Set(2f, 2f, 2f);
        Vector3.UnitY.Set(3f, 3f, 3f);
        Vector3.UnitZ.Set(4f, 4f, 4f);

        Assert.Multiple(() =>
        {
            Assert.That(Vector3.Zero, Is.EqualTo(originalZero));
            Assert.That(Vector3.UnitX, Is.EqualTo(originalUnitX));
            Assert.That(Vector3.UnitY, Is.EqualTo(originalUnitY));
            Assert.That(Vector3.UnitZ, Is.EqualTo(originalUnitZ));
        });
    }

    [Test]
    public void Distance_SamePoint_ShouldBeZero()
    {
        Vector3 point = new Vector3(5f, 5f, 5f);
        float distance = Vector3.Distance(point, point);

        Assert.That(distance, Is.EqualTo(0f));
    }

    [Test]
    public void DistanceSquared_SamePoint_ShouldBeZero()
    {
        Vector3 point = new Vector3(5f, 5f, 5f);
        float distanceSquared = Vector3.DistanceSquared(point, point);

        Assert.That(distanceSquared, Is.EqualTo(0f));
    }

    [Test]
    public void Cross_Product_OrthogonalVectors()
    {
        Vector3 v1 = new Vector3(1f, 0f, 0f);
        Vector3 v2 = new Vector3(0f, 1f, 0f);
        Vector3 result = Vector3.Cross(v1, v2);

        Assert.That(result, Is.EqualTo(new Vector3(0f, 0f, 1f))); // Should be perpendicular
    }

    [Test]
    public void Cross_Product_ParallelVectors()
    {
        Vector3 v1 = new Vector3(2f, 2f, 2f);
        Vector3 v2 = new Vector3(4f, 4f, 4f);
        Vector3 result = Vector3.Cross(v1, v2);

        Assert.That(result, Is.EqualTo(Vector3.Zero)); // Cross product of parallel vectors should be zero
    }
}