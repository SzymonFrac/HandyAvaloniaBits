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

internal sealed record MorphQuadToArc : MorphToArc
{
    private readonly MorphPointLerp _control;

    private MorphQuadToArc(MorphPointLerp control, MorphPointLerp lerp, MorphSizeLerp size, MorphRotationLerp rotation, ArcSegment arc)
        : base(lerp, size, rotation, arc) =>
            _control = control;

    public static MorphQuadToArc Create(in QuadraticBezierSegment from, in ArcSegment to, ref (Point from, Point to) start) =>
        new(from.Point1.LerpTo((start.from + from.Point2) / 2),
            from.Point2.LerpTo((start = (from.Point2, to.Point)).to),
            default(Size).LerpTo(to.Size),
            0.0.LerpTo(to.RotationAngle),
            to);

    public override void Apply(in double t, in StreamGeometryContext sgc)
    {
        if (t <= .5)
            sgc.QuadraticBezierTo(_control(t * 2), Lerp(t * 2));
        else
        {
            var tOffset = (t * 2) - 1;
            var tScale = Math.Pow(tOffset, .08);

            sgc.ArcTo(Lerp(1), Size!(tScale), Rotation(tScale), Arc.IsLargeArc, Arc.SweepDirection, Arc.IsStroked);
        }
    }
}
