using System.Collections.Generic;
using VectorLang.Model;

namespace VectorLang.Compilation;

public sealed class ExecutableProgram
{
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
    }

    public IReadOnlyList<PlottedVector> PlotVectors()
    {
        _PlotLibrary.ClearVectors();

        _MainFunction.Call();

        return _PlotLibrary.PlottedVectors;
    }
}
