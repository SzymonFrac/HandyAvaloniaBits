using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Ext;

internal static partial class MorphSegmentExt
{
    extension(BezierSegment cubic)
    {
        public (BezierSegment first, BezierSegment second) Split(ref Point start, in double t = .5)
        {
            var m0 = start.LerpTo(cubic.Point1)(in t);
            var m1 = cubic.Point1.LerpTo(cubic.Point2)(in t);
            var m2 = cubic.Point2.LerpTo(cubic.Point2)(in t);

            var n0 = m0.LerpTo(m1)(in t);
            var n1 = m1.LerpTo(m2)(in t);

            var cp = n0.LerpTo(n1)(in t);

            return (new BezierSegment { Point1 = m0, Point2 = n0, Point3 = cp },
                new BezierSegment { Point1 = n1, Point2 = m2, Point3 = start = cubic.Point3 });
        }
    }
}
