using Avalonia;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillPerimeterShapeCutout.Abstract;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillPerimeterShapeCutout.Implementations;

internal sealed class BottomRightFillPerimeterShapeCutoutLerpFactory : AbstractFillPerimeterShapeCutoutFactory
{
    public override Point LerpPosition(in double t) =>
        WindowBounds.BottomRight - base.LerpPosition(in t);
}
