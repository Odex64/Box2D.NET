using System;
using Box2D.NET.Common.Primitives;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common.Primitives;

[TestFixture]
public sealed class TransformUnit
{
    [Test]
    public void Constructor()
    {
        Rotation rotation = new Rotation(MathF.PI / 4f); // 45 degrees
        Vector2 position = new Vector2(1f, 2f);
        Transform transform = new Transform(rotation, position);

        Assert.Multiple(() =>
        {
            Assert.That(transform.Rotation, Is.EqualTo(rotation));
            Assert.That(transform.Position, Is.EqualTo(position));
        });
    }

    [Test]
    public void Set()
    {
        Transform transform = new Transform();
        Vector2 position = new Vector2(3f, 4f);
        const float angle = MathF.PI / 2f; // 90 degrees

        transform.Set(position, angle);

        Assert.That(transform, Is.EqualTo(new Transform(new Rotation(angle), position)));
    }

    [Test]
    public void SetIdentity()
    {
        Transform transform = new Transform(new Rotation(MathF.PI / 4f), new Vector2(1f, 2f));
        transform.SetIdentity();

        Assert.That(transform, Is.EqualTo(new Transform(new Rotation(0f, 1f), new Vector2(0f, 0f))));
    }

    [Test]
    public void MultiplyByVector()
    {
        Transform transform = new Transform(new Rotation(MathF.PI / 2f), new Vector2(1f, 2f)); // 90 degrees rotation
        Vector2 vector = new Vector2(1f, 0f);

        Vector2 result = Transform.Multiply(transform, vector);

        // Rotating (1, 0) by 90 degrees gives (0, 1), then translating by (1, 2) gives (1, 3)
        Assert.That(result, Is.EqualTo(new Vector2(1f, 3f)));
    }

    [Test]
    public void MultiplyTransposeByVector()
    {
        Transform transform = new Transform(new Rotation(MathF.PI / 2f), new Vector2(1f, 2f)); // 90 degrees rotation
        Vector2 vector = new Vector2(1f, 3f);

        Vector2 result = Transform.MultiplyTranspose(transform, vector);

        // Translating (1, 3) by (-1, -2) gives (0, 1), then rotating by -90 degrees gives (1, 0)
        Assert.That(result, Is.EqualTo(new Vector2(1f, 0f)));
    }

    [Test]
    public void Multiply()
    {
        Transform transform1 = new Transform(new Rotation(MathF.PI / 2f), new Vector2(1f, 2f)); // 90 degrees rotation
        Transform transform2 = new Transform(new Rotation(MathF.PI / 2f), new Vector2(3f, 4f)); // 90 degrees rotation

        Transform result = Transform.Multiply(transform1, transform2);

        // Combined rotation: 90 + 90 = 180 degrees
        // Combined position: Rotate (3, 4) by 90 degrees to get (-4, 3), then add (1, 2) to get (-3, 5)
        Assert.That(result, Is.EqualTo(new Transform(new Rotation(MathF.PI), new Vector2(-3f, 5f))));
    }

    [Test]
    public void MultiplyTranspose()
    {
        Transform transform1 = new Transform(new Rotation(MathF.PI / 2f), new Vector2(1f, 2f)); // 90 degrees rotation
        Transform transform2 = new Transform(new Rotation(MathF.PI / 2f), new Vector2(3f, 4f)); // 90 degrees rotation
        Transform result = Transform.MultiplyTranspose(transform1, transform2);

        Assert.That(result, Is.EqualTo(new Transform(new Rotation(0f, 1f), new Vector2(2f, -2f))));
    }
}