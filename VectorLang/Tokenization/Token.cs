using System.Text.RegularExpressions;

namespace VectorLang.Tokenization;

public sealed record Token(TextLocation Location, string Value, TokenType Type)
{
    public GroupCollection? RegexGroups { get; init; }

    private TextSelection? _Selection = null;

    public TextSelection Selection => _Selection ??= TextSelection.FromToken(this);
}
