using Avalonia;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.GridShapeCutoutLerp.Abstract;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.GridShapeCutoutLerp.Implementations;

internal sealed class BottomRightGridShapeCutoutLerpFactory : AbstractGridShapeCutoutLerpFactory
{
    public override Point GetTransformPosition(in Point relativePositionInGrid) =>
        WindowBounds.BottomRight - new Point(DestinationRect.Width * relativePositionInGrid.X, DestinationRect.Height * relativePositionInGrid.Y);
}
