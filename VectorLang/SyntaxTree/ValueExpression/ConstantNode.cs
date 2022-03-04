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

internal sealed record NumberNode(double Value, Token Token) : ConstantNode(TextSelection.FromToken(Token));

internal sealed record StringNode(string Value, Token Token) : ConstantNode(TextSelection.FromToken(Token));

internal sealed record VectorNode(double X, double Y, TextSelection Selection) : ConstantNode(Selection);

internal sealed record ColorNode(double R, double G, double B, TextSelection Selection) : ConstantNode(Selection);
