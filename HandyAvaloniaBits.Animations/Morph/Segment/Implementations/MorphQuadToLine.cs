using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphQuadToLine : MorphToQuad
{
    private MorphQuadToLine(MorphPointLerp c, MorphPointLerp point) : base(c, point) { }

    public static MorphQuadToLine Create(in QuadraticBezierSegment from, in LineSegment to, ref (Point from, Point to) start) =>
        new(from.Point1.LerpTo((to.Point + start.to) / 2),
            from.Point2.LerpTo(
                (start = (from.Point2, to.Point)).to));
}
