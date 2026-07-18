using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphArcToCubic : MorphSegment
{
    private readonly MorphSizeLerp _size;
    private readonly MorphRotationLerp _rotation;
    private readonly MorphPointLerp _arcPoint;

    private readonly ArcSegment _arc;


    private readonly MorphPointLerp _firstControl;
    private readonly MorphPointLerp _secondControl;
    private readonly MorphPointLerp _cubicPoint;

    private MorphArcToCubic(MorphSizeLerp size, MorphRotationLerp rotation, MorphPointLerp arcPoint, ArcSegment arc, MorphPointLerp firstControl, MorphPointLerp secondControl, MorphPointLerp cubicPoint) =>
        (_size, _rotation, _arcPoint, _arc, _firstControl, _secondControl, _cubicPoint) = (size, rotation, arcPoint, arc, firstControl, secondControl, cubicPoint);

    public static MorphArcToCubic Create(in ArcSegment from, in BezierSegment to, ref (Point from, Point to) start)
    {
        var cubicLerpMidpoint = (from.Point + to.Point3) / 2;
        var startMidpoint = (start.from + start.to) / 2;
        var halfwayMidpoint = (startMidpoint + cubicLerpMidpoint) / 2;

        var firstControlPointLerp = halfwayMidpoint.LerpTo(to.Point1);
        var secondControlPointLerp = halfwayMidpoint.LerpTo(to.Point2);


        var arcLerp = from.Point.LerpTo(cubicLerpMidpoint);
        var cubicLerp = cubicLerpMidpoint.LerpTo(to.Point3);


        var ellipseEndSize = from.Size.WithHeight(0);
        var sizeLerp = from.Size.LerpTo(ellipseEndSize);


        var dy = cubicLerpMidpoint.Y - startMidpoint.Y;
        var dx = cubicLerpMidpoint.X - startMidpoint.X;
        var rotation = Math.Atan2(dy, dx);

        var positiveRotation = (rotation + Math.Tau) % Math.PI;
        var arcRotation = positiveRotation * 180 / Math.PI;
        var rotationLerp = from.RotationAngle.LerpTo(arcRotation);


        start = (from.Point, to.Point3);
        return new(sizeLerp, rotationLerp, arcLerp, from, firstControlPointLerp, secondControlPointLerp, cubicLerp);
    }

    public override void Apply(in double t, in StreamGeometryContext sgc)
    {
        if (t <= .5)
            LerpArc(t * 2, in sgc);
        else
            LerpCubic((t * 2) - 1, in sgc);
    }


    private void LerpArc(in double t, in StreamGeometryContext sgc) =>
        sgc.ArcTo(_arcPoint(in t), _size(in t), _rotation(in t), _arc.IsLargeArc, _arc.SweepDirection, _arc.IsStroked);
    private void LerpCubic(in double t, in StreamGeometryContext sgc) =>
        sgc.CubicBezierTo(_firstControl(in t), _secondControl(in t), _cubicPoint(in t));
}
