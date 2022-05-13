using System.Linq;
using VectorLang.Diagnostics;
using VectorLang.Interpretation;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class BinaryExpressionCompiler
{
    public static CompiledExpression Compile(CompilationContext context, BinaryExpressionNode binaryExpression)
    {
        if (binaryExpression is { Operator: BinaryOperator.And or BinaryOperator.Or })
        {
            return CompileBooleanExpression(context, binaryExpression);
        }

        var (leftType, leftInstructions) = ValueExpressionCompiler.Compile(context, binaryExpression.Left);
        var (rightType, rightInstructions) = ValueExpressionCompiler.Compile(context, binaryExpression.Right);

        if (!leftType.BinaryOperators.TryGetValue((binaryExpression.Operator, rightType), out var binaryOperator))
        {
            context.Reporter.ReportError(binaryExpression.Selection, ReportMessage.UndefinedTypeMember(leftType, binaryExpression.Operator, rightType));
            return CompiledExpression.Invalid;
        }

        return new(
            binaryOperator.ReturnType,
            leftInstructions.Concat(rightInstructions).Append(new BinaryOperatorInstruction(binaryOperator))
        );
    }

    private static CompiledExpression CompileBooleanExpression(CompilationContext context, BinaryExpressionNode binaryExpression)
    {
        var compiledLeft = ValueExpressionCompiler.Compile(context, binaryExpression.Left, BooleanInstance.InstanceType);
        var compiledRight = ValueExpressionCompiler.Compile(context, binaryExpression.Right, BooleanInstance.InstanceType);

        var instructions = compiledLeft.Instructions;

        instructions = binaryExpression.Operator is BinaryOperator.And
            ? instructions
                .Append(new JumpIfInstruction(3))
                .Append(new PushInstruction(BooleanInstance.False))
            : instructions
                .Append(new JumpIfNotInstruction(3))
                .Append(new PushInstruction(BooleanInstance.True));

        var rightInstructions = compiledRight.Instructions.Counted(out int rightLength);

        instructions = instructions
            .Append(new JumpInstruction(rightLength + 1))
            .Concat(rightInstructions);

        return new(BooleanInstance.InstanceType, instructions);
    }
}
