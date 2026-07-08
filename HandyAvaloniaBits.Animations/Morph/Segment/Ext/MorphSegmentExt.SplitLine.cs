using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Ext;

internal static partial class MorphSegmentExt
{
    extension(LineSegment line)
    {
        public (LineSegment first, LineSegment second) Split(ref Point start, in double t = .5) =>
            (new LineSegment { Point = start.LerpTo(line.Point)(in t) },
            new LineSegment { Point = start = line.Point });
    }
}
