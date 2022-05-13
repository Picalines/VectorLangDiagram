using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record ConditionalExpressionNode(
    Token IfToken,
    ValueExpressionNode Condition,
    ValueExpressionNode TrueValue,
    ValueExpressionNode? FalseValue) : ValueExpressionNode
{
    public override TextSelection Selection { get; } = IfToken.Selection.Merged((FalseValue ?? TrueValue).Selection);
}
