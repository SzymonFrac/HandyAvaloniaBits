using Avalonia;
using System.Numerics;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.KeyFrame.Implementations;

public sealed record QuaternionKeyFrame : CompositionKeyFrame
{
    public required Vector3D? Direction { get; init; }
    public float Angle { get; init; } = 1;

    public Quaternion Quaternion => Direction is Vector3D v
        ? Quaternion.CreateFromAxisAngle(Vector3.Normalize(new((float)v.X, (float)v.Y, (float)v.Z)), Angle)
        : Quaternion.Identity;
}
