using Avalonia;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Offset;

public interface IImplicitOffsetPropertyGroupPointerMap
{
    Vector3D Map(in PointerFrame frame);
    Vector3D Stop(in PointerFrame frame);
}
