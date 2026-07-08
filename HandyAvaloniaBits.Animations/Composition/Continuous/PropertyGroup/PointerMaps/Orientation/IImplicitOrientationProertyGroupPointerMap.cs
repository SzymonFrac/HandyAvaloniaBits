using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using System.Numerics;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Orientation;

public interface IImplicitOrientationProertyGroupPointerMap
{
    Quaternion Map(in PointerFrame frame);
    Quaternion Stop(in PointerFrame frame);
}
