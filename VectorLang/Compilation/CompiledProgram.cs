using System.Collections.Generic;
using System.Linq;
using VectorLang.Model;

namespace VectorLang.Compilation;

public sealed class CompiledProgram
{
    private readonly IReadOnlyList<CompiledPlot> _CompiledPlots;

    internal CompiledProgram(IReadOnlyList<CompiledPlot> compiledPlots)
    {
        _CompiledPlots = compiledPlots;
    }

    public IReadOnlyList<PlottedVector> PlotVectors()
    {
        // TODO: hadle ProgramExceptions?

        return _CompiledPlots.Select(p => p.Plot()).ToList();
    }
}
