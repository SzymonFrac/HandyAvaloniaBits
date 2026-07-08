using Avalonia.Media;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout;

internal abstract record ShapeCutout
{
    public Geometry Shape { get; }
    public required LerpShapeCutout LerpFunc { get; init; }

    protected ShapeCutout(Geometry shape) => Shape = shape;

    public virtual void Update(in double t, in DrawingContext dc)
    {
        using (dc.PushTransform(LerpFunc(in t)))
            dc.DrawGeometry(Brushes.Black, null, Shape);
    }
}
