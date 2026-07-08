using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.Segment.Implementations;
using System.Diagnostics;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Ext;

internal static partial class MorphSegmentExt
{
    extension((PathSegment from, PathSegment to) segments)
    {
        public MorphSegment ToMorphSegment(ref (Point from, Point to) start) => (segments.from, segments.to) switch
        {
            (LineSegment from, LineSegment to) => MorphLineToLine.Create(in from, in to, ref start),
            (LineSegment from, QuadraticBezierSegment to) => MorphLineToQuad.Create(in from, in to, ref start),
            (LineSegment from, BezierSegment to) => MorphLineToCubic.Create(in from, in to, ref start),
            (LineSegment from, ArcSegment to) => MorphLineToArc.Create(in from, in to, ref start),

            (QuadraticBezierSegment from, LineSegment to) => MorphQuadToLine.Create(in from, in to, ref start),
            (QuadraticBezierSegment from, QuadraticBezierSegment to) => MorphQuadToQuad.Create(in from, in to, ref start),
            (QuadraticBezierSegment from, BezierSegment to) => MorphQuadToCubic.Create(in from, in to, ref start),
            (QuadraticBezierSegment from, ArcSegment to) => MorphQuadToArc.Create(in from, in to, ref start),

            (BezierSegment from, LineSegment to) => MorphCubicToLine.Create(in from, in to, ref start),
            (BezierSegment from, QuadraticBezierSegment to) => MorphCubicToQuad.Create(in from, in to, ref start),
            (BezierSegment from, BezierSegment to) => MorphCubicToCubic.Create(in from, in to, ref start),
            (BezierSegment from, ArcSegment to) => MorphCubicToArc.Create(in from, in to, ref start),

            (ArcSegment from, LineSegment to) => MorphArcToLine.Create(in from, in to, ref start),
            (ArcSegment from, QuadraticBezierSegment to) => MorphArcToQuad.Create(in from, in to, ref start),
            (ArcSegment from, BezierSegment to) => MorphArcToCubic.Create(in from, in to, ref start),
            (ArcSegment from, ArcSegment to) => MorphArcToArc.Create(in from, in to, ref start),

            _ => throw new UnreachableException()
        };
    }
}
