using System;

namespace VectorLang.Model;

public sealed class NotAssignableTypeException : Exception
{
    internal NotAssignableTypeException(InstanceType gotType, InstanceType expectedType)
        : base($"value of type {gotType} is not assignable to type {expectedType}") { }
}
