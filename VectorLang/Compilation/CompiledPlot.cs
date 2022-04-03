using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed class CompiledPlot
{
    private readonly IReadOnlyList<Instruction> _VectorInstructions;

    private readonly IReadOnlyList<Instruction>? _LabelInstructions;

    private readonly IReadOnlyList<Instruction>? _ColorInstructions;

    public CompiledPlot(CompiledExpression vectorExpression, CompiledExpression? labelExpression, CompiledExpression? colorExpression)
    {
        Debug.Assert(vectorExpression.Type.IsAssignableTo(VectorInstance.InstanceType));
        Debug.Assert(labelExpression?.Type.IsAssignableTo(StringInstance.InstanceType) ?? true);
        Debug.Assert(colorExpression?.Type.IsAssignableTo(ColorInstance.InstanceType) ?? true);

        _VectorInstructions = vectorExpression.Instructions.ToList();
        _LabelInstructions = labelExpression?.Instructions.ToList();
        _ColorInstructions = colorExpression?.Instructions.ToList();
    }

    public PlottedVector Plot()
    {
        // TODO (requirers interpretation)

        throw new NotImplementedException();
    }
}
