using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class BlockCompiler
{
    public static CompiledExpression Compile(CompilationContext context, BlockNode block)
    {
        context = context.WithChildSymbols();

        if (!block.PriorExpressions.Any())
        {
            return ValueExpressionCompiler.Compile(context, block.ResultExpression!);
        }

        var priorExpressions = block.PriorExpressions.Select(expression => ValueExpressionCompiler.Compile(context, expression)).ToList();

        var resultExpression = block.ResultExpression switch
        {
            not null => ValueExpressionCompiler.Compile(context, block.ResultExpression),

            null => new CompiledExpression(VoidInstance.InstanceType, new PushInstruction(VoidInstance.Instance)),
        };

        return new(
            resultExpression.Type,
            priorExpressions
                .SelectMany(expression => expression.Instructions.Append(PopInstruction.Instance))
                .Concat(resultExpression.Instructions)
        );
    }
}
