using System.Collections.Generic;
using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;
using VectorLang.Tokenization;

namespace VectorLang.Compilation;

internal static class ArgumentsCompiler
{
    public static List<CompiledExpression>? Compile(
        CompilationContext context,
        TextSelection callSelection,
        CallSignature signature,
        IEnumerable<ValueExpressionNode> arguments)
    {
        var compiledExpressions = arguments.Select((expression, index) => ValueExpressionCompiler.Compile(context, expression)).ToList();

        if (!signature.CheckArguments(compiledExpressions.Select(arg => arg.Type), out var argumentIndex, out var reportMessage))
        {
            var reportSelection = argumentIndex switch
            {
                int index => arguments.ElementAt(index).Selection,
                _ => callSelection,
            };

            context.Reporter.ReportError(reportSelection, reportMessage);
            return null;
        }

        return compiledExpressions;
    }
}
