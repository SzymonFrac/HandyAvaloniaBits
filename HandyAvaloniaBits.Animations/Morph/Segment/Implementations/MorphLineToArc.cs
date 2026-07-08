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

internal sealed record MorphLineToArc : MorphToArc
{
    private MorphLineToArc(MorphPointLerp lerp, MorphSizeLerp sizeLerp, MorphRotationLerp rotation, ArcSegment toArc)
        : base(lerp, sizeLerp, rotation, toArc) { }

    public static MorphLineToArc Create(in LineSegment from, in ArcSegment to, ref (Point from, Point to) start) =>
        new(from.Point.LerpTo((start = (from.Point, to.Point)).to),
            default(Size).LerpTo(to.Size),
            0.0.LerpTo(to.RotationAngle),
            to);
}
