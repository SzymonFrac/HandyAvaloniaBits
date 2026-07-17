using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphCubicToLine : MorphToCubic
{
    private MorphCubicToLine(MorphPointLerp fc, MorphPointLerp sc, MorphPointLerp lerp) : base(fc, sc, lerp) { }

    public static MorphCubicToLine Create(in BezierSegment from, in LineSegment to, ref (Point from, Point to) start) =>
        new(from.Point1.LerpTo((to.Point + start.to) / 2),
            from.Point2.LerpTo((to.Point + start.to) / 2),
            from.Point3.LerpTo(
                (start = (from.Point3, to.Point)).to));
}
