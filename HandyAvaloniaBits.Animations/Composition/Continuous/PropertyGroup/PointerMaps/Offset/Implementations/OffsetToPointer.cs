using Avalonia;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using System.Numerics;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Offset.Implementations;

internal sealed class OffsetToPointer : IImplicitOffsetPropertyGroupPointerMap
{
    private readonly Vector2 _center;
    private readonly float _multiplier;
    public Point Center
    {
        get;
        init
        {
            _center = new Vector2((float)value.X, (float)value.Y);
            field = value;
        }
    }
    public float Multiplier
    {
        get;
        init
        {
            _multiplier = value;
            field = value;
        }
    }
    public Vector3D Map(in PointerFrame frame)
    {
        var direction = frame.Vector - _center;
        var scaledDirection = direction * _multiplier;

        var res = new Vector3D(scaledDirection.X, scaledDirection.Y, 0);
        return res;
    }

    public Vector3D Stop(in PointerFrame frame) => Vector3.Zero;
}
