using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

internal abstract record MorphToArc : MorphSegment
{
    protected MorphSizeLerp Size { get; }
    protected MorphRotationLerp Rotation { get; }
    protected MorphPointLerp Lerp { get; }
    
    protected ArcSegment Arc { get; }

    protected MorphToArc(MorphSizeLerp size, MorphRotationLerp rotation, MorphPointLerp lerp, ArcSegment arc) =>
        (Size, Rotation, Lerp, Arc) = (size, rotation, lerp, arc);

    public override void Apply(in double t, in StreamGeometryContext sgc) =>
        sgc.ArcTo(Lerp(in t), Size(in t), Rotation(t), Arc.IsLargeArc, Arc.SweepDirection, Arc.IsStroked);
}
