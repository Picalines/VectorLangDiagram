using System;

namespace VectorLang.Tokenization;

public sealed class UnknownTokenException : Exception
{
    public readonly string UnknownToken;

    public readonly TextLocation Location;

    public UnknownTokenException(TextLocation location, string token)
        : base($"unknown token '{token}' at {location}")
    {
        Location = location;
        UnknownToken = token;
    }
}
