using System;
using System.Diagnostics;

namespace VectorLang.Parsing;

internal static class ParseResultExtensions
{
    public static IParseResult<T> MergeFailure<T>(this IParseResult<T> firstFailure, string secondError)
    {
        Debug.Assert(!firstFailure.IsSuccessfull);

        return ParseResult<T>.Failure(MergeErrorMessages(firstFailure.ErrorMessage, secondError), firstFailure.Remainder);
    }

    public static IParseResult<T> MergeFailure<T, U>(this IParseResult<T> firstFailure, IParseResult<U> otherFailure)
    {
        Debug.Assert(!otherFailure.IsSuccessfull);
        Debug.Assert(firstFailure.Remainder.Position == otherFailure.Remainder.Position);

        return firstFailure.MergeFailure(otherFailure.ErrorMessage);
    }

    private static string MergeErrorMessages(string first, string second)
    {
        return $"{first} | {second}";
    }

    public static IParseResult<U> IfSuccess<T, U>(this IParseResult<T> result, Func<IParseResult<T>, IParseResult<U>> next)
    {
        return result.IsSuccessfull ? next(result) : ParseResult<U>.CastFailure(result);
    }

    public static IParseResult<T> IfFailure<T>(this IParseResult<T> result, Func<IParseResult<T>, IParseResult<T>> next)
    {
        return result.IsSuccessfull ? result : next(result);
    }
}
