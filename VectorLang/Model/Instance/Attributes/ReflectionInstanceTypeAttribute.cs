using System;

namespace VectorLang.Model;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
internal sealed class ReflectionInstanceTypeAttribute : Attribute
{
    public string TypeName { get; }

    public ReflectionInstanceTypeAttribute(string typeName)
    {
        TypeName = typeName;
    }
}
