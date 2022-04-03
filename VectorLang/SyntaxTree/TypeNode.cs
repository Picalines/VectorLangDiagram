using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record TypeNode(Token Token)
{
    public string Name => Token.Value;

    // TODO: lazy?
    public TextSelection Selection { get; } = TextSelection.FromToken(Token);
}
