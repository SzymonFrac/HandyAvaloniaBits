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

internal sealed record MorphArcToArc : MorphToArc
{
    private MorphArcToArc(MorphPointLerp lerp, MorphSizeLerp size, MorphRotationLerp rotation, ArcSegment toArc) : base(lerp, size, rotation, toArc) { }

    public static MorphArcToArc Create(in ArcSegment from, in ArcSegment to, ref (Point from, Point to) start) =>
        new(from.Point.LerpTo((start = (from.Point, to.Point)).to),
            from.Size.LerpTo(to.Size),
            from.RotationAngle.LerpTo(to.RotationAngle),
            to);
}
