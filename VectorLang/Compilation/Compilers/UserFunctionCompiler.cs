using System.Collections.Generic;
using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class UserFunctionCompiler
{
    public static UserFunction CompileDefinition(SymbolTable symbols, FunctionDefinition definition)
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

        var signature = new CallSignature(returnType, arguments.ToArray());

        return new UserFunction(definition.Name, signature, definition.ValueExpression);
    }

    public static IReadOnlyList<Instruction> CompileBody(SymbolTable symbols, UserFunction function, ValueExpressionNode functionBody)
    {
        symbols = symbols.Child();

        var contextSymbol = new FunctionContextSymbol(function.Signature.ReturnType);
        symbols.Insert(contextSymbol);

        var functionInstructions = new List<Instruction>();

        foreach (var (name, type) in function.Signature.Arguments)
        {
            var address = contextSymbol.GenerateVariableAddress();

            var argSymbol = new VariableSymbol(name, address, type);
            symbols.Insert(argSymbol);

            functionInstructions.Add(new StoreInstruction(address));
        }

        var compiledBody = ValueExpressionCompiler.Compile(symbols, functionBody);

        compiledBody.AssertIsAssignableTo(function.Signature.ReturnType, functionBody.Selection);

        functionInstructions.AddRange(compiledBody.Instructions);

        return functionInstructions;
    }
}
