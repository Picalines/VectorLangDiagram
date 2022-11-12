using System;
using System.Collections.Generic;

namespace VectorLang.Model;

/// <vl-doc><name>Plotting</name></vl-doc>
internal sealed class PlotLibrary : ReflectionLibrary
{
    private const int MaxContextStackSize = 32;

    private const int MaxPlottedVectorsCount = 5000;

    private record PlotContext
    {
        public VectorInstance Offset { get; set; } = new(0, 0);

        public VectorInstance Scale { get; set; } = new(1, 1);

        public NumberInstance Rotation { get; set; } = 0;

        public ColorInstance Color { get; set; } = new(1, 1, 1);
    }

    public event Action<PlottedVector>? VectorPlotted;

    private readonly Stack<PlotContext> _ContextStack = new();

    private int _PlottedVectorsCount = 0;

    public PlotLibrary()
    {
        Reset();
    }

    public void Reset()
    {
        _PlottedVectorsCount = 0;
        _ContextStack.Clear();
        _ContextStack.Push(new PlotContext());
    }

    /// <vl-doc>
    /// <name>push</name>
    /// <summary>
    /// Pushes current transformation matrix.
    /// "Starts temporary transformation", that should be ended with pop()
    /// </summary>
    /// </vl-doc>
    [ReflectionFunction("push")]
    public VoidInstance Push()
    {
        if (_ContextStack.Count == MaxContextStackSize + 1)
        {
            throw RuntimeException.PushStackOverflow(MaxContextStackSize);
        }

        _ContextStack.Push(_ContextStack.Peek() with { });

        return VoidInstance.Instance;
    }

    /// <vl-doc>
    /// <name>pop</name>
    /// <summary>
    /// Pops current transformation matrix.
    /// "Ends temporary transformation", that was started with push()
    /// </summary>
    /// </vl-doc>
    [ReflectionFunction("pop")]
    public VoidInstance Pop()
    {
        if (_ContextStack.Count <= 1)
        {
            throw RuntimeException.PopBeforePush();
        }

        _ContextStack.Pop();

        return VoidInstance.Instance;
    }

    /// <vl-doc>
    /// <name>translate</name>
    /// <summary>
    /// Translates next plotted vectors by specified offset
    /// </summary>
    /// </vl-doc>
    [ReflectionFunction("translate")]
    public VoidInstance Translate(VectorInstance offset)
    {
        var origin = _ContextStack.Peek();

        origin.Offset += offset.Scale(origin.Scale).Rotate(origin.Rotation);

        return VoidInstance.Instance;
    }

    /// <vl-doc>
    /// <name>scale</name>
    /// <summary>
    /// Scales next plotted vectors by specified scale
    /// </summary>
    /// </vl-doc>
    [ReflectionFunction("scale")]
    public VoidInstance Scale(VectorInstance scale)
    {
        var origin = _ContextStack.Peek();

        origin.Scale = origin.Scale.Scale(scale);

        return VoidInstance.Instance;
    }

    /// <vl-doc>
    /// <name>rotate</name>
    /// <summary>
    /// Rotates next plotted vectors by specified angle in radians
    /// </summary>
    /// <param name="rotation">angle in radians</param>
    /// </vl-doc>
    [ReflectionFunction("rotate")]
    public VoidInstance Rotate(NumberInstance rotation)
    {
        var origin = _ContextStack.Peek();

        origin.Rotation += rotation;

        return VoidInstance.Instance;
    }

    /// <vl-doc>
    /// <name>fill</name>
    /// <summary>
    /// Sets the color of next plotted vectors. Affected by push() and pop()
    /// </summary>
    /// </vl-doc>
    [ReflectionFunction("fill")]
    public VoidInstance Fill(ColorInstance color)
    {
        _ContextStack.Peek().Color = color;

        return VoidInstance.Instance;
    }

    /// <vl-doc>
    /// <name>plot</name>
    /// <summary>
    /// Plots given vector to the diagram with applied transformations.
    /// </summary>
    /// </vl-doc>
    [ReflectionFunction("plot")]
    public VoidInstance Plot(VectorInstance vector)
    {
        if (++_PlottedVectorsCount == MaxPlottedVectorsCount)
        {
            throw RuntimeException.PlotLimitReached(MaxPlottedVectorsCount);
        }

        var context = _ContextStack.Peek();

        VectorPlotted?.Invoke(new PlottedVector(
            context.Offset,
            context.Offset + vector.Scale(context.Scale).Rotate(context.Rotation),
            context.Color
        ));

        return VoidInstance.Instance;
    }
}
