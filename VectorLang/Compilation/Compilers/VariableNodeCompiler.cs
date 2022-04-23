using VectorLang.Diagnostics;
using VectorLang.Interpretation;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class VariableNodeCompiler
{
    public static CompiledExpression Compile(CompilationContext context, VariableNode variableNode)
    {
        var variableName = variableNode.Identifier;

        context.Symbols.TryLookup(variableName, out VariableSymbol? variableSymbol);

        context.Symbols.TryLookup(variableName, out ConstantSymbol? constantSymbol);

        if (variableSymbol is not null)
        {
            return new(variableSymbol.Type, new LoadInstruction(variableSymbol.Address));
        }

        if (constantSymbol is not null)
        {
            return constantSymbol.Value is not null
                ? new(constantSymbol.InstanceType, new PushInstruction(constantSymbol.Value))
                : new(constantSymbol.InstanceType);
        }

        context.Reporter.ReportError(variableNode.Selection, ReportMessage.UndefinedValue($"variable or constant '{variableName}'"));

        return CompiledExpression.Invalid;
    }
}
