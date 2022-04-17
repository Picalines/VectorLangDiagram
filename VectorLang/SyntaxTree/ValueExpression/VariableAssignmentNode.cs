using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record VariableAssignmentNode(VariableNode TargetVariable, ValueExpressionNode ValueExpression) : ValueExpressionNode
{
    public override TextSelection Selection { get; } = TargetVariable.Selection.Merged(ValueExpression.Selection);
}
