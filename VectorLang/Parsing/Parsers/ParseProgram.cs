using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static class ParseProgram
{
    private static readonly Parser<Definition> Definition =
        (ParseDefinition.Function as Parser<Definition>).XOr(ParseDefinition.Plot);

    public static readonly Parser<Program> Program =
        from definitions in Definition.UntilEnd()
        select new Program(definitions);
}
