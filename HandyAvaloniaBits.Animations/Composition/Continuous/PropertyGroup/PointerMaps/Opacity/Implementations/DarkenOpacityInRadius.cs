using Avalonia;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using System.Numerics;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Opacity.Implementations;

public sealed class DarkenOpacityInRadius : IImplicitOpacityPropertyGroupPointerMap
{
    private Vector2 _center;

    public Point Center
    {
        get;
        init
        {
            _center = new Vector2((float)value.X, (float)value.Y);
            field = value;
        }
    }

    public required float RadiusX { get; init; }
    public required float RadiusY { get; init; }

    public float MinOpacity { get; init; } = 0f;
    public float MaxOpacity { get; init; } = 1f;

    public float Falloff { get; init; } = 2f;

    public float Map(in PointerFrame frame)
    {
        var p = frame.Vector - _center;

        float nx = p.X / RadiusX;
        float ny = p.Y / RadiusY;

        float dist = nx * nx + ny * ny;

        if (dist >= 1f)
            return MinOpacity;

        float t = 1f - dist;

        t = MathF.Pow(t, Falloff);

        return MinOpacity + t * (MaxOpacity - MinOpacity);
    }

    public float Stop(in PointerFrame frame)
        => (MinOpacity + MaxOpacity) * 0.5f;
}