using Avalonia;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillPerimeterShapeCutout.Abstract;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillPerimeterShapeCutout.Implementations;

internal sealed class TopRightFillPerimeterShapeCutoutLerpFactory : AbstractFillPerimeterShapeCutoutFactory
{
    public override Point LerpPosition(in double t) =>
        WindowBounds.TopRight - base.LerpPosition(in t);
}
