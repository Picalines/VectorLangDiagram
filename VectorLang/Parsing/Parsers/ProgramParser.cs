using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static class ProgramParser
{
    private static readonly Parser<Definition> Definition =
        (DefinitionParser.Function as Parser<Definition>)
        .XOr(DefinitionParser.Constant);

    public static readonly Parser<Program> Program =
        from definitions in Definition.UntilEnd()
        select new Program(definitions);
}
