using System;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class ValueExpressionCompiler
{
    public static CompiledExpression Compile(CompilationContext context, ValueExpressionNode expression) => expression switch
    {
        ConstantNode constantNode => ConstantCompiler.Compile(constantNode),

        VariableNode variable => VariableNodeCompiler.Compile(context, variable),

        MemberNode memberNode => MemberNodeCompiler.Compile(context, memberNode),

        UnaryExpressionNode unaryExpression => UnaryExpressionCompiler.Compile(context, unaryExpression),

        BinaryExpressionNode binaryExpression => BinaryExpressionCompiler.Compile(context, binaryExpression),

        CalledNode calledNode => CalledNodeCompiler.Compile(context, calledNode),

        VectorNode vectorNode => VectorNodeCompiler.Compile(context, vectorNode),

        VariableCreationNode variableCreation => VariableCreationCompiler.Compile(context, variableCreation),

        BlockNode blockNode => BlockCompiler.Compile(context, blockNode),

        _ => throw new NotImplementedException(),
    };

    public static CompiledExpression Compile(CompilationContext context, ValueExpressionNode expression, InstanceType expectedType)
    {
        var compiledExpression = Compile(context, expression);

        if (!compiledExpression.Type.IsAssignableTo(expectedType))
        {
            context.Reporter.ReportError(expression.Selection, ReportMessage.NotAssignableType(compiledExpression.Type, expectedType));
            return CompiledExpression.Invalid;
        }

        return compiledExpression;
    }
}
