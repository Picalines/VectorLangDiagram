using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record VectorNode(
    ValueExpressionNode X,
    ValueExpressionNode Y,
    Token OpeningBraceToken,
    Token ClosingBraceToken) : ValueExpressionNode
{
    public override TextSelection Selection { get; } = TextSelection.FromTokens(OpeningBraceToken, ClosingBraceToken);
}
