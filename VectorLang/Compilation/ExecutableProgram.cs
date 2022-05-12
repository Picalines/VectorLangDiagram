using System;
using System.Collections.Generic;
using VectorLang.Model;

namespace VectorLang.Compilation;

public sealed class ExecutableProgram
{
    public event Action<PlottedVector>? VectorPlotted;

    public IReadOnlyDictionary<string, ExternalValue> ExternalValues { get; }

    private readonly PlotLibrary _PlotLibrary;

    private readonly Function _MainFunction;

    internal ExecutableProgram(
        PlotLibrary plotLibrary,
        IReadOnlyDictionary<string, ExternalValue> externalValues,
        Function mainFunction)
    {
        _PlotLibrary = plotLibrary;
        ExternalValues = externalValues;
        _MainFunction = mainFunction;

        _PlotLibrary.VectorPlotted += (vector) => VectorPlotted?.Invoke(vector);
    }

    public void Execute()
    {
        _PlotLibrary.Reset();

        _MainFunction.Call();
    }
}
