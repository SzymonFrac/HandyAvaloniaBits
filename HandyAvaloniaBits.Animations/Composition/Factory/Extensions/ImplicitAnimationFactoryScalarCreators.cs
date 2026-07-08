using Avalonia.Rendering.Composition;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.Implementations;

namespace HandyAvaloniaBits.Animations.Composition.Factory.Extensions;

public static class ImplicitAnimationFactoryScalarCreators
{
    extension(ICompositionAnimationFactory factory)
    {
        public ScalarKeyFrameAnimation CreateImplicitOpacity(ImplicitOpacityPropertyGroup scalarGroup)
        {
            var opacity = factory.Compositor.CreateScalarKeyFrameAnimation();
            opacity.Target = nameof(CompositionVisual.Opacity);
            opacity.Duration = factory.Duration;
            opacity.DelayTime = factory.Delay;

            foreach (var kf in scalarGroup.GetKeyFrames())
                opacity.InsertExpressionKeyFrame(kf.Progress, kf.Value, kf.Easing);

            return opacity;
        }
    }
}
