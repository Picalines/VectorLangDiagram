using System;
using System.Reflection;

namespace VectorLang.Model;

internal sealed class ReflectionFunction : Function
{
    private readonly object? _Target;

    private readonly MethodInfo _Method;

    private ReflectionFunction(string name, object? target, MethodInfo method)
        : base(name, CallSignature.From(method))
    {
        _Target = target;
        _Method = method;
    }

    public static ReflectionFunction FromMethod(string languageName, Delegate methodDelegate)
    {
        return new(languageName, methodDelegate.Target, methodDelegate.Method);
    }

    protected override Instance CallInternal(params Instance[] arguments)
    {
        return (_Method.InvokeAndRethrow(_Target, arguments) as Instance)!;
    }
}
