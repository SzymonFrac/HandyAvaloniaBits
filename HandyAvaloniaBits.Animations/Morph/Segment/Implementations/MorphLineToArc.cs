using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;
using HandyAvaloniaBits.Animations.Morph.Segment.Abstract;
using ReactiveUI;
using System.Diagnostics;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Implementations;

internal sealed record MorphLineToArc : MorphToArc
{
    private MorphLineToArc(MorphPointLerp lerp, MorphSizeLerp sizeLerp, MorphRotationLerp rotation, ArcSegment toArc)
        : base(lerp, sizeLerp, rotation, toArc) { }

    public static MorphLineToArc Create(in LineSegment from, in ArcSegment to, ref (Point from, Point to) start)
    {
        var pointLerp = from.Point.LerpTo(to.Point);

        var distance = from.Point - start.from;
        var diameter = Math.Sqrt(distance.X * distance.X + distance.Y * distance.Y);
        var ellipseStartSize = new Size(diameter / 2, 0);
        var sizeLerp = ellipseStartSize.LerpTo(to.Size);

        var startRotation = Math.Atan2(from.Point.Y - start.from.Y, from.Point.X - start.from.X) * 180 / Math.PI;
        var rotationLerp = startRotation.LerpTo(to.RotationAngle);

        start = (from.Point, to.Point);
        return new(pointLerp, sizeLerp, rotationLerp, to);
    }
}
