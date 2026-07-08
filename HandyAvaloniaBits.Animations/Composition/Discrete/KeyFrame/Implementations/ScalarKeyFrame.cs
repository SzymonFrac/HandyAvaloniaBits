namespace HandyAvaloniaBits.Animations.Composition.Discrete.KeyFrame.Implementations;

public sealed record ScalarKeyFrame : CompositionKeyFrame
{
    public required float Scalar { get; init; }
}
