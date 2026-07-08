using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

internal abstract record MorphToCubic : MorphSegment
{
    protected MorphToCubic(MorphPointLerp lerp) : base(lerp) { }

    public override void Apply(in double t, in StreamGeometryContext sgc)
    {
        var lerps = CubicMorphPointLerps.Create(Lerp);

        sgc.CubicBezierTo(lerps.First(in t), lerps.Second(in t), lerps.Third(in t));
    }



    private readonly ref struct CubicMorphPointLerps
    {
        public MorphPointLerp First { get; }
        public MorphPointLerp Second { get; }
        public MorphPointLerp Third { get; }

        public CubicMorphPointLerps(MorphPointLerp first, MorphPointLerp second, MorphPointLerp third) =>
            (First, Second, Third) = (first, second, third);

        public static CubicMorphPointLerps Create(MorphPointLerp multicast)
        {
            var lerps = multicast.GetInvocationList()
                .Cast<MorphPointLerp>()
                .ToList();

            return new(lerps[0], lerps[1], lerps[2]);
        }
    }
}
