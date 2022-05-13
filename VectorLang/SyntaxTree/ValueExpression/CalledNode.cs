using System.Collections.Generic;
using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record CalledNode(
    ValueExpressionNode CalledValue,
    IReadOnlyList<ValueExpressionNode> Arguments,
    Token ClosingParenthesis) : ValueExpressionNode
{
    public override TextSelection Selection { get; } = CalledValue.Selection.Merged(ClosingParenthesis.Selection);
}
