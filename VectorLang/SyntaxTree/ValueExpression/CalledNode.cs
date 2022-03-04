using System.Collections.Generic;
using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record CalledNode : ValueExpressionNode
{
    public ValueExpressionNode CalledValue { get; }

    public IReadOnlyList<ValueExpressionNode> Arguments { get; }

    public override TextSelection Selection { get; }

    public CalledNode(ValueExpressionNode calledValue, IReadOnlyList<ValueExpressionNode> arguments, Token closeParen)
    {
        CalledValue = calledValue;
        Arguments = arguments;

        Selection = CalledValue.Selection.Merged(TextSelection.FromToken(closeParen));
    }
}
