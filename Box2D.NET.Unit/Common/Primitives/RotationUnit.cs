using System;
using Box2D.NET.Common.Primitives;
using Box2D.NET.Unit.Helpers;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common.Primitives;

[TestFixture]
public sealed class RotationUnit
{
    [Test]
    public void AngleConstructor()
    {
        const float angle = MathF.PI / 4f; // 45 degrees
        Rotation rotation = new Rotation(angle);

        Assert.Multiple(() =>
        {
            Assert.That(rotation.Sine, Is.EqualTo(MathF.Sin(angle)));
            Assert.That(rotation.Cosine, Is.EqualTo(MathF.Cos(angle)));
        });
    }

    [Test]
    public void CosineSineConstructor()
    {
        const float sine = 0.6f;
        const float cosine = 0.8f;
        Rotation rotation = new Rotation(sine, cosine);

        Assert.Multiple(() =>
        {
            Assert.That(rotation.Sine, Is.EqualTo(sine));
            Assert.That(rotation.Cosine, Is.EqualTo(cosine));
        });
    }

    [Test]
    public void DefaultConstructor()
    {
        Rotation rotation = new Rotation(0f);

        Assert.Multiple(() =>
        {
            Assert.That(rotation.Sine, Is.EqualTo(0f));
            Assert.That(rotation.Cosine, Is.EqualTo(1f));
        });
    }

    [Test]
    public void SetAngle()
    {
        Rotation rotation = new Rotation(0f);
        const float angle = MathF.PI / 3f; // 60 degrees
        rotation.Set(angle);

        Assert.That(rotation, Is.EqualTo(new Rotation(angle)));
    }

    [Test]
    public void SetIdentity()
    {
        const float angle = MathF.PI / 2f;
        Rotation rotation = new Rotation(angle);
        rotation.SetIdentity();

        Assert.That(rotation, Is.EqualTo(new Rotation(0f, 1f)));
    }

    [Test]
    public void GetAngle()
    {
        const float angle = MathF.PI / 6f; // 30 degrees
        Rotation rotation = new Rotation(angle);

        Assert.That(rotation.Angle, Is.EqualTo(angle));
    }

    [Test]
    public void Multiply()
    {
        Rotation rotation1 = new Rotation(MathF.PI / 4f); // 45 degrees
        Rotation rotation2 = new Rotation(MathF.PI / 6f); // 30 degrees
        Rotation result = Rotation.Multiply(rotation1, rotation2);

        float expectedSine = MathF.Sin(MathF.PI / 4f + MathF.PI / 6f);
        float expectedCosine = MathF.Cos(MathF.PI / 4f + MathF.PI / 6f);

        Assert.That(result, Is.EqualTo(new Rotation(expectedSine, expectedCosine)));
    }

    [Test]
    public void MultiplyTranspose()
    {
        Rotation rotation1 = new Rotation(MathF.PI / 4f); // 45 degrees
        Rotation rotation2 = new Rotation(MathF.PI / 6f); // 30 degrees
        Rotation result = Rotation.MultiplyTranspose(rotation1, rotation2);

        float expectedSine = MathF.Sin(MathF.PI / 6f - MathF.PI / 4f);
        float expectedCosine = MathF.Cos(MathF.PI / 6f - MathF.PI / 4f);

        Assert.That(result, Is.EqualTo(new Rotation(expectedSine, expectedCosine)));
    }

    [Test]
    public void MultiplyByVector()
    {
        Rotation rotation = new Rotation(MathF.PI / 2f); // 90 degrees
        Vector2 vector = Vector2.UnitX;
        Vector2 result = Rotation.Multiply(rotation, vector);

        // Rotating (1, 0) by 90 degrees should result in (0, 1)
        Assert.That(result, Is.EqualTo(Vector2.UnitY));
    }

    [Test]
    public void MultiplyTransposeByVector()
    {
        Rotation rotation = new Rotation(MathF.PI / 2f); // 90 degrees
        Vector2 vector = Vector2.UnitY;
        Vector2 result = Rotation.MultiplyTranspose(rotation, vector);

        // Inverse rotating (0, 1) by 90 degrees should result in (1, 0)
        Assert.That(result, Is.EqualTo(Vector2.UnitX));
    }


