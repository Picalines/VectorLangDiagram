using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static partial class ParseDefinition
{
    public static readonly Parser<FunctionDefinition> Function =
        from defKeyword in ParseToken.KeywordDef
        from name in ParseToken.Identifier
        from openParen in ParseToken.OpenParenthesis
        from arguments in Parse.Ref(() => Argument).DelimitedBy(ParseToken.Comma)
        from closeParen in ParseToken.CloseParenthesis.WithAdditionalError("argument list expected")
        from arrow in ParseToken.Arrow
        from returnType in ParseType.Type
        from equalsOp in ParseToken.OperatorEquals
        from expression in ParseValueExpression.Lambda
        select new FunctionDefinition(name.Value, returnType, arguments, expression);
}
