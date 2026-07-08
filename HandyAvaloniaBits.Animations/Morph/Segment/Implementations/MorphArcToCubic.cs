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

internal sealed record MorphArcToCubic : MorphToArc
{
    private readonly MorphPointLerp _firstControl;
    private readonly MorphPointLerp _secondControl;
    private readonly MorphPointLerp _end;

    private MorphArcToCubic(MorphPointLerp lerp, MorphSizeLerp size, MorphRotationLerp rotation, ArcSegment arc, MorphPointLerp firstControl, MorphPointLerp secondControl, MorphPointLerp end)
        : base(lerp, size, rotation, arc) =>
            (_firstControl, _secondControl, _end) = (firstControl, secondControl, end);

    public static MorphArcToCubic Create(in ArcSegment from, in BezierSegment to, ref (Point from, Point to) start) =>
        new(from.Point.LerpTo(start.to),
            from.Size.LerpTo(default),
            from.RotationAngle.LerpTo(0),
            from,
            from.Point.LerpTo(to.Point1),
            from.Point.LerpTo(to.Point2),
            from.Point.LerpTo((start = (from.Point, to.Point3)).to));

    public override void Apply(in double t, in StreamGeometryContext sgc)
    {
        base.Apply(in t, in sgc);
        sgc.CubicBezierTo(_firstControl(in t), _secondControl(in t), _end(in t));
    }
}
