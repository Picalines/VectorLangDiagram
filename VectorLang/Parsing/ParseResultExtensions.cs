using System;

namespace VectorLang.Parsing;

internal static class ParseResultExtensions
{
    public static IParseResult<U> IfSuccess<T, U>(this IParseResult<T> result, Func<IParseResult<T>, IParseResult<U>> next)
    {
        return result.IsSuccessfull ? next(result) : ParseResult<U>.CastFailure(result);
    }

    public static IParseResult<T> IfFailure<T>(this IParseResult<T> result, Func<IParseResult<T>, IParseResult<T>> next)
    {
        return result.IsSuccessfull ? result : next(result);
    }
}
