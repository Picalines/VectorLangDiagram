using System;
using System.Text.RegularExpressions;

namespace VectorLang.Tokenization;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal sealed class TokenRegexAttribute : Attribute
{
    public Regex Regex { get; }

    public TokenRegexAttribute(string regex)
    {
        Regex = new(regex, RegexOptions.Compiled);
    }
}
