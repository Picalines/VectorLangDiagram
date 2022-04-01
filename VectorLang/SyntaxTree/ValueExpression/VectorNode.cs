using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record VectorNode : ValueExpressionNode
{
    public ValueExpressionNode X { get; }

    public ValueExpressionNode Y { get; }

    public override TextSelection Selection { get; }

    public VectorNode(ValueExpressionNode x, ValueExpressionNode y, Token openBrace, Token closeBrace)
    {
        X = x;
        Y = y;

        Selection = TextSelection.FromTokens(openBrace, closeBrace);
    }
}
