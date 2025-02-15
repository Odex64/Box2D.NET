using System;
using Box2D.NET.Common.Primitives;
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
        Vector2 vector = new Vector2(1f, 0f);
        Vector2 result = Rotation.Multiply(rotation, vector);

        // Rotating (1, 0) by 90 degrees should result in (0, 1)
        Assert.That(result, Is.EqualTo(new Vector2(0f, 1f)));
    }

    [Test]
    public void MultiplyTransposeByVector()
    {
        Rotation rotation = new Rotation(MathF.PI / 2f); // 90 degrees
        Vector2 vector = new Vector2(0f, 1f);
        Vector2 result = Rotation.MultiplyTranspose(rotation, vector);

        // Inverse rotating (0, 1) by 90 degrees should result in (1, 0)
        Assert.That(result, Is.EqualTo(new Vector2(1f, 0f)));
    }

    [Test]
    public void DefaultConstructor_IsIdentity()
    {
        Rotation rotation = new Rotation(0f);

        Assert.Multiple(() =>
        {
            Assert.That(rotation.Sine, Is.EqualTo(0f));
            Assert.That(rotation.Cosine, Is.EqualTo(1f));
        });
    }

    [Test]
    public void GetAngle_FloatingPointEdgeCase()
    {
        Rotation rotation = new Rotation(MathF.PI / 2f);
        float computedAngle = rotation.Angle;

        Assert.That(computedAngle, Is.EqualTo(MathF.PI / 2f));
    }

    [Test]
    public void SetAngle_ExtremeValues()
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
    public void Multiply_Identity()
    {
        Rotation identity = new Rotation(0f);
        Rotation rotation = new Rotation(MathF.PI / 4f);

        Rotation result = Rotation.Multiply(rotation, identity);

        Assert.That(result, Is.EqualTo(rotation));
    }

    [Test]
    public void Multiply_QuarterTurns()
    {
        Rotation rotation90 = new Rotation(MathF.PI / 2f);
        Rotation rotation180 = new Rotation(MathF.PI);
        Rotation rotation360 = new Rotation(2f * MathF.PI);

        Rotation result90 = Rotation.Multiply(rotation90, rotation90); // 90 + 90 = 180
        Rotation result180 = Rotation.Multiply(rotation180, rotation180); // 180 + 180 = 360
        Rotation result360 = Rotation.Multiply(rotation360, rotation360); // 360 + 360 = 0 (mod 360)

        Assert.Multiple(() =>
        {
            Assert.That(result90.Angle, Is.EqualTo(MathF.PI));
            Assert.That(result180.Angle, Is.EqualTo(2f * MathF.PI));
            Assert.That(result360.Angle, Is.EqualTo(0f));
        });
    }

    [Test]
    public void MultiplyTranspose_QuarterTurns()
    {
        Rotation rotation90 = new Rotation(MathF.PI / 2f);
        Rotation rotationNeg90 = new Rotation(-MathF.PI / 2f);

        Rotation result = Rotation.MultiplyTranspose(rotation90, rotation90);

        Assert.That(result.Angle, Is.EqualTo(0f)); // Should cancel out
    }

    [Test]
    public void MultiplyByVector_StandardVectors()
    {
        Rotation rotation = new Rotation(MathF.PI / 2f); // 90 degrees

        Assert.Multiple(() =>
        {
            Assert.That(Rotation.Multiply(rotation, new Vector2(1f, 0f)), Is.EqualTo(new Vector2(0f, 1f))); // (1,0) -> (0,1)
            Assert.That(Rotation.Multiply(rotation, new Vector2(0f, 1f)), Is.EqualTo(new Vector2(-1f, 0f))); // (0,1) -> (-1,0)
            Assert.That(Rotation.Multiply(rotation, new Vector2(-1f, 0f)), Is.EqualTo(new Vector2(0f, -1f))); // (-1,0) -> (0,-1)
            Assert.That(Rotation.Multiply(rotation, new Vector2(0f, -1f)), Is.EqualTo(new Vector2(1f, 0f))); // (0,-1) -> (1,0)
        });
    }

    [Test]
    public void MultiplyTransposeByVector_StandardVectors()
    {
        Rotation rotation = new Rotation(MathF.PI / 2f); // 90 degrees

        Assert.Multiple(() =>
        {
            Assert.That(Rotation.MultiplyTranspose(rotation, new Vector2(0f, 1f)), Is.EqualTo(new Vector2(1f, 0f))); // (0,1) -> (1,0)
            Assert.That(Rotation.MultiplyTranspose(rotation, new Vector2(-1f, 0f)), Is.EqualTo(new Vector2(0f, 1f))); // (-1,0) -> (0,1)
            Assert.That(Rotation.MultiplyTranspose(rotation, new Vector2(0f, -1f)), Is.EqualTo(new Vector2(-1f, 0f))); // (0,-1) -> (-1,0)
            Assert.That(Rotation.MultiplyTranspose(rotation, new Vector2(1f, 0f)), Is.EqualTo(new Vector2(0f, -1f))); // (1,0) -> (0,-1)
        });
    }

    [Test]
    public void GetHashCode_Consistency()
    {
        Rotation rotation = new Rotation(MathF.PI / 4f);
        int hash1 = rotation.GetHashCode();
        int hash2 = rotation.GetHashCode();

        Assert.That(hash1, Is.EqualTo(hash2)); // Hash should remain the same
    }

}