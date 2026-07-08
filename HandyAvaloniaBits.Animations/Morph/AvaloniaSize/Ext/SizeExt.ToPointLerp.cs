using Avalonia;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;
using System.Diagnostics;

namespace HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Ext;

internal static partial class SizeExt
{
    extension(MorphSizeLerp size)
    {
        public MorphPointLerp ToPoint() => (in t) => size(in t) is Size s ? new(s.Width, s.Height) : throw new UnreachableException();
    }
}
