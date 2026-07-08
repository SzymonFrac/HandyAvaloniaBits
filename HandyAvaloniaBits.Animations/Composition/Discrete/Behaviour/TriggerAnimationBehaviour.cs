using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Rendering.Composition;
using Avalonia.Threading;
using Avalonia.VisualTree;
using HandyAvaloniaBits.Animations.Composition.Discrete.AnimationSegment;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.Behaviour;

public sealed class TriggerAnimationBehaviour : AvaloniaObject
{
    public static readonly AttachedProperty<bool> EnabledProperty =
        AvaloniaProperty.RegisterAttached<Control, bool>(
            "Enabled",
            typeof(TriggerAnimationBehaviour),
            false);

    /// <summary>
    /// Minimum should be 5ms (maybe could be a little less), to ensure that compositor is not null.
    /// Delay must be set first in xaml if there is a different delay.
    /// </summary>
    public static readonly AttachedProperty<TimeSpan> DeferEnabledDelayProperty =
        AvaloniaProperty.RegisterAttached<Control, TimeSpan>(
            "DeferEnabledDelay",
            typeof(TriggerAnimationBehaviour),
            TimeSpan.FromMilliseconds(5),
            false,
            Avalonia.Data.BindingMode.Default,
            t => t is { TotalMilliseconds: >= 5 });

    /// <summary>
    /// When setting Enabled to true instantly, the CompositionVisual is null, so it needs to be defered
    /// </summary>
    public static readonly AttachedProperty<bool> DeferEnabledProperty =
        AvaloniaProperty.RegisterAttached<Control, bool>(
            "DeferEnabled",
            typeof(TriggerAnimationBehaviour),
            false);

    public static readonly AttachedProperty<IterationCount> IterationsProperty =
        AvaloniaProperty.RegisterAttached<Control, IterationCount>(
            "Iterations",
            typeof(TriggerAnimationBehaviour),
            new(1));

    public static readonly AttachedProperty<IList<CompositionAnimationSegment>> OnTriggerProperty =
        AvaloniaProperty.RegisterAttached<Control, IList<CompositionAnimationSegment>>(
            "OnTrigger",
            typeof(TriggerAnimationBehaviour));


    public static void SetEnabled(AvaloniaObject element, bool value) => element.SetValue(EnabledProperty, value);
    public static bool GetEnabled(AvaloniaObject element) => element.GetValue(EnabledProperty);
    public static void SetDeferEnabled(AvaloniaObject element, bool value) => element.SetValue(DeferEnabledProperty, value);
    public static bool GetDeferEnabled(AvaloniaObject element) => element.GetValue(DeferEnabledProperty);
    public static void SetDeferEnabledDelay(AvaloniaObject element, TimeSpan value) => element.SetValue(DeferEnabledDelayProperty, value);
    public static TimeSpan GetDeferEnabledDelay(AvaloniaObject element) => element.GetValue(DeferEnabledDelayProperty);
    public static void SetIterations(AvaloniaObject element, IterationCount value) => element.SetValue(IterationsProperty, value);
    public static IterationCount GetIterations(AvaloniaObject element) => element.GetValue(IterationsProperty);

    public static void SetOnTrigger(AvaloniaObject element, IList<CompositionAnimationSegment> value) => element.SetValue(OnTriggerProperty, value);
    public static IList<CompositionAnimationSegment> GetOnTrigger(AvaloniaObject element)
    {
        var list = element.GetValue(OnTriggerProperty);
        if (list is null)
        {
            list = [];
            element.SetValue(OnTriggerProperty, list);
        }
        return list;
    }



    static TriggerAnimationBehaviour()
    {
        DeferEnabledProperty.Changed.AddClassHandler<Control, bool>(async (control, e) =>
        {
            await Task.Delay(GetDeferEnabledDelay(control));

            SetEnabled(control, e.NewValue.Value);
        });
        EnabledProperty.Changed.AddClassHandler<Control, bool>(async (control, e) =>
        {
            if (e.NewValue.Value)
                await StartAnimation(control);
            else
                await StopAnimation(control);
        });
    }

    private static async Task StartAnimation(Control control)
    {
        var visual = ElementComposition.GetElementVisual(control)!;

        var triggerAnimation = GetOnTrigger(control);

        await RunTriggerAnimationAsync(visual, control, triggerAnimation);
    }
    private static async Task StopAnimation(Control control)
    {

    }

    private static async Task RunTriggerAnimationAsync(CompositionVisual visual, Control control, IList<CompositionAnimationSegment> triggerAnimation)
    {
        async Task RunAnimation()
        {
            var cancelTask = CreateDisableTask(in control);

            foreach (var segment in triggerAnimation)
            {
                if (!control.IsAttachedToVisualTree())
                    break;

                await Dispatcher.UIThread.InvokeAsync(() => segment.Trigger(visual, control));

                var wait = Task.Delay(segment.TotalTriggerDuration);
                if (await WaitOrCancel(wait, cancelTask))
                {
                    await Dispatcher.UIThread.InvokeAsync(segment.Cancel);
                    break;
                }
            }
        }


        var iterations = GetIterations(control);

        if (iterations.IsInfinite)
            while (control.IsAttachedToVisualTree())
                await RunAnimation();

        for (ulong i = 0; i < iterations.Value; i++)
            if (control.IsAttachedToVisualTree())
                await RunAnimation();
    }

    private static Task CreateDisableTask(in Control control)
    {
        var tcs = new TaskCompletionSource();
        var subscirption = new CompositeDisposable(1);

        control.GetObservable(EnabledProperty)
            .DistinctUntilChanged()
            .Where(e => !e)
            .Subscribe(_ => tcs.TrySetResult())
            .DisposeWith(subscirption);

        return tcs.Task;
    }

    async static Task<bool> WaitOrCancel(Task waitTask, Task cancelTask) =>
        await Task.WhenAny(waitTask, cancelTask) == cancelTask;
}