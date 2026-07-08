using Avalonia.Controls;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup;

public abstract record ImplicitControlPorpertyGroup
{
    public abstract void Update(ref Control control, in PointerFrame frame);
    public virtual void Stop(ref Control control, in PointerFrame frame) { }
}
