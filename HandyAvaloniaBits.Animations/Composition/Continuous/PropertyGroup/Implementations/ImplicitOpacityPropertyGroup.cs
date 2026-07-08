using Avalonia.Rendering.Composition;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Opacity;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.Implementations;

public sealed record ImplicitOpacityPropertyGroup : ImplicitCompositionPropertyGroup
{
    public required IImplicitOpacityPropertyGroupPointerMap PointerMap { get; init; }

    public override void Update(ref CompositionVisual visual, in PointerFrame frame) =>
        visual.Opacity = PointerMap.Map(in frame);
    public override void Stop(ref CompositionVisual visual, in PointerFrame frame) =>
        visual.Opacity = PointerMap.Stop(in frame);
}
