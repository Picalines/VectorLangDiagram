using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static partial class ParseDefinition
{
    public static readonly Parser<PlotDefinition> Plot =
        from plotKeyword in ParseToken.KeywordPlot
        from labelExpression in ParseValueExpression.Lambda
        from comma in ParseToken.Comma
        from colorExpression in ParseValueExpression.Lambda
        from arrow in ParseToken.Arrow
        from vectorExpression in ParseValueExpression.Lambda
        from semicolon in ParseToken.Semicolon
        select new PlotDefinition(vectorExpression, labelExpression, colorExpression);
}
