using Avalonia;

namespace HandyAvaloniaBits.PageTransitions.Models.ShapeCutout.Lerp.Factory.GridShapeCutoutLerp.Abstract;

internal abstract class AbstractGridShapeCutoutLerpFactory
{
    private double? timeScale;
    private int? frameLastCutoutStarts;
    private int? gridRadiusScaleFrameDifference;

    public required Rect WindowBounds { get; init; }
    public required Rect DestinationRect { get; init; }

    public required int TotalFrames { get; init; }
    public required int FillCutoutFrames { get; init; }

    public double TimeScale => timeScale ??= (double)FillCutoutFrames / TotalFrames;
    public int FrameLastCutoutStarts => frameLastCutoutStarts ??= TotalFrames - FillCutoutFrames;


    public required int SumOfGridDimensions { get; set; }

    public int GridRadiusScaleFrameDifference => gridRadiusScaleFrameDifference ??= FrameLastCutoutStarts / SumOfGridDimensions;


    public double RotateAngleRad { get; init; } = 0;



    public abstract Point GetTransformPosition(in Point relativePositionInGrid);

    public LerpShapeCutout Create(in Point relativePositionInGrid)
    {
        var transformPosition = GetTransformPosition(relativePositionInGrid);


        var gridDistanceFromStart = relativePositionInGrid.X + relativePositionInGrid.Y;

        var startFrame = GridRadiusScaleFrameDifference * gridDistanceFromStart;
        var relativeStart = startFrame / TotalFrames;

        var radiusLerp = FrameRadiusLerp.Create(DestinationRect.Width, in relativeStart, TimeScale);

        var angle = relativeStart * RotateAngleRad;

        return (in t) =>
        {
            var radius = radiusLerp.LerpRadius(in t);
            return new Matrix(radius, 0, 0, radius, transformPosition.X, transformPosition.Y)
                * Matrix.CreateRotation(angle, transformPosition);
        };
    }



    private readonly record struct FrameRadiusLerp
    {
        private readonly double finalRadius;

        private readonly double relativeStart;
        private readonly double timeScale;

        private FrameRadiusLerp(double fr, double rs, double ts) =>
            (finalRadius, relativeStart, timeScale) = (fr, rs, ts);

        public static FrameRadiusLerp Create(in double finalRadius, in double relativeStart, in double timeScale) =>
            new(finalRadius, relativeStart, timeScale);

        public readonly double LerpRadius(in double t)
        {
            var rt = (t - relativeStart) / timeScale;
            return rt switch
            {
                <= 0 => 0,
                >= 1 => finalRadius,
                _ => rt * finalRadius
            };
        }
    }
}
