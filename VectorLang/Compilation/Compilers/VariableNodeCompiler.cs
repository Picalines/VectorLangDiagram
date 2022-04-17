using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class VariableNodeCompiler
{
    public static CompiledExpression Compile(CompilationContext context, VariableNode variableNode)
    {
        var variableName = variableNode.Identifier;

        context.Symbols.TryLookup(variableName, out VariableSymbol? variableSymbol);

        if (variableSymbol is null)
        {
            context.Reporter.ReportError(variableNode.Selection, ReportMessage.UndefinedValue($"variable '{variableName}'"));
            return CompiledExpression.Invalid;
        }

        return new(
            type: variableSymbol.Type,
            instruction: new LoadInstruction(variableSymbol.Address)
        );
    }
}
