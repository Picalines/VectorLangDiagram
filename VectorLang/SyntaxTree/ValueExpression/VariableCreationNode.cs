using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record class VariableCreationNode(
    Token ValKeyword,
    VariableNode Variable,
    ValueExpressionNode ValueExpression) : ValueExpressionNode
{
    public override TextSelection Selection { get; } = ValKeyword.Selection.Merged(ValueExpression.Selection);
}
