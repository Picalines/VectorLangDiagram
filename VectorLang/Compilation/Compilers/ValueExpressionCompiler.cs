using System;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class ValueExpressionCompiler
{
    public static CompiledExpression Compile(SymbolTable symbols, ValueExpressionNode expression) => expression switch
    {
        ConstantNode constantNode => ConstantCompiler.Compile(constantNode),

        VariableNode variable => VariableNodeCompiler.Compile(symbols, variable),

        UnaryExpressionNode unaryExpression => UnaryExpressionCompiler.Compile(symbols, unaryExpression),

        BinaryExpressionNode binaryExpression => BinaryExpressionCompiler.Compile(symbols, binaryExpression),

        CalledNode calledNode => CalledNodeCompiler.Compile(symbols, calledNode),

        VectorNode vectorNode => VectorNodeCompiler.Compile(symbols, vectorNode),

        // TODO: block compilation

        _ => throw new NotImplementedException(),
    };
}
