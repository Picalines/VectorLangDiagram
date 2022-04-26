using System.Diagnostics;

namespace VectorLang.Model;

internal sealed class InstanceUnaryOperator
{
    public InstanceType InstanceType { get; }

    public InstanceType ReturnType { get; }

    public delegate Instance CallableDelegate(Instance thisInstance);

    private readonly CallableDelegate _Callable;

    public InstanceUnaryOperator(InstanceType instanceType, InstanceType returnType, CallableDelegate callable)
    {
        InstanceType = instanceType;
        ReturnType = returnType;
        _Callable = callable;
    }

    public Instance Call(Instance thisInstance)
    {
        Debug.Assert(thisInstance.Type.IsAssignableTo(InstanceType));

        var result = _Callable(thisInstance);

        Debug.Assert(result.Type.IsAssignableTo(ReturnType));

        return result;
    }
}
