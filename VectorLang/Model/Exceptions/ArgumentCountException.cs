using System;

namespace VectorLang.Model;

public sealed class ArgumentCountException : Exception
{
    internal ArgumentCountException(int gotCount, int expectedCount)
        : base($"got {gotCount} arguments, but {expectedCount} expected") { }
}
