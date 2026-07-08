using System.Numerics;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;

public readonly record struct PointerFrame
{
    public static PointerFrame Zero => default;

    public Vector2 Vector { get; }
    public Vector2 Previous { get; }
    public Vector2 Delta { get; }
    public float Force { get; }
    private PointerFrame(in Vector2 vector) =>
        (Vector, Previous, Delta, Force) = (vector, Vector2.Zero, Vector2.Zero, 0);
    private PointerFrame(in Vector2 vector, in Vector2 previous, in Vector2 delta, in float force) =>
        (Vector, Previous, Delta, Force) = (vector, previous, delta, force);

    public static PointerFrame Create(in Vector2 vector) => new(vector);
    public PointerFrame CreateNext(in Vector2 vector) =>
        new(vector, Vector, vector - Vector, Vector2.DistanceSquared(Vector, vector));

    public override string ToString() => $"[{Previous}, {Vector}, {Force}, {Delta}]";
}