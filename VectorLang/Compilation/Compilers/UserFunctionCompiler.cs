using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VectorLang.Diagnostics;
using VectorLang.Interpretation;
using VectorLang.Model;
using VectorLang.SyntaxTree;
using VectorLang.Tokenization;

namespace VectorLang.Compilation;

internal static class UserFunctionCompiler
{
    public static UserFunction? Compile(CompilationContext context, FunctionDefinition definition)
    {
        if (CompileSignature(context, definition) is not CallSignature signature)
        {
            return null;
        }

        var lazyBody = new Lazy<IReadOnlyList<Instruction>>(() => CompileBody(context, signature, definition));

        var userFunction = new UserFunction(definition.Name, signature, lazyBody);

        context.Symbols.Insert(new FunctionSymbol(userFunction));

        return userFunction;
    }

    private static CallSignature? CompileSignature(CompilationContext context, FunctionDefinition definition)
    {
        bool compiledSuccessfully = true;

        if (context.Symbols.ContainsLocal(definition.Name))
        {
            context.Reporter.ReportError(definition.NameToken.Selection, ReportMessage.RedefinedValue($"function {definition.Name}"));
            compiledSuccessfully = false;
        }

        var redefinedArguments = definition.Arguments
            .GroupBy(arg => arg.Name)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        foreach (var redefinedArgument in redefinedArguments)
        {
            context.Reporter.ReportError(definition.NameToken.Selection, ReportMessage.RedefinedValue($"argument '{redefinedArgument}'"));
            compiledSuccessfully = false;
        }

        if (!TryLookupType(context, definition.ReturnType, out var returnType))
        {
            compiledSuccessfully = false;
        }

        var arguments = new List<(string, InstanceType)>();

        foreach (var defArgument in definition.Arguments)
        {
            if (!TryLookupType(context, defArgument.Type, out var argumentType))
            {
                compiledSuccessfully = false;
                continue;
            }

            if (argumentType.IsAssignableTo(VoidInstance.InstanceType))
            {
                context.Reporter.ReportError(defArgument.Type.Selection, ReportMessage.TypeIsNotAllowed(argumentType));
                compiledSuccessfully = false;
                continue;
            }

            arguments.Add((defArgument.Name, argumentType));
        }

        return compiledSuccessfully
            ? new CallSignature(returnType!, arguments)
            : null;
    }

    private static IReadOnlyList<Instruction> CompileBody(CompilationContext context, CallSignature signature, FunctionDefinition definition)
    {
        context.CompletionProvider.AddDefinitionScope(TextSelection.FromTokens(definition.NameToken, definition.EqualsToken), context.Symbols);

        context = context.WithChildSymbols();

        context.CompletionProvider.AddExpressionScope(TextSelection.FromTokens(definition.EqualsToken, definition.EndToken), context.Symbols);

        var functionContextSymbol = new FunctionContextSymbol(signature.ReturnType);
        context.Symbols.Insert(functionContextSymbol);

        var functionInstructions = new List<Instruction>();

        foreach (var (name, type) in signature.Arguments.Reverse())
        {
            var address = functionContextSymbol.GenerateVariableAddress();

            context.Symbols.Insert(new VariableSymbol(name, address, type));

            functionInstructions.Add(new StoreInstruction(address, true));
        }

        var compiledBody = ValueExpressionCompiler.Compile(context, definition.ValueExpression, signature.ReturnType);

        functionInstructions.AddRange(compiledBody.Instructions);

        return functionInstructions;
    }

    private static bool TryLookupType(CompilationContext context, TypeNode typeNode, [NotNullWhen(true)] out InstanceType? type)
    {
        if (context.Symbols.TryLookup<InstanceTypeSymbol>(typeNode.Name, out var typeSymbol))
        {
            type = typeSymbol.Type;
            return true;
        }

        context.Reporter.ReportError(typeNode.Selection, ReportMessage.UndefinedValue($"type '{typeNode.Name}'"));

        type = null;
        return false;
    }
}
