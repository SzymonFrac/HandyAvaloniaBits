using Avalonia;
using Avalonia.Media;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Ext;

internal static partial class MorphSegmentExt
{
    extension(ArcSegment arc)
    {
        public (ArcSegment first, ArcSegment second) Split(ref Point start, in double t = .5)
        {
            var normalizedSize = GetNormalizedSize(in arc, in start, out var arcRotation, out var unrotatedChord);

            var center = GetCenter(in arc, in start, in arcRotation, in normalizedSize, in unrotatedChord);

            var splitAngle = GetSplitAngle(in arc, in center, in start, in t, out var angleToRotate);

            var splitPoint = GetSplitPoint(in start, in center, in arcRotation, in splitAngle, in normalizedSize);



            var (firstRotation, secondRotation) = (angleToRotate * t, angleToRotate * (1 - t));

            var first = new ArcSegment
            {
                Size = normalizedSize,
                RotationAngle = arc.RotationAngle,
                IsLargeArc = firstRotation.Degrees > 180,
                SweepDirection = arc.SweepDirection,
                Point = splitPoint
            };

            var second = new ArcSegment
            {
                Size = normalizedSize,
                RotationAngle = arc.RotationAngle,
                IsLargeArc = secondRotation.Degrees > 180,
                SweepDirection = arc.SweepDirection,
                Point = start = arc.Point
            };

            return (first, second);
        }
    }


    private static Size GetNormalizedSize(in ArcSegment arc, in Point start, out Angle arcRotation, out Point unrotatedChord)
    {
        var chord = (start - arc.Point) / 2;
        arcRotation = Angle.FromDegrees(arc.RotationAngle);
        var rotationMatrix = Matrix.CreateRotation(-arcRotation);
        unrotatedChord = rotationMatrix * chord;

        var numerator = arc.Size.Width * arc.Size.Width * unrotatedChord.Y * unrotatedChord.Y
            + arc.Size.Height * arc.Size.Height * unrotatedChord.X * unrotatedChord.X;
        var denominator = arc.Size.Width * arc.Size.Width * arc.Size.Height * arc.Size.Height;
        var radiiScale = numerator / denominator;

        var scale = Math.Sqrt(Math.Max(1, radiiScale));
        var absSize = new Size(Math.Abs(arc.Size.Width), Math.Abs(arc.Size.Height));
        var normalizedSize = absSize * scale;
        return normalizedSize;
    }

    private static Point GetCenter(in ArcSegment arc, in Point start, in Angle arcRotation, in Size normalizedSize, in Point unrotatedChord)
    {
        var numerator = (normalizedSize.Width * normalizedSize.Width * normalizedSize.Height * normalizedSize.Height
            - normalizedSize.Width * normalizedSize.Width * unrotatedChord.Y * unrotatedChord.Y
            - normalizedSize.Height * normalizedSize.Height * unrotatedChord.X * unrotatedChord.X);
        var denominator = (normalizedSize.Width * normalizedSize.Width * unrotatedChord.Y * unrotatedChord.Y
            + normalizedSize.Height * normalizedSize.Height * unrotatedChord.X * unrotatedChord.X);
        var centerScaleFactorSquared = numerator / denominator;
        var centerScaleFactor = Math.Sqrt(Math.Max(0, centerScaleFactorSquared));


        var sign = arc.SweepDirection == SweepDirection.Clockwise ^ arc.IsLargeArc ? 1 : -1;

        var cx = sign * centerScaleFactor * unrotatedChord.Y * (normalizedSize.Width / normalizedSize.Height);
        var cy = -sign * centerScaleFactor * unrotatedChord.X * (normalizedSize.Height / normalizedSize.Width);

        var rotationToCenter = Matrix.CreateRotation(arcRotation);
        var scaleToCenter = Matrix.CreateScale(cx, cy);

        var centerOffset = rotationToCenter * scaleToCenter * new Point(1, 1);

        var chordMidpoint = (start + arc.Point) / 2;
        var center = chordMidpoint + centerOffset;
        return center;
    }

    private static Angle GetSplitAngle(in ArcSegment arc, in Point center, in Point start, in double t, out Angle angleToRotate)
    {
        var toCenter = Vector2.Create(center - start);
        var toEnd = Vector2.Create(arc.Point - center);

        var crossAngle = toCenter.AngleIn(toEnd);
        var angleNotInArc = Math.PI - crossAngle;

        var angleInArc = Math.Tau - angleNotInArc;
        angleToRotate = arc.IsLargeArc ? angleInArc : angleNotInArc;

        var splitAngle = angleToRotate * t * (arc.SweepDirection == SweepDirection.Clockwise ? 1 : -1);
        return splitAngle;
    }

