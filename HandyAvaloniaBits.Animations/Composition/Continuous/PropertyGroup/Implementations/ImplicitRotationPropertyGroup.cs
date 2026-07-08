using Avalonia.Rendering.Composition;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Rotation;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.Implementations;

public sealed record ImplicitRotationPropertyGroup : ImplicitCompositionPropertyGroup
{
    public required IImplicitRotationPropertyGroupPointerMap PointerMap { get; init; }
    public override void Update(ref CompositionVisual visual, in PointerFrame frame) =>
        visual.RotationAngle = PointerMap.Map(in frame);
    public override void Stop(ref CompositionVisual visual, in PointerFrame frame) =>
        visual.RotationAngle = PointerMap.Stop(in frame);
}
