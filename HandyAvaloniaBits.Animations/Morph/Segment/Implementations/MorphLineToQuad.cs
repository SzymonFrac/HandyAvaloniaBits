using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphLineToQuad : MorphToQuad
{
    private MorphLineToQuad(MorphPointLerp lerp) : base(lerp) { }

    public static MorphLineToQuad Create(in LineSegment from, in QuadraticBezierSegment to, ref (Point from, Point to) start) =>
        new(((from.Point + start.from) / 2).LerpTo(to.Point1) +
            from.Point.LerpTo(
                (start = (from.Point, to.Point2)).to));
}
