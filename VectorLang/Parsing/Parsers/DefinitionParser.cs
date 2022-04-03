using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static class DefinitionParser
{
    public static readonly Parser<ArgumentDefinition> Argument =
        from type in TypeParser.Type.Named("argument type")
        from name in ParseToken.Identifier.Named("argument name")
        select new ArgumentDefinition(type, name.Value);

    public static readonly Parser<FunctionDefinition> Function =
        from defKeyword in ParseToken.KeywordDef
        from name in ParseToken.Identifier.Named("function name")
        from openParen in ParseToken.OpenParenthesis
        from arguments in Argument.DelimitedBy(ParseToken.Comma)
        from closeParen in ParseToken.CloseParenthesis.Named("argument list")
        from arrow in ParseToken.Arrow
        from returnType in TypeParser.Type.Named("function return type")
        from equalsOp in ParseToken.OperatorEquals
        from expression in ValueExpressionParser.Lambda.Named("function value expression")
        from semicolon in ParseToken.Semicolon
        select new FunctionDefinition(name, returnType, arguments, expression);

    public static readonly Parser<PlotDefinition> Plot =
        from plotKeyword in ParseToken.KeywordPlot
        from labelExpression in ValueExpressionParser.Lambda.Named("vector label")
        from comma in ParseToken.Comma
        from colorExpression in ValueExpressionParser.Lambda.Named("vector color")
        from arrow in ParseToken.Arrow
        from vectorExpression in ValueExpressionParser.Lambda.Named("vector expression")
        from semicolon in ParseToken.Semicolon
        select new PlotDefinition(vectorExpression, labelExpression, colorExpression);
}
