using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

internal abstract record MorphToLine : MorphSegment
{
    protected MorphPointLerp Lerp { get; }

    protected MorphToLine(MorphPointLerp lerp) => Lerp = lerp;

    public override void Apply(in double t, in StreamGeometryContext sgc) => sgc.LineTo(Lerp(t));
}
