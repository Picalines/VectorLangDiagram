using System;
using System.Diagnostics;
using System.Reflection;

namespace VectorLang.Model;

internal sealed class ReflectionFunction : Function
{
    private readonly MethodInfo _Method;

    private ReflectionFunction(MethodInfo method)
        : base(method.Name, CallSignature.From(method))
    {
        _Method = method;
    }

    public static ReflectionFunction FromMethod(Delegate methodDelegate)
    {
        Debug.Assert(methodDelegate.Target is null, $"{nameof(ReflectionFunction)}.{nameof(FromMethod)} expects a static method delegate");

        return new(methodDelegate.Method);
    }

    protected override Instance CallInternal(params Instance[] arguments)
    {
        return (_Method.Invoke(null, arguments) as Instance)!;
    }
}
