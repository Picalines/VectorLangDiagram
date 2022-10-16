using System;
using System.Diagnostics;
using System.Reflection;

namespace VectorLang.Model;

internal abstract class ReflectionInstance<TSelf> : Instance where TSelf : ReflectionInstance<TSelf>
{
    public static ReflectionInstanceType InstanceType { get; }

    static ReflectionInstance()
    {
        if (typeof(TSelf).GetCustomAttribute<ReflectionInstanceTypeAttribute>() is not { TypeName: var typeName })
        {
            throw new InvalidOperationException($"{typeof(TSelf).Name} is missing the {nameof(ReflectionInstanceTypeAttribute)}");
        }

        InstanceType = ReflectionInstanceType.Of<TSelf>(typeName);
    }

    public ReflectionInstance(ReflectionInstanceType type) : base(type)
    {
        Debug.Assert(ReflectionInstanceType.From(GetType()) == type);
    }

    protected sealed override Instance? GetFieldInternal(string name)
    {
        return (Type as ReflectionInstanceType)!.GetInstanceField(this, name);
    }
}
