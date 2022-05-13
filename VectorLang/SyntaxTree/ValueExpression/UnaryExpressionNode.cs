using VectorLang.Model;
using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record UnaryExpressionNode(
    ValueExpressionNode Right,
    UnaryOperator Operator,
    Token OperatorToken) : ValueExpressionNode
{
    public override TextSelection Selection { get; } = OperatorToken.Selection.Merged(Right.Selection);
}
