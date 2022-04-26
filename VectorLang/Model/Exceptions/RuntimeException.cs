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

    internal static void Catch(Action action)
    {
        try
        {
            action();
        }
        catch (RuntimeException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new RuntimeException(exception, null);
        }
    }

    internal static T Catch<T>(Func<T> func)
    {
        try
        {
            return func();
        }
        catch (RuntimeException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new RuntimeException(exception, null);
        }
    }
}
