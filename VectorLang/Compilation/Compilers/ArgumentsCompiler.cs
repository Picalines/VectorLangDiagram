using System.Collections.Generic;
using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;
using VectorLang.Tokenization;

namespace VectorLang.Compilation;

internal static class ArgumentsCompiler
{
    public static List<CompiledExpression> Compile(
        SymbolTable symbols,
        TextSelection callSelection,
        CallSignature signature,
        IEnumerable<ValueExpressionNode> arguments)
    {
        var compiledExpressions = arguments.Select((expression, index) => ValueExpressionCompiler.Compile(symbols, expression)).ToList();

        try
        {
            signature.AssertArguments(compiledExpressions.Select(arg => arg.Type));
        }
        catch (ArgumentCountException argumentCountException)
        {
            throw ProgramException.At(callSelection, argumentCountException);
        }
        catch (ArgumentTypeException argumentTypeException)
        {
            throw ProgramException.At(arguments.ElementAt(argumentTypeException.ArgumentIndex).Selection, argumentTypeException);
        }

        return compiledExpressions;
    }
}
