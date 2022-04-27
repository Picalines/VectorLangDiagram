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

        Instance? constantValue = null;

        if (!context.Reporter.AnyErrors())
        {
            try
            {
                constantValue = Interpreter.Interpret(compiledValue.Instructions.ToList());
            }
            catch (RuntimeException)
            {
                context.Reporter.ReportError(constantDefinition.ValueExpression.Selection, ReportMessage.CantEvaluateConstantExpression);
            }
        }

        ConstantSymbol constantSymbol = constantValue switch
        {
            null => new(constantName, compiledValue.Type),
            not null => new(constantName, constantValue),
        };

        context.Symbols.Insert(constantSymbol);
    }
}
