using VectorLang.Model;
using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record UnaryExpressionNode : ValueExpressionNode
{
    public ValueExpressionNode Right { get; }

    public UnaryOperator Operator { get; }

    public override TextSelection Selection { get; }

    public UnaryExpressionNode(ValueExpressionNode right, UnaryOperator @operator, Token operatorToken)
    {
        Right = right;
        Operator = @operator;

        Selection = operatorToken.Selection.Merged(right.Selection);
    }
}
