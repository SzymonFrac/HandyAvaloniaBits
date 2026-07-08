using Avalonia.Media;

namespace HandyAvaloniaBits.Animations.Morph.Figure.Lerp.Factory.Implementations;

internal sealed class SegmentOrderMorphFigureLerpFactory : IMorphFigureLerpFactory
{
    public MorphFigureLerp Create(in PathGeometry from, in PathGeometry to)
    {
        if ((from.Figures, to.Figures) is not (PathFigures fromFigures, PathFigures toFigures))
            throw new ArgumentException("Geometry must have non-null figures", from.Figures is null ? nameof(from) : nameof(to));

        var morphFigures = fromFigures.Zip(toFigures)
            .Select(s => MorphFigure.CreateInOrder(s.First, s.Second));


        return (in t) =>
        {
            var sg = new StreamGeometry();
            using var sgc = sg.Open();

            foreach (var morphFigure in morphFigures)
                morphFigure.Apply(in t, in sgc);

            return sg;
        };

    }
}
