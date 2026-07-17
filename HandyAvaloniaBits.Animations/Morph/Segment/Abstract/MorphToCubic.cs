using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

internal abstract record MorphToCubic : MorphSegment
{
    protected MorphPointLerp FirstControlLerp { get; }
    protected MorphPointLerp SecondControlLerp { get; }
    protected MorphPointLerp Lerp { get; }

    protected MorphToCubic(MorphPointLerp firstControlLerp, MorphPointLerp secondControlLerp, MorphPointLerp lerp) =>
        (FirstControlLerp, SecondControlLerp, Lerp) = (firstControlLerp, secondControlLerp, lerp);

    public override void Apply(in double t, in StreamGeometryContext sgc) =>
        sgc.CubicBezierTo(FirstControlLerp(in t), SecondControlLerp(in t), Lerp(in t));
}
