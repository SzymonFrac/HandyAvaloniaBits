using Avalonia.Animation;
using Avalonia.Media;
using HandyAvaloniaBits.Animations.Morph.Figure.Lerp;
using HandyAvaloniaBits.Animations.Morph.Figure.Lerp.Factory.Implementations;

namespace HandyAvaloniaBits.Animations.Morph.Animator.Implementations;

public sealed class SegmentOrderMorphAnimator : MorphAnimator
{
    private Geometry? lastMorphed;
    private MorphFigureLerp? lerpCache;

    public override Geometry Interpolate(double progress, Geometry oldValue, Geometry newValue)
    {
        if (oldValue == newValue)
            return newValue;

        if (newValue == lastMorphed)
            return lerpCache!(in progress);


        if ((oldValue, newValue) is not (PathGeometry from, PathGeometry to))
            return newValue;

        var factory = new SegmentOrderMorphFigureLerpFactory();
        lerpCache = factory.Create(in from, in to);

        lastMorphed = newValue;
        return lerpCache(in progress);
    }



    internal override void Register() => Animation.RegisterCustomAnimator<Geometry, SegmentOrderMorphAnimator>();
}
