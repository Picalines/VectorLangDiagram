using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace VectorLang.Parsing;

internal interface IParseResult<out T>
{
    ParseInput Remainder { get; }

    T Value { get; }

    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    bool IsSuccessfull { get; }

    string? ErrorMessage { get; }

    IEnumerable<string> Expectations { get; }
}

internal class ParseResult<T> : IParseResult<T>
{
    public ParseInput Remainder { get; }

    public string? ErrorMessage { get; }

    public IEnumerable<string> Expectations { get; }

    private readonly T? _Value;

    private ParseResult(ParseInput remainder, T? value, string? errorMessage, IEnumerable<string> expectations)
    {
        _Value = value;
        ErrorMessage = errorMessage;
        Expectations = expectations;
        Remainder = remainder;
    }

    public bool IsSuccessfull => ErrorMessage is null;

    public T Value
    {
        get
        {
            Debug.Assert(IsSuccessfull);

            return _Value!;
        }
    }

    public static IParseResult<T> Success(ParseInput remainder, T value)
    {
        return new ParseResult<T>(remainder, value, null, Enumerable.Empty<string>());
    }

    public static IParseResult<T> Failure(ParseInput remainder, string errorMessage, IEnumerable<string>? expectations = null)
    {
        return new ParseResult<T>(remainder, default!, errorMessage, expectations ?? Enumerable.Empty<string>());
    }

    public static IParseResult<T> CastFailure<U>(IParseResult<U> otherFailure)
    {
        Debug.Assert(!otherFailure.IsSuccessfull);

        return new ParseResult<T>(otherFailure.Remainder, default!, otherFailure.ErrorMessage, otherFailure.Expectations);
    }
}
