using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using HandyAvaloniaBits.PageTransitions.Models;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Implementations;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.GridShapeCutoutLerp.Abstract;
using HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.GridShapeCutoutLerp.Implementations;
using System.Diagnostics;

namespace HandyAvaloniaBits.PageTransitions;

public sealed class ShapeCutoutWaveTransition : IPageTransition
{
    private TimeSpan? frameTime;

    public Geometry Shape { get; init; } = new EllipseGeometry { RadiusX = 2, RadiusY = 2 };


    public TimeSpan TransitionDuration { get; init; } = TimeSpan.FromSeconds(1);
    public uint TargetFPS { get; init; } = 100;
    public double CutoutFillRatio { get; init; } = .25;
    public TimeSpan FrameTime => frameTime ??= TimeSpan.FromSeconds(1.0 / TargetFPS);

    public Easing Easing { get; init; } = Easing.Parse("0,0 1,1");

    private int TotalFrames => int.CreateTruncating(TransitionDuration.TotalSeconds * TargetFPS);
    public int CutoutFillFrames => int.CreateTruncating(CutoutFillRatio * TotalFrames);

    public Rect DestinationRect { get; init; } = new(new(200, 200));

    public Corner TransitionFrom { get; init; } = Corner.BottomRight;

    public bool Bidirectional { get; init; } = true;
    public TimeSpan? MidwayDelay { get; init; }

    public bool WaveRotate { get; init; } = false;

