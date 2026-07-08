using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.Extensions;
using HandyAvaloniaBits.Animations.Composition.Factory.Implementations;
using System.Numerics;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.Behaviour;

public sealed class PointerAnimationBehaviour : AvaloniaObject
{
    public static readonly AttachedProperty<bool> EnabledProperty =
        AvaloniaProperty.RegisterAttached<Control, bool>(
            "Enabled",
            typeof(PointerAnimationBehaviour),
            false);

    public static readonly AttachedProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.RegisterAttached<Control, TimeSpan>(
            "Duration",
            typeof(PointerAnimationBehaviour),
            TimeSpan.FromMilliseconds(50));

    public static readonly AttachedProperty<TimeSpan> DelayProperty =
        AvaloniaProperty.RegisterAttached<Control, TimeSpan>(
            "Delay",
            typeof(PointerAnimationBehaviour),
            TimeSpan.Zero);

    public static readonly AttachedProperty<Vector3D?> TransformOriginProperty =
        AvaloniaProperty.RegisterAttached<Control, Vector3D?>(
            "TransformOrigin",
            typeof(PointerAnimationBehaviour));

    public static readonly AttachedProperty<IList<ImplicitCompositionPropertyGroup>> ExpressionsProperty =
        AvaloniaProperty.RegisterAttached<Control, IList<ImplicitCompositionPropertyGroup>>(
            "Expressions",
            typeof(PointerAnimationBehaviour));


    private static readonly AttachedProperty<PointerFrame> PoitnerFrameProperty =
        AvaloniaProperty.RegisterAttached<Control, PointerFrame>(
            "PointerFrame",
            typeof(PointerAnimationBehaviour),
            default);


    public static void SetEnabled(AvaloniaObject element, bool value) => element.SetValue(EnabledProperty, value);
    public static bool GetEnabled(AvaloniaObject element) => element.GetValue(EnabledProperty);

    public static void SetDuration(AvaloniaObject element, TimeSpan value) => element.SetValue(DurationProperty, value);
    public static TimeSpan GetDuration(AvaloniaObject element) => element.GetValue(DurationProperty);

    public static void SetDelay(AvaloniaObject element, TimeSpan value) => element.SetValue(DelayProperty, value);
    public static TimeSpan GetDelay(AvaloniaObject element) => element.GetValue(DelayProperty);

    public static void SetTransformOrigin(AvaloniaObject element, Vector3D? value) => element.SetValue(TransformOriginProperty, value);
    public static Vector3D? GetTransformOrigin(AvaloniaObject element) => element.GetValue(TransformOriginProperty);

    public static void SetExpressions(AvaloniaObject element, IList<ImplicitCompositionPropertyGroup> value) => element.SetValue(ExpressionsProperty, value);
    public static IList<ImplicitCompositionPropertyGroup> GetExpressions(AvaloniaObject element)
    {
        var list = element.GetValue(ExpressionsProperty);
        if (list is null)
        {
            list = [];
            element.SetValue(ExpressionsProperty, list);
        }
        return list;
    }
    private static IEnumerable<ImplicitCompositionPropertyGroup> GetExpressions(ref Control control) => control.GetValue(ExpressionsProperty);

    private static void SetPoitnerFrame(AvaloniaObject element, in PointerFrame value) => element.SetValue(PoitnerFrameProperty, value);
    private static PointerFrame GetPoitnerFrame(in Control control) => control.GetValue(PoitnerFrameProperty);


    static PointerAnimationBehaviour()
    {
        EnabledProperty.Changed.AddClassHandler<Control, bool>((control, e) =>
        {
            if (e.NewValue.Value)
            {
                control.PointerEntered += OnPointerEntered;
                control.PointerMoved += OnPointerMoved;
            }
            else
            {
                control.PointerEntered -= OnPointerEntered;
                control.PointerMoved -= OnPointerMoved;
            }
        });
    }



    private static void OnPointerEntered(object? sender, PointerEventArgs e)
    {
        var control = (Control)sender!;
        var visual = ElementComposition.GetElementVisual(control)!;

        visual.CenterPoint = GetTransformOrigin(control) ?? new(
            control.Width / 2,
            control.Height / 2,
            control.ZIndex);

        visual.ImplicitAnimations ??= CreateImplicitAnimationCollection(control);
    }
    private static void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var control = (Control)sender!;
        var p = GetNextPointerFrame(in control, in e);

        Update(ref control, in p);
    }



    private static void Update(ref Control control, in PointerFrame pointer)
    {
        var visual = ElementComposition.GetElementVisual(control)!;
        var expressions = GetExpressions(ref control);

        foreach (var expression in expressions)
            expression.Update(ref visual, in pointer);
    }

    private static ImplicitAnimationCollection CreateImplicitAnimationCollection(Control control)
    {
        var visual = ElementComposition.GetElementVisual(control)!;
        var compositor = visual.Compositor;

        var duration = GetDuration(control);
        var delay = GetDelay(control);

        var animationFactory = new CompositionAnimationFactory
        {
            Compositor = compositor,
            Duration = duration,
            Delay = delay,
        };

        var implicitAnimations = compositor.CreateImplicitAnimationCollection();
        var expressions = GetExpressions(control);

        foreach (var expression in expressions)
        {
            var implicitAnimation = expression.CreateImplicitAnimation(animationFactory);
            implicitAnimations.Add(implicitAnimation.Target!, implicitAnimation);
        }

        return implicitAnimations;
    }

    private static PointerFrame GetNextPointerFrame(in Control control, in PointerEventArgs e)
    {
        var p = e.GetPosition(control);
        var v = new Vector2((float)p.X, (float)p.Y);

        var previous = GetPoitnerFrame(in control);
        var next = previous.CreateNext(in v);
        SetPoitnerFrame(control, in next);
        
        return next;
    }
}