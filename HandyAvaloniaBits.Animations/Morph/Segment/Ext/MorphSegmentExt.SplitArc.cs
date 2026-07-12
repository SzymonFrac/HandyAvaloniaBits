using Avalonia;
using Avalonia.Media;
using System.Diagnostics;
using System.Numerics;

namespace HandyAvaloniaBits.Animations.Morph.Segment.Ext;

internal static partial class MorphSegmentExt
{
    extension(ArcSegment arc)
    {
        public (ArcSegment? first, ArcSegment? second) Split(ref Point start, in double t = .5)
        {
            var unrotatedChord = Avalonia.Matrix.CreateRotation(-arc.RotationAngle * Math.PI / 180).Transform(((start - arc.Point) / 2));

            var radiiScale = (arc.Size.Width * arc.Size.Width * unrotatedChord.Y * unrotatedChord.Y
                + arc.Size.Height * arc.Size.Height * unrotatedChord.X * unrotatedChord.X)
                    / (arc.Size.Width * arc.Size.Width * arc.Size.Height * arc.Size.Height);

            var scale = Math.Sqrt(Math.Max(1, radiiScale));

            var normalizedSize = arc.Size * scale;
            


            var factor = Math.Sqrt(Math.Max(0,
                (normalizedSize.Width * normalizedSize.Width * normalizedSize.Height * normalizedSize.Height
                - normalizedSize.Width * normalizedSize.Width * unrotatedChord.Y * unrotatedChord.Y
                - normalizedSize.Height * normalizedSize.Height * unrotatedChord.X * unrotatedChord.X)
                    / (normalizedSize.Width * normalizedSize.Width * unrotatedChord.Y * unrotatedChord.Y
                    + normalizedSize.Height * normalizedSize.Height * unrotatedChord.X * unrotatedChord.X)));

            var sign = arc.SweepDirection == SweepDirection.Clockwise ^ arc.IsLargeArc ? 1 : -1;

            var cx = sign * factor * unrotatedChord.Y * (normalizedSize.Width / normalizedSize.Height);
            var cy = -sign * factor * unrotatedChord.X * (normalizedSize.Height / normalizedSize.Width);

            var chordMidpoint = (start + arc.Point) / 2;
            double centerX = (Math.Cos(arc.RotationAngle * Math.PI / 180) * cx - Math.Sin(arc.RotationAngle * Math.PI / 180) * cy) + chordMidpoint.X;
            double centerY = (Math.Sin(arc.RotationAngle * Math.PI / 180) * cx + Math.Cos(arc.RotationAngle * Math.PI / 180) * cy) + chordMidpoint.Y;

            var center = new Point(centerX, centerY);



            var toCenter = new Vector2((float)(center.X - start.X), (float)(center.Y - start.Y));
            var toEnd = new Vector2((float)(arc.Point.X - center.X), (float)(arc.Point.Y - center.Y));

            var angleNotInArc = Math.PI - Math.Acos(Vector2.Dot(toCenter, toEnd) / (toCenter.Length() * toEnd.Length()));
            var arcAngle = Math.Tau - angleNotInArc;

            var angleToRotate = arc.IsLargeArc ? arcAngle : angleNotInArc;
            var splitAtAngle = (angleToRotate * t) * (arc.SweepDirection == SweepDirection.Clockwise ? 1 : -1);


            var arcRotationRad = arc.RotationAngle * Math.PI / 180;

            var scaleRotation = Matrix.CreateScale(normalizedSize.Width, normalizedSize.Height);
            var rotationMatrix = Matrix.CreateRotation(splitAtAngle);

            var totalTransformation = scaleRotation * rotationMatrix;


            var rotateFrom = start - center;
            var rotateFromUnit = rotateFrom / Math.Sqrt(rotateFrom.X * rotateFrom.X + rotateFrom.Y * rotateFrom.Y);

            var splitPoint = (totalTransformation * rotateFromUnit) + center;

            // This implementation gets the point that the arc should be split at.
            // This implementation assumes that the rotation is zero, otherwise breaks.

            return (default, default);
        }
    }

    private readonly ref struct Matrix
    {
        public double M11 { get; }
        public double M12 { get; }
        public double M21 { get; }
        public double M22 { get; }

        public static Matrix Identity => new(1, 0, 0, 1);

        private Matrix(double m11, double m12, double m21, double m22) => (M11, M12, M21, M22) = (m11, m12, m21, m22);

        public static Matrix CreateRotation(in double rad) => new(Math.Cos(rad), -Math.Sin(rad), Math.Sin(rad), Math.Cos(rad));
        public static Matrix CreateScale(in double x, in double y) => new(x, 0, 0, y);
        public static Matrix CreateReflectionAtOrigin(in double rad) =>
            new(Math.Cos(2 * rad), Math.Sin(2 * rad), Math.Sin(2 * rad), -Math.Cos(2 * rad));


        public static Point operator *(Matrix matrix, Point point) =>
            new(matrix.M11 * point.X + matrix.M12 * point.Y,
                matrix.M21 * point.X + matrix.M22 * point.Y);

        public static Matrix operator *(Matrix left, Matrix right) =>
            new(left.M11 * right.M11 + left.M12 * right.M21,
                left.M11 * right.M12 + left.M12 * right.M22,
                left.M21 * right.M11 + left.M22 * right.M21,
                left.M21 * right.M12 + left.M22 * right.M22);

        public static Matrix operator *(Matrix matrix, double scale) =>
            new(matrix.M11 * scale,
                matrix.M12 * scale,
                matrix.M21 * scale,
                matrix.M21 * scale);

        public static Matrix operator /(Matrix matrix, double scale) =>
            new(matrix.M11 / scale,
                matrix.M12 / scale,
                matrix.M21 / scale,
                matrix.M21 / scale);

        public static Matrix operator +(Matrix matrix, double scale) =>
            new(matrix.M11 + scale,
                matrix.M12 + scale,
                matrix.M21 + scale,
                matrix.M21 + scale);

        public static Matrix operator -(Matrix matrix, double scale) =>
            new(matrix.M11 - scale,
                matrix.M12 - scale,
                matrix.M21 - scale,
                matrix.M21 - scale);

    }
}