    public async Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken = default)
    {
        if ((from?.GetVisualParent(), to?.GetVisualParent()) is not (Panel parentFrom, Panel parentTo))
            return;


        Stopwatch stopwatch;
        double t;

        if (Bidirectional)
        {
            // transition from

            from.IsVisible = true;
            to.IsVisible = false;

            using var rtbFrom = new RenderTargetBitmap(PixelSize.FromSize(parentTo.Bounds.Size, 1));
            var opacityMaskFrom = new ImageBrush(rtbFrom)
            {
                Stretch = Stretch.Fill,
                SourceRect = new(0, 0, 1, 1, RelativeUnit.Relative)
            };


            var parentFromOpacityMask = parentFrom.OpacityMask;
            parentFrom.OpacityMask = opacityMaskFrom;


            var gridFrom = new GridShapeCutoutGrid
            {
                WindowBounds = parentTo.Bounds,
                DestinationRect = DestinationRect,
                Corner = TransitionFrom,
                Shape = Shape,
                TotalFrames = TotalFrames,
                FillCutoutFrames = CutoutFillFrames,
                RotateAngleRad = WaveRotate ? Math.Tau : 0
            };


            stopwatch = Stopwatch.StartNew();
            t = 0;
            while (t <= 1)
            {
                t = stopwatch.Elapsed.TotalSeconds / TransitionDuration.TotalSeconds;
                var rt = 1 - t;
                var eased = Easing.Ease(rt);

                using (var dc = rtbFrom.CreateDrawingContext(true))
                    gridFrom.Update(in eased, in dc);

                await Task.Delay(FrameTime, cancellationToken);
            }


            if (MidwayDelay is not null)
                await Task.Delay(MidwayDelay.Value, cancellationToken);

            parentFrom.OpacityMask = parentFromOpacityMask;
        }


        // transition to

        from.IsVisible = false;
        to.IsVisible = true;

        using var rtbTo = new RenderTargetBitmap(PixelSize.FromSize(parentTo.Bounds.Size, 1));
        var opacityMaskTo = new ImageBrush(rtbTo)
        {
            Stretch = Stretch.Fill,
            SourceRect = new RelativeRect(0, 0, 1, 1, RelativeUnit.Relative)
        };


        var parentOpacityMask = parentTo.OpacityMask;
        parentTo.OpacityMask = opacityMaskTo;


        var gridTo = new GridShapeCutoutGrid
        {
            WindowBounds = parentTo.Bounds,
            DestinationRect = DestinationRect,
            Corner = TransitionFrom,
            Shape = Shape,
            TotalFrames = TotalFrames,
            FillCutoutFrames = CutoutFillFrames,
            RotateAngleRad = WaveRotate ? -Math.Tau : 0
        };


        stopwatch = Stopwatch.StartNew();
        t = 0;
        while (t <= 1)
        {
            t = stopwatch.Elapsed.TotalSeconds / TransitionDuration.TotalSeconds;
            var eased = Easing.Ease(t);

            using (var dc = rtbTo.CreateDrawingContext())
                gridTo.Update(in eased, in dc);

            await Task.Delay(FrameTime, cancellationToken);
        }

        parentTo.OpacityMask = parentOpacityMask;
    }



    private sealed class GridShapeCutoutGrid
    {
        private System.Drawing.Point? sizeInCutouts;
        private AbstractGridShapeCutoutLerpFactory? factory;

        private IEnumerable<GridShapeCutout>? cutouts;

        public required Geometry Shape { get; init; }

        public required Rect WindowBounds { get; init; }
        public required Rect DestinationRect { get; init; }

        public required Corner Corner { get; init; }

        public required int TotalFrames { get; init; }
        public required int FillCutoutFrames { get; init; }

        public double RotateAngleRad { get; init; } = 0;


        public System.Drawing.Point SizeInCutouts => sizeInCutouts ??= new(
            (int)Math.Ceiling(WindowBounds.Width / DestinationRect.Width),
            (int)Math.Ceiling(WindowBounds.Height / DestinationRect.Height));

        public AbstractGridShapeCutoutLerpFactory Factory => factory ??= Corner switch
        {
            Corner.TopLeft => new TopLeftGridShapeCutoutLerpFactory
            {
                WindowBounds = WindowBounds,
                DestinationRect = DestinationRect,
                TotalFrames = TotalFrames,
                FillCutoutFrames = FillCutoutFrames,
                SumOfGridDimensions = SizeInCutouts.X + SizeInCutouts.Y,
                RotateAngleRad = RotateAngleRad
            },
            Corner.TopRight => new TopRightGridShapeCutoutLerpFactory
            {
                WindowBounds = WindowBounds,
                DestinationRect = DestinationRect,
                TotalFrames = TotalFrames,
                FillCutoutFrames = FillCutoutFrames,
                SumOfGridDimensions = SizeInCutouts.X + SizeInCutouts.Y,
                RotateAngleRad = RotateAngleRad
            },
            Corner.BottomLeft => new BottomLeftGridShapeCutoutLerpFactory
            {
                WindowBounds = WindowBounds,
                DestinationRect = DestinationRect,
                TotalFrames = TotalFrames,
                FillCutoutFrames = FillCutoutFrames,
                SumOfGridDimensions = SizeInCutouts.X + SizeInCutouts.Y,
                RotateAngleRad = RotateAngleRad
            },
            Corner.BottomRight => new BottomRightGridShapeCutoutLerpFactory
            {
                WindowBounds = WindowBounds,
                DestinationRect = DestinationRect,
                TotalFrames = TotalFrames,
                FillCutoutFrames = FillCutoutFrames,
                SumOfGridDimensions = SizeInCutouts.X + SizeInCutouts.Y,
                RotateAngleRad = RotateAngleRad
            },
            _ => throw new UnreachableException()
        };


        private IEnumerable<GridShapeCutout> GetCutouts() =>
            Enumerable.Range(0, SizeInCutouts.X)
                .SelectMany(x => Enumerable.Range(0, SizeInCutouts.Y).Select(y => new Point(x, y)))
                .Select(relativePoint =>
                {
                    var lerpFunc = Factory.Create(in relativePoint);
                    return GridShapeCutout.Create(Shape, lerpFunc);
                });

        public void Update(in double t, in DrawingContext dc)
        {
            foreach (var cutout in cutouts ??= GetCutouts())
                cutout.Update(in t, in dc);
        }
    }
}
