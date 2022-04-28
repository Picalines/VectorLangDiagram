using System;
using System.Collections.Generic;
using System.Diagnostics;
using VectorLang.Diagnostics;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class ExternalValueCompiler
{
    private static readonly Dictionary<InstanceType, Type> _ExternalValueTypes = new()
    {
        [NumberInstance.InstanceType] = typeof(ExternalNumberValue),
        [BooleanInstance.InstanceType] = typeof(ExternalBooleanValue),
        [VectorInstance.InstanceType] = typeof(ExternalVectorValue),
        [ColorInstance.InstanceType] = typeof(ExternalColorValue),
    };

    public static void Compile(CompilationContext context, ExternalValueDefinition externalValueDefinition)
    {
        var name = externalValueDefinition.Name;

        if (context.Symbols.ContainsLocal(name))
        {
            context.Reporter.ReportError(externalValueDefinition.NameToken.Selection, ReportMessage.RedefinedValue(name));
            return;
        }

        var type = TypeNodeCompiler.Compile(context, externalValueDefinition.Type);

        if (!_ExternalValueTypes.TryGetValue(type, out var externalValueType))
        {
            context.Reporter.ReportError(externalValueDefinition.Type.Selection, ReportMessage.TypeIsNotAllowed(type));
            return;
        }

        var externalValue = Activator.CreateInstance(externalValueType, true) as ExternalValue;

        Debug.Assert(externalValue is not null);

        context.Symbols.Insert(new ExternalValueSymbol(name, externalValue));
    }
}
