using System;
using System.Reflection;

namespace VectorLang.Tokenization;

internal static class TokenTypeExtensions
{
    public static TokenRegexAttribute GetRegexAttribute(this TokenType tokenType) =>
        typeof(TokenType)
        .GetField(tokenType.ToString())!
        .GetCustomAttribute<TokenRegexAttribute>()
        ?? throw new NotImplementedException($"missing {nameof(TokenRegexAttribute)} on enum {nameof(TokenType)}.{tokenType}");
}
