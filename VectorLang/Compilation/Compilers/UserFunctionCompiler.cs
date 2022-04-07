using System;
using System.Collections.Generic;
using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class UserFunctionCompiler
{
    public static UserFunction? Compile(CompilationContext context, FunctionDefinition definition)
    {
        if (CompileSignature(context, definition) is not CallSignature signature)
        {
            return null;
        }

        var lazyBody = new Lazy<IReadOnlyList<Instruction>>(() => CompileBody(context, signature, definition.ValueExpression));

        return new UserFunction(definition.Name, signature, lazyBody);
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

        if (!context.Symbols.TryLookup<InstanceTypeSymbol>(definition.ReturnType.Name, out var returnTypeSymbol))
        {
            context.Reporter.ReportError(definition.ReturnType.Selection, ReportMessage.UndefinedValue($"type '{definition.ReturnType.Name}'"));
            compiledSuccessfully = false;
        }

        var arguments = new List<(string, InstanceType)>();

        foreach (var defArgument in definition.Arguments)
        {
            if (context.Symbols.TryLookup<InstanceTypeSymbol>(defArgument.Type.Name, out var argumentTypeSymbol))
            {
                if (argumentTypeSymbol.Type.IsAssignableTo(VoidInstance.InstanceType))
                {
                    context.Reporter.ReportError(defArgument.Type.Selection, ReportMessage.TypeIsNotAllowed(argumentTypeSymbol.Type));
                    compiledSuccessfully = false;
                    continue;
                }

                arguments.Add((defArgument.Name, argumentTypeSymbol.Type));
            }
            else
            {
                context.Reporter.ReportError(defArgument.Type.Selection, ReportMessage.UndefinedValue($"type '{defArgument.Type.Name}'"));
                compiledSuccessfully = false;
            }
        }

        return compiledSuccessfully
            ? new CallSignature(returnTypeSymbol!.Type, arguments)
            : null;
    }

    private static IReadOnlyList<Instruction> CompileBody(CompilationContext context, CallSignature signature, ValueExpressionNode body)
    {
        context = context.WithChildSymbols();

        var functionContextSymbol = new FunctionContextSymbol(signature.ReturnType);
        context.Symbols.Insert(functionContextSymbol);

        var functionInstructions = new List<Instruction>();

        foreach (var (name, type) in signature.Arguments)
        {
            var address = functionContextSymbol.GenerateVariableAddress();

            context.Symbols.Insert(new VariableSymbol(name, address, type));

            functionInstructions.Add(new StoreInstruction(address));
        }

        var compiledBody = ValueExpressionCompiler.Compile(context, body, signature.ReturnType);

        functionInstructions.AddRange(compiledBody.Instructions);

        return functionInstructions;
    }
}
