using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillShapeCutout.Abstract;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillShapeCutout.Implementations;

internal sealed class TopRightFillShapeCutoutLerpFactory : AbstractFillShapeCutoutFactory
{
    public override LerpShapeCutout Create() => (in t) =>
    {
        var radius = LerpRadius(in t);
        return new(radius, 0, 0, radius, WindowBounds.Width, 0);
    };
}
