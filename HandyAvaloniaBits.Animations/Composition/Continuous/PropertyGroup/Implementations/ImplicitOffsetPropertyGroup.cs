using Avalonia.Rendering.Composition;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Offset;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.Implementations;

public sealed record ImplicitOffsetPropertyGroup : ImplicitCompositionPropertyGroup
{
    public required IImplicitOffsetPropertyGroupPointerMap PointerMap { get; init; }

    public override void Update(ref CompositionVisual visual, in PointerFrame frame) =>
        visual.Offset = PointerMap.Map(in frame);
    public override void Stop(ref CompositionVisual visual, in PointerFrame frame) =>
        visual.Offset = PointerMap.Stop(in frame);
}
