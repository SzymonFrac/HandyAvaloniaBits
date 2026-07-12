using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.Segment;
using HandyAvaloniaBits.Animations.Morph.Segment.Ext;

namespace HandyAvaloniaBits.Animations.Morph.Figure;

internal readonly record struct MorphFigure
{
    private readonly bool _isClosed;
    private readonly bool _isFilled;

    private readonly MorphPointLerp _startLerp;
    private readonly IEnumerable<MorphSegment> _segmentLerps;

    private MorphFigure(bool isClosed, bool isFilled, MorphPointLerp startLerp, IEnumerable<MorphSegment> segmentLerps) =>
        (_isClosed, _isFilled, _startLerp, _segmentLerps) = (isClosed, isFilled, startLerp, segmentLerps);

    public static MorphFigure CreateInOrder(in PathFigure from, in PathFigure to)
    {
        if ((from.Segments, to.Segments) is not (PathSegments fromSegments, PathSegments toSegments))
            throw new ArgumentException("PathFigure must have non-null segmnets", from.Segments is null ? nameof(from) : nameof(to));

        Normalize(ref fromSegments, ref toSegments, (from.StartPoint, to.StartPoint));

        var startLerp = from.StartPoint.LerpTo(to.StartPoint);

        var start = (from.StartPoint, to.StartPoint);
        var segmentLerps = fromSegments.Zip(toSegments)
            .Select(s => s.ToMorphSegment(ref start));

        return new(to.IsClosed, to.IsFilled, startLerp, segmentLerps);
    }

    public void Apply(in double t, in StreamGeometryContext sgc)
    {
        sgc.BeginFigure(_startLerp(in t), _isFilled);

        foreach (var segmentMorph in _segmentLerps)
            segmentMorph.Apply(in t, in sgc);

        sgc.EndFigure(_isClosed);
    }


    private static void Normalize(ref PathSegments from, ref PathSegments to, in (Point from, Point to) s)
    {
        var normalizedSegmentCount = Math.Max(from.Count, to.Count);
        
        var i = 0;
        var start = s.from;
        while (from.Count < normalizedSegmentCount)
        {
            var cycle = i % from.Count;

            //if (from[cycle].Split(ref start) is not (PathSegment first, PathSegment second))
            //{
            //    i++;
            //    continue;
            //}

            var (first, second) = from[cycle].Split(ref start);

            from.RemoveAt(cycle);

            from.Insert(cycle, first);
            from.Insert(cycle + 1, second);
            
            i += 2;
        }

        i = 0;
        start = s.to;
        while (to.Count < normalizedSegmentCount)
        {
            var cycle = i % to.Count;

            //if (to[cycle].Split(ref start) is not (PathSegment first, PathSegment second))
            //{
            //    i++;
            //    continue;
            //}

            var (first, second) = to[cycle].Split(ref start);

            to.RemoveAt(cycle);

            to.Insert(cycle, first);
            to.Insert(cycle + 1, second);
            
            i += 2;
        }
    }
}