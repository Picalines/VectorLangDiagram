using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static partial class ParseDefinition
{
    public static readonly Parser<ArgumentDefinition> Argument =
        from type in ParseType.Type.Named("argument type")
        from name in ParseToken.Identifier.Named("argument name")
        select new ArgumentDefinition(type, name.Value);
}
