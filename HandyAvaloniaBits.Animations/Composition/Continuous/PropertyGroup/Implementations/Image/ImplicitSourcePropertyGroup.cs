using Avalonia.Controls;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Image.Source;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.Implementations.Image;

public sealed record ImplicitSourcePropertyGroup : ImplicitControlPorpertyGroup
{
    public required IImplicitSourcePropertyGroupPointerMap PointerMap { get; init; }

    public override void Update(ref Control control, in PointerFrame frame) =>
        ((Avalonia.Controls.Image)control).Source = PointerMap.Map(in frame);
    public override void Stop(ref Control control, in PointerFrame frame) =>
        ((Avalonia.Controls.Image)control).Source = PointerMap.Stop(in frame);
}
