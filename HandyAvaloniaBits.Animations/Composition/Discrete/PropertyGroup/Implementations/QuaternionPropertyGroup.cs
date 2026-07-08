using HandyAvaloniaBits.Animations.Composition.Discrete.KeyFrame.Implementations;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.PropertyGroup.Implementations;

public sealed record QuaternionPropertyGroup : CompositionPropertyGroup
{
    public IList<QuaternionKeyFrame> QuaternionKeyFrames { get; } = [];
}
