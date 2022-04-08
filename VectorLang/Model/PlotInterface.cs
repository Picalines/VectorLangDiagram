using System.Collections.Generic;

namespace VectorLang.Model;

internal sealed class PlotInterface
{
    public Function PlotFunction { get; }

    private readonly List<PlottedVector> _PlottedVectors = new();

    public PlotInterface()
    {
        PlotFunction = ReflectionFunction.FromMethod("plot", Plot);
    }

    public IReadOnlyList<PlottedVector> PlottedVectors => _PlottedVectors;

    public void ClearVectors()
    {
        _PlottedVectors.Clear();
    }

    private VoidInstance Plot(VectorInstance vector, StringInstance label, ColorInstance color)
    {
        _PlottedVectors.Add(new PlottedVector(
            (vector.X.Value, vector.Y.Value),
            label.Value,
            (color.R.Value, color.G.Value, color.B.Value)
        ));

        return VoidInstance.Instance;
    }
}
