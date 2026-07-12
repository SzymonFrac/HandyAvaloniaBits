using Avalonia;
using Avalonia.Media;
using System.Diagnostics;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Ext;

internal static partial class MorphSegmentExt
{
    extension(PathSegment segment)
    {
        public (PathSegment first, PathSegment second) Split(ref Point start, in double t = .5) => segment switch
        {
            LineSegment l => l.Split(ref start, in t),
            QuadraticBezierSegment q => q.Split(ref start, in t),
            BezierSegment b => b.Split(ref start, in t),
            ArcSegment a => a.Split(ref start, in t),
            _ => throw new UnreachableException()
        };
    }
}
