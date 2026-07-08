using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

internal abstract record MorphToQuad : MorphSegment
{
    protected MorphToQuad(MorphPointLerp lerp) : base(lerp) { }

    public override void Apply(in double t, in StreamGeometryContext sgc)
    {
        var lerps = QuadraticMorphPointLerps.Create(Lerp);

        sgc.QuadraticBezierTo(lerps.First(in t), lerps.Second(in t));
    }



    private readonly ref struct QuadraticMorphPointLerps
    {
        public MorphPointLerp First { get; }
        public MorphPointLerp Second { get; }

        public QuadraticMorphPointLerps(MorphPointLerp first, MorphPointLerp second) =>
            (First, Second) = (first, second);

        public static QuadraticMorphPointLerps Create(MorphPointLerp multicast)
        {
            var lerps = multicast.GetInvocationList()
                .Cast<MorphPointLerp>()
                .ToList();

            return new(lerps[0], lerps[1]);
        }
    }
}
