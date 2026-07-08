using Avalonia.Media;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Image.Source;

public interface IImplicitSourcePropertyGroupPointerMap
{
    IImage Map(in PointerFrame frame);
    IImage Stop(in PointerFrame frame);
}
