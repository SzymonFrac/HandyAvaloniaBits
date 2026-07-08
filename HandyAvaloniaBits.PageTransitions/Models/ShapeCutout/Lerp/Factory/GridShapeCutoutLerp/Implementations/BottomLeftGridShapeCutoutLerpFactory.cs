using Avalonia;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.GridShapeCutoutLerp.Abstract;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.GridShapeCutoutLerp.Implementations;

internal sealed class BottomLeftGridShapeCutoutLerpFactory : AbstractGridShapeCutoutLerpFactory
{
    public override Point GetTransformPosition(in Point relativePositionInGrid) =>
        WindowBounds.BottomLeft + new Point(DestinationRect.Width * relativePositionInGrid.X, -DestinationRect.Height * relativePositionInGrid.Y);
}
