using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.VisualTree;
using System.Numerics;

namespace HandyAvaloniaBits.PageTransitions;

[Obsolete($"Use ShapeCutoutBubbleTransition instead.")]
public sealed class BubblePageTransition : IPageTransition
{
    private float lerpFactor;

    public int ExtraBubbleCount { get; init; } = 20;
    public float ExtraBubbleMaxRadius { get; init; } = 50;

    public TimeSpan TotalDuration { get; init; } = TimeSpan.FromSeconds(1);
    public TimeSpan Speed { get; init; } = TimeSpan.FromMilliseconds(10);
    public float StartFillProgress { get; init; } = .2f;
    public float DirectionAngle { get; init; } = 220;
    public float DirectionAngleVariance { get; init; } = 80;

    public async Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
    {
        if (to is null)
            return;
        if (from is null)
            return;

        lerpFactor = (float)(Speed / TotalDuration);

        var parent = (to.GetVisualParent() as Panel)!;

        var overlay = new Avalonia.Controls.Shapes.Path
        {
            Fill = Brushes.Black,
            Data = new RectangleGeometry(parent.Bounds)
        };

        parent.Children.Add(overlay);

        var startBound = new Rect(parent.Bounds.TopRight + new Point(0, parent.Bounds.Height / 2),
            parent.Bounds.BottomRight + new Point(160, 0));

        var fillBubble = FillBubble.Create(parent.Bounds, DirectionAngle, 90,  StartFillProgress);
        var bubbles = Enumerable.Repeat(0, ExtraBubbleCount)
            .Select(_ => Bubble.CreateRandomPreFill(StartFillProgress, startBound, DirectionAngle, DirectionAngleVariance, ExtraBubbleMaxRadius, fillBubble.Radius * 1.5))
            .ToArray();

        to.IsVisible = false;

        await PlayBackward(parent, overlay, fillBubble, bubbles, cancellationToken);

        from.IsVisible = false;
        to.IsVisible = true;

        fillBubble = FillBubble.Create(parent.Bounds, DirectionAngle, 90, StartFillProgress);
        bubbles = [.. Enumerable.Repeat(0, ExtraBubbleCount).Select(_ => Bubble.CreateRandomPreFill(StartFillProgress, startBound, DirectionAngle, DirectionAngleVariance, ExtraBubbleMaxRadius, fillBubble.Radius * 1.5))];

        await PlayForward(parent, overlay, fillBubble, bubbles, cancellationToken);

        parent.Children.Remove(overlay);
    }

    private async Task PlayForward(Panel parent, Avalonia.Controls.Shapes.Path overlay, FillBubble fillBubble, Bubble[] bubbles, CancellationToken cancellationToken = default)
    {
        var rect = new RectangleGeometry(parent.Bounds);
        for (double i = 0; i < StartFillProgress; i += lerpFactor)
        {
            var geometries = bubbles
                .Select(b => b.GetEllipseGeometry(i))
                .ToList();

            var bubbleGeometry = CombineGeometriesBalanced(geometries);

            var final = new CombinedGeometry(GeometryCombineMode.Exclude, rect, bubbleGeometry);
            overlay.Data = final;

            await Task.Delay(Speed, cancellationToken);
        }

        for (double i = StartFillProgress; i < 1; i += lerpFactor)
        {
            var geometries = bubbles
                .Select(b => b.GetEllipseGeometry(i))
                .ToList();

            geometries.Insert(0, fillBubble.GetFillGeometry(i));
            var bubbleGeometry = CombineGeometriesBalanced(geometries);

            var final = new CombinedGeometry(GeometryCombineMode.Exclude, rect, bubbleGeometry);
            overlay.Data = final;

            await Task.Delay(Speed, cancellationToken);
        }
    }

    private async Task PlayBackward(Panel parent, Avalonia.Controls.Shapes.Path overlay, FillBubble fillBubble, Bubble[] bubbles, CancellationToken cancellationToken = default)
    {
        var rect = new RectangleGeometry(parent.Bounds);
        for (double i = 1; i > StartFillProgress; i -= lerpFactor)
        {
            var geometries = bubbles
                .Select(b => b.GetEllipseGeometry(i))
                .ToList();

            geometries.Insert(0, fillBubble.GetFillGeometry(i));
            var bubbleGeometry = CombineGeometriesBalanced(geometries);

            var final = new CombinedGeometry(GeometryCombineMode.Exclude, rect, bubbleGeometry);
            overlay.Data = final;

            await Task.Delay(Speed, cancellationToken);
        }

        for (double i = StartFillProgress; i > 0; i -= lerpFactor)
        {
            var geometries = bubbles
                .Select(b => b.GetEllipseGeometry(i))
                .ToList();

            var bubbleGeometry = CombineGeometriesBalanced(geometries);

            var final = new CombinedGeometry(GeometryCombineMode.Exclude, rect, bubbleGeometry);
            overlay.Data = final;

            await Task.Delay(Speed, cancellationToken);
        }
    }

    private static Geometry CombineGeometriesBalanced(IReadOnlyList<Geometry> geometries)
    {
        if (geometries.Count == 0)
            throw new ArgumentException("No geometries");

        if (geometries.Count == 1)
            return geometries[0];

        var next = new List<Geometry>((geometries.Count + 1) / 2);

        for (int i = 0; i < geometries.Count; i += 2)
        {
            if (i + 1 < geometries.Count)
            {
                next.Add(new CombinedGeometry(
                    GeometryCombineMode.Union,
                    geometries[i],
                    geometries[i + 1]));
            }
            else
            {
                next.Add(geometries[i]);
            }
        }

        return CombineGeometriesBalanced(next);
    }

