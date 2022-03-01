using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VectorLang.Tokenization;

public static class Tokenizer
{
    private static readonly Dictionary<TokenType, Regex> _TokenRegexes;

    static Tokenizer()
    {
        var tokenTypeValues = Enum.GetValues<TokenType>();

        _TokenRegexes = new(tokenTypeValues.Length);

        foreach (var member in tokenTypeValues)
        {
            _TokenRegexes[member] = member.GetRegex();
        }
    }

    public static IEnumerable<Token> Tokenize(string code)
    {
        int index = 0;
        int line = 1;
        int column = 1;
        Token? token;

        while ((token = Next(code, ref index, ref line, ref column)) is not null)
        {
            yield return token;
        }
    }

    private static Token? Next(string code, ref int index, ref int line, ref int column)
    {
        if (index >= code.Length)
        {
            return null;
        }

        var location = new TextLocation(line, column);

        foreach (var (tokenType, regex) in _TokenRegexes)
        {
            var match = regex.Match(code, index);
            if (!match.Success || match.Index != index)
            {
                continue;
            }

            var token = new Token(location, match.Value, tokenType);

            index += match.Length;

            var matchLines = match.Value.Split('\n');

            if (matchLines.Length > 1)
            {
                line += matchLines.Length - 1;
                column = matchLines[^1].Length + 1;
            }
            else
            {
                column += match.Value.Length;
            }

            return token;
        }

        throw new UnknownTokenException(location, code[index].ToString());
    }
}
