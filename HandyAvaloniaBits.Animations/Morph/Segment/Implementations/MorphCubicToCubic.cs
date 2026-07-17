using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphCubicToCubic : MorphToCubic
{
    private MorphCubicToCubic(MorphPointLerp fc, MorphPointLerp sc, MorphPointLerp lerp) : base(fc, sc, lerp) { }

    public static MorphCubicToCubic Create(in BezierSegment from, in BezierSegment to, ref (Point from, Point to) start) =>
        new(from.Point1.LerpTo(to.Point1),
            from.Point2.LerpTo(to.Point2),
            from.Point3.LerpTo(
                (start = (from.Point3, to.Point3)).to));
}
