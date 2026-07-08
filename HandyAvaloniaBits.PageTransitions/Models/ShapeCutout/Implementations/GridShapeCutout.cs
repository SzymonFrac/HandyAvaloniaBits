using Avalonia.Media;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Implementations;

internal sealed record GridShapeCutout : ShapeCutout
{
    public GridShapeCutout(Geometry shape) : base(shape) { }

    public static GridShapeCutout Create(in Geometry shape, in LerpShapeCutout lerpFunc) => new(shape)
    {
        LerpFunc = lerpFunc
    };
}
