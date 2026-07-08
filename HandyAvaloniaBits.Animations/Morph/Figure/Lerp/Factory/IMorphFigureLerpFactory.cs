using Avalonia.Media;

namespace HandyAvaloniaBits.Animations.Morph.Figure.Lerp.Factory;

internal interface IMorphFigureLerpFactory
{
    MorphFigureLerp Create(in PathGeometry from, in PathGeometry to);
}
