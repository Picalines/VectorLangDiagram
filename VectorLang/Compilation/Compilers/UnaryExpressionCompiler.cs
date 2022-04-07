using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class UnaryExpressionCompiler
{
    public static CompiledExpression Compile(CompilationContext context, UnaryExpressionNode unaryExpression)
    {
        var (instanceType, instructions) = ValueExpressionCompiler.Compile(context, unaryExpression.Right);

        if (!instanceType.UnaryOperators.TryGetValue(unaryExpression.Operator, out var unaryOperator))
        {
            context.Reporter.ReportError(unaryExpression.Selection, ReportMessage.UndefinedTypeMember(instanceType, unaryExpression.Operator));
            return CompiledExpression.Invalid;
        }

        return new(
            unaryOperator.ReturnType,
            instructions.Append(new UnaryOperatorInstruction(unaryOperator))
        );
    }
}
