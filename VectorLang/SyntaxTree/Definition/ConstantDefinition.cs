using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record ConstantDefinition(
    Token NameToken,
    Token EqualsToken,
    Token EndToken,
    ValueExpressionNode ValueExpression) : Definition
{
    public string Name => NameToken.Value;
}
