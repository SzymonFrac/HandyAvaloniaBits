using Avalonia.Controls;
using Avalonia.Interactivity;
using HandyAvaloniaBits.Controls.Dialog.Desktop.Context;
using HandyAvaloniaBits.Controls.Dialog.Desktop.Context.Factory;

namespace HandyAvaloniaBits.Controls.Dialog.Desktop;

public abstract class DesktopDialog<TContext, TResult> : UserControl, IDisposable
    where TContext : DesktopDialogContext<TResult>, new()
{
    protected Window Dialog => field ??= new Window
    {
        Title = Title,
        Content = this,
        DataContext = Context,
        Width = Width,
        Height = Height,
        WindowStartupLocation = WindowStartupLocation.CenterOwner,
        CanMinimize = false,
        CanMaximize = false,
        CanResize = false,
        ShowInTaskbar = false
    };
    protected TContext Context => field ??= ContextFactory?.Create() ?? new();
    protected IDesktopDialogContextFactory<TContext, TResult>? ContextFactory { get; init; }

    protected string Title { get; set; } = string.Empty;



    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        Context.Close.Task.ContinueWith(t => Dialog.Close(t.Result), TaskScheduler.FromCurrentSynchronizationContext());
    }

    public Task<TResult> Show(Window owner) => Dialog.ShowDialog<TResult>(owner);


    public void Dispose()
    {
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}
