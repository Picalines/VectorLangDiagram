using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class BinaryExpressionCompiler
{
    public static CompiledExpression Compile(SymbolTable symbols, BinaryExpressionNode binaryExpression)
    {
        var (leftType, leftInstructions) = ValueExpressionCompiler.Compile(symbols, binaryExpression.Left);
        var (rightType, rightInstructions) = ValueExpressionCompiler.Compile(symbols, binaryExpression.Right);

        if (!leftType.BinaryOperators.TryGetValue((binaryExpression.Operator, rightType), out var binaryOperator))
        {
            throw ProgramException.At(binaryExpression.Selection, UndefinedException.TypeMember(leftType, binaryExpression.Operator, rightType));
        }

        return new(
            Type: binaryOperator.ReturnType,
            Instructions: leftInstructions.Concat(rightInstructions).Append(new BinaryOperatorInstruction(binaryOperator))
        );
    }
}
