using System;
using System.Collections.Generic;
using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class UserFunctionCompiler
{
    public static UserFunction Compile(SymbolTable symbols, FunctionDefinition definition)
    {
        var signature = CompileSignature(symbols, definition);

        var lazyBody = new Lazy<IReadOnlyList<Instruction>>(() => CompileBody(symbols, signature, definition.ValueExpression));

        return new UserFunction(definition.Name, signature, lazyBody);
    }

    private static CallSignature CompileSignature(SymbolTable symbols, FunctionDefinition definition)
    {
        if (symbols.ContainsLocal(definition.Name))
        {
            throw ProgramException.At(definition.NameToken.Selection, new RedefenitionException(definition.Name));
        }

        var redefinedArgument = definition.Arguments
            .GroupBy(arg => arg.Name)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .FirstOrDefault();

        if (redefinedArgument is not null)
        {
            throw ProgramException.At(definition.NameToken.Selection, new RedefenitionException($"argument '{redefinedArgument}'"));
        }

        var returnType = symbols.Lookup<InstanceTypeSymbol>(definition.ReturnType.Name).Type;

        var arguments = definition.Arguments.Select(arg => (arg.Name, symbols.Lookup<InstanceTypeSymbol>(arg.Type.Name).Type));

        return new CallSignature(returnType, arguments.ToArray());
    }

    private static IReadOnlyList<Instruction> CompileBody(SymbolTable symbols, CallSignature signature, ValueExpressionNode body)
    {
        symbols = symbols.Child();

        var contextSymbol = new FunctionContextSymbol(signature.ReturnType);
        symbols.Insert(contextSymbol);

        var functionInstructions = new List<Instruction>();

        foreach (var (name, type) in signature.Arguments)
        {
            var address = contextSymbol.GenerateVariableAddress();

            var argSymbol = new VariableSymbol(name, address, type);
            symbols.Insert(argSymbol);

            functionInstructions.Add(new StoreInstruction(address));
        }

        var compiledBody = ValueExpressionCompiler.Compile(symbols, body);

        compiledBody.AssertIsAssignableTo(signature.ReturnType, body.Selection);

        functionInstructions.AddRange(compiledBody.Instructions);

        return functionInstructions;
    }
}
