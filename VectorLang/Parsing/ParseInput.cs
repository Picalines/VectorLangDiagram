using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VectorLang.Tokenization;

namespace VectorLang.Parsing;

internal class ParseInput
{
    public readonly int Position;

    public readonly bool AtEnd;

    public readonly TextSelection Selection;

    private readonly IEnumerable<Token> _Tokens;

    private readonly Token? _Current;

    public ParseInput(IEnumerable<Token> tokens)
        : this(FilteredTokens(tokens), 0) { }

    private ParseInput(IEnumerable<Token> tokens, int position)
    {
        _Tokens = tokens;
        Position = position;

        _Current = _Tokens.ElementAtOrDefault(position);
        AtEnd = _Current is null;

        Selection = AtEnd
            ? new TextSelection(Location, 1)
            : TextSelection.FromToken(Current);
    }

    public Token Current
    {
        get
        {
            Debug.Assert(!AtEnd);

            return _Current!;
        }
    }

    public TextLocation Location
    {
        get
        {
            if (!AtEnd)
            {
                return _Current!.Location;
            }

            var lastToken = _Tokens.Last();

            return lastToken.Location with
            {
                Column = lastToken.Location.Column + lastToken.Value.Length - 1
            };
        }
    }

    public ParseInput Advance()
    {
        Debug.Assert(!AtEnd);

        return new(_Tokens, Position + 1);
    }

    private static IEnumerable<Token> FilteredTokens(IEnumerable<Token> tokens)
    {
        return tokens
            .Where(token => token.Type is not (TokenType.Space or TokenType.Comment))
            .Cached();
    }
}
