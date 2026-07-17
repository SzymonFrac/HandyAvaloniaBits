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

internal sealed record MorphArcToQuad : MorphToArc
{
    private readonly MorphPointLerp _control;
    private readonly MorphPointLerp _end;

    private MorphArcToQuad(MorphPointLerp lerp, MorphSizeLerp size, MorphRotationLerp rotation, ArcSegment arc, MorphPointLerp control, MorphPointLerp end)
        : base(size, rotation, lerp, arc) =>
            (_control, _end) = (control, end);

    public static MorphArcToQuad Create(in ArcSegment from, in QuadraticBezierSegment to, ref (Point from, Point to) start) =>
        new(from.Point.LerpTo(start.to),
            from.Size.LerpTo(default),
            from.RotationAngle.LerpTo(0),
            from,
            from.Point.LerpTo(to.Point1),
            from.Point.LerpTo((start = (from.Point, to.Point2)).to));

    public override void Apply(in double t, in StreamGeometryContext sgc)
    {
        base.Apply(in t, in sgc);
        sgc.QuadraticBezierTo(_control(in t), _end(in t));
    }
}
