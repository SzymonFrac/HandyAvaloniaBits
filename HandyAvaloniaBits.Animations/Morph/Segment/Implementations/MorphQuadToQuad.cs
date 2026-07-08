using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphQuadToQuad : MorphToQuad
{
    private MorphQuadToQuad(MorphPointLerp lerp) : base(lerp) { }

    public static MorphQuadToQuad Create(in QuadraticBezierSegment from, in QuadraticBezierSegment to, ref (Point from, Point to) start) =>
        new(from.Point1.LerpTo(to.Point1) +
            from.Point2.LerpTo(
                (start = (from.Point2, to.Point2)).to));
}
