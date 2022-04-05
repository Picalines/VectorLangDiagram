using System;
using System.Collections.Generic;
using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class CalledNodeCompiler
{
    public static CompiledExpression Compile(SymbolTable symbols, CalledNode called)
    {
        return called.CalledValue switch
        {
            MemberNode method => CompileMethodCall(symbols, called.Arguments, method),

            VariableNode function => CompileFunctionCall(symbols, called.Arguments, function),

            _ => throw new NotImplementedException(),
        };
    }

    private static CompiledExpression CompileMethodCall(SymbolTable symbols, IReadOnlyList<ValueExpressionNode> arguments, MemberNode method)
    {
        var methodName = method.Member;
        var (objectType, objectInstructions) = ValueExpressionCompiler.Compile(symbols, method.Object);

        if (!objectType.Methods.TryGetValue(methodName, out var instanceMethod))
        {
            throw ProgramException.At(method.MemberToken.Selection, UndefinedException.TypeMember(objectType, methodName, Array.Empty<InstanceType>()));
        }

        var compiledArguments = ArgumentsCompiler.Compile(symbols, method.MemberToken.Selection, instanceMethod.Signature, arguments);

        return new(
            instanceMethod.Signature.ReturnType,
            objectInstructions
                .Concat(compiledArguments.SelectMany(arg => arg.Instructions))
                .Append(new CallMethodInstruction(instanceMethod, arguments.Count))
        );
    }

    private static CompiledExpression CompileFunctionCall(SymbolTable symbols, IReadOnlyList<ValueExpressionNode> arguments, VariableNode functionNode)
    {
        var functionName = functionNode.Token.Value;
        symbols.TryLookup(functionName, out var functionSymbol);

        if (functionSymbol is not FunctionSymbol { Function: { Signature: var signature } function })
        {
            throw ProgramException.At(functionNode.Token.Selection, new UndefinedException($"function '{functionName}'"));
        }

        var compiledArguments = ArgumentsCompiler.Compile(symbols, functionNode.Selection, signature, arguments);

        return new(
            Type: signature.ReturnType,
            Instructions: compiledArguments.SelectMany(arg => arg.Instructions)
                .Append(new CallFunctionInstruction(function, arguments.Count))
        );
    }
}
