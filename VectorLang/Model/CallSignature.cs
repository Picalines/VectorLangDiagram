using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VectorLang.Model;

internal sealed class CallSignature
{
    public InstanceType ReturnType { get; }

    public IReadOnlyList<(string Name, InstanceType Type)> Arguments { get; }

    public CallSignature(InstanceType returnType, IReadOnlyList<(string Name, InstanceType Type)> arguments)
    {
        ReturnType = returnType;
        Arguments = arguments;
    }

    public CallSignature(InstanceType returnType, params (string Name, InstanceType Type)[] arguments)
        : this(returnType, arguments as IReadOnlyList<(string, InstanceType)>) { }

    public CallSignature(InstanceType returnType)
        : this(returnType, Array.Empty<(string, InstanceType)>()) { }

    public bool Equals(CallSignature otherSignature)
    {
        return otherSignature.ReturnType.IsAssignableTo(ReturnType)
            && Arguments.Zip(otherSignature.Arguments).All(argPair => argPair.First.Type == argPair.Second.Type);
    }

    public void AssertArguments(params Instance[] arguments)
    {
        if (arguments.Length != Arguments.Count)
        {
            throw new ArgumentCountException(arguments.Length, Arguments.Count);
        }

        foreach (var ((_, defType), givenValue) in Arguments.Zip(arguments))
        {
            if (!givenValue.Type.IsAssignableTo(defType))
            {
                throw new NotAssignableTypeException(givenValue.Type, defType);
            }
        }
    }

    [Conditional("DEBUG")]
    public void AssertArgumentsDebug(params Instance[] arguments)
    {
        AssertArguments(arguments);
    }
}
