using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphQuadToCubic : MorphToCubic
{
    private MorphQuadToCubic(MorphPointLerp fc, MorphPointLerp sc, MorphPointLerp point) : base(fc, sc, point) { }

    public static MorphQuadToCubic Create(in QuadraticBezierSegment from, in BezierSegment to, ref (Point from, Point to) start) =>
        new((start.from + (2 * (from.Point1 - start.from) / 3)).LerpTo(to.Point1),
            (from.Point1 + ((from.Point2 - from.Point1) / 3)).LerpTo(to.Point2),
            from.Point2.LerpTo(
                (start = (from.Point2, to.Point3)).to));
}
