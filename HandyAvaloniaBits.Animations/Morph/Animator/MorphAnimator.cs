using Avalonia.Animation;
using Avalonia.Media;

namespace HandyAvaloniaBits.Animations.Morph.Animator;

public abstract class MorphAnimator : InterpolatingAnimator<Geometry>
{
    internal abstract void Register();
}
