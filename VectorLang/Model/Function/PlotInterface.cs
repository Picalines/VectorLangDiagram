﻿using System.Collections.Generic;
using System.Diagnostics;

namespace VectorLang.Model;

internal sealed class PlotInterface : ReflectionFunctionLibrary
{
    private record PlotContext
    {
        public VectorInstance Offset { get; set; } = new(0, 0);

        public VectorInstance Scale { get; set; } = new(1, 1);

        public NumberInstance Rotation { get; set; } = new(0);

        public ColorInstance Color { get; set; } = new(1, 1, 1);
    }

    private readonly Stack<PlotContext> _ContextStack = new();

    private readonly List<PlottedVector> _PlottedVectors = new();

    public PlotInterface()
    {
        _ContextStack.Push(new PlotContext());
    }

    public IReadOnlyList<PlottedVector> PlottedVectors => _PlottedVectors;

    public void ClearVectors()
    {
        _PlottedVectors.Clear();
    }

    [ReflectionFunction("push")]
    public VoidInstance Push()
    {
        _ContextStack.Push(_ContextStack.Peek() with { });

        return VoidInstance.Instance;
    }

    [ReflectionFunction("pop")]
    public VoidInstance Pop()
    {
        // TODO: throw runtime exception?

        Debug.Assert(_ContextStack.Count > 1);

        _ContextStack.Pop();

        return VoidInstance.Instance;
    }

    [ReflectionFunction("translate")]
    public VoidInstance Translate(VectorInstance offset)
    {
        var origin = _ContextStack.Peek();

        origin.Offset += offset.Scale(origin.Scale).Rotate(origin.Rotation);

        return VoidInstance.Instance;
    }

    [ReflectionFunction("scale")]
    public VoidInstance Scale(VectorInstance scale)
    {
        var origin = _ContextStack.Peek();

        origin.Scale = origin.Scale.Scale(scale);

        return VoidInstance.Instance;
    }

    [ReflectionFunction("rotate")]
    public VoidInstance Rotate(NumberInstance rotation)
    {
        var origin = _ContextStack.Peek();

        origin.Rotation += rotation;

        return VoidInstance.Instance;
    }

    [ReflectionFunction("fill")]
    public VoidInstance Fill(ColorInstance color)
    {
        _ContextStack.Peek().Color = color;

        return VoidInstance.Instance;
    }

    [ReflectionFunction("plot")]
    public VoidInstance Plot(VectorInstance vector)
    {
        var context = _ContextStack.Peek();

        _PlottedVectors.Add(new PlottedVector(
            context.Offset,
            context.Offset + vector.Scale(context.Scale).Rotate(context.Rotation),
            context.Color
        ));

        return VoidInstance.Instance;
    }
}
