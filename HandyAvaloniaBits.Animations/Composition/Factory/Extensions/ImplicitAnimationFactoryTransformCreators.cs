using Avalonia.Rendering.Composition;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.Implementations;

namespace HandyAvaloniaBits.Animations.Composition.Factory.Extensions;

public static class ImplicitAnimationFactoryTransformCreators
{
    extension(ICompositionAnimationFactory factory)
    {
        public ScalarKeyFrameAnimation CreateImplicitRotation(ImplicitRotationPropertyGroup rotationGroup)
        {
            var rotation = factory.Compositor.CreateScalarKeyFrameAnimation();
            rotation.Target = nameof(CompositionVisual.RotationAngle);
            rotation.Duration = factory.Duration;
            rotation.DelayTime = factory.Delay;

            foreach (var kf in rotationGroup.GetKeyFrames())
                rotation.InsertExpressionKeyFrame(kf.Progress, kf.Value, kf.Easing);

            return rotation;
        }

        public Vector3DKeyFrameAnimation CreateImplicitOffset(ImplicitOffsetPropertyGroup offsetGroup)
        {
            var offset = factory.Compositor.CreateVector3DKeyFrameAnimation();
            offset.Target = nameof(CompositionVisual.Offset);
            offset.Duration = factory.Duration;
            offset.DelayTime = factory.Delay;

            foreach (var kf in offsetGroup.GetKeyFrames())
                offset.InsertExpressionKeyFrame(kf.Progress, kf.Value, kf.Easing);

            return offset;
        }

        public QuaternionKeyFrameAnimation CreateImplicitOrientation(ImplicitOrientationPropertyGroup orientationGroup)
        {
            var orientation = factory.Compositor.CreateQuaternionKeyFrameAnimation();
            orientation.Target = nameof(CompositionVisual.Orientation);
            orientation.Duration = factory.Duration;
            orientation.DelayTime = factory.Delay;

            foreach (var kf in orientationGroup.GetKeyFrames())
                orientation.InsertExpressionKeyFrame(kf.Progress, kf.Value, kf.Easing);

            return orientation;
        }

        //public Vector3DKeyFrameAnimation CreateOffset(OffsetPropertyGroup offsetGroup)
        //{
        //    var offset = factory.Compositor.CreateVector3DKeyFrameAnimation();
        //    offset.Target = "Offset";
        //    offset.Duration = factory.Duration;
        //    offset.DelayTime = factory.Delay;
        //    //quaternion.IterationBehavior = AnimationIterationBehavior.Forever;

        //    foreach (var kf in offsetGroup.OffsetKeyFrames)
        //        if (kf.Easing is not null)
        //            offset.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Offset, kf.Easing);
        //        else
        //            offset.InsertKeyFrame(kf.GetProgress(factory.Duration), kf.Offset);

        //    return offset;
        //}
    }
}
