using System;
using Box2D.NET.Collisions.RayCasts;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Shapes;

/// <summary>
/// A shape is used for collision detection. You can create a shape however you like.
/// Shapes used for simulation in World are created automatically when a Fixture
/// is created. Shapes may encapsulate one or more child shapes.
/// </summary>
public abstract class Shape : IEquatable<Shape>, IDisposable
{
    /// <summary>
    /// Gets or sets the radius of the shape. For polygonal shapes, this must be a constant value.
    /// There is no support for making rounded polygons.
    /// </summary>
    public readonly float Radius;

    /// <summary>
    /// Gets or sets the type of this shape. You can use this to downcast to the concrete shape.
    /// </summary>
    public readonly ShapeType ShapeType;

    /// <summary>
    /// Initializes a new instance of the <see cref="Shape" /> class.
    /// </summary>
    protected Shape(ShapeType type, float radius)
    {
        ShapeType = type;
        Radius = radius;
    }

    /// <summary>
    /// Disposes the shape and releases any resources.
    /// </summary>
    public virtual void Dispose() => GC.SuppressFinalize(this);

    /// <inheritdoc />
    public bool Equals(Shape? other)
    {
        if (other is null)
        {
            return false;
        }

        return ShapeType == other.ShapeType && Radius.Equals(other.Radius);
    }

    /// <summary>
    /// Clones the concrete shape.
    /// </summary>
    /// <returns>A new instance of the cloned shape.</returns>
    public abstract Shape Clone();

    /// <summary>
    /// Gets the number of child primitives.
    /// </summary>
    /// <returns>The number of child shapes.</returns>
    public abstract int GetChildCount();

    /// <summary>
    /// Tests a point for containment in this shape. This only works for convex shapes.
    /// </summary>
    /// <param name="transform">The shape world transform.</param>
    /// <param name="point">A point in world coordinates.</param>
    /// <returns>True if the point is contained in the shape, otherwise false.</returns>
    public abstract bool TestPoint(in Transform transform, in Vector2 point);

    /// <summary>
    /// Casts a ray against a child shape.
    /// </summary>
    /// <param name="output">The ray-cast results.</param>
    /// <param name="input">The ray-cast input parameters.</param>
    /// <param name="transform">The transform to be applied to the shape.</param>
    /// <param name="childIndex">The child shape index.</param>
    /// <returns>True if the ray hits the shape, otherwise false.</returns>
    public abstract bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform transform, int childIndex);

    /// <summary>
    /// Given a transform, computes the associated axis-aligned bounding box for a child shape.
    /// </summary>
    /// <param name="aabb">Returns the axis-aligned bounding box.</param>
    /// <param name="transform">The world transform of the shape.</param>
    /// <param name="childIndex">The child shape index.</param>
    public abstract void ComputeAABB(out AABB aabb, in Transform transform, int childIndex);

    /// <summary>
    /// Computes the mass properties of this shape using its dimensions and density.
    /// The inertia tensor is computed about the local origin.
    /// </summary>
    /// <param name="massData">Returns the mass data for this shape.</param>
    /// <param name="density">The density in kilograms per meter squared.</param>
    public abstract void ComputeMass(out MassData massData, float density);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Shape shape && Equals(shape);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(ShapeType, Radius);

    /// <summary>
    /// Checks if two shapes are equal.
    /// </summary>
    public static bool operator ==(Shape? left, Shape? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Checks if two shapes are not equal.
    /// </summary>
    public static bool operator !=(Shape? left, Shape? right)
    {
        if (left is null)
        {
            return right is not null;
        }

        return !left.Equals(right);
    }
}