using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Bound;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.Extensions;
using HandyAvaloniaBits.Animations.Composition.Factory.Implementations;
using ReactiveUI;
using System.Reactive.Linq;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Controls;

public sealed class PointerAnimationTarget : TemplatedControl
{
    private ImplicitAnimationCollection? animationCache;
    private CompositionVisual? visualCache;
    private Vector3D? transformOriginCache;
    private Control? contentCache;

    public required Control Content { get; init; }
    public PointerAnimationBound PointerBound { get; set; } = null!;
    public int PointerBoundPoolIndex { get; init; } = 0;


    public Vector3D? TransformOrigin { get; init; }
    public TimeSpan Duration { get; init; } = TimeSpan.FromMicroseconds(50);
    public TimeSpan Delay { get; init; } = TimeSpan.Zero;
    public TimeSpan? StopDelay { get; init; }

    public IList<ImplicitCompositionPropertyGroup> Expressions { get; } = [];
    public IList<ImplicitControlPorpertyGroup> ControlExpressions { get; } = [];

    public void AttachBoundsToContent(in AvaloniaList<PointerAnimationBound> canvasBounds, out Shape? staticBound)
    {
        staticBound = PointerBound?.Bounds;
        PointerBound ??= canvasBounds[PointerBoundPoolIndex];

        PointerBound.OnPointerEntered.Subscribe(_ => Initialize());
        PointerBound.OnPointerMoved.Subscribe(f => UpdateAnimationVisual(in f));

        if (StopDelay is not null)
            PointerBound.OnPointerMoved
                .Throttle(StopDelay.Value, RxSchedulers.MainThreadScheduler)
                .Subscribe(f =>
                {
                    StopAnimationVisual(in f);
                    StopAnimationControl(in f);
                });
    }


    public void Initialize()
    {
        contentCache ??= Content;
        visualCache ??= ElementComposition.GetElementVisual(Content)!;
        transformOriginCache ??= CreateTransformOrigin();

        visualCache.ImplicitAnimations = animationCache ??= CreateImplicitAnimationCollection(in visualCache);
        visualCache.CenterPoint = transformOriginCache.Value;
    }

    private void UpdateAnimationVisual(in PointerFrame frame)
    {
        foreach (var expression in Expressions)
            expression.Update(ref visualCache!, in frame);
    }

    private void UpdateAnimationControl(in PointerFrame frame)
    {
        foreach (var controlExpression in ControlExpressions)
            controlExpression.Update(ref contentCache!, frame);
    }

    private void StopAnimationVisual(in PointerFrame frame)
    {
        foreach (var expression in Expressions)
            expression.Stop(ref visualCache!, in frame);
    }

    private void StopAnimationControl(in PointerFrame frame)
    {
        foreach (var controlExpression in ControlExpressions)
            controlExpression.Stop(ref contentCache!, frame);
    }


    private ImplicitAnimationCollection CreateImplicitAnimationCollection(in CompositionVisual visual)
    {
        var compositor = visual.Compositor;
        var animationFactory = new CompositionAnimationFactory
        {
            Compositor = compositor,
            Duration = Duration,
            Delay = Delay
        };

        var implicitAnimations = compositor.CreateImplicitAnimationCollection();
        foreach (var expression in Expressions)
        {
            var implicitAnimation = expression.CreateImplicitAnimation(animationFactory);
            implicitAnimations.Add(implicitAnimation.Target!, implicitAnimation);
        }

        return implicitAnimations;
    }

    private Vector3D CreateTransformOrigin() =>
        TransformOrigin ?? new Vector3D(
            Content.Width / 2,
            Content.Height / 2,
            Content.ZIndex);
}
