using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment;

internal abstract record MorphSegment
{
    protected MorphPointLerp Lerp;

    protected MorphSegment(MorphPointLerp lerp) => Lerp = lerp;

    public abstract void Apply(in double t, in StreamGeometryContext sgc);
}
