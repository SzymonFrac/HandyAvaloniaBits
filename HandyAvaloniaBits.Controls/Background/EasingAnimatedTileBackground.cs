using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using DynamicData.Binding;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace HandyAvaloniaBits.Controls.Background;

public sealed class EasingAnimatedTileBackground : ContentControl
{
    private readonly CompositeDisposable _subscriptions = new(2);

    private readonly Size _tileSize;
    private readonly Point _tileDimensions;

# pragma warning disable CS0649
    private readonly Point _totalDirection;
# pragma warning restore

    private Point animationOffset;

    public required TimeSpan TotalCycleTime { get; init; } = TimeSpan.FromSeconds(1);
    public required TimeSpan UpdateSpeed { get; init; } = TimeSpan.FromMilliseconds(16);
    public required DrawingBrush TileBrush
    {
        get;
        init
        {
            field = value;
            _tileSize = value.SourceRect.Rect.Size;
            _tileDimensions = value.SourceRect.Rect.BottomRight;
        }
    }


    public static readonly StyledProperty<Point> DirectionProperty = AvaloniaProperty.Register<AnimatedTileBackground, Point>(nameof(Direction), new(1, 1));
    public static readonly StyledProperty<KeySpline> EasingProperty = AvaloniaProperty.Register<AnimatedTileBackground, KeySpline>(nameof(Easing), new KeySpline(0, 0, 1, 1));
    public Point Direction
    {
        get => GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }
    public KeySpline Easing
    {
        get => GetValue(EasingProperty);
        set => SetValue(EasingProperty, value);
    }



    public override void Render(DrawingContext context)
    {
        context.PushTransform(new Matrix(1, 0, 0, 1, animationOffset.X, animationOffset.Y));
        context.FillRectangle(TileBrush, new Rect(Bounds.TopLeft - _tileDimensions, Bounds.Size + _tileSize * 2));
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        this.WhenValueChanged(o => o.Direction)
            .DistinctUntilChanged()
            .Select(d => new Point(
                _tileDimensions.X * d.X,
                _tileDimensions.Y * d.Y))
            .BindTo(this, o => o._totalDirection)
            .DisposeWith(_subscriptions);

        Observable.Interval(UpdateSpeed, RxSchedulers.MainThreadScheduler)
            .TimeInterval()
            .Select(t => t.Interval / TotalCycleTime)
            .Select(Easing.GetSplineProgress)
            .Subscribe(UpdateAnimation)
            .DisposeWith(_subscriptions);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _subscriptions.Dispose();
    }

    private void UpdateAnimation(double dt)
    {
        animationOffset += _totalDirection * dt;

        animationOffset = new(
            animationOffset.X % _tileDimensions.X,
            animationOffset.Y % _tileDimensions.Y);

        InvalidateVisual();
    }
}
