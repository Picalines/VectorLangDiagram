using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace VectorLang.Tokenization;

internal static class TokenTypeExtensions
{
    public static Regex GetRegex(this TokenType tokenType) =>
        typeof(TokenType)
        .GetField(tokenType.ToString())!
        .GetCustomAttribute<TokenRegexAttribute>()?.Regex
        ?? throw new NotImplementedException($"missing {nameof(TokenRegexAttribute)} on enum {nameof(TokenType)}.{tokenType}");
}
