using System.Collections.Generic;
using VectorLang.Diagnostics;
using VectorLang.Model;
using VectorLang.SyntaxTree;
using VectorLang.Tokenization;

namespace VectorLang.Compilation;

internal static class ExternalValueCompiler
{
    private static readonly Dictionary<InstanceType, Instance> _DefaultTypeValues = new()
    {
        [NumberInstance.InstanceType] = NumberInstance.From(0),
        [BooleanInstance.InstanceType] = BooleanInstance.False,
        [VectorInstance.InstanceType] = new VectorInstance(0, 0),
        [ColorInstance.InstanceType] = ColorLibrary.White,
    };

    public static void Compile(CompilationContext context, ExternalValueDefinition externalValueDefinition)
    {
        var name = externalValueDefinition.Name;

        context.CompletionProvider.AddExpressionScope(TextSelection.FromTokens(externalValueDefinition.EqualsToken, externalValueDefinition.EndToken), context.Symbols);

        if (context.Symbols.ContainsLocal(name))
        {
            context.Reporter.ReportError(externalValueDefinition.NameToken.Selection, ReportMessage.RedefinedValue(name));
            return;
        }

        // TODO: create block with FunctionContext
        var compiledDefaultValue = ValueExpressionCompiler.Compile(context, externalValueDefinition.DefaultValue);

        var defaultValue = UserConstantCompiler.TryEvaluateConstant(context, compiledDefaultValue, externalValueDefinition.NameToken.Selection);

        var externalValue = _DefaultTypeValues.TryGetValue(compiledDefaultValue.Type, out var defaultValueOfType)
            ? TryCreateExternalValue(defaultValue ?? defaultValueOfType)
            : null;

        if (externalValue is null)
        {
            context.Reporter.ReportError(externalValueDefinition.NameToken.Selection, ReportMessage.TypeIsNotAllowed(compiledDefaultValue.Type));
            return;
        }

        context.Symbols.Insert(new ExternalValueSymbol(name, externalValue));
    }

    private static ExternalValue? TryCreateExternalValue(Instance defaultValue) => defaultValue switch
    {
        NumberInstance number => new ExternalNumberValue(number),
        BooleanInstance boolean => new ExternalBooleanValue(boolean),
        VectorInstance vector => new ExternalVectorValue(vector),
        ColorInstance color => new ExternalColorValue(color),
        _ => null,
    };
}
