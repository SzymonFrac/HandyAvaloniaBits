using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Opacity;

public interface IImplicitOpacityPropertyGroupPointerMap
{
    float Map(in PointerFrame frame);
    float Stop(in PointerFrame frame);
}
