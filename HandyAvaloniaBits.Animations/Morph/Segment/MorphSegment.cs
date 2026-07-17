using Avalonia.Media;

namespace HandyAvaloniaBits.Animations.Morph.Segment;

internal abstract record MorphSegment
{
    public abstract void Apply(in double t, in StreamGeometryContext sgc);
}
