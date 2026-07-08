using Avalonia.Animation.Easings;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.KeyFrame;

public sealed record ImplicitCompositionKeyFrame
{
    public required float Progress { get; init; }
    public required string Value { get; init; }
    public Easing? Easing { get; init; }
}
