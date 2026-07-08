using HandyAvaloniaBits.Animations.Composition.Progress.Extensions;

namespace HandyAvaloniaBits.Animations.Composition.Progress;

public readonly record struct CompositionProgress
{
    public float Value { get; }
    private CompositionProgress(float value) => Value = value;

    public static CompositionProgress Create(float value) => new(Normalize(value));

    public static CompositionProgress Parse(ReadOnlySpan<char> expression, TimeSpan duration) =>
        Create(CompositionProgress.TrySimpleCueParse(expression) ??
            CompositionProgress.SimpleTimeParse(expression, duration));

    private static float Normalize(float value) => Math.Clamp(value, 0, 1);
}