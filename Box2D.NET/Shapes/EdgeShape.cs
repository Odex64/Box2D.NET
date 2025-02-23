using System;
using Box2D.NET.Collisions.RayCasts;
using Box2D.NET.Common;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Shapes;

public class EdgeShape() : Shape(ShapeType.Edge, Constants.PolygonRadius), IEquatable<EdgeShape>
{
    public bool IsOneSided;
    public Vector2 Vertex0;
    public Vector2 Vertex1;
    public Vector2 Vertex2;
    public Vector2 Vertex3;

    public bool Equals(EdgeShape? other)
    {
        if (other is null)
        {
            return false;
        }

        return Vertex1 == other.Vertex1 &&
               Vertex2 == other.Vertex2 &&
               Vertex0 == other.Vertex0 &&
               Vertex3 == other.Vertex3 &&
               IsOneSided == other.IsOneSided &&
               Radius.ToleranceEquals(other.Radius) &&
               ShapeType == other.ShapeType;
    }

    public void SetOneSided(in Vector2 v0, in Vector2 v1, in Vector2 v2, in Vector2 v3)
    {
        Vertex0 = v0;
        Vertex1 = v1;
        Vertex2 = v2;
        Vertex3 = v3;
        IsOneSided = true;
    }

    public void SetTwoSided(in Vector2 v1, in Vector2 v2)
    {
        Vertex1 = v1;
        Vertex2 = v2;
        IsOneSided = false;
    }

    public override Shape Clone() => new EdgeShape
    {
        Vertex0 = Vertex0,
        Vertex1 = Vertex1,
        Vertex2 = Vertex2,
        Vertex3 = Vertex3,
        IsOneSided = IsOneSided
    };

    public override int GetChildCount() => 1;

    public override bool TestPoint(in Transform transform, in Vector2 point) => false;

    public override bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform transform, int childIndex)
    {
        output = new RayCastOutput();

        Vector2 p1 = Rotation.MultiplyTranspose(transform.Rotation, input.Start - transform.Position);
        Vector2 p2 = Rotation.MultiplyTranspose(transform.Rotation, input.End - transform.Position);
        Vector2 d = p2 - p1;

        Vector2 e = Vertex2 - Vertex1;

        Vector2 normal = new Vector2(e.Y, -e.X);
        _ = normal.Normalize();

        float numerator = Vector2.Dot(normal, Vertex1 - p1);
        if (IsOneSided && numerator > 0.0f)
        {
            return false;
        }

        float denominator = Vector2.Dot(normal, d);

        if (denominator == 0.0f)
        {
            return false;
        }

        float t = numerator / denominator;
        if (t < 0.0f || input.MaxFraction < t)
        {
            return false;
        }

        Vector2 q = p1 + t * d;

        Vector2 r = Vertex2 - Vertex1;
        float rr = Vector2.Dot(r, r);
        if (rr == 0.0f)
        {
            return false;
        }

        float s = Vector2.Dot(q - Vertex1, r) / rr;
        if (s is < 0.0f or > 1.0f)
        {
            return false;
        }

        output.Fraction = t;
        if (numerator > 0.0f)
        {
            output.Normal = -Rotation.Multiply(transform.Rotation, normal);
        }
        else
        {
            output.Normal = Rotation.Multiply(transform.Rotation, normal);
        }

        return true;
    }

    public override void ComputeAABB(out AABB aabb, in Transform transform, int childIndex)
    {
        Vector2 v1 = Transform.Multiply(transform, Vertex1);
        Vector2 v2 = Transform.Multiply(transform, Vertex2);

        Vector2 lower = Vector2.Min(v1, v2);
        Vector2 upper = Vector2.Max(v1, v2);

        Vector2 r = new Vector2(Radius, Radius);
        aabb.LowerBound = lower - r;
        aabb.UpperBound = upper + r;
    }

    public override void ComputeMass(out MassData massData, float density) => massData = new MassData(0f, 0.5f * (Vertex1 + Vertex2), 0f);

    public override bool Equals(object? obj) => obj is EdgeShape edgeShape && Equals(edgeShape);

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Vertex1.GetHashCode();
            hash = hash * 23 + Vertex2.GetHashCode();
            hash = hash * 23 + Vertex0.GetHashCode();
            hash = hash * 23 + Vertex3.GetHashCode();
            hash = hash * 23 + IsOneSided.GetHashCode();
            return hash;
        }
    }

    public static bool operator ==(EdgeShape? left, EdgeShape? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(EdgeShape? left, EdgeShape? right)
    {
        if (left is null)
        {
            return right is not null;
        }

        return !left.Equals(right);
    }

    public override string ToString() => $"EdgeShape(V1: {Vertex1}, V2: {Vertex2}, V0: {Vertex0}, V3: {Vertex3}, OneSided: {IsOneSided})";
}