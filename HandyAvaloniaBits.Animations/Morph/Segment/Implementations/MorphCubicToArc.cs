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
    private readonly MorphPointLerp _cubicLerp;

    private readonly MorphSizeLerp _size;
    private readonly MorphRotationLerp _rotation;
    private readonly MorphPointLerp _arcLerp;

    private readonly ArcSegment _arc;

    private MorphCubicToArc(MorphPointLerp firstControl, MorphPointLerp secondControl, MorphPointLerp cubicLerp, MorphSizeLerp size, MorphRotationLerp rotation, MorphPointLerp arcLerp, ArcSegment arc) =>
        (_firstControl, _secondControl, _cubicLerp, _size, _rotation, _arcLerp, _arc) = (firstControl, secondControl, cubicLerp, size, rotation, arcLerp, arc);

    public static MorphCubicToArc Create(in BezierSegment from, in ArcSegment to, ref (Point from, Point to) start)
    {
        var cubicLerpMidpoint = (from.Point3 + to.Point) / 2;
        var startMidpoint = (start.from + start.to) / 2;
        var halfwayMidpoint = (startMidpoint + cubicLerpMidpoint) / 2;

        var firstControlPointLerp = from.Point1.LerpTo(halfwayMidpoint);
        var secondControlPointLerp = from.Point2.LerpTo(halfwayMidpoint);


        var cubicLerp = from.Point3.LerpTo(cubicLerpMidpoint);
        var arcLerp = cubicLerpMidpoint.LerpTo(to.Point);


        var ellipseStartSize = new Size(1, 0);
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
        sgc.CubicBezierTo(_firstControl(t), _secondControl(t), _cubicLerp(t));
    private void LerpArc(in double t, in StreamGeometryContext sgc) =>
        sgc.ArcTo(_arcLerp(t), _size(t), _rotation(t), _arc.IsLargeArc, _arc.SweepDirection, _arc.IsStroked);
}
