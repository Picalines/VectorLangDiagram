using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record ExternalValueDefinition(
    Token NameToken,
    Token EqualsToken,
    ValueExpressionNode DefaultValue,
    Token EndToken) : Definition
{
    public string Name => NameToken.Value;
}
