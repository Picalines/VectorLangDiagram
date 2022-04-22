using System;
using System.Collections.Generic;
using System.Reflection;

namespace VectorLang.Tokenization;

public static class TokenTypeExtensions
{
    private static readonly Dictionary<TokenType, TokenRegexAttribute> _CachedRegexAttributes = new();

    internal static TokenRegexAttribute GetRegexAttribute(this TokenType tokenType)
    {
        if (!_CachedRegexAttributes.TryGetValue(tokenType, out var regexAttribute))
        {
            regexAttribute = typeof(TokenType)
                .GetField(tokenType.ToString())!
                .GetCustomAttribute<TokenRegexAttribute>();

            if (regexAttribute is null)
            {
                throw new NotImplementedException($"missing {nameof(TokenRegexAttribute)} on enum {nameof(TokenType)}.{tokenType}");
            }

            _CachedRegexAttributes.Add(tokenType, regexAttribute);
        }

        return regexAttribute;
    }
}
