using System.Diagnostics;

namespace VectorLang.Model;

internal sealed class InstanceBinaryOperator
{
    public InstanceType InstanceType { get; }

    public InstanceType ReturnType { get; }

    public InstanceType RightType { get; }

    public delegate Instance CallableDelegate(Instance thisInstance, Instance right);

    private readonly CallableDelegate _Callable;

    public InstanceBinaryOperator(InstanceType instanceType, InstanceType returnType, InstanceType rightType, CallableDelegate callable)
    {
        InstanceType = instanceType;
        ReturnType = returnType;
        RightType = rightType;
        _Callable = callable;
    }

    public Instance Call(Instance thisInstance, Instance right)
    {
        Debug.Assert(thisInstance.Type.IsAssignableTo(InstanceType));
        Debug.Assert(right.Type.IsAssignableTo(RightType));

        var result = _Callable(thisInstance, right);

        Debug.Assert(result.Type.IsAssignableTo(ReturnType));

        return result;
    }
}
