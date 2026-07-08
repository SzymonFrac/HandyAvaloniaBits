using Avalonia;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.GridShapeCutoutLerp.Abstract;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.GridShapeCutoutLerp.Implementations;

internal sealed class TopLeftGridShapeCutoutLerpFactory : AbstractGridShapeCutoutLerpFactory
{
    public override Point GetTransformPosition(in Point relativePositionInGrid) =>
        new(DestinationRect.Width * relativePositionInGrid.X, DestinationRect.Height * relativePositionInGrid.Y);
};
