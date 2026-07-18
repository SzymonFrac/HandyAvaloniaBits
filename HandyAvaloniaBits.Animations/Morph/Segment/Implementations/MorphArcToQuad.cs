using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphArcToQuad : MorphSegment
{
    private readonly MorphSizeLerp _size;
    private readonly MorphRotationLerp _rotation;
    private readonly MorphPointLerp _arcPoint;

    private readonly ArcSegment _arc;


    private readonly MorphPointLerp _control;
    private readonly MorphPointLerp _quadPoint;

    private MorphArcToQuad(MorphSizeLerp size, MorphRotationLerp rotation, MorphPointLerp arcPoint, ArcSegment arc, MorphPointLerp control, MorphPointLerp quadPoint) =>
        (_size, _rotation, _arcPoint, _arc, _control, _quadPoint) = (size, rotation, arcPoint, arc, control, quadPoint);

    public static MorphArcToQuad Create(in ArcSegment from, in QuadraticBezierSegment to, ref (Point from, Point to) start)
    {
        var startMidpoint = (start.from + start.to) / 2;
        var quadLerpMidpoint = (from.Point + to.Point2) / 2;
        var halfwayMidpoint = (startMidpoint + quadLerpMidpoint) / 2;

        var controlPointLerp = halfwayMidpoint.LerpTo(to.Point1);


        var quadLerp = from.Point.LerpTo(quadLerpMidpoint);
        var arcLerp = quadLerpMidpoint.LerpTo(to.Point2);


        var ellipseEndSize = from.Size.WithHeight(0);
        var sizeLerp = from.Size.LerpTo(ellipseEndSize);


        var dy = quadLerpMidpoint.Y - startMidpoint.Y;
        var dx = quadLerpMidpoint.X - startMidpoint.X;
        var rotation = Math.Atan2(dy, dx);

        var positiveRotation = (rotation + Math.Tau) % Math.PI;
        var arcRotation = positiveRotation * 180 / Math.PI;
        var rotationLerp = from.RotationAngle.LerpTo(arcRotation);


        start = (from.Point, to.Point2);
        return new(sizeLerp, rotationLerp, arcLerp, from, controlPointLerp, quadLerp);
    }

    public override void Apply(in double t, in StreamGeometryContext sgc)
    {
        if (t <= .5)
            LerpArc(t * 2, in sgc);
        else
            LerpQuad((t * 2) - 1, in sgc);
    }


    private void LerpArc(in double t, in StreamGeometryContext sgc) =>
        sgc.ArcTo(_arcPoint(in t), _size(in t), _rotation(in t), _arc.IsLargeArc, _arc.SweepDirection, _arc.IsStroked);
    private void LerpQuad(in double t, in StreamGeometryContext sgc) =>
        sgc.QuadraticBezierTo(_control(in t), _quadPoint(in t));
}
