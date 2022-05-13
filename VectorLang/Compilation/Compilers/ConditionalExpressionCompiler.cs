using System.Linq;
using VectorLang.Diagnostics;
using VectorLang.Interpretation;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class ConditionalExpressionCompiler
{
    public static CompiledExpression Compile(CompilationContext context, ConditionalExpressionNode conditionalExpresion)
    {
        var compiledCondition = ValueExpressionCompiler.Compile(context, conditionalExpresion.Condition, BooleanInstance.InstanceType);

        var (trueType, trueInstructions) = ValueExpressionCompiler.Compile(context, conditionalExpresion.TrueValue);

        var (falseType, falseInstructions) = conditionalExpresion.FalseValue switch
        {
            { } expression => ValueExpressionCompiler.Compile(context, expression),
            null => new CompiledExpression(VoidInstance.InstanceType, new PushInstruction(VoidInstance.Instance))
        };

        trueInstructions = trueInstructions.Counted(out var trueLength);
        falseInstructions = falseInstructions.Counted(out var falseLength);

        if (trueType != falseType)
        {
            context.Reporter.ReportError(conditionalExpresion.TrueValue.Selection, ReportMessage.NotAssignableType(trueType, falseType));

            if (conditionalExpresion.FalseValue is { Selection: var falseValueSelection })
            {
                context.Reporter.ReportError(falseValueSelection, ReportMessage.NotAssignableType(falseType, trueType));
            }

            return CompiledExpression.Invalid;
        }

        return new(
            trueType,
            compiledCondition.Instructions
                .Append(new JumpIfNotInstruction(trueLength + 2))
                .Concat(trueInstructions)
                .Append(new JumpInstruction(falseLength + 1))
                .Concat(falseInstructions)
        );
    }
}
