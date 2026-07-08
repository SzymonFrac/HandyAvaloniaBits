using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Frame;
using ReactiveUI;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;

namespace HandyAvaloniaBits.Animations.Composition.Continuous.Pointer.Bound;

public sealed class PointerAnimationBound
{
    public required Shape Bounds
    {
        get;
        init
        {
            OnPointerEntered = Observable.FromEventPattern<PointerEventArgs>(
                h => value.PointerEntered += h,
                h => value.PointerEntered -= h)
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Scan(PointerFrame.Zero, (p, c) => p.CreateNext(CurrentVectorFromEventArgs(in c)))
                .Publish()
                .RefCount();

            OnPointerMoved = Observable.FromEventPattern<PointerEventArgs>(
                h => value.PointerMoved += h,
                h => value.PointerMoved -= h)
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Scan(PointerFrame.Zero, (p, c) => p.CreateNext(CurrentVectorFromEventArgs(in c)))
                .Publish()
                .RefCount();

            OnPointerExited = Observable.FromEventPattern<PointerEventArgs>(
                h => value.PointerExited += h,
                h => value.PointerExited -= h)
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Scan(PointerFrame.Zero, (p, c) => p.CreateNext(CurrentVectorFromEventArgs(in c)))
                .Publish()
                .RefCount();

            field = value;
        }
    }

    public required IObservable<PointerFrame> OnPointerEntered { get; init; }
    public required IObservable<PointerFrame> OnPointerMoved { get; init; }
    public required IObservable<PointerFrame> OnPointerExited { get; init; }

    private static Vector2 CurrentVectorFromEventArgs(in EventPattern<PointerEventArgs> e)
    {
        var control = (Control)e.Sender!;
        var p = e.EventArgs.GetPosition(control);
        var v = new Vector2((float)p.X, (float)p.Y);
        return v;
    }
}
