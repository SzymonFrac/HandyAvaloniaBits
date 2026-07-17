using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;
using HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphArcToLine : MorphToArc
{
    private MorphArcToLine(MorphSizeLerp sizeLerp, MorphRotationLerp rotation, MorphPointLerp lerp, ArcSegment arc)
        : base(sizeLerp, rotation, lerp, arc) { }

    public static MorphArcToLine Create(in ArcSegment from, in LineSegment to, ref (Point from, Point to) start)
    {
        var morph = new MorphArcToLine(
            from.Size.LerpTo(default),
            from.RotationAngle.LerpTo(0),
            from.Point.LerpTo(start.to),
            from);

        start = (from.Point, to.Point);

        return morph;
    }
}
