using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class UnaryExpressionCompiler
{
    public static CompiledExpression Compile(SymbolTable symbols, UnaryExpressionNode unaryExpression)
    {
        var (instanceType, instructions) = ValueExpressionCompiler.Compile(symbols, unaryExpression.Right);

        if (!instanceType.UnaryOperators.TryGetValue(unaryExpression.Operator, out var unaryOperator))
        {
            throw ProgramException.At(unaryExpression.Selection, UndefinedException.TypeMember(instanceType, unaryExpression.Operator));
        }

        return new(
            Type: unaryOperator.ReturnType,
            Instructions: instructions.Append(new UnaryOperatorInstruction(unaryOperator))
        );
    }
}
