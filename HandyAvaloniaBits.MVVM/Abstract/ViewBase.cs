using Avalonia;
using Avalonia.Controls;
using ReactiveUI;

namespace HandyAvaloniaBits.MVVM.Abstract;

public abstract class ViewBase<TView, TViewModel> : UserControl, IViewFor<TViewModel>
    where TView : UserControl, IViewFor<TViewModel>
    where TViewModel : ViewModelBase
{
    public static readonly StyledProperty<TViewModel?> ViewModelProperty = AvaloniaProperty.Register<TView, TViewModel?>(nameof(ViewModel));

    public TViewModel? ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TViewModel?)value;
    }
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DataContextProperty)
        {
            if (ReferenceEquals(change.OldValue, ViewModel)
                && change.NewValue is null or TViewModel)
            {
                SetCurrentValue(ViewModelProperty, change.NewValue);
            }
        }
        else if (change.Property == ViewModelProperty)
        {
            if (ReferenceEquals(change.OldValue, DataContext))
            {
                SetCurrentValue(DataContextProperty, change.NewValue);
            }
        }
    }
}
