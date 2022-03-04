using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal record MemberNode(ValueExpressionNode Object, Token MemberToken) : ValueExpressionNode
{
    public string Member => MemberToken.Value;

    public override TextSelection Selection { get; } = Object.Selection.Merged(TextSelection.FromToken(MemberToken));
}
