using System.Collections.Generic;

namespace VectorLang.Model;

// TODO: ReflectionFunctionLibrary

internal sealed class PlotInterface : FunctionLibrary
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

        AddFunction(ReflectionFunction.FromMethod("push", Push));
        AddFunction(ReflectionFunction.FromMethod("pop", Pop));

        AddFunction(ReflectionFunction.FromMethod("translate", Translate));
        AddFunction(ReflectionFunction.FromMethod("scale", Scale));
        AddFunction(ReflectionFunction.FromMethod("rotate", Rotate));

        AddFunction(ReflectionFunction.FromMethod("plot", Plot));
    }

    public IReadOnlyList<PlottedVector> PlottedVectors => _PlottedVectors;

    public void ClearVectors()
    {
        _PlottedVectors.Clear();
    }

    private VoidInstance Push()
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

    private VoidInstance Pop()
    {
        // TODO: throw error?

        if (_Transformations.Count > 1)
        {
            _Transformations.Pop();
        }

        return VoidInstance.Instance;
    }

    private VoidInstance Translate(VectorInstance offset)
    {
        var origin = _Transformations.Peek();

        origin.Offset += offset.Scale(origin.Scale).Rotate(origin.Rotation);

        return VoidInstance.Instance;
    }

    private VoidInstance Scale(VectorInstance scale)
    {
        var origin = _Transformations.Peek();

        origin.Scale = origin.Scale.Scale(scale);

        return VoidInstance.Instance;
    }

    private VoidInstance Rotate(NumberInstance rotation)
    {
        var origin = _Transformations.Peek();

        origin.Rotation += rotation;

        return VoidInstance.Instance;
    }

    private VoidInstance Plot(VectorInstance vector, StringInstance label, ColorInstance color)
    {
        var origin = _Transformations.Peek();

        _PlottedVectors.Add(new PlottedVector(
            origin.Offset.ToTuple(),
            (origin.Offset + vector.Scale(origin.Scale).Rotate(origin.Rotation)).ToTuple(),
            label.Value,
            color.ToTuple()
        ));

        return VoidInstance.Instance;
    }
}
