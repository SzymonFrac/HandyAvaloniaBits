using Avalonia;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using System.Numerics;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Opacity.Implementations;

public sealed class DarkenOpacityToOrientation : IImplicitOpacityPropertyGroupPointerMap
{
    private readonly Vector2 _start;
    private readonly Vector2 _end;
    private Vector2 _direction;
    private float _lengthSq;
    public Point End
    {
        get;
        init
        {
            _end = new Vector2((float)value.X, (float)value.Y);
            
            _direction = _end - _start;
            _lengthSq = _direction.LengthSquared();

            field = value;
        }
    }
    public Point Start
    {
        get;
        init
        {
            _start = new Vector2((float)value.X, (float)value.Y);

            _direction = _end - _start;
            _lengthSq = _direction.LengthSquared();

            field = value;
        }
    }

    public float MinOpacity { get; init; } = 0f;
    public float MaxOpacity { get; init; } = 1f;

    public float Map(in PointerFrame frame)
    {
        var relative = frame.Vector - _start;

        float t = Vector2.Dot(relative, _direction) / _lengthSq;

        t = Math.Clamp(t, 0f, 1f);

        return MinOpacity + t * (MaxOpacity - MinOpacity);
    }

    public float Stop(in PointerFrame frame) => (MinOpacity + MaxOpacity) * 0.5f;
}
