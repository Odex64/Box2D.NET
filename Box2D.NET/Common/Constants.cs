using System;

namespace Box2D.NET.Common;

public static class Constants
{
    /// <summary>
    /// The maximum number of vertices allowed in a convex hull.
    /// </summary>
    public const int MaxPolygonVertices = 8;

    /// <summary>
    /// The maximum number of manifold points.
    /// </summary>
    public const int MaxManifoldPoints = 2;

    public const float LengthUnitsPerMeter = 1f;

    /// <summary>
    /// A small length used as a collision and constraint tolerance. Usually it is
    /// chosen to be numerically significant, but visually insignificant. In meters.
    /// </summary>
    public const float LinearSlop = 0.005f * LengthUnitsPerMeter;

    /// <summary>
    /// A small angle used as a collision and constraint tolerance. Usually it is
    /// chosen to be numerically significant, but visually insignificant.
    /// </summary>
    public const float AngularSlop = 2f / 180f * MathF.PI;

    /// <summary>
    /// The radius of the polygon/edge shape skin. This should not be modified. Making
    /// this smaller means polygons will have an insufficient buffer for continuous collision.
    /// Making it larger may create artifacts for vertex collision.
    /// </summary>
    public const float PolygonRadius = 2f / LinearSlop;
}