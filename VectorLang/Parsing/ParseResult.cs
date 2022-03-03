using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace VectorLang.Parsing;

internal interface IParseResult<out T>
{
    ParseInput Remainder { get; }

    T Value { get; }

    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    bool IsSuccessfull { get; }

    string? ErrorMessage { get; }
}

internal class ParseResult<T> : IParseResult<T>
{
    public ParseInput Remainder { get; }

    public string? ErrorMessage { get; }

    private readonly T? _Value;

    private ParseResult(T? value, string? errorMessage, ParseInput remainder)
    {
        _Value = value;
        ErrorMessage = errorMessage;
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

    public static IParseResult<T> Success(T value, ParseInput remainder)
    {
        return new ParseResult<T>(value, null, remainder);
    }

    public static IParseResult<T> Failure(string errorMessage, ParseInput remainder)
    {
        return new ParseResult<T>(default!, errorMessage, remainder);
    }

    public static IParseResult<T> CastFailure<U>(IParseResult<U> otherFailure)
    {
        Debug.Assert(!otherFailure.IsSuccessfull);

        return new ParseResult<T>(default!, otherFailure.ErrorMessage, otherFailure.Remainder);
    }
}
