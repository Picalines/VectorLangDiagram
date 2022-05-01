using VectorLang.Diagnostics;
using VectorLang.Interpretation;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class VariableNodeCompiler
{
    public static CompiledExpression Compile(CompilationContext context, VariableNode variableNode)
    {
        var variableName = variableNode.Identifier;

        if (context.Symbols.TryLookup<VariableSymbol>(variableName, out var variableSymbol))
        {
            return new(variableSymbol.Type, new LoadInstruction(variableSymbol.Address));
        }

        if (context.Symbols.TryLookup<ConstantSymbol>(variableName, out var constantSymbol))
        {
            return constantSymbol.Value is not null
                ? new(constantSymbol.InstanceType, new PushInstruction(constantSymbol.Value))
                : new(constantSymbol.InstanceType);
        }

        if (context.Symbols.TryLookup<ExternalValueSymbol>(variableName, out var externalValueSymbol))
        {
            var externalValue = externalValueSymbol.ExternalValue;

            return new(externalValue.ValueInstance.Type, new LazyPushInstruction(() =>
            {
                return externalValue.ValueInstance;
            }));
        }

        context.Reporter.ReportError(variableNode.Selection, ReportMessage.UndefinedValue($"variable or constant '{variableName}'"));

        return CompiledExpression.Invalid;
    }
}
