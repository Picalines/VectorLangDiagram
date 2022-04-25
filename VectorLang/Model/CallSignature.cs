using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using VectorLang.Diagnostics;

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

    public bool CheckArguments(IEnumerable<InstanceType> arguments, out int? wrongArgumentIndex, [NotNullWhen(false)] out string? reportMessage)
    {
        int index = 0;

        foreach (var givenType in arguments)
        {
            if (index >= Arguments.Count)
            {
                reportMessage = ReportMessage.WrongArgumentCount(index + 1, Arguments.Count);
                wrongArgumentIndex = null;
                return false;
            }

            var (defName, defType) = Arguments[index];

            if (!givenType.IsAssignableTo(defType))
            {
                reportMessage = ReportMessage.WrongArgumentType(index, defName, givenType, defType);
                wrongArgumentIndex = index;
                return false;
            }

            index++;
        }

        if (index != Arguments.Count)
        {
            reportMessage = ReportMessage.WrongArgumentCount(index, Arguments.Count);
            wrongArgumentIndex = null;
            return false;
        }

        reportMessage = null;
        wrongArgumentIndex = null;
        return true;
    }

    public bool CheckArguments(IEnumerable<Instance> arguments, out int? wrongArgumentIndex, [NotNullWhen(false)] out string? reportMessage)
    {
        return CheckArguments(arguments.Select(arg => arg.Type), out wrongArgumentIndex, out reportMessage);
    }

    [Conditional("DEBUG")]
    public void AssertArgumentsDebug(params Instance[] arguments)
    {
        Debug.Assert(CheckArguments(arguments, out _, out var reportMessage), reportMessage);
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
