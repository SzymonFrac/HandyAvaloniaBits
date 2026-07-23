namespace HandyAvaloniaBits.Controls.Dialog.Desktop.Context.Factory;

public interface IDesktopDialogContextFactory<TContext, TResult>
    where TContext : DesktopDialogContext<TResult>
{
    TContext Create();
}