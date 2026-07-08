using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Ext;

internal static partial class MorphSegmentExt
{
    extension(QuadraticBezierSegment quad)
    {
        public (QuadraticBezierSegment first, QuadraticBezierSegment second) Split(ref Point start, in double t = .5)
        {
            var m0 = start.LerpTo(quad.Point1)(in t);
            var m1 = quad.Point1.LerpTo(quad.Point2)(in t);
            var cp = m0.LerpTo(m1)(in t);

            return (new QuadraticBezierSegment { Point1 = m0, Point2 = cp },
                new QuadraticBezierSegment { Point1 = m1, Point2 = start = quad.Point2 });
        }
    }
}
