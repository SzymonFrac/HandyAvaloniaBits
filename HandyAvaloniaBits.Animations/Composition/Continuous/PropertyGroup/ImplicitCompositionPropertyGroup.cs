using Avalonia.Rendering.Composition;
using HandyAvaloniaBits.Animations.Composition.Continuous.KeyFrame;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup;

public abstract record ImplicitCompositionPropertyGroup
{
    public IList<ImplicitCompositionKeyFrame> KeyFrames { get; } = [];
    public IEnumerable<ImplicitCompositionKeyFrame> GetKeyFrames() =>
        KeyFrames is { Count: > 0 }
            ? KeyFrames
            : [new ImplicitCompositionKeyFrame { Progress = 1, Value = "this.FinalValue" }];

    public abstract void Update(ref CompositionVisual visual, in PointerFrame frame);
    public virtual void Stop(ref CompositionVisual visual, in PointerFrame frame) { }
};
