using System.Linq;
using VectorLang.Diagnostics;
using VectorLang.Interpretation;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

// TODO: rename to UserConstant

internal static class ConstantDefinitionCompiler
{
    public static void Compile(CompilationContext context, ConstantDefinition constantDefinition)
    {
        var constantName = constantDefinition.Name;

        // TODO: definition "ScopeSelection"?
        context.CompletionProvider.AddScope(constantDefinition.ValueExpression.Selection, context.Symbols);

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

        ConstantSymbol constantSymbol;

        if (context.Reporter.AnyErrors())
        {
            constantSymbol = new(constantName, compiledValue.Type);
        }
        else
        {
            // TODO: handle "runtime" error

            var value = Interpreter.Interpret(compiledValue.Instructions.ToList());

            constantSymbol = new(constantName, value);
        }

        context.Symbols.Insert(constantSymbol);
    }
}
