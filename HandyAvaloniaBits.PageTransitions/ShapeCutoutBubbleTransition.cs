using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using HandyAvaloniaBits.PageTransitions.Models;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Implementations;

namespace HandyAvaloniaBits.PageTransitions;

// Not done
public sealed class ShapeCutoutBubbleTransition : IPageTransition
{
    public Geometry Shape { get; init; } = new EllipseGeometry { RadiusX = 1, RadiusY = 1 };

    public TimeSpan TotalDuration { get; init; } = TimeSpan.FromSeconds(1);
    public int FramesCount { get; init; } = 60;
    public int PerimeterShapesCount { get; init; } = 20;

    public Corner TransitionFrom { get; init; } = Corner.BottomRight;
    public async Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
    {
        if (to is null)
            return;
        if (from is null)
            return;

        if (to.GetVisualParent() is not Panel parentTo)
            return;

        using var rtb = new RenderTargetBitmap(PixelSize.FromSize(parentTo.Bounds.Size, 1));

        var originalOpacityMask = parentTo.OpacityMask;
        parentTo.OpacityMask = new ImageBrush(rtb);

        var fillShapeCutout = FillShapeCutout.Create(Shape, PerimeterShapesCount, TransitionFrom, parentTo.Bounds);

        var lerpFactor = 1 / (double)FramesCount;
        var delayTime = TotalDuration / FramesCount;

        for (double t = 0; t <= 1; t += lerpFactor)
        {
            using (var dc = rtb.CreateDrawingContext())
                fillShapeCutout.Update(in t, in dc);

            await Task.Delay(delayTime, cancellationToken);
        }

        parentTo.OpacityMask = originalOpacityMask;
    }
}
