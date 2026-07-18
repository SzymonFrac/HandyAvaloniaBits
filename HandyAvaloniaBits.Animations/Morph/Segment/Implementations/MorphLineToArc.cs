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

internal sealed record MorphLineToArc : MorphToArc
{
    private MorphLineToArc(MorphSizeLerp sizeLerp, MorphRotationLerp rotation, MorphPointLerp point, ArcSegment arc)
        : base(sizeLerp, rotation, point, arc) { }

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

    public override void Apply(in double t, in StreamGeometryContext sgc) =>
        sgc.ArcTo(Point(in t), Size(in t), Rotation(in t), Arc.IsLargeArc, AlternateSweepIfEased(t < 0), Arc.IsStroked);



    private SweepDirection AlternateSweepIfEased(in bool negativeProgress) =>
        (SweepDirection)((int)Arc.SweepDirection ^ Convert.ToInt32(negativeProgress));
}
