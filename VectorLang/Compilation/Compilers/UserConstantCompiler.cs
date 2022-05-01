using System.Linq;
using VectorLang.Diagnostics;
using VectorLang.Interpretation;
using VectorLang.Model;
using VectorLang.SyntaxTree;
using VectorLang.Tokenization;

namespace VectorLang.Compilation;

internal static class UserConstantCompiler
{
    public static void Compile(CompilationContext context, ConstantDefinition constantDefinition)
    {
        var constantName = constantDefinition.Name;

        context.CompletionProvider.AddExpressionScope(TextSelection.FromTokens(constantDefinition.EqualsToken, constantDefinition.EndToken), context.Symbols);

        // TODO: create block with FunctionContext
        var compiledValue = ValueExpressionCompiler.Compile(context, constantDefinition.ValueExpression);

        if (context.Symbols.ContainsLocal(constantName))
        {
            context.Reporter.ReportError(constantDefinition.NameToken.Selection, ReportMessage.RedefinedValue(constantName));
            return;
        }

        if (compiledValue.IsInvalid)
        {
            return;
        }

        var constantValue = TryEvaluateConstant(context, compiledValue, constantDefinition.NameToken.Selection);

        ConstantSymbol constantSymbol = constantValue switch
        {
            null => new(constantName, compiledValue.Type),
            not null => new(constantName, constantValue),
        };

        context.Symbols.Insert(constantSymbol);
    }

    public static Instance? TryEvaluateConstant(CompilationContext context, CompiledExpression compiledValue, TextSelection errorSelection)
    {
        if (context.Reporter.AnyErrors())
        {
            return null;
        }

        try
        {
            return Interpreter.Interpret(compiledValue.Instructions.ToList());
        }
        catch (RuntimeException runtimeException)
        {
            context.Reporter.ReportError(errorSelection, ReportMessage.FailedToEvaluateConstantExpression(runtimeException.Message));
            return null;
        }
    }
}
