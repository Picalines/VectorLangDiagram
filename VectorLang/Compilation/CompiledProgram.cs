using System;
using System.Collections.Generic;
using VectorLang.Model;

namespace VectorLang.Compilation;

public sealed class CompiledProgram
{
    private readonly PlotLibrary _PlotLibrary;

    private readonly Function _MainFunction;

    internal CompiledProgram(PlotLibrary plotLibrary, Function mainFunction)
    {
        _PlotLibrary = plotLibrary;
        _MainFunction = mainFunction;
    }

    public IReadOnlyList<PlottedVector> PlotVectors()
    {
        _PlotLibrary.ClearVectors();

        try
        {
            _MainFunction.Call();
        }
        catch (Exception exception)
        {
            // TODO: handle errors in the Interpreter to specify their location
            throw new RuntimeException(exception, null);
        }

        return _PlotLibrary.PlottedVectors;
    }
}
