using System;
using VectorLang.Tokenization;

namespace VectorLang.Model;

public sealed class RuntimeException : Exception
{
    public TextSelection? Selection { get; }

    private RuntimeException(string message, Exception? innerException, TextSelection? selection)
        : base(message, innerException)
    {
        Selection = selection;
    }

    internal RuntimeException(string message, TextSelection? selection)
        : this(message, null, selection)
    {
    }

    internal RuntimeException(Exception innerException, TextSelection? selection)
        : this(innerException.Message, innerException, selection)
    {
    }
}
