using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

internal abstract record MorphToQuad : MorphSegment
{
    protected MorphPointLerp ControlLerp { get; }
    protected MorphPointLerp Point { get; }

    protected MorphToQuad(MorphPointLerp controlLerp, MorphPointLerp point) =>
        (ControlLerp, Point) = (controlLerp, point);

    public override void Apply(in double t, in StreamGeometryContext sgc) =>
        sgc.QuadraticBezierTo(ControlLerp(in t), Point(in t));
}