    private readonly record struct Bubble(Point Start, Vector2 Direction, float Radius, double Speed, double Offset)
    {
        public static Bubble CreateRandom(Rect startBound, float directionAngle, float angleVariance, float maxRadius, double totalTravel = 1)
        {
            var start = new Point(
                startBound.X + Random.Shared.NextDouble() * startBound.Width,
                startBound.Y + Random.Shared.NextDouble() * startBound.Height
            );

            float angleDeg = directionAngle + (Random.Shared.NextSingle() - .5f) * angleVariance;
            float angleRad = angleDeg * float.Pi / 180;
            var dir = new Vector2(float.Cos(angleRad), float.Sin(angleRad));

            var radius = Random.Shared.NextSingle() * maxRadius;
            var offset = Random.Shared.NextDouble() * -.3;
            var speed = Random.Shared.NextDouble() * totalTravel;

            return new Bubble(start, dir, radius, speed, offset);
        }

        public static Bubble CreateRandom(Rect startBound, float directionAngle, float angleVariance,
            Func<float, float> radiusRand, Func<double, double> offsetRand, Func<double, double> speedRand, double totalTravel = 1)
        {
            var start = new Point(
                startBound.X + Random.Shared.NextDouble() * startBound.Width,
                startBound.Y + Random.Shared.NextDouble() * startBound.Height
            );

            float angleDeg = directionAngle + (Random.Shared.NextSingle() - .5f) * angleVariance;
            float angleRad = angleDeg * float.Pi / 180;
            var dir = new Vector2(float.Cos(angleRad), float.Sin(angleRad));

            var radius = radiusRand(Random.Shared.NextSingle());
            var offset = offsetRand(Random.Shared.NextDouble());
            var speed = speedRand(Random.Shared.NextDouble()) * totalTravel;

            return new Bubble(start, dir, radius, speed, offset);
        }

        public static Bubble CreateRandomPreFill(float startFillProgress, Rect startBound, float directionAngle, float angleVariance, float maxRadius, double totalTravel = 1) =>
            CreateRandom(startBound, directionAngle, angleVariance,
                r => (1 - float.Pow(r, 3)) * maxRadius,
                r => (1 - Math.Pow(r, 2)) * -startFillProgress,
                r => (1 - Math.Pow(r, 2)),
                totalTravel);

        public Point Lerp(in double t) => Start + Direction * (float)(Speed * (t + Offset));

        public Geometry GetEllipseGeometry(in double t) => new EllipseGeometry
            {
                Center = Lerp(in t),
                RadiusX = Radius,
                RadiusY = Radius
            };
    }

    private readonly record struct FillBubble
    {
        private readonly Bubble[] EdgeBubbles;
        public Rect Bounds { get; }
        public double StartFillProgress { get; }
        public Point Start { get; }
        public double Radius { get; }

        private FillBubble(Rect bounds, double startFillProgress, Point start, double radius, Bubble[] edgeBubbles) =>
            (Bounds, StartFillProgress, Start, Radius, EdgeBubbles) = (bounds, startFillProgress, start, radius, edgeBubbles);
        public static FillBubble Create(Rect bounds, float directionAngle, float directionAngleVariance, double startFillProgress, float edgeBubbleRadius = 60)
        {
            var start = bounds.BottomRight;
            var radius = bounds.Size.Height + bounds.Size.Width;

            var startBound = new Rect(start + new Point(edgeBubbleRadius, edgeBubbleRadius), new Size(edgeBubbleRadius / 10, edgeBubbleRadius / 10));

            var edgeBubblesCount = (int)(radius / edgeBubbleRadius) * 2;
            var edgeBubbles = CreateRandomSequence(edgeBubblesCount, startBound, directionAngle, directionAngleVariance, edgeBubbleRadius, startFillProgress, radius);

            return new FillBubble(bounds, startFillProgress, start, radius, edgeBubbles);
        }

        public static Bubble[] CreateRandomSequence(
            int count,
            Rect startBound,
            float baseAngle,
            float totalAngleSpread,
            float bubbleRadius,
            double startFillProgress,
            double totalTravel = 1)
        {
            var bubbles = new Bubble[count];

            float angleStart = baseAngle - totalAngleSpread / 2f;
            float segmentSize = totalAngleSpread / count;

            for (int i = 0; i < count; i++)
            {
                float segmentMin = angleStart + i * segmentSize;

                float angleDeg = segmentMin + Random.Shared.NextSingle() * segmentSize;
                float angleRad = angleDeg * float.Pi / 180f;

                var dir = new Vector2(float.Cos(angleRad), float.Sin(angleRad));
                dir /= dir.Length();

                var start = new Point(
                    startBound.X + Random.Shared.NextDouble() * startBound.Width,
                    startBound.Y + Random.Shared.NextDouble() * startBound.Height
                );

                var radius = (1 - float.Pow(Random.Shared.NextSingle(), 3)) * bubbleRadius;
                var offset = -startFillProgress * .95;
                var speed = (totalTravel / (1 - startFillProgress)) * (float.Pow(Random.Shared.NextSingle(), 3) * .1 + 1);

                bubbles[i] = new Bubble(start, dir, radius, speed, offset);
            }

            return bubbles;
        }

        public double Lerp(in double t) => (t - StartFillProgress) / (1 - StartFillProgress) * Radius;
        public Geometry GetFillGeometry(double t) => EdgeBubbles
            .Select(b => b.GetEllipseGeometry(t))
            .Aggregate(GetFill(t), (p, n) => new CombinedGeometry(p, n));
        private Geometry GetFill(double t) => new EllipseGeometry
        {
            Center = Start,
            RadiusX = Lerp(in t),
            RadiusY = Lerp(in t)
        };
    }
}