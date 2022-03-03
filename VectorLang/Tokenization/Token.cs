using System.Text.RegularExpressions;

namespace VectorLang.Tokenization;

public sealed record Token(TextLocation Location, string Value, TokenType Type)
{
    public GroupCollection? RegexGroups { get; init; }
}
