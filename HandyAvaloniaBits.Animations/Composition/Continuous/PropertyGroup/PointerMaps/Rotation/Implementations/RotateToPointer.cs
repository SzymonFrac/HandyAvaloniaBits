using Avalonia;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using System.Numerics;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.PropertyGroup.PointerMaps.Rotation.Implementations;

public sealed class RotateToPointer : IImplicitRotationPropertyGroupPointerMap
{
    private readonly Vector2 _center;
    private float rotation = 0;
    private Vector2 previousDirection = Vector2.Zero;
    public Point Center
    { 
        get;
        init
        {
            _center = new Vector2((float)value.X, (float)value.Y);
            field = value;
        }
    }
    public float Offset
    {
        get;
        init
        {
            var rad = value * (float.Pi / 180);
            previousDirection = new(float.Cos(rad), float.Sin(rad));
            field = rad;
        }
    }

    public float Map(in PointerFrame frame)
    {
        var direction = frame.Vector - _center;
        
        var cross = Vector2.Cross(previousDirection, direction);
        var dot = Vector2.Dot(previousDirection, direction);

        previousDirection = direction;

        rotation += MathF.Atan2(cross, dot);
        return rotation;
    }
    public float Stop(in PointerFrame pointer) =>
        float.Tau * MathF.Round(rotation / float.Tau);
}
