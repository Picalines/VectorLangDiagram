using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record ConstantDefinition(Token NameToken, ValueExpressionNode ValueExpression) : Definition
{
    public string Name => NameToken.Value;
}
