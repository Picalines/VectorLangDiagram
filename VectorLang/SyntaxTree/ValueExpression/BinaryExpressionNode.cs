using VectorLang.Model;
using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record BinaryExpressionNode(ValueExpressionNode Left, ValueExpressionNode Right, BinaryOperator Operator)
    : ValueExpressionNode
{
    public override TextSelection Selection { get; } = Left.Selection.Merged(Right.Selection);
}
