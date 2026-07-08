using Avalonia.Animation.Easings;
using HandyAvaloniaBits.Animations.Composition.Progress;
using HandyAvaloniaBits.Animations.Composition.Progress.Extensions;

namespace HandyAvaloniaBits.Animations.Composition.Discrete.KeyFrame;

public abstract record CompositionKeyFrame
{
    public required string Time
    { 
        get;
        init
        {
            field = value;
            progressCache = CompositionProgress.TrySimpleCueParse(value);
        }
    }
    public Easing? Easing { get; init; }

    private float? progressCache;
    public float GetProgress(in TimeSpan duration) => progressCache ??= CompositionProgress.SimpleTimeParse(Time, duration);
};