using System;
using VectorLang.Tokenization;

namespace VectorLang.Model;

public sealed class ProgramException : Exception
{
    public TextSelection? Location { get; internal init; }

    internal ProgramException(string message, Exception? innerException = null)
        : base(message, innerException) { }

    internal ProgramException(Exception innerException)
        : base(innerException.Message, innerException) { }

    internal static ProgramException At(TextSelection selection, string message) => new(message) { Location = selection };

    internal static ProgramException At(TextSelection selection, Exception exception) => new(exception) { Location = selection };

    internal static T MaybeAt<T>(TextSelection selection, Func<T> func)
    {
        try
        {
            return func();
        }
        catch (ProgramException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new ProgramException(exception) { Location = selection };
        }
    }

    internal static void MaybeAt(TextSelection selection, Action action)
    {
        try
        {
            action();
        }
        catch (ProgramException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new ProgramException(exception) { Location = selection };
        }
    }
}
