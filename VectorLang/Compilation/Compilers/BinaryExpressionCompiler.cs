using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class BinaryExpressionCompiler
{
    public static CompiledExpression Compile(CompilationContext context, BinaryExpressionNode binaryExpression)
    {
        var (leftType, leftInstructions) = ValueExpressionCompiler.Compile(context, binaryExpression.Left);
        var (rightType, rightInstructions) = ValueExpressionCompiler.Compile(context, binaryExpression.Right);

        if (!leftType.BinaryOperators.TryGetValue((binaryExpression.Operator, rightType), out var binaryOperator))
        {
            // TODO: selection at operator
            context.Reporter.ReportError(binaryExpression.Selection, ReportMessage.UndefinedTypeMember(leftType, binaryExpression.Operator, rightType));
            return CompiledExpression.Invalid;
        }

        return new(
            binaryOperator.ReturnType,
            leftInstructions.Concat(rightInstructions).Append(new BinaryOperatorInstruction(binaryOperator))
        );
    }
}
