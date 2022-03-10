using System.Collections.Generic;

namespace VectorLang.SyntaxTree;

internal sealed record FunctionDefinition(
    string Name,
    TypeNode ReturnType,
    IReadOnlyList<ArgumentDefinition> Arguments,
    ValueExpressionNode ValueExpression) : Definition;
