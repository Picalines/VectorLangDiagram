using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class PlotCompiler
{
    public static CompiledPlot Compile(SymbolTable symbols, PlotDefinition plotDefinition)
    {
        var compiledVector = ValueExpressionCompiler.Compile(symbols, plotDefinition.VectorExpression);
        compiledVector.AssertIsAssignableTo(VectorInstance.InstanceType, plotDefinition.VectorExpression.Selection);

        CompiledExpression? compiledLabel = null;
        CompiledExpression? compiledColor = null;

        if (plotDefinition is { LabelExpression: { } labelExpression })
        {
            compiledLabel = ValueExpressionCompiler.Compile(symbols, labelExpression);
            compiledLabel.AssertIsAssignableTo(StringInstance.InstanceType, labelExpression.Selection);
        }

        if (plotDefinition is { ColorExpression: { } colorExpression })
        {
            compiledColor = ValueExpressionCompiler.Compile(symbols, colorExpression);
            compiledColor.AssertIsAssignableTo(ColorInstance.InstanceType, colorExpression.Selection);
        }

        return new CompiledPlot(compiledVector, compiledLabel, compiledColor);
    }
}
