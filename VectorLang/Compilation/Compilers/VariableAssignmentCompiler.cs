using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class VariableAssignmentCompiler
{
    public static CompiledExpression Compile(CompilationContext context, VariableAssignmentNode variableAssignment)
    {
        var variableName = variableAssignment.TargetVariable.Identifier;

        if (!context.Symbols.TryLookup<VariableSymbol>(variableName, out var varSymbol))
        {
            context.Reporter.ReportError(variableAssignment.TargetVariable.Selection, ReportMessage.UndefinedValue($"variable '{variableName}'"));
        }

        var expectedValueType = varSymbol?.Type ?? InvalidInstanceType.Instance;

        var compiledValue = ValueExpressionCompiler.Compile(context, variableAssignment.ValueExpression, expectedValueType);

        if (varSymbol is null)
        {
            return CompiledExpression.Invalid;
        }

        return new CompiledExpression(
            expectedValueType,
            compiledValue.Instructions.Append(new StoreInstruction(varSymbol.Address, false))
        );
    }
}
