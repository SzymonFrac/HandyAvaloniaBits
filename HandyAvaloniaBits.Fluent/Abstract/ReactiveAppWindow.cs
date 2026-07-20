using Avalonia;
using FluentAvalonia.UI.Windowing;
using ReactiveUI;

namespace HandyAvaloniaBits.Fluent.Abstract;

public abstract class ReactiveAppWindow<TAppWindow, TViewModel> : FAAppWindow, IViewFor<TViewModel>
    where TAppWindow : FAAppWindow, IViewFor<TViewModel>
    where TViewModel : ReactiveObject
{
    public static readonly StyledProperty<TViewModel?> ViewModelProperty = AvaloniaProperty.Register<TAppWindow, TViewModel?>(nameof(ViewModel));

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
