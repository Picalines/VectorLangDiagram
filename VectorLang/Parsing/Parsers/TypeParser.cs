using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static class TypeParser
{
    public static readonly Parser<TypeNode> Type =
        from identifier in ParseToken.Identifier.Or(ParseToken.LiteralVoid).Named("type name")
        select new TypeNode(identifier);
}
