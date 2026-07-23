using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;

namespace HandyAvaloniaBits.Controls.Dialog.Desktop.Context;

public abstract class DesktopDialogContext<TResult> : ReactiveObject, IDisposable
{
    protected readonly CompositeDisposable _disposables = [];

    public TaskCompletionSource<TResult> Close { get; } = new();


    protected DesktopDialogContext() => Close.Task.DisposeWith(_disposables);


    public virtual void Dispose()
    {
        _disposables.Dispose();
        GC.SuppressFinalize(this);
    }
}
