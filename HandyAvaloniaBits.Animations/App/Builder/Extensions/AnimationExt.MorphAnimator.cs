using Avalonia;
using HandyAvaloniaBits.Animations.Morph.Animator;
using HandyAvaloniaBits.Animations.Morph.Animator.Implementations;

namespace HandyAvaloniaBits.Animations.App.Builder.Extensions;

public static partial class AnimationExt
{
    extension(AppBuilder builder)
    {
        public AppBuilder UseExtensionsAnimators(MorphAnimator? morphAnimator = default)
        {
            var animator = morphAnimator ?? new SegmentOrderMorphAnimator();
            animator.Register();

            return builder;
        }
    }
}
