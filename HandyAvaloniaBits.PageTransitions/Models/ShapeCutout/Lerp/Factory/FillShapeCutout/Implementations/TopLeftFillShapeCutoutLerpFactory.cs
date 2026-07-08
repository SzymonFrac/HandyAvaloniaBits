using Avalonia;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillShapeCutout.Abstract;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillShapeCutout.Implementations;

internal sealed class TopLeftFillShapeCutoutLerpFactory : AbstractFillShapeCutoutFactory
{
    public override LerpShapeCutout Create() => (in t) =>
    {
        var radius = LerpRadius(in t);
        return Matrix.CreateScale(radius, radius);
    };
}
