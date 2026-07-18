using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

internal abstract record MorphToArc : MorphSegment
{
    protected MorphSizeLerp Size { get; }
    protected MorphRotationLerp Rotation { get; }
    protected MorphPointLerp Point { get; }
    
    protected ArcSegment Arc { get; }

    protected MorphToArc(MorphSizeLerp size, MorphRotationLerp rotation, MorphPointLerp point, ArcSegment arc) =>
        (Size, Rotation, Point, Arc) = (size, rotation, point, arc);

    public override void Apply(in double t, in StreamGeometryContext sgc) =>
        sgc.ArcTo(Point(in t), Size(in t), Rotation(t), Arc.IsLargeArc, Arc.SweepDirection, Arc.IsStroked);
}
