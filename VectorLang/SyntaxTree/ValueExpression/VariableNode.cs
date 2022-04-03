using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record VariableNode(Token Token) : ValueExpressionNode
{
    public string Identifier => Token.Value;

    public override TextSelection Selection => Token.Selection;
}
