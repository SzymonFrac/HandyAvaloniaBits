using HandyAvaloniaBits.Animations.Composition.Discrete.KeyFrame.Implementations;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.PropertyGroup.Implementations;

public sealed record OffsetPropertyGroup : CompositionPropertyGroup
{
    public IList<OffsetKeyFrame> OffsetKeyFrames { get; } = [];
}
