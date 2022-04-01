using System;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class ValueExpressionCompiler
{
    public static CompiledExpression Compile(SymbolTable symbols, ValueExpressionNode expression) => expression switch
    {
        // TODO

        _ => throw new NotImplementedException(),
    };
}
