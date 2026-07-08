using Avalonia;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;
using HandyAvaloniaBits.Animations.Morph.AvaloniaSize.Lerp;
using System.Diagnostics;

namespace AvaloniaAnimationExtensions.Animations.Morph.AvaloniaPoint.Ext
{
    internal static partial class PointExt
    {
        extension(MorphPointLerp point)
        {
            public MorphSizeLerp ToSize() => (in t) => point(t) is Point s ? new(s.X, s.Y) : throw new UnreachableException();
        }
    }
}
