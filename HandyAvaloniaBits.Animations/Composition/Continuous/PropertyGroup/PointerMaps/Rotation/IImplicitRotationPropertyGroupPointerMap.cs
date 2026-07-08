using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Rotation;

public interface IImplicitRotationPropertyGroupPointerMap
{
    float Map(in PointerFrame pointer);
    float Stop(in PointerFrame pointer);
}
