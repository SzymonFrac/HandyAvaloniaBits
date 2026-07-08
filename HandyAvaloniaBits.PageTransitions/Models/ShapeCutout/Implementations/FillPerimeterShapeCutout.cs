using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillPerimeterShapeCutout.Abstract;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Implementations;

internal sealed record FillPerimeterShapeCutout : ShapeCutout
{
    public FillPerimeterShapeCutout(Geometry shape) : base(shape) { }

    public static FillPerimeterShapeCutout Create(in Geometry shape, Corner corner, in Rect windowBounds) =>
        new(shape)
        {
            LerpFunc = AbstractFillPerimeterShapeCutoutFactory
                .Get(corner, in windowBounds)
                .Create()
        };
}
