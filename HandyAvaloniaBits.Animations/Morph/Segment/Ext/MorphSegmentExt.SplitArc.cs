using Avalonia;
using Avalonia.Media;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Ext;

internal static partial class MorphSegmentExt
{
    extension(ArcSegment arc)
    {
        public (ArcSegment? first, ArcSegment? second) Split(ref Point start, in double t = .5)
        {
            start = arc.Point;

            // possible, but I can't be bothered...
            return (default, default);
        }
    }
}
