using System;
using System.Collections.Generic;
using VectorLang.Tokenization;

namespace VectorLang.Parsing;

internal static class ParserExtensions
{
    public static Parser<V> SelectMany<T, U, V>(this Parser<T> first, Func<T, Parser<U>> second, Func<T, U, V> select)
    {
        return first.Then(firstResult => second(firstResult).Select(secondResult => select(firstResult, secondResult)));
    }

    public static IParseResult<T> Parse<T>(this Parser<T> parser, IEnumerable<Token> tokens)
    {
        var parseInput = new ParseInput(tokens);

        return parser(parseInput);
    }
}
