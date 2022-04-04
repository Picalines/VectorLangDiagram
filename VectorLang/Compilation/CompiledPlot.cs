using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VectorLang.Interpretation;
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
        var vector = Interpreter.Interpret(_VectorInstructions) as VectorInstance;
        Debug.Assert(vector is not null);

        StringInstance? label = null;
        ColorInstance? color = null;

        if (_LabelInstructions is not null)
        {
            label = Interpreter.Interpret(_LabelInstructions) as StringInstance;
            Debug.Assert(label is not null);
        }

        if (_ColorInstructions is not null)
        {
            color = Interpreter.Interpret(_ColorInstructions) as ColorInstance;
            Debug.Assert(color is not null);
        }

        return new PlottedVector(
            Coordinates: (vector.X.Value, vector.Y.Value),
            Label: label?.Value,
            Color: color is not null ? (color.R.Value, color.G.Value, color.B.Value) : null
        );
    }
}
