using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

internal abstract record MorphToLine : MorphSegment
{
    protected MorphPointLerp Point { get; }

    protected MorphToLine(MorphPointLerp point) => Point = point;

    public override void Apply(in double t, in StreamGeometryContext sgc) => sgc.LineTo(Point(t));
}
