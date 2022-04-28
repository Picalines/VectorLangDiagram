using VectorLang.Diagnostics;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class TypeNodeCompiler
{
    public static InstanceType Compile(CompilationContext context, TypeNode typeNode)
    {
        if (context.Symbols.TryLookup<InstanceTypeSymbol>(typeNode.Name, out var typeSymbol))
        {
            return typeSymbol.Type;
        }

        context.Reporter.ReportError(typeNode.Selection, ReportMessage.UndefinedValue($"type '{typeNode.Name}'"));

        return InvalidInstanceType.Instance;
    }
}
