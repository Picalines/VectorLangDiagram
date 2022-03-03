using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal abstract record class ValueExpressionConstant : ValueExpressionNode
{
    public override TextSelection Selection { get; }

    public ValueExpressionConstant(TextSelection selection)
    {
        Selection = selection;
    }
}

internal sealed record NumberNode(double Value, Token Token) : ValueExpressionConstant(TextSelection.FromToken(Token));

internal sealed record StringNode(string Value, Token Token) : ValueExpressionConstant(TextSelection.FromToken(Token));

internal sealed record VectorNode(double X, double Y, TextSelection Selection) : ValueExpressionConstant(Selection);

internal sealed record ColorNode(double R, double G, double B, TextSelection Selection) : ValueExpressionConstant(Selection);
