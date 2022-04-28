using System.Linq;
using VectorLang.Diagnostics;
using VectorLang.Interpretation;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class VariableAssignmentCompiler
{
    public static CompiledExpression Compile(CompilationContext context, VariableAssignmentNode variableAssignment)
    {
        var varSymbol = LookupVariable(context, variableAssignment.TargetVariable);

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

    private static VariableSymbol? LookupVariable(CompilationContext context, VariableNode variable)
    {
        context.Symbols.TryLookup(variable.Identifier, out var symbol);

        if (symbol is null)
        {
            context.Reporter.ReportError(variable.Selection, ReportMessage.UndefinedValue($"variable '{variable.Identifier}'"));
            return null;
        }

        if (symbol is not VariableSymbol variableSymbol)
        {
            context.Reporter.ReportError(variable.Selection, ReportMessage.NotAssignableValue(variable.Identifier));
            return null;
        }

        return variableSymbol;
    }
}
