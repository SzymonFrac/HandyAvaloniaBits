using Avalonia;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.KeyFrame.Implementations;

public sealed record OffsetKeyFrame : CompositionKeyFrame
{
    public required Vector3D Offset { get; init; }
}