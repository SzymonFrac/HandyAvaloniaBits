using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphLineToCubic : MorphToCubic
{
    private MorphLineToCubic(MorphPointLerp fc, MorphPointLerp sc, MorphPointLerp lerp) : base(fc, sc, lerp) { }

    public static MorphLineToCubic Create(in LineSegment from, in BezierSegment to, ref (Point from, Point to) start) =>
        new(((from.Point + start.from) / 2).LerpTo(to.Point1),
            ((from.Point + start.from) / 2).LerpTo(to.Point2),
            from.Point.LerpTo(
                (start = (from.Point, to.Point3)).to));
}
