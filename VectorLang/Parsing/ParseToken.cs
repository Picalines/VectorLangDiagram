﻿using VectorLang.Tokenization;

namespace VectorLang.Parsing;

internal static class ParseToken
{
    private static Parser<Token> Parser(this TokenType tokenType) => input =>
    {
        if (input.AtEnd || input.Current.Type != tokenType)
        {
            return ParseResult<Token>.Failure($"{tokenType.GetDescription()} expected", input);
        }

        return ParseResult<Token>.Success(input.Current, input.Advance());
    };

    public static readonly Parser<Token> Space = TokenType.Space.Parser();
    public static readonly Parser<Token> Comment = TokenType.Comment.Parser();

    public static readonly Parser<Token> Comma = TokenType.Comma.Parser();
    public static readonly Parser<Token> Dot = TokenType.Dot.Parser();
    public static readonly Parser<Token> Arrow = TokenType.Arrow.Parser();

    public static readonly Parser<Token> OpenParenthesis = TokenType.OpenParenthesis.Parser();
    public static readonly Parser<Token> CloseParenthesis = TokenType.CloseParenthesis.Parser();
    public static readonly Parser<Token> CurlyBraceOpen = TokenType.CurlyBraceOpen.Parser();
    public static readonly Parser<Token> CurlyBraceClose = TokenType.CurlyBraceClose.Parser();

    public static readonly Parser<Token> KeywordDef = TokenType.KeywordDef.Parser();
    public static readonly Parser<Token> KeywordExternal = TokenType.KeywordExternal.Parser();
    public static readonly Parser<Token> KeywordVal = TokenType.KeywordVal.Parser();
    public static readonly Parser<Token> KeywordPlot = TokenType.KeywordPlot.Parser();

    public static readonly Parser<Token> OperatorNotEquals = TokenType.OperatorNotEquals.Parser();
    public static readonly Parser<Token> OperatorEquals = TokenType.OperatorEquals.Parser();
    public static readonly Parser<Token> OperatorPlus = TokenType.OperatorPlus.Parser();
    public static readonly Parser<Token> OperatorMinus = TokenType.OperatorMinus.Parser();
    public static readonly Parser<Token> OperatorMultiply = TokenType.OperatorMultiply.Parser();
    public static readonly Parser<Token> OperatorDivide = TokenType.OperatorDivide.Parser();
    public static readonly Parser<Token> OperatorModulo = TokenType.OperatorModulo.Parser();

    public static readonly Parser<Token> LiteralNumber = TokenType.LiteralNumber.Parser();
    public static readonly Parser<Token> LiteralString = TokenType.LiteralString.Parser();
    public static readonly Parser<Token> LiteralColor = TokenType.LiteralColor.Parser();

    public static readonly Parser<Token> Identifier = TokenType.Identifier.Parser();
}