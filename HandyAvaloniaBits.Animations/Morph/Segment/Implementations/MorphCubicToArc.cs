using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphCubicToArc : MorphSegment
{
    private readonly MorphPointLerp _firstControl;
    private readonly MorphPointLerp _secondControl;
    private readonly MorphPointLerp _cubicPoint;


    private readonly MorphSizeLerp _size;
    private readonly MorphRotationLerp _rotation;
    private readonly MorphPointLerp _arcPoint;

    private readonly ArcSegment _arc;

    private MorphCubicToArc(MorphPointLerp firstControl, MorphPointLerp secondControl, MorphPointLerp cubicPoint, MorphSizeLerp size, MorphRotationLerp rotation, MorphPointLerp arcPoint, ArcSegment arc) =>
        (_firstControl, _secondControl, _cubicPoint, _size, _rotation, _arcPoint, _arc) = (firstControl, secondControl, cubicPoint, size, rotation, arcPoint, arc);

    public static MorphCubicToArc Create(in BezierSegment from, in ArcSegment to, ref (Point from, Point to) start)
    {
        var cubicLerpMidpoint = (from.Point3 + to.Point) / 2;
        var startMidpoint = (start.from + start.to) / 2;
        var halfwayMidpoint = (startMidpoint + cubicLerpMidpoint) / 2;

        var firstControlPointLerp = from.Point1.LerpTo(halfwayMidpoint);
        var secondControlPointLerp = from.Point2.LerpTo(halfwayMidpoint);


        var cubicLerp = from.Point3.LerpTo(cubicLerpMidpoint);
        var arcLerp = cubicLerpMidpoint.LerpTo(to.Point);


        var distance = from.Point3 - start.from;
        var diameter = Math.Sqrt(distance.X * distance.X + distance.Y * distance.Y);

        var ellipseStartSize = new Size(diameter / 2, 0);
        var sizeLerp = ellipseStartSize.LerpTo(to.Size);


        var dy = cubicLerpMidpoint.Y - startMidpoint.Y;
        var dx = cubicLerpMidpoint.X - startMidpoint.X;
        var rotation = Math.Atan2(dy, dx);

        var positiveRotation = (rotation + Math.Tau) % Math.PI;
        var arcRotation = positiveRotation * 180 / Math.PI;
        var rotationLerp = arcRotation.LerpTo(to.RotationAngle);


        start = (from.Point3, to.Point);
        return new(firstControlPointLerp, secondControlPointLerp, cubicLerp, sizeLerp, rotationLerp, arcLerp, to);
    }

    public override void Apply(in double t, in StreamGeometryContext sgc)
    {
        if (t <= .5)
            LerpCubic(t * 2, in sgc);
        else
            LerpArc((t * 2) - 1, in sgc);
    }


    private void LerpCubic(in double t, in StreamGeometryContext sgc) =>
        sgc.CubicBezierTo(_firstControl(t), _secondControl(t), _cubicPoint(t));
    private void LerpArc(in double t, in StreamGeometryContext sgc) =>
        sgc.ArcTo(_arcPoint(t), _size(t), _rotation(t), _arc.IsLargeArc, _arc.SweepDirection, _arc.IsStroked);
}
