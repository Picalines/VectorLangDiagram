namespace VectorLang.Tokenization;

public sealed record TextSelection(TextLocation Start, TextLocation End)
{
    public TextSelection(TextLocation start, int columnOffset)
        : this(start, start.Shifted(0, columnOffset)) { }

    public TextSelection Merged(TextSelection other) => new(Start, other.End);

    public override string ToString() => $"from {Start} to {End}";

    public static TextSelection FromToken(Token token) => FromTokens(token, token);

    public static TextSelection FromTokens(Token start, Token end) => new(
        start.Location,
        end.Location.Shifted(0, end.Value.Length)
    );
}
