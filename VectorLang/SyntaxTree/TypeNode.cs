using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record TypeNode(Token Token)
{
    public string Name => Token.Value;

    public TextSelection Selection => Token.Selection;
}
