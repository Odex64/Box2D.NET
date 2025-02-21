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

    public static float LinearSlop;

    public static float LengthUnitsPerMeter
    {
        set
        {
            field = value;
            LinearSlop = 0.005f * field;
        }
    } = 1f;
}