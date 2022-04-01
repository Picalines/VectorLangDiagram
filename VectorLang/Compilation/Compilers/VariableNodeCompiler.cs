using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class VariableNodeCompiler
{
    public static CompiledExpression Compile(SymbolTable symbols, VariableNode variableNode)
    {
        var variableName = variableNode.Token.Value;

        symbols.TryLookup(variableName, out VariableSymbol? variableSymbol);

        if (variableSymbol is null)
        {
            throw ProgramException.At(variableNode.Selection, new UndefinedException($"variable '{variableName}'"));
        }

        return new(
            type: variableSymbol.Type,
            instruction: new LoadInstruction(variableSymbol.Address)
        );
    }
}
