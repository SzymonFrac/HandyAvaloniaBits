namespace HandyAvaloniaBits.Animations.Composition.Discrete.KeyFrame.Implementations;

public sealed record RotationKeyFrame : CompositionKeyFrame
{
    public required float Degrees { get; init; }
    internal float Radians => Degrees * (MathF.PI / 180);
}
