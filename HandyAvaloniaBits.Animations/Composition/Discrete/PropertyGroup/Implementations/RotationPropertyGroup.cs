using HandyAvaloniaBits.Animations.Composition.Discrete.KeyFrame.Implementations;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.PropertyGroup.Implementations;

public sealed record RotationPropertyGroup : CompositionPropertyGroup
{
    public IList<RotationKeyFrame> RotationKeyFrames { get; } = [];
};
