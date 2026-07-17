using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

internal abstract record MorphToQuad : MorphSegment
{
    protected MorphPointLerp ControlLerp { get; }
    protected MorphPointLerp Lerp { get; }

    protected MorphToQuad(MorphPointLerp controlLerp, MorphPointLerp lerp) =>
        (ControlLerp, Lerp) = (controlLerp, lerp);

    public override void Apply(in double t, in StreamGeometryContext sgc) =>
        sgc.QuadraticBezierTo(ControlLerp(in t), Lerp(in t));
}
