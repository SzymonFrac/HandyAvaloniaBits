using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphLineToArc : MorphSegment
{
    private readonly MorphSizeLerp _size;
    private readonly MorphRotationLerp _rotation;
    private readonly MorphPointLerp _point;

    private readonly ArcSegment _arc;

    private MorphLineToArc(MorphSizeLerp size, MorphRotationLerp rotation, MorphPointLerp point, ArcSegment arc) =>
        (_size, _rotation, _point, _arc) = (size, rotation, point, arc);

    public static MorphLineToArc Create(in LineSegment from, in ArcSegment to, ref (Point from, Point to) start)
    {
        var pointLerp = from.Point.LerpTo(to.Point);


        var distance = from.Point - start.from;
        var diameter = Math.Sqrt(distance.X * distance.X + distance.Y * distance.Y);

        var ellipseStartSize = new Size(diameter / 2, 0);
        var sizeLerp = ellipseStartSize.LerpTo(to.Size);


        var dy = from.Point.Y - start.from.Y;
        var dx = from.Point.X - start.from.X;
        var rotation = Math.Atan2(dy, dx);

        var positiveRotation = (rotation + Math.Tau) % Math.PI;
        var arcRotation = positiveRotation * 180 / Math.PI;
        var rotationLerp = arcRotation.LerpTo(to.RotationAngle);

        start = (from.Point, to.Point);
        return new(sizeLerp, rotationLerp, pointLerp, to);
    }

    public override void Apply(in double t, in StreamGeometryContext sgc)
    {
        if (t < 0)
            sgc.LineTo(_point(in t));
        else
            sgc.ArcTo(_point(in t), _size(in t), _rotation(in t), _arc.IsLargeArc, _arc.SweepDirection, _arc.IsStroked);
    }
}
