using Avalonia.Rendering.Composition;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Orientation;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.Implementations;

public sealed record ImplicitOrientationPropertyGroup : ImplicitCompositionPropertyGroup
{
    public required IImplicitOrientationProertyGroupPointerMap PointerMap { get; init; }
    public override void Update(ref CompositionVisual visual, in PointerFrame frame) =>
        visual.Orientation = PointerMap.Map(in frame);
    public override void Stop(ref CompositionVisual visual, in PointerFrame frame) =>
        visual.Orientation = PointerMap.Stop(in frame);
}
