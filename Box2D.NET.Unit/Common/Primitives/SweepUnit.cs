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
            Assert.That(sweep.A0, Is.EqualTo(MathF.PI / 4f).Within(1e-6f));
            Assert.That(sweep.Alpha0, Is.EqualTo(0.6f).Within(1e-6f));
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
            Assert.That(sweep.A0, Is.EqualTo(MathF.PI).Within(1e-6f));
            Assert.That(sweep.A, Is.EqualTo(MathF.PI / 2f).Within(1e-6f));
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
            Assert.That(sweep.A0, Is.EqualTo(-MathF.PI).Within(1e-6f));
            Assert.That(sweep.A, Is.EqualTo(-MathF.PI / 2f).Within(1e-6f));
        });
    }
}