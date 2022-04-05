using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VectorLang.Parsing;

internal static class Parse
{
    public static Parser<N> Then<T, N>(this Parser<T> current, Parser<N> next)
    {
        return input => current(input).IfSuccess(result => next(result.Remainder));
    }

    public static Parser<N> Then<T, N>(this Parser<T> current, Func<T, Parser<N>> next)
    {
        return input => current(input).IfSuccess(result => next(result.Value)(result.Remainder));
    }

    public static Parser<T> Or<T>(this Parser<T> first, Parser<T> second) => input =>
    {
        var firstResult = first(input);

        if (!firstResult.IsSuccessfull)
        {
            return second(input).IfFailure(secondResult => DetermineBestError(firstResult, secondResult));
        }

        return firstResult;
    };

    public static Parser<T> XOr<T>(this Parser<T> first, Parser<T> second) => input =>
    {
        var firstResult = first(input);

        if (!firstResult.IsSuccessfull)
        {
            if (firstResult.Remainder != input)
            {
                return firstResult;
            }

            return second(input).IfFailure(secondResult => DetermineBestError(firstResult, secondResult));
        }

        return firstResult;
    };

    private static IParseResult<T> DetermineBestError<T>(IParseResult<T> firstFailure, IParseResult<T> secondFailure)
    {
        var firstRemainder = firstFailure.Remainder;
        var secondRemainder = secondFailure.Remainder;

        if (firstRemainder.Position == secondRemainder.Position)
        {
            return firstFailure.MergeFailure(secondFailure);
        }

        return secondRemainder.Position > firstRemainder.Position
            ? secondFailure
            : firstFailure;
    }

    public static Parser<T> Return<T>(T value)
    {
        return input => ParseResult<T>.Success(value, input);
    }

    public static Parser<T> ReturnNew<T>(Func<T> create)
    {
        return input => ParseResult<T>.Success(create(), input);
    }

    public static Parser<T> Throw<T>(string error)
    {
        return input => ParseResult<T>.Failure(error, input);
    }

    public static Parser<T> WithError<T>(this Parser<T> parser, string errorMessage)
    {
        return input => parser(input).IfFailure(error => ParseResult<T>.Failure(errorMessage, error.Remainder));
    }

    public static Parser<T> WithAdditionalError<T>(this Parser<T> parser, string additionalErrorMessage)
    {
        return input => parser(input).IfFailure(error => error.MergeFailure(additionalErrorMessage));
    }

    public static Parser<T> Named<T>(this Parser<T> parser, string name)
    {
        return parser.WithAdditionalError($"{name} expected");
    }

    public static Parser<U> Select<T, U>(this Parser<T> parser, Func<U> create)
    {
        return parser.Then(result => Return(create()));
    }

    public static Parser<U> Select<T, U>(this Parser<T> parser, Func<T, U> convert)
    {
        return parser.Then(result => Return(convert(result)));
    }

    public static Parser<U> As<T, U>(this Parser<T> parser, U value)
    {
        return parser.Select(() => value);
    }

    public static Parser<(T, U)> With<T, U>(this Parser<T> parser, U info)
    {
        return parser.Select(value => (value, info));
    }

    public static Parser<T> MaybeThen<T>(this Parser<T> current, Parser<T> optionalNext)
    {
        return current.Then(result => optionalNext.XOr(Return(result)));
    }

    public static Parser<T> MaybeThen<T>(this Parser<T> current, Func<T, Parser<T>> optionalNext)
    {
        return current.Then(result => optionalNext(result).XOr(Return(result)));
    }

    public static Parser<T> FollowedBy<T, U>(this Parser<T> current, Parser<U> next)
    {
        return current.Then(currentResult => next.As(currentResult));
    }

    public static Parser<T> AtEnd<T>(this Parser<T> parser)
    {
        return input => parser(input).IfSuccess(
            result => result.Remainder.AtEnd ? result : ParseResult<T>.Failure("end of input expected", result.Remainder)
        );
    }

    public static Parser<T> Ref<T>(Func<Parser<T>?> reference)
    {
        Parser<T>? parser = null;

        return input =>
        {
            parser ??= reference();

            Debug.Assert(parser is not null);

            return parser(input);
        };
    }

    public static Parser<List<T>> Many<T>(this Parser<T> parser, int listCapacity = 0) => input =>
    {
        ParseInput lastRemainder = input;
        List<T> resultValues = new(listCapacity);
        IParseResult<T> result;

        while ((result = parser(lastRemainder)).IsSuccessfull)
        {
            resultValues.Add(result.Value);
            lastRemainder = result.Remainder;
        }

        return ParseResult<List<T>>.Success(resultValues, lastRemainder);
    };

    public static Parser<List<T>> UntilEnd<T>(this Parser<T> parser, int listCapacity = 0) => input =>
    {
        ParseInput lastRemainder = input;
        List<T> resultValues = new(listCapacity);

        while (!lastRemainder.AtEnd)
        {
            var result = parser(lastRemainder);

            if (!result.IsSuccessfull)
            {
                return ParseResult<List<T>>.CastFailure(result);
            }

            resultValues.Add(result.Value);
            lastRemainder = result.Remainder;
        }

        return ParseResult<List<T>>.Success(resultValues, lastRemainder);
    };

    public static Parser<List<T>> AtLeastOnce<T>(this Parser<T> parser)
    {
        return parser.Then(first => parser.Many().Select(rest =>
        {
            rest.Insert(0, first);
            return rest;
        }));
    }

    public static Parser<List<T>> DelimitedBy<T, U>(this Parser<T> element, Parser<U> delimiter)
    {
        return element.FollowedBy(delimiter).Many()
            .Then(tail =>
                element.Select(last =>
                {
                    tail.Add(last);
                    return tail;
                })
            )
            .XOr(ReturnNew(() => new List<T>()));
    }

    public static Parser<List<T>> Until<T, U>(this Parser<T> parser, Parser<U> stop, int listCapacity = 0) => input =>
    {
        ParseInput lastRemainder = input;
        List<T> parserResultValues = new(listCapacity);
        IParseResult<T> parserResult;
        IParseResult<U> stopResult;

        while (!(stopResult = stop(lastRemainder)).IsSuccessfull)
        {
            parserResult = parser(lastRemainder);

            if (!parserResult.IsSuccessfull)
            {
                if (lastRemainder.AtEnd)
                {
                    return ParseResult<List<T>>.CastFailure(stop(lastRemainder));
                }

                return ParseResult<List<T>>.CastFailure(parserResult);
            }

            parserResultValues.Add(parserResult.Value);
            lastRemainder = parserResult.Remainder;
        }

        return ParseResult<List<T>>.Success(parserResultValues, stopResult.Remainder);
    };

    public static Parser<T> ChainOperator<T, TOp>(Parser<TOp> @operator, Parser<T> term, Func<TOp, T, T, T> apply)
    {
        return term.Then(first => ChainOperatorRest(first, @operator, term, apply));
    }

    private static Parser<T> ChainOperatorRest<T, TOp>(T firstTerm, Parser<TOp> @operator, Parser<T> term, Func<TOp, T, T, T> apply)
    {
        return @operator.Then(opvalue =>
            term.Then(operandValue => ChainOperatorRest(apply(opvalue, firstTerm, operandValue), @operator, term, apply))
        )
        .Or(Return(firstTerm));
    }
}
