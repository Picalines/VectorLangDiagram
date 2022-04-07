using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class PlotCompiler
{
    public static CompiledPlot Compile(CompilationContext context, PlotDefinition plotDefinition)
    {
        var compiledVector = ValueExpressionCompiler.Compile(context, plotDefinition.VectorExpression, VectorInstance.InstanceType);

        CompiledExpression? compiledLabel = null;
        CompiledExpression? compiledColor = null;

        if (plotDefinition is { LabelExpression: { } labelExpression })
        {
            compiledLabel = ValueExpressionCompiler.Compile(context, labelExpression, StringInstance.InstanceType);
        }

        if (plotDefinition is { ColorExpression: { } colorExpression })
        {
            compiledColor = ValueExpressionCompiler.Compile(context, colorExpression, ColorInstance.InstanceType);
        }

        return new CompiledPlot(compiledVector, compiledLabel, compiledColor);
    }
}
