using System;
using Box2D.NET.Common.Primitives;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common.Primitives;

[TestFixture]
public sealed class SweepUnit
{
    [Test]
    public void GetTransform()
    {
        Sweep sweep = new Sweep
        {
            C0 = new Vector2(1f, 2f),
            C = new Vector2(3f, 4f),
            A0 = 0f,
            A = MathF.PI / 2f, // 90 degrees
            LocalCenter = new Vector2(0.5f, 0.5f)
        };

        Transform transform = new Transform();
        sweep.GetTransform(ref transform, 0.5f);

        // Interpolated position: (1 - 0.5) * C0 + 0.5 * C = (2, 3)
        // Interpolated angle: (1 - 0.5) * A0 + 0.5 * A = π/4 (45 degrees)
        // Shifted position: (2, 3) - Rotate(LocalCenter, π/4)
        Vector2 expectedPosition = new Vector2(2f, 3f) - Rotation.Multiply(new Rotation(MathF.PI / 4f), new Vector2(0.5f, 0.5f));

        Assert.Multiple(() =>
        {
            Assert.That(transform.Position, Is.EqualTo(expectedPosition));
            Assert.That(transform.Rotation.Angle, Is.EqualTo(MathF.PI / 4f));
        });
    }

    [Test]
    public void Advance()
    {
        Sweep sweep = new Sweep
        {
            C0 = new Vector2(1f, 2f),
            C = new Vector2(3f, 4f),
            A0 = 0f,
            A = MathF.PI / 2f, // 90 degrees
            Alpha0 = 0.2f
        };

        sweep.Advance(0.6f);

        // Beta = (0.6 - 0.2) / (1 - 0.2) = 0.5
        // C0 += 0.5 * (C - C0) = (1, 2) + 0.5 * (2, 2) = (2, 3)
        // A0 += 0.5 * (A - A0) = 0 + 0.5 * (π/2 - 0) = π/4
        // Alpha0 = 0.6
        Assert.Multiple(() =>
        {
            Assert.That(sweep.C0, Is.EqualTo(new Vector2(2f, 3f)));
            Assert.That(sweep.A0, Is.EqualTo(MathF.PI / 4f));
            Assert.That(sweep.Alpha0, Is.EqualTo(0.6f));
        });
    }

    [Test]
    public void Normalize()
    {
        Sweep sweep = new Sweep
        {
            A0 = 3f * MathF.PI, // 3π (540 degrees)
            A = 5f * MathF.PI / 2f // 5π/2 (450 degrees)
        };

        sweep.Normalize();

        // A0 = 3π - 2π * Floor(3π / 2π) = 3π - 2π = π
        // A = 5π/2 - 2π * Floor(3π / 2π) = 5π/2 - 2π = π/2
        Assert.Multiple(() =>
        {
            Assert.That(sweep.A0, Is.EqualTo(MathF.PI));
            Assert.That(sweep.A, Is.EqualTo(MathF.PI / 2f));
        });
    }

    [Test]
    public void NormalizeNegativeAngle()
    {
        Sweep sweep = new Sweep
        {
            A0 = -3f * MathF.PI, // -3π (-540 degrees)
            A = -5f * MathF.PI / 2f // -5π/2 (-450 degrees)
        };

        sweep.Normalize();

        // A0 = -3π - 2π * Floor(-3π / 2π) = -3π - (-2π) = -π
        // A = -5π/2 - 2π * Floor(-3π / 2π) = -5π/2 - (-2π) = -π/2
        Assert.Multiple(() =>
        {
            Assert.That(sweep.A0, Is.EqualTo(-MathF.PI));
            Assert.That(sweep.A, Is.EqualTo(-MathF.PI / 2f));
        });
    }

    [Test]
    public void DefaultConstructor_IsZero()
    {
        Sweep sweep = new Sweep();

        Assert.Multiple(() =>
        {
            Assert.That(sweep.LocalCenter, Is.EqualTo(Vector2.Zero));
            Assert.That(sweep.C0, Is.EqualTo(Vector2.Zero));
            Assert.That(sweep.C, Is.EqualTo(Vector2.Zero));
            Assert.That(sweep.A0, Is.EqualTo(0f));
            Assert.That(sweep.A, Is.EqualTo(0f));
            Assert.That(sweep.Alpha0, Is.EqualTo(0f));
        });
    }

