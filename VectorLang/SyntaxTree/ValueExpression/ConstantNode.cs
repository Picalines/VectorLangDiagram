using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal abstract record class ConstantNode : ValueExpressionNode
{
    public override TextSelection Selection { get; }

    public ConstantNode(TextSelection selection)
    {
        Selection = selection;
    }
}

internal sealed record NumberNode(double Value, Token Token) : ConstantNode(Token.Selection);

internal sealed record BooleanNode(bool Value, Token Token) : ConstantNode(Token.Selection);

internal sealed record ColorNode(double R, double G, double B, TextSelection Selection) : ConstantNode(Selection);
