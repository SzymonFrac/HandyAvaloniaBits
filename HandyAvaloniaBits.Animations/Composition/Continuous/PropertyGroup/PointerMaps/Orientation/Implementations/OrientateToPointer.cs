using Avalonia;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using System.Numerics;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Orientation.Implementations;

public sealed class OrientateToPointer : IImplicitOrientationProertyGroupPointerMap
{
    private readonly Vector2 _center;
    private readonly Vector2 _size;
    private readonly float _maxTilt = 12;
    public Point Center
    {
        get;
        init
        {
            _center = new Vector2((float)value.X, (float)value.Y);
            field = value;
        }
    }
    public Point Size
    {
        get;
        init
        {
            _size = new Vector2((float)value.X, (float)value.Y) / 2;
            field = value;
        }
    }
    public float MaxTiltDegrees
    { 
        get;
        init
        {
            _maxTilt = value * MathF.PI / 180f;
            field = value;
        }
    }

    public Quaternion Map(in PointerFrame frame)
    {
        var delta = frame.Vector - _center;

        var n = delta / _size;
        n = Vector2.Clamp(n, -Vector2.One * 5, Vector2.One * 5);

        var rot = n * _maxTilt;

        var qx = Quaternion.CreateFromAxisAngle(Vector3.UnitX, rot.Y);
        var qy = Quaternion.CreateFromAxisAngle(Vector3.UnitY, -rot.X);

        return Quaternion.Normalize(qy * qx);
    }

    public Quaternion Stop(in PointerFrame frame) => Quaternion.Identity;
}
