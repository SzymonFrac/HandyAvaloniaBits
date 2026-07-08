using Avalonia;
using Avalonia.Media;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.FillShapeCutout.Abstract;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Implementations;

internal sealed record FillShapeCutout : ShapeCutout
{
    public FillPerimeterShapeCutout[] PerimeterShapeCutouts { get; }
    public FillShapeCutout(Geometry shape, FillPerimeterShapeCutout[] perimeterShapeCutouts) : base(shape) =>
        PerimeterShapeCutouts = perimeterShapeCutouts;

    public static FillShapeCutout Create(in Geometry shape, int peremeterShapeCutoutsCount, Corner corner, in Rect windowBounds)
    {
        var peremeterShapeCutouts = new FillPerimeterShapeCutout[peremeterShapeCutoutsCount];
        for (int i = 0; i < peremeterShapeCutoutsCount; i++)
            peremeterShapeCutouts[i] = FillPerimeterShapeCutout.Create(shape, corner, windowBounds);

        var factory = AbstractFillShapeCutoutFactory.Get(corner, in windowBounds);

        return new(shape, peremeterShapeCutouts)
        {
            LerpFunc = factory.Create()
        };
    }

    public override void Update(in double t, in DrawingContext dc)
    {
        base.Update(in t, in dc);

        foreach (var psc in PerimeterShapeCutouts)
            psc.Update(in t, in dc);
    }
}
