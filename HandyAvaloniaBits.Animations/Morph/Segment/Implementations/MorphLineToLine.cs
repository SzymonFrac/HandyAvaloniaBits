using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphLineToLine : MorphToLine
{
    private MorphLineToLine(MorphPointLerp point) : base(point) { }

    public static MorphLineToLine Create(in LineSegment from, in LineSegment to, ref (Point from, Point to) start) =>
        new(from.Point.LerpTo(
            (start = (from.Point, to.Point)).to));
}
