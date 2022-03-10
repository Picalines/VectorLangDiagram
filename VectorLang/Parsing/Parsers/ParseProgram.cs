using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static class ParseProgram
{
    public static readonly Parser<Program> Program =
        from definitions in ParseDefinition.Function.AtLeastOnce().AtEnd() // TODO: plot
        select new Program(definitions);
}
