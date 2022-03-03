using System;
using VectorLang.Tokenization;

namespace VectorLang.Parsing;

internal static class ParserExtensions
{
    public static Parser<V> SelectMany<T, U, V>(this Parser<T> first, Func<T, Parser<U>> second, Func<T, U, V> select)
    {
        return first.Then(firstResult => second(firstResult).Select(secondResult => select(firstResult, secondResult)));
    }

    public static IParseResult<T> Parse<T>(this Parser<T> parser, string input)
    {
        var parseInput = new ParseInput(Tokenizer.Tokenize(input));

        try
        {
            return parser(parseInput);
        }
        catch (Exception exception)
        {
            return ParseResult<T>.Failure($"Exception: {exception.Message}", parseInput);
        }
    }
}