    private static Point GetSplitPoint(in Point start, in Point center, in Angle arcRotation, in Angle splitAngle, in Size normalizedSize)
    {
        var unrotateEllipse = Matrix.CreateRotation(-arcRotation);
        var toUnitCircle = Matrix.CreateScale(normalizedSize).Invert();
        var ellipseTransformation = toUnitCircle * unrotateEllipse;

        var rotateFromToUnit = start - center;
        var unitPoint = ellipseTransformation * rotateFromToUnit;

        var angleOfUnitPoint = Angle.FromPoint(unitPoint);
        var nextAngle = angleOfUnitPoint + splitAngle;

        var rotateFromToSplitPoint = new Point(Math.Cos(nextAngle.Radians), Math.Sin(nextAngle.Radians));

        var splitPointAboutOrigin = ellipseTransformation.Invert() * rotateFromToSplitPoint;
        var splitPoint = splitPointAboutOrigin + center;

        return splitPoint;
    }



    private readonly record struct Angle
    {
        public double Degrees { get; }
        public double Radians { get; }

        public Angle(double deg, double rad) => (Degrees, Radians) = (deg, rad);

        public static Angle FromDegrees(in double deg) => new(deg, deg * Math.PI / 180);
        public static Angle FromRadians(in double rad) => new(rad * 180 / Math.PI, rad);
        public static Angle FromPoint(in Point p) => FromRadians(Math.Atan2(p.Y, p.X));

        public static Angle operator *(Angle a, double scale) =>
            new(a.Degrees * scale, a.Radians * scale);
        public static Angle operator -(Angle a) =>
            new(-a.Degrees, -a.Radians);
        public static Angle operator -(double rad, Angle a) =>
            new((rad * 180 / Math.PI) - a.Degrees, rad - a.Radians);
        public static Angle operator +(Angle left, Angle right) =>
            new(left.Degrees + right.Degrees, left.Radians + right.Radians);
    }

    private readonly ref struct Vector2
    {
        public double X { get; }
        public double Y { get; }

        private Vector2(double x, double y) => (X, Y) = (x, y);

        public static Vector2 Create(in Point p) => new(p.X, p.Y);

        public double Magnitude() => Math.Sqrt(X * X + Y * Y);
        public double Dot(in Vector2 other) => X * other.X + Y * other.Y;

        public Angle AngleIn(in Vector2 other) =>
            Angle.FromRadians(Math.Cos(Dot(other) / (Magnitude() * other.Magnitude())));


        public static implicit operator Point(Vector2 v) => new(v.X, v.Y);

    }

    private readonly ref struct Matrix
    {
        public double M11 { get; }
        public double M12 { get; }
        public double M21 { get; }
        public double M22 { get; }

        private Matrix(double m11, double m12, double m21, double m22) => (M11, M12, M21, M22) = (m11, m12, m21, m22);

        public static Matrix CreateRotation(in Angle angle) =>
            new(Math.Cos(angle.Radians), -Math.Sin(angle.Radians), Math.Sin(angle.Radians), Math.Cos(angle.Radians));
        public static Matrix CreateScale(in double x, in double y) => new(x, 0, 0, y);
        public static Matrix CreateScale(in Size size) => new(size.Width, 0, 0, size.Height);

        public double Determinant() => M11 * M22 - M12 * M21;
        public Matrix Invert() =>
            new Matrix(M22, -M12, -M21, M11) / Determinant();

        public static Matrix operator /(Matrix m, double scale) =>
            new(m.M11 / scale,
                m.M12 / scale,
                m.M21 / scale,
                m.M22 / scale);

        public static Point operator *(Matrix m, Point point) =>
            new(m.M11 * point.X + m.M12 * point.Y,
                m.M21 * point.X + m.M22 * point.Y);

        public static Matrix operator *(Matrix left, Matrix right) =>
            new(left.M11 * right.M11 + left.M12 * right.M21,
                left.M11 * right.M12 + left.M12 * right.M22,
                left.M21 * right.M11 + left.M22 * right.M21,
                left.M21 * right.M12 + left.M22 * right.M22);
    }
}
