using Avalonia.Media;
using AvaloniaAnimationExtensions.Animations.Morph.AvaloniaPoint.Ext;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Abstract;

internal abstract record MorphToArc : MorphSegment
{
    protected MorphSizeLerp? Size { get; }
    protected MorphRotationLerp Rotation { get; }
    
    protected ArcSegment Arc { get; }

    protected MorphToArc(MorphPointLerp lerp, MorphRotationLerp rotation, ArcSegment arc) : base(lerp) =>
        (Rotation, Arc) = (rotation, arc);
    protected MorphToArc(MorphPointLerp lerp, MorphSizeLerp size, MorphRotationLerp rotation, ArcSegment arc) : base(lerp) =>
        (Size, Rotation, Arc) = (size, rotation, arc);

    public override void Apply(in double t, in StreamGeometryContext sgc)
    {
        var lerps = Size is not null
            ? ArcMorphPointLerps.Create(Lerp, Size)
            : ArcMorphPointLerps.Create(Lerp);

        sgc.ArcTo(lerps.Point(in t), lerps.Size(in t), Rotation(t), Arc.IsLargeArc, Arc.SweepDirection, Arc.IsStroked);
    }

    private readonly ref struct ArcMorphPointLerps
    {
        public MorphPointLerp Point { get; }
        public MorphSizeLerp Size { get; }

        public ArcMorphPointLerps(MorphPointLerp point, MorphPointLerp size) =>
            (Point, Size) = (point, size.ToSize());
        public ArcMorphPointLerps(MorphPointLerp point, MorphSizeLerp size) =>
            (Point, Size) = (point, size);

        public static ArcMorphPointLerps Create(MorphPointLerp multicast)
        {
            var lerps = multicast.GetInvocationList()
                .Cast<MorphPointLerp>()
                .ToList();

            return new(lerps[0], lerps[1]);
        }

        public static ArcMorphPointLerps Create(MorphPointLerp point, MorphSizeLerp size) => new(point, size);
    }
}
