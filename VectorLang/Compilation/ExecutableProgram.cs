using System.Collections.Generic;
using VectorLang.Model;

namespace VectorLang.Compilation;

public sealed class ExecutableProgram
{
    private readonly PlotLibrary _PlotLibrary;

    private readonly Function _MainFunction;

    internal ExecutableProgram(PlotLibrary plotLibrary, Function mainFunction)
    {
        _PlotLibrary = plotLibrary;
        _MainFunction = mainFunction;
    }

    public IReadOnlyList<PlottedVector> PlotVectors()
    {
        _PlotLibrary.ClearVectors();

        _MainFunction.Call();

        return _PlotLibrary.PlottedVectors;
    }
}
