using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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

    public void AssertArguments(IEnumerable<InstanceType> arguments)
    {
        int index = 0;

        foreach (var givenType in arguments)
        {
            if (index >= Arguments.Count)
            {
                throw new ArgumentCountException(index + 1, Arguments.Count);
            }

            var (defName, defType) = Arguments[index];

            if (!givenType.IsAssignableTo(defType))
            {
                throw new ArgumentTypeException(index, defName, givenType, defType);
            }

            index++;
        }

        if (index != Arguments.Count)
        {
            throw new ArgumentCountException(index, Arguments.Count);
        }
    }

    public void AssertArguments(IEnumerable<Instance> arguments)
    {
        AssertArguments(arguments.Select(arg => arg.Type));
    }

    [Conditional("DEBUG")]
    public void AssertArgumentsDebug(params Instance[] arguments)
    {
        AssertArguments(arguments);
    }

    public static CallSignature From(MethodInfo methodInfo)
    {
        var returnInstanceType = ReflectionInstanceType.From(methodInfo.ReturnType);

        var parameters = methodInfo.GetParameters()
            .Where(param => param is { Name: not null, IsOut: false, IsIn: false })
            .Select(param => (param.Name!, ReflectionInstanceType.From(param.ParameterType) as InstanceType));

        return new CallSignature(returnInstanceType, parameters.ToArray());
    }

    public void Deconstruct(out InstanceType returnType, out IReadOnlyList<(string Name, InstanceType Type)> arguments)
    {
        returnType = ReturnType;
        arguments = Arguments;
    }
}
