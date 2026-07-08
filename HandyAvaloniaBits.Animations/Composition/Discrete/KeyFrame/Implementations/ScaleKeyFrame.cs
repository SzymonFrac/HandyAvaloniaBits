using Avalonia;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.KeyFrame.Implementations;

public sealed record ScaleKeyFrame : CompositionKeyFrame
{
    public required Vector3D? Scale { get; init; }
}
