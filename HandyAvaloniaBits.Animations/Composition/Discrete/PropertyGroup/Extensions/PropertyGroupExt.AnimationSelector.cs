using Avalonia.Rendering.Composition.Animations;
using HandyAvaloniaBits.Animations.Composition.Discrete.PropertyGroup.Implementations;
using HandyAvaloniaBits.Animations.Composition.Factory;
using HandyAvaloniaBits.Animations.Composition.Factory.Extensions;
using System.Diagnostics;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.PropertyGroup.Extensions;

public static partial class PropertyGroupExt
{
    extension(CompositionPropertyGroup propertyGroup)
    {
        public CompositionAnimation CreateAnimation(in ICompositionAnimationFactory animationFactory) => propertyGroup switch
        {
            ScalarPropertyGroup scalarGroup => animationFactory.CreateScalar(scalarGroup),
            RotationPropertyGroup rotationGroup => animationFactory.CreateRotation(rotationGroup),
            ScalePropertyGroup scaleGroup => animationFactory.CreateScale(scaleGroup),
            QuaternionPropertyGroup quaternionGroup => animationFactory.CreateOrientation(quaternionGroup),
            OffsetPropertyGroup booleanGroup => animationFactory.CreateOffset(booleanGroup), //?
            _ => throw new UnreachableException()
        };
    }
}
