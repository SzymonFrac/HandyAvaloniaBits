using Avalonia.Rendering.Composition;

namespace HandyAvaloniaBits.Animations.Composition.Factory;

public interface ICompositionAnimationFactory
{
    Compositor Compositor { get; }
    TimeSpan Delay { get; }
    TimeSpan Duration { get; }
}
