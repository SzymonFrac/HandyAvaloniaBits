using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphQuadToArc : MorphSegment
{
    private readonly MorphPointLerp _control;
    private readonly MorphPointLerp _quadLerp;

    private readonly MorphSizeLerp _size;
    private readonly MorphRotationLerp _rotation;
    private readonly MorphPointLerp _arcLerp;

    private readonly ArcSegment _arc;

    private MorphQuadToArc(MorphPointLerp control, MorphPointLerp quadLerp, MorphSizeLerp size, MorphRotationLerp rotation, MorphPointLerp arcLerp, ArcSegment arc) =>
        (_control, _quadLerp, _size, _rotation, _arcLerp, _arc) = (control, quadLerp, size, rotation, arcLerp, arc);

    public static MorphQuadToArc Create(in QuadraticBezierSegment from, in ArcSegment to, ref (Point from, Point to) start)
    {
        var startMidpoint = (start.from + start.to) / 2;
        var quadLerpMidpoint = (from.Point2 + to.Point) / 2;
        var halfwayMidpoint = (startMidpoint + quadLerpMidpoint) / 2;

        var controlPointLerp = from.Point1.LerpTo(halfwayMidpoint);


        var quadLerp = from.Point2.LerpTo(quadLerpMidpoint);
        var arcLerp = quadLerpMidpoint.LerpTo(to.Point);


        var ellipseStartSize = new Size(1, 0);
        var sizeLerp = ellipseStartSize.LerpTo(to.Size);


        var dy = quadLerpMidpoint.Y - startMidpoint.Y;
        var dx = quadLerpMidpoint.X - startMidpoint.X;
        var rotation = Math.Atan2(dy, dx);

        var positiveRotation = (rotation + Math.Tau) % Math.PI;
        var arcRotation = positiveRotation * 180 / Math.PI;
        var rotationLerp = arcRotation.LerpTo(to.RotationAngle);


        start = (from.Point2, to.Point);
        return new(controlPointLerp, quadLerp, sizeLerp, rotationLerp, arcLerp, to);
    }

    public override void Apply(in double t, in StreamGeometryContext sgc)
    {
        if (t <= .5)
            LerpQuad(t * 2, in sgc);
        else
            LerpArc((t * 2) - 1, in sgc);
    }


    private void LerpQuad(in double t, in StreamGeometryContext sgc) =>
        sgc.QuadraticBezierTo(_control(in t), _quadLerp(in t));
    private void LerpArc(in double t, in StreamGeometryContext sgc) =>
        sgc.ArcTo(_arcLerp(in t), _size(in t), _rotation(in t), _arc.IsLargeArc, _arc.SweepDirection, _arc.IsStroked);
}
