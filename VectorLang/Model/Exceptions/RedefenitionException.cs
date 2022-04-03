using System;

namespace VectorLang.Model;

public sealed class RedefenitionException : Exception
{
    internal RedefenitionException(string redefinedValue)
        : base($"{redefinedValue} is already defined") { }
}
