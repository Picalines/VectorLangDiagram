namespace VectorLang.Tokenization;

public sealed record Token(TextLocation Location, string Value, TokenType Type);
