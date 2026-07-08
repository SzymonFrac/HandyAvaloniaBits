using Avalonia.Rendering.Composition;
using HandyAvaloniaBits.Animations.Composition.Discrete.PropertyGroup.Implementations;

namespace HandyAvaloniaBits.Animations.Composition.Factory.Extensions;

public static class AnimationFactoryTransformCreators
{
    extension(ICompositionAnimationFactory factory)
    {
        public ScalarKeyFrameAnimation CreateRotation(RotationPropertyGroup rotationGroup)
        {
            var rotation = factory.Compositor.CreateScalarKeyFrameAnimation();
            rotation.Target = nameof(CompositionVisual.RotationAngle);
            rotation.Duration = factory.Duration;
            rotation.DelayTime = factory.Delay;

            foreach (var kf in rotationGroup.RotationKeyFrames)
                if (kf.Easing is not null)
                    rotation.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Radians, kf.Easing);
                else
                    rotation.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Radians);

            return rotation;
        }

        public Vector3DKeyFrameAnimation CreateScale(ScalePropertyGroup scaleGroup)
        {
            var scale = factory.Compositor.CreateVector3DKeyFrameAnimation();
            scale.Target = nameof(CompositionVisual.Scale);
            scale.Duration = factory.Duration;
            scale.DelayTime = factory.Delay;
            //scale.IterationBehavior = AnimationIterationBehavior.Forever;

            foreach (var kf in scaleGroup.ScaleKeyFrames)
                if (kf.Easing is not null)
                    scale.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Scale ?? default, kf.Easing);
                else
                    scale.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Scale ?? default);

            return scale;
        }

        //?
        public Vector3DKeyFrameAnimation CreateOffset(OffsetPropertyGroup offsetGroup)
        {
            var offset = factory.Compositor.CreateVector3DKeyFrameAnimation();
            offset.Target = nameof(CompositionVisual.Offset);
            offset.Duration = factory.Duration;
            offset.DelayTime = factory.Delay;
            //quaternion.IterationBehavior = AnimationIterationBehavior.Forever;

            foreach (var kf in offsetGroup.OffsetKeyFrames)
                if (kf.Easing is not null)
                    offset.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Offset, kf.Easing);
                else
                    offset.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Offset);

            return offset;
        }

        public QuaternionKeyFrameAnimation CreateOrientation(QuaternionPropertyGroup quaternionGroup)
        {
            var quaternion = factory.Compositor.CreateQuaternionKeyFrameAnimation();
            quaternion.Target = nameof(CompositionVisual.Orientation);
            quaternion.Duration = factory.Duration;
            quaternion.DelayTime = factory.Delay;
            //quaternion.IterationBehavior = AnimationIterationBehavior.Forever;

            foreach (var kf in quaternionGroup.QuaternionKeyFrames)
                if (kf.Easing is not null)
                    quaternion.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Quaternion, kf.Easing);
                else
                    quaternion.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Quaternion);

            return quaternion;
        }
    }
}
