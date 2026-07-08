using Avalonia;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillShapeCutout.Implementations;
using System.Diagnostics;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillShapeCutout.Abstract;

internal abstract class AbstractFillShapeCutoutFactory : ILerpShapeCutoutFactory
{
    public static AbstractFillShapeCutoutFactory Get(Corner corner, in Rect windowBounds) => corner switch
    {
        Corner.TopLeft => new TopLeftFillShapeCutoutLerpFactory() { WindowBounds = windowBounds },
        Corner.TopRight => new TopRightFillShapeCutoutLerpFactory() { WindowBounds = windowBounds },
        Corner.BottomLeft => new BottomLeftFillShapeCutoutLerpFactory() { WindowBounds = windowBounds },
        Corner.BottomRight => new BottomRightFillShapeCutoutLerpFactory() { WindowBounds = windowBounds },
        _ => throw new UnreachableException()
    };

    protected double? finalRadius;
    public required Rect WindowBounds { get; init; }

    public abstract LerpShapeCutout Create();
    public virtual double LerpRadius(in double t) => t * (finalRadius ??= Vector.Distance(Vector.Zero, new(WindowBounds.Width, WindowBounds.Height)));
}
