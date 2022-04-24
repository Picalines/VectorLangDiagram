using System;
using System.Collections.Generic;
using System.Linq;
using VectorLang.Diagnostics;
using VectorLang.Interpretation;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class CalledNodeCompiler
{
    public static CompiledExpression Compile(CompilationContext context, CalledNode called)
    {
        return called.CalledValue switch
        {
            MemberNode method => CompileMethodCall(context, called.Arguments, method),

            VariableNode function => CompileFunctionCall(context, called.Arguments, function),

            _ => ReportNotCallable(context, called.CalledValue),
        };
    }

    private static CompiledExpression CompileMethodCall(CompilationContext context, IReadOnlyList<ValueExpressionNode> arguments, MemberNode method)
    {
        var methodName = method.Member;
        var (objectType, objectInstructions) = ValueExpressionCompiler.Compile(context, method.Object);

        if (!objectType.Methods.TryGetValue(methodName, out var instanceMethod))
        {
            context.Reporter.ReportError(method.MemberToken.Selection, ReportMessage.UndefinedTypeMember(objectType, methodName, Array.Empty<InstanceType>()));
            return CompiledExpression.Invalid;
        }

        var compiledArguments = ArgumentsCompiler.Compile(context, method.MemberToken.Selection, instanceMethod.Signature, arguments);

        if (compiledArguments is null)
        {
            return new(instanceMethod.Signature.ReturnType);
        }

        return new(
            instanceMethod.Signature.ReturnType,
            objectInstructions
                .Concat(compiledArguments.SelectMany(arg => arg.Instructions))
                .Append(new CallMethodInstruction(instanceMethod, arguments.Count))
        );
    }

    private static CompiledExpression CompileFunctionCall(CompilationContext context, IReadOnlyList<ValueExpressionNode> arguments, VariableNode functionNode)
    {
        var functionName = functionNode.Token.Value;
        context.Symbols.TryLookup(functionName, out var functionSymbol);

        if (functionSymbol is not FunctionSymbol { Function: { Signature: var signature } function })
        {
            context.Reporter.ReportError(functionNode.Token.Selection, ReportMessage.UndefinedValue($"function '{functionName}'"));
            return CompiledExpression.Invalid;
        }

        var compiledArguments = ArgumentsCompiler.Compile(context, functionNode.Selection, signature, arguments);

        if (compiledArguments is null)
        {
            return new(signature.ReturnType);
        }

        return new(
            signature.ReturnType,
            compiledArguments.SelectMany(arg => arg.Instructions)
                .Append(new CallFunctionInstruction(function, arguments.Count))
        );
    }

    private static CompiledExpression ReportNotCallable(CompilationContext context, ValueExpressionNode calledValue)
    {
        context.Reporter.ReportError(calledValue.Selection, ReportMessage.NotCallableValue("expression"));

        return CompiledExpression.Invalid;
    }
}
