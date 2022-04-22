using System;
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

        try
        {
            _MainFunction.Call();
        }
        catch (Exception exception)
        {
            // TODO: handle errors in the Interpreter to specify their location
            throw new RuntimeException(exception, null);
        }

        return _PlotInterface.PlottedVectors;
    }
}
