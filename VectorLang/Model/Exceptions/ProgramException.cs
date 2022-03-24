using System;
using VectorLang.Tokenization;

namespace VectorLang.Model;

public sealed class ProgramException : Exception
{
    public TextSelection? Location { get; internal init; }

    internal ProgramException(string message, Exception? innerException = null)
        : base(message, innerException) { }
}
