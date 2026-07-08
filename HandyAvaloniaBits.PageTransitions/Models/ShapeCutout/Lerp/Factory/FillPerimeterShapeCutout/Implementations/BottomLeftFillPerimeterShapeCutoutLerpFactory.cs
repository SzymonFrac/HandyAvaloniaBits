using Avalonia;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillPerimeterShapeCutout.Abstract;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillPerimeterShapeCutout.Implementations;

internal sealed class BottomLeftFillPerimeterShapeCutoutLerpFactory : AbstractFillPerimeterShapeCutoutFactory
{
    public override Point LerpPosition(in double t) =>
        WindowBounds.BottomLeft - base.LerpPosition(in t);
}
