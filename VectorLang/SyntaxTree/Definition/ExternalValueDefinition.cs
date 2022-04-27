using VectorLang.Tokenization;

namespace VectorLang.SyntaxTree;

internal sealed record ExternalValueDefinition(TypeNode Type, Token NameToken, Token EndToken) : Definition
{
    public string Name => NameToken.Value;
}
