using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal abstract record ValueExpressionNode
{
    public abstract TextSelection Selection { get; }
}
