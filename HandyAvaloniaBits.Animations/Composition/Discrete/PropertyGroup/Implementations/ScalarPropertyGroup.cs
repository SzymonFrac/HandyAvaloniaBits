using HandyAvaloniaBits.Animations.Composition.Discrete.KeyFrame.Implementations;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.PropertyGroup.Implementations;

public sealed record ScalarPropertyGroup : CompositionPropertyGroup
{
    public required string PropertyName { get; init; }
    public IList<ScalarKeyFrame> ScalarKeyFrames { get; } = [];
};
