using System.Diagnostics;

namespace VectorLang.Model;

internal abstract class ReflectionInstance : Instance
{
    public ReflectionInstance(ReflectionInstanceType type) : base(type)
    {
        Debug.Assert(ReflectionInstanceType.From(GetType()) == type);
    }

    protected sealed override Instance? GetFieldInternal(string name)
    {
        return (Type as ReflectionInstanceType)!.GetInstanceField(this, name);
    }
}
