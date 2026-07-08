using HandyAvaloniaBits.Animations.Composition.Discrete.KeyFrame.Implementations;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.PropertyGroup.Implementations;

public sealed record ScalePropertyGroup : CompositionPropertyGroup
{
    public IList<ScaleKeyFrame> ScaleKeyFrames { get; } = [];
};
