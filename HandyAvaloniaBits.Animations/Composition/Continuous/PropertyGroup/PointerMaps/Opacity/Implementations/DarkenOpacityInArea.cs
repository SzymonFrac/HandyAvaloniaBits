using Avalonia;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using System.Numerics;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Opacity.Implementations;

public sealed class DarkenOpacityInArea : IImplicitOpacityPropertyGroupPointerMap
{
    private readonly Vector2 _min;
    private readonly Vector2 _max;
    private readonly Vector2 _center;
    private readonly Vector2 _halfSize;

    public required Rect Area
    {
        get;
        init
        {
            _min = new Vector2((float)value.X, (float)value.Y);
            _max = new Vector2((float)value.Right, (float)value.Bottom);

            _center = (_min + _max) * 0.5f;
            _halfSize = (_max - _min) * 0.5f;

            field = value;
        }
    }

    public float MinOpacity { get; init; } = 0f;
    public float MaxOpacity { get; init; } = 1f;

    // optimise some time mabey.
    public float Map(in PointerFrame frame)
    {
        var p = frame.Vector;

        // Outside area
        if (p.X < _min.X || p.X > _max.X || p.Y < _min.Y || p.Y > _max.Y)
            return MinOpacity;

        // normalized position relative to center
        var local = (p - _center);

        float nx = MathF.Abs(local.X) / _halfSize.X;
        float ny = MathF.Abs(local.Y) / _halfSize.Y;

        float t = 1f - Math.Clamp(MathF.Max(nx, ny), 0f, 1f);

        return MinOpacity + t * (MaxOpacity - MinOpacity);
    }

    public float Stop(in PointerFrame frame)
        => (MinOpacity + MaxOpacity) * 0.5f;
}