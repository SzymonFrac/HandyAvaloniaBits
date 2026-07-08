using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using static Avalonia.Input.InputElement;
using static Avalonia.Interactivity.RoutingStrategies;
using static Avalonia.Rendering.RendererDebugOverlays;

namespace HandyAvaloniaBits.Diagnostics.App.Builder.Extensions;

public static partial class AnimationExt
{
    extension(AppBuilder builder)
    {
        public AppBuilder UseAlternativeProfiler() => builder.AfterSetup(_ =>
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            desktop.Startup += (_, _) => desktop.MainWindow!.AddHandler(KeyDownEvent, AlternateDebugOverlay, Tunnel);
        });
    }

    private static void AlternateDebugOverlay(object? sender, KeyEventArgs e)
    {
        if (e is not { Key: Key.F12, KeyModifiers: KeyModifiers.Alt })
            return;

        var window = (sender as Window)!;
        window.RendererDiagnostics.DebugOverlays = window.RendererDiagnostics.DebugOverlays switch
        {
            None => Fps,
            Fps => Fps | LayoutTimeGraph,
            Fps | LayoutTimeGraph => Fps | LayoutTimeGraph | RenderTimeGraph,
            Fps | LayoutTimeGraph | RenderTimeGraph => Fps | LayoutTimeGraph | RenderTimeGraph | DirtyRects,
            _ => None
        };
    }
}
