using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Bound;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Controls;

public sealed class PointerAnimationCanvas : Canvas
{
    public AvaloniaList<PointerAnimationTarget> PointerTargets { get; } = [];
    public AvaloniaList<PointerAnimationBound> PointerBoundsPool { get; } = [];
    public PointerAnimationCanvas() =>
        AttachedToVisualTree += PointerTargetsAttached;

    private void PointerTargetsAttached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        foreach (var target in PointerTargets)
        {
            target.AttachBoundsToContent(PointerBoundsPool, out var staticBound);
            Children.Add(target.Content);

            if (staticBound is not null)
                Children.Add(staticBound);
        }
        Children.AddRange(PointerBoundsPool.Select(p => p.Bounds));
    }
}