    [Test]
    public void Equals_NearEqualSweeps()
    {
        Sweep s1 = new Sweep
        {
            C0 = new Vector2(1.00001f, 2.00001f),
            C = new Vector2(3.00001f, 4.00001f),
            A0 = MathF.PI / 2f,
            A = MathF.PI / 2f,
            Alpha0 = 0.5f
        };

        Sweep s2 = new Sweep
        {
            C0 = new Vector2(1.00002f, 2.00002f),
            C = new Vector2(3.00002f, 4.00002f),
            A0 = MathF.PI / 2f,
            A = MathF.PI / 2f,
            Alpha0 = 0.5f
        };

        Assert.That(s1, Is.EqualTo(s2));
    }

    [Test]
    public void GetHashCode_Consistency()
    {
        Sweep sweep = new Sweep
        {
            C0 = new Vector2(1f, 2f),
            C = new Vector2(3f, 4f),
            A0 = MathF.PI / 2f,
            A = MathF.PI / 3f,
            Alpha0 = 0.5f
        };

        int hash1 = sweep.GetHashCode();
        int hash2 = sweep.GetHashCode();

        Assert.That(hash1, Is.EqualTo(hash2)); // Hash should remain the same
    }

    [Test]
    public void GetTransform_AlphaZero_AlphaOne()
    {
        Sweep sweep = new Sweep
        {
            C0 = new Vector2(1f, 2f),
            C = new Vector2(3f, 4f),
            A0 = 0f,
            A = MathF.PI / 2f, // 90 degrees
            LocalCenter = new Vector2(0.5f, 0.5f)
        };

        Transform transformAtStart = new Transform();
        Transform transformAtEnd = new Transform();

        sweep.GetTransform(ref transformAtStart, 0f);
        sweep.GetTransform(ref transformAtEnd, 1f);

        Assert.Multiple(() =>
        {
            Assert.That(transformAtStart.Position, Is.EqualTo(sweep.C0 - Rotation.Multiply(new Rotation(sweep.A0), sweep.LocalCenter)));
            Assert.That(transformAtEnd.Position, Is.EqualTo(sweep.C - Rotation.Multiply(new Rotation(sweep.A), sweep.LocalCenter)));
        });
    }

    [Test]
    public void Advance_AlphaZero()
    {
        Sweep sweep = new Sweep
        {
            C0 = new Vector2(1f, 2f),
            C = new Vector2(3f, 4f),
            A0 = 0f,
            A = MathF.PI / 2f,
            Alpha0 = 0.2f
        };

        sweep.Advance(0.2f); // No change expected

        Assert.Multiple(() =>
        {
            Assert.That(sweep.C0, Is.EqualTo(new Vector2(1f, 2f)));
            Assert.That(sweep.A0, Is.EqualTo(0f));
            Assert.That(sweep.Alpha0, Is.EqualTo(0.2f));
        });
    }

    [Test]
    public void Advance_AlphaOne()
    {
        Sweep sweep = new Sweep
        {
            C0 = new Vector2(1f, 2f),
            C = new Vector2(3f, 4f),
            A0 = 0f,
            A = MathF.PI / 2f,
            Alpha0 = 0.2f
        };

        sweep.Advance(1f); // Should fully match C and A

        Assert.Multiple(() =>
        {
            Assert.That(sweep.C0, Is.EqualTo(sweep.C));
            Assert.That(sweep.A0, Is.EqualTo(sweep.A));
            Assert.That(sweep.Alpha0, Is.EqualTo(1f));
        });
    }

    [Test]
    public void Normalize_AnglesAtBoundaries()
    {
        Sweep sweep = new Sweep
        {
            A0 = MathF.PI,  // π
            A = -MathF.PI   // -π
        };

        sweep.Normalize();

        Assert.Multiple(() =>
        {
            Assert.That(sweep.A0, Is.EqualTo(MathF.PI));
            Assert.That(sweep.A, Is.EqualTo(-MathF.PI));
        });
    }

    [Test]
    public void Normalize_AnglesBeyondTwoPi()
    {
        Sweep sweep = new Sweep
        {
            A0 = 3f * MathF.PI, // 3π
            A = -3f * MathF.PI  // -3π
        };

        sweep.Normalize();

        Assert.Multiple(() =>
        {
            Assert.That(sweep.A0, Is.EqualTo(MathF.PI));
            Assert.That(sweep.A, Is.EqualTo(-MathF.PI));
        });
    }
}