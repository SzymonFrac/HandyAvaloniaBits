using Avalonia;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillPerimeterShapeCutout.Implementations;
using System.Diagnostics;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillPerimeterShapeCutout.Abstract;

internal abstract class AbstractFillPerimeterShapeCutoutFactory : ILerpShapeCutoutFactory
{
    public static AbstractFillPerimeterShapeCutoutFactory Get(Corner corner, in Rect windowBounds) => corner switch
    {
        Corner.TopLeft => new TopLeftFillPerimeterShapeCutoutLerpFactory() { WindowBounds = windowBounds },
        Corner.TopRight => new TopRightFillPerimeterShapeCutoutLerpFactory() { WindowBounds = windowBounds },
        Corner.BottomLeft => new BottomLeftFillPerimeterShapeCutoutLerpFactory() { WindowBounds = windowBounds },
        Corner.BottomRight => new BottomRightFillPerimeterShapeCutoutLerpFactory() { WindowBounds = windowBounds },
        _ => throw new UnreachableException()
    };

    protected double? finalRadius;
    protected Point? finalPosition;
    public required Rect WindowBounds { get; init; }

    public virtual double LerpRadius(in double t) => t * (finalRadius ??= Vector.Distance(Vector.Zero, new(WindowBounds.Width, WindowBounds.Height)) * ((Random.Shared.NextSingle() * .1) + .15));
    public virtual Point LerpPosition(in double t)
    {
        if (finalPosition is not null)
            return t * finalPosition.Value;

        var rad = Random.Shared.NextSingle() * (Math.PI / 2);
        var direction = new Point(Math.Cos(rad), Math.Sin(rad));
        finalPosition = direction * Vector.Distance(Vector.Zero, new(WindowBounds.Width, WindowBounds.Height)) * ((Random.Shared.NextSingle() * .2) + .9);

        return t * finalPosition.Value;
    }

    public virtual LerpShapeCutout Create() => (in t) =>
    {
        var (radius, position) = (LerpRadius(in t), LerpPosition(in t));
        return new(radius, 0, 0, radius, position.X, position.Y);
    };
}