    [Test]
    public void GetAngleFloatingPointEdgeCase()
    {
        Rotation rotation = new Rotation(MathF.PI / 2f);
        float computedAngle = rotation.Angle;

        Assert.That(computedAngle, Is.EqualTo(MathF.PI / 2f));
    }

    [Test]
    public void SetAngleExtremeValues()
    {
        Rotation rotation = new Rotation(0f);

        rotation.Set(MathF.PI);
        Assert.Multiple(() =>
        {
            Assert.That(rotation.Sine, Is.EqualTo(0f));
            Assert.That(rotation.Cosine, Is.EqualTo(-1f));
        });

        rotation.Set(-MathF.PI);
        Assert.Multiple(() =>
        {
            Assert.That(rotation.Sine, Is.EqualTo(0f));
            Assert.That(rotation.Cosine, Is.EqualTo(-1f));
        });

        rotation.Set(2f * MathF.PI);
        Assert.Multiple(() =>
        {
            Assert.That(rotation.Sine, Is.EqualTo(0f));
            Assert.That(rotation.Cosine, Is.EqualTo(1f));
        });
    }

    [Test]
    public void MultiplyIdentity()
    {
        Rotation identity = new Rotation(0f);
        Rotation rotation = new Rotation(MathF.PI / 4f);

        Rotation result = Rotation.Multiply(rotation, identity);

        Assert.That(result, Is.EqualTo(rotation));
    }

    [Test]
    public void MultiplyTransposeQuarterTurns()
    {
        Rotation rotation90 = new Rotation(MathF.PI / 2f);
        Rotation rotationNeg90 = new Rotation(-MathF.PI / 2f);

        Rotation result = Rotation.Multiply(rotation90, rotationNeg90);

        Assert.That(result.Angle, Generics.ToleranceEqualTo(0f));
    }

    [Test]
    public void MultiplyByStandardVectors()
    {
        Rotation rotation = new Rotation(MathF.PI / 2f); // 90 degrees

        Assert.Multiple(() =>
        {
            Assert.That(Rotation.Multiply(rotation, Vector2.UnitX), Is.EqualTo(Vector2.UnitY)); // (1,0) -> (0,1)
            Assert.That(Rotation.Multiply(rotation, Vector2.UnitY), Is.EqualTo(-Vector2.UnitX)); // (0,1) -> (-1,0)
            Assert.That(Rotation.Multiply(rotation, -Vector2.UnitX), Is.EqualTo(-Vector2.UnitY)); // (-1,0) -> (0,-1)
            Assert.That(Rotation.Multiply(rotation, -Vector2.UnitY), Is.EqualTo(Vector2.UnitX)); // (0,-1) -> (1,0)
        });
    }

    [Test]
    public void MultiplyTransposeByStandardVectors()
    {
        Rotation rotation = new Rotation(MathF.PI / 2f); // 90 degrees

        Assert.Multiple(() =>
        {
            Assert.That(Rotation.MultiplyTranspose(rotation, Vector2.UnitY), Is.EqualTo(Vector2.UnitX)); // (0,1) -> (1,0)
            Assert.That(Rotation.MultiplyTranspose(rotation, -Vector2.UnitX), Is.EqualTo(Vector2.UnitY)); // (-1,0) -> (0,1)
            Assert.That(Rotation.MultiplyTranspose(rotation, -Vector2.UnitY), Is.EqualTo(-Vector2.UnitX)); // (0,-1) -> (-1,0)
            Assert.That(Rotation.MultiplyTranspose(rotation, Vector2.UnitX), Is.EqualTo(-Vector2.UnitY)); // (1,0) -> (0,-1)
        });
    }

    [Test]
    public void GetHashCodeConsistency()
    {
        Rotation rotation = new Rotation(MathF.PI / 4f);
        int hash1 = rotation.GetHashCode();
        int hash2 = rotation.GetHashCode();

        Assert.That(hash1, Is.EqualTo(hash2)); // Hash should remain the same
    }
}