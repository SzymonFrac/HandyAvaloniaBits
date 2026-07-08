using Avalonia.Rendering.Composition;

namespace HandyAvaloniaBits.Animations.Composition.Factory.Implementations;

public sealed class CompositionAnimationFactory : ICompositionAnimationFactory
{
    public required Compositor Compositor { get; init; }
    public TimeSpan Delay { get; init; }
    public TimeSpan Duration { get; init; }

}
