using System.Collections.Generic;
using System.Diagnostics;

namespace VectorLang.Model;

internal sealed class PlotInterface : ReflectionFunctionLibrary
{
    private class Transformation
    {
        public VectorInstance Offset { get; set; } = new(0, 0);

        public VectorInstance Scale { get; set; } = new(1, 1);

        public NumberInstance Rotation { get; set; } = new(0);
    }

    private readonly Stack<Transformation> _Transformations = new();

    private readonly List<PlottedVector> _PlottedVectors = new();

    public PlotInterface()
    {
        _Transformations.Push(new Transformation());
    }

    public IReadOnlyList<PlottedVector> PlottedVectors => _PlottedVectors;

    public void ClearVectors()
    {
        _PlottedVectors.Clear();
    }

    [ReflectionFunction("push")]
    public VoidInstance Push()
    {
        var lastTransform = _Transformations.Peek();

        _Transformations.Push(new Transformation()
        {
            Offset = lastTransform.Offset,
            Scale = lastTransform.Scale,
            Rotation = lastTransform.Rotation,
        });

        return VoidInstance.Instance;
    }

    [ReflectionFunction("pop")]
    public VoidInstance Pop()
    {
        // TODO: throw runtime exception?

        Debug.Assert(_Transformations.Count > 1);

        _Transformations.Pop();

        return VoidInstance.Instance;
    }

    [ReflectionFunction("translate")]
    public VoidInstance Translate(VectorInstance offset)
    {
        var origin = _Transformations.Peek();

        origin.Offset += offset.Scale(origin.Scale).Rotate(origin.Rotation);

        return VoidInstance.Instance;
    }

    [ReflectionFunction("scale")]
    public VoidInstance Scale(VectorInstance scale)
    {
        var origin = _Transformations.Peek();

        origin.Scale = origin.Scale.Scale(scale);

        return VoidInstance.Instance;
    }

    [ReflectionFunction("rotate")]
    public VoidInstance Rotate(NumberInstance rotation)
    {
        var origin = _Transformations.Peek();

        origin.Rotation += rotation;

        return VoidInstance.Instance;
    }

    [ReflectionFunction("plot")]
    public VoidInstance Plot(VectorInstance vector, ColorInstance color)
    {
        var origin = _Transformations.Peek();

        _PlottedVectors.Add(new PlottedVector(
            origin.Offset,
            origin.Offset + vector.Scale(origin.Scale).Rotate(origin.Rotation),
            color
        ));

        return VoidInstance.Instance;
    }
}
