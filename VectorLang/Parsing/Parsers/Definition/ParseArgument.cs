using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static partial class ParseDefinition
{
    public static readonly Parser<ArgumentDefinition> Argument =
        from type in ParseType.Type.WithError("argument type expected")
        from name in ParseToken.Identifier.WithError("argument name expected")
        select new ArgumentDefinition(type, name.Value);
}
