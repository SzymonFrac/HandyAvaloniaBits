using Avalonia;
using Avalonia.Controls;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using Avalonia.Styling;
using HandyAvaloniaBits.Animations.Composition.Discrete.PropertyGroup;
using HandyAvaloniaBits.Animations.Composition.Discrete.PropertyGroup.Extensions;
using HandyAvaloniaBits.Animations.Composition.Factory.Implementations;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.AnimationSegment;

public sealed class CompositionAnimationSegment
{
    private CompositionAnimationGroup? triggerCache;
    private CompositionAnimationGroup? cancelCache;
    private CompositionVisual? visualCache;

    public ControlTheme? Prepare { get; init; }
    public Vector3D? TransformOrigin { get; init; }

    public IList<CompositionPropertyGroup> TriggerGroup { get; } = [];
    public required TimeSpan TriggerDuration { get; init; }
    public TimeSpan TriggerDelay { get; init; } = TimeSpan.Zero;
    public TimeSpan TotalTriggerDuration => TriggerDuration + TriggerDelay;

    public IList<CompositionPropertyGroup> CancelGroup { get; } = [];
    public TimeSpan CancelDuration { get; init; } = TimeSpan.Zero;
    public TimeSpan CancelDelay { get; init; } = TimeSpan.Zero;

    public void Trigger(CompositionVisual visual, Control control)
    {
        visualCache ??= visual;

        control.Theme = Prepare;
        visualCache.CenterPoint = TransformOrigin ?? new(
            control.Width / 2,
            control.Height / 2,
            control.ZIndex);

        triggerCache ??= ResolveTriggerCache(in visual);
        cancelCache ??= ResolveCancelCache(in visual);

        visualCache.StartAnimationGroup(triggerCache);
    }

    public void Cancel()
    {
        if (cancelCache is not null)
            visualCache?.StartAnimationGroup(cancelCache);
    }

    private CompositionAnimationGroup ResolveTriggerCache(in CompositionVisual visual)
    {
        var compositor = visual.Compositor;

        var animationGroup = compositor.CreateAnimationGroup();

        var animationFactory = new CompositionAnimationFactory
        {
            Compositor = compositor,
            Duration = TriggerDuration,
            Delay = TriggerDelay,
        };

        foreach (var group in TriggerGroup)
        {
            var compositionAnimation = group.CreateAnimation(animationFactory);
            animationGroup.Add(compositionAnimation);
        }

        return animationGroup;
    }

    private CompositionAnimationGroup ResolveCancelCache(in CompositionVisual visual)
    {
        var compositor = visual.Compositor;

        var animationGroup = compositor.CreateAnimationGroup();

        var animationFactory = new CompositionAnimationFactory
        {
            Compositor = compositor,
            Duration = CancelDuration,
            Delay = CancelDelay,
        };

        foreach (var group in CancelGroup)
        {
            var compositionAnimation = group.CreateAnimation(animationFactory);
            animationGroup.Add(compositionAnimation);
        }

        return animationGroup;
    }
}
