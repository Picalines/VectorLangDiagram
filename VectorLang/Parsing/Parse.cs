﻿using System;
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

    private static IParseResult<T> DetermineBestError<T>(IParseResult<T> firstFailure, IParseResult<T> secondFailure) =>
        secondFailure.Remainder.Position > firstFailure.Remainder.Position
            ? secondFailure
            : firstFailure;

    public static Parser<T> Return<T>(T value)
    {
        return input => ParseResult<T>.Success(value, input);
    }

    public static Parser<T> Throw<T>(string error)
    {
        return input => ParseResult<T>.Failure(error, input);
    }

    public static Parser<U> Select<T, U>(this Parser<T> parser, Func<U> create)
    {
        return input => parser(input).IfSuccess(r => ParseResult<U>.Success(create(), r.Remainder));
    }

    public static Parser<U> Select<T, U>(this Parser<T> parser, Func<T, U> convert)
    {
        return input => parser(input).IfSuccess(r => ParseResult<U>.Success(convert(r.Value), r.Remainder));
    }

    public static Parser<U> As<T, U>(this Parser<T> parser, U value)
    {
        return parser.Select(() => value);
    }

    public static Parser<T> MaybeThen<T>(this Parser<T> current, Parser<T> optionalNext)
    {
        return current.Then(result => optionalNext.Or(Return(result)));
    }

    public static Parser<T> MaybeThen<T>(this Parser<T> current, Func<T, Parser<T>> optionalNext)
    {
        return current.Then(result => optionalNext(result).Or(Return(result)));
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
