using System;
using System.Diagnostics;

namespace VectorLang.Model;

internal sealed class InstanceMethod
{
    public InstanceType InstanceType { get; }

    public CallSignature Signature { get; }

    public delegate Instance CallableDelegate(Instance thisInstance, params Instance[] arguments);

    private readonly CallableDelegate _Callable;

    public InstanceMethod(InstanceType instanceType, CallSignature signature, CallableDelegate callable)
    {
        InstanceType = instanceType;
        Signature = signature;
        _Callable = callable;
    }

    public Instance Call(Instance thisInstance, params Instance[] arguments)
    {
        Debug.Assert(thisInstance.Type.IsAssignableTo(InstanceType));

        Signature.AssertArgumentsDebug(arguments);

        var result = _Callable(thisInstance, arguments);

        Debug.Assert(result.Type.IsAssignableTo(Signature.ReturnType));

        return result;
    }

    public Instance Call(Instance thisInstance)
    {
        return Call(thisInstance, Array.Empty<Instance>());
    }
}
