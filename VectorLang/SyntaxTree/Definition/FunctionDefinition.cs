using System.Collections.Generic;
using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record FunctionDefinition(
    Token NameToken,
    Token EqualsToken,
    Token EndToken,
    TypeNode ReturnType,
    IReadOnlyList<ArgumentDefinition> Arguments,
    ValueExpressionNode ValueExpression) : Definition
{
    public string Name => NameToken.Value;
}
