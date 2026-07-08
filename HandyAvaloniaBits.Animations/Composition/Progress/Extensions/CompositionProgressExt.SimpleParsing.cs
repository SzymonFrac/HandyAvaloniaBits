using HandyAvaloniaBits.Animations.Composition.Progress;

namespace HandyAvaloniaBits.Animations.Composition.Progress.Extensions;

public static partial class CompositionProgressExt
{
    extension(CompositionProgress)
    {
        public static float? TrySimpleCueParse(ReadOnlySpan<char> percentage) =>
            percentage.EndsWith('%') ?
                float.Parse(percentage.TrimEnd('%')) / 100 :
                null;
        public static float SimpleCueParse(ReadOnlySpan<char> percentage) => float.Parse(percentage.TrimEnd('%')) / 100;

        public static float? TrySimpleTimeParse(ReadOnlySpan<char> expression, in TimeSpan animationDuration) =>
            TimeSpan.TryParse(expression, out var time) ?
                (float)time.Divide(animationDuration) :
                null;
        public static float SimpleTimeParse(ReadOnlySpan<char> expression, in TimeSpan animationDuration) =>
            (float)TimeSpan.Parse(expression).Divide(animationDuration);
    }
}
