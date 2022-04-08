using System.Collections.Generic;
using VectorLang.Model;

namespace VectorLang.Compilation;

public sealed class CompiledProgram
{
    private readonly PlotInterface _PlotInterface;

    private readonly Function _MainFunction;

    internal CompiledProgram(PlotInterface plotInterface, Function mainFunction)
    {
        _PlotInterface = plotInterface;
        _MainFunction = mainFunction;
    }

    public IReadOnlyList<PlottedVector> PlotVectors()
    {
        _PlotInterface.ClearVectors();

        _MainFunction.Call();

        return _PlotInterface.PlottedVectors;
    }
}
