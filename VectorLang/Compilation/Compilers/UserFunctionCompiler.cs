using System.Collections.Generic;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class UserFunctionCompiler
{
    public static IReadOnlyList<Instruction> Compile(SymbolTable symbols, UserFunction function, ValueExpressionNode functionBody)
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

        var (bodyReturnType, bodyInstructions) = ValueExpressionCompiler.Compile(symbols, functionBody);

        if (!bodyReturnType.IsAssignableTo(function.Signature.ReturnType))
        {
            throw ProgramException.At(functionBody.Selection, new NotAssignableTypeException(bodyReturnType, function.Signature.ReturnType));
        }

        functionInstructions.AddRange(bodyInstructions);

        return functionInstructions;
    }
}
