using System;
using System.Reflection;

namespace VectorLang.Tokenization;

public static class TokenTypeExtensions
{
    internal static TokenRegexAttribute GetRegexAttribute(this TokenType tokenType) =>
        typeof(TokenType)
        .GetField(tokenType.ToString())!
        .GetCustomAttribute<TokenRegexAttribute>()
        ?? throw new NotImplementedException($"missing {nameof(TokenRegexAttribute)} on enum {nameof(TokenType)}.{tokenType}");

    public static bool IsKeyword(this TokenType tokenType) =>
        tokenType.ToString().StartsWith("Keyword");

    public static bool IsOperator(this TokenType tokenType) =>
        tokenType.ToString().StartsWith("Operator");

    public static bool IsLiteral(this TokenType tokenType) =>
        tokenType.ToString().StartsWith("Literal");
}
