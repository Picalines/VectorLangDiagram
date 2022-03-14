namespace VectorLang.SyntaxTree;

internal sealed record PlotDefinition(
    ValueExpressionNode VectorExpression,
    ValueExpressionNode? LabelExpression,
    ValueExpressionNode? ColorExpression) : Definition;
