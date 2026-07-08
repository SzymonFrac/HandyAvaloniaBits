using Avalonia.Rendering.Composition.Animations;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.Implementations;
using HandyAvaloniaBits.Animations.Composition.Factory;
using HandyAvaloniaBits.Animations.Composition.Factory.Extensions;
using System.Diagnostics;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.Extensions;

public static partial class ImplicitPropertyGroupExt
{
    extension(ImplicitCompositionPropertyGroup implicitPropertyGroup)
    {
        public CompositionAnimation CreateImplicitAnimation(in ICompositionAnimationFactory animationFactory) => implicitPropertyGroup switch
        {
            ImplicitRotationPropertyGroup rotationGroup => animationFactory.CreateImplicitRotation(rotationGroup),
            ImplicitOffsetPropertyGroup offsetGroup => animationFactory.CreateImplicitOffset(offsetGroup),
            ImplicitOrientationPropertyGroup orientationGroup => animationFactory.CreateImplicitOrientation(orientationGroup),
            ImplicitOpacityPropertyGroup opacityGroup => animationFactory.CreateImplicitOpacity(opacityGroup),
            _ => throw new UnreachableException()
        };
    }
}
