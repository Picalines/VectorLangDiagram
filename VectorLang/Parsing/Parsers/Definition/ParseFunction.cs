using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static partial class ParseDefinition
{
    public static readonly Parser<FunctionDefinition> Function =
        from defKeyword in ParseToken.KeywordDef
        from name in ParseToken.Identifier.Named("function name")
        from openParen in ParseToken.OpenParenthesis
        from arguments in Parse.Ref(() => Argument).DelimitedBy(ParseToken.Comma)
        from closeParen in ParseToken.CloseParenthesis.Named("argument list")
        from arrow in ParseToken.Arrow
        from returnType in ParseType.Type.Named("function return type")
        from equalsOp in ParseToken.OperatorEquals
        from expression in ParseValueExpression.Lambda.Named("function value expression")
        from semicolon in ParseToken.Semicolon
        select new FunctionDefinition(name.Value, returnType, arguments, expression);
}
