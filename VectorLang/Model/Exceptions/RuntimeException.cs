using System;
using VectorLang.Tokenization;

namespace VectorLang.Model;

internal sealed class RuntimeException : Exception
{
    public TextSelection? Selection { get; }

    private RuntimeException(string message, Exception? innerException, TextSelection? selection)
        : base(message, innerException)
    {
        Selection = selection;
    }

    public RuntimeException(string message, TextSelection? selection)
        : this(message, null, selection)
    {
    }

    public RuntimeException(Exception innerException, TextSelection? selection)
        : this(innerException.Message, innerException, selection)
    {
    }
}
