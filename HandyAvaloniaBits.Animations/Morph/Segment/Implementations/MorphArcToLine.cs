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

internal sealed record MorphArcToLine : MorphToArc
{
    private MorphArcToLine(MorphSizeLerp size, MorphRotationLerp rotation, MorphPointLerp point, ArcSegment arc)
        : base(size, rotation, point, arc) { }

    public static MorphArcToLine Create(in ArcSegment from, in LineSegment to, ref (Point from, Point to) start)
    {
        var pointLerp = from.Point.LerpTo(to.Point);


        var ellipseEndSize = from.Size.WithHeight(0);
        var sizeLerp = from.Size.LerpTo(ellipseEndSize);


        var dy = from.Point.Y - start.from.Y;
        var dx = from.Point.X - start.from.X;
        var rotation = Math.Atan2(dy, dx);

        var positiveRotation = (rotation + Math.Tau) % Math.PI;
        var arcRotation = positiveRotation * 180 / Math.PI;
        var rotationLerp = from.RotationAngle.LerpTo(arcRotation);

        start = (from.Point, to.Point);
        return new(sizeLerp, rotationLerp, pointLerp, from);
    }
}
