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

        Assert.That(transform, Is.EqualTo(new Transform(new Rotation(0f, 1f), Vector2.Zero)));
    }

    [Test]
    public void MultiplyByVector()
    {
        Transform transform = new Transform(new Rotation(MathF.PI / 2f), new Vector2(1f, 2f)); // 90 degrees rotation
        Vector2 vector = Vector2.UnitX;

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
        Assert.That(result, Is.EqualTo(Vector2.UnitX));
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

    [Test]
    public void HashCodeConsistency()
    {
        Transform transform = new Transform(new Rotation(MathF.PI / 4f), new Vector2(1f, 2f));
        int hash1 = transform.GetHashCode();
        int hash2 = transform.GetHashCode();

        Assert.That(hash1, Is.EqualTo(hash2)); // Hash should remain the same
    }

    [Test]
    public void IdentityTransformDoesNotModifyVector()
    {
        Transform identity = new Transform();
        identity.SetIdentity();
        Vector2 vector = new Vector2(3f, 4f);

        Vector2 result = Transform.Multiply(identity, vector);

        Assert.That(result, Is.EqualTo(vector)); // Should not change
    }

    [Test]
    public void InverseTransform()
    {
        Transform transform = new Transform(new Rotation(MathF.PI / 4f), new Vector2(2f, 3f));
        Transform inverse = Transform.MultiplyTranspose(transform, transform);

        Assert.That(inverse, Is.EqualTo(new Transform(new Rotation(0f, 1f), Vector2.Zero)));
    }

    [Test]
    public void ByZeroTranslation()
    {
        Transform transform = new Transform(new Rotation(MathF.PI / 2f), Vector2.Zero);
        Vector2 vector = new Vector2(1f, 2f);

        Vector2 result = Transform.Multiply(transform, vector);

        // Rotating (1,2) by 90 degrees gives (-2,1), no translation should occur
        Assert.That(result, Is.EqualTo(new Vector2(-2f, 1f)));
    }

    [Test]
    public void TransposeByZeroTranslation()
    {
        Transform transform = new Transform(new Rotation(MathF.PI / 2f), Vector2.Zero);
        Vector2 vector = new Vector2(1f, 2f);

        Vector2 result = Transform.MultiplyTranspose(transform, vector);

        // Applying inverse rotation should give back original vector
        Assert.That(result, Is.EqualTo(new Vector2(2f, -1f)));
    }

    [Test]
    public void TranslationOnly()
    {
        Transform transform = new Transform(new Rotation(0f), new Vector2(3f, 4f));
        Vector2 vector = new Vector2(1f, 2f);

        Vector2 result = Transform.Multiply(transform, vector);

        // Only translation should occur
        Assert.That(result, Is.EqualTo(new Vector2(4f, 6f)));
    }

    [Test]
    public void TranslationThenRotation()
    {
        Transform transform = new Transform(new Rotation(MathF.PI / 2f), new Vector2(3f, 4f));
        Vector2 vector = Vector2.UnitX;

        Vector2 result = Transform.Multiply(transform, vector);

        // Rotating (1,0) by 90 degrees gives (0,1), then translating by (3,4) gives (3,5)
        Assert.That(result, Is.EqualTo(new Vector2(3f, 5f)));
    }

    [Test]
    public void TransposeTranslationThenRotation()
    {
        Transform transform = new Transform(new Rotation(MathF.PI / 2f), new Vector2(3f, 4f));
        Vector2 vector = new Vector2(3f, 5f);

        Vector2 result = Transform.MultiplyTranspose(transform, vector);

        // Undo translation (-3,-4) then undo rotation (-90 degrees) to get back original (1,0)
        Assert.That(result, Is.EqualTo(Vector2.UnitX));
    }
}