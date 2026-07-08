using Avalonia;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Ext;

internal static partial class SizeExt
{
    extension(Size from)
    {
        public MorphSizeLerp LerpTo(Size to) => (in t) => from + (to - from) * t;
    }
}
