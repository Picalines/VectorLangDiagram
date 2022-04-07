using System.Collections.Generic;
using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record BlockNode(
    IReadOnlyList<ValueExpressionNode> PriorExpressions,
    ValueExpressionNode? ResultExpression,
    Token OpenToken,
    Token CloseToken) : ValueExpressionNode
{
    public override TextSelection Selection { get; } = TextSelection.FromTokens(OpenToken, CloseToken);
}
