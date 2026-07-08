using Avalonia.Rendering.Composition;
using HandyAvaloniaBits.Animations.Composition.Discrete.PropertyGroup.Implementations;

namespace HandyAvaloniaBits.Animations.Composition.Factory.Extensions;

public static class AnimationFactoryScalarCreators
{
    extension(ICompositionAnimationFactory factory)
    {
        public ScalarKeyFrameAnimation CreateScalar(ScalarPropertyGroup scalarGroup)
        {
            var scalar = factory.Compositor.CreateScalarKeyFrameAnimation();
            scalar.Target = scalarGroup.PropertyName;
            scalar.Duration = factory.Duration;
            scalar.DelayTime = factory.Delay;
            //rotation.IterationBehavior = AnimationIterationBehavior.Forever;

            foreach (var kf in scalarGroup.ScalarKeyFrames)
                if (kf.Easing is not null)
                    scalar.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Scalar, kf.Easing);
                else
                    scalar.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Scalar);

            return scalar;
        }
    }
}
