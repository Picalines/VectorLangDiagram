using System;

namespace VectorLang.Model;

internal sealed class ArgumentTypeException : Exception
{
    public int ArgumentIndex { get; }

    public string ArgumentName { get; }

    internal ArgumentTypeException(int argIndex, string argName, InstanceType gotType, InstanceType expectedType)
        : base($"{gotType} is not assignable to {expectedType} in argument #{argIndex + 1} {argName}")
    {
        ArgumentIndex = argIndex;
        ArgumentName = argName;
    }
}
