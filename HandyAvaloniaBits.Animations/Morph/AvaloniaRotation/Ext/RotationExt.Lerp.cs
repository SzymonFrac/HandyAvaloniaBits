using HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.AvaloniaRotation.Ext;

internal static partial class RotationExt
{
    extension(double from)
    {
        public MorphRotationLerp LerpTo(double to) => (in t) => from + (to - from) * t;
    }
}
