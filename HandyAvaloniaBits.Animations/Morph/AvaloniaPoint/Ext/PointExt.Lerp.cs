using Avalonia;
using HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Lerp;

namespace HandyAvaloniaBits.Animations.Morph.AvaloniaPoint.Ext;

internal static partial class PointExt
{
    extension(Point from)
    {
        public MorphPointLerp LerpTo(Point to) => (in t) => from + (to - from) * t;
    }
}
