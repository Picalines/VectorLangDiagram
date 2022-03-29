using System;

namespace VectorLang.Model;

[AttributeUsage(AttributeTargets.Method)]
internal sealed class InstanceMethodAttribute : Attribute
{
    public string MethodName { get; }

    public InstanceMethodAttribute(string methodName)
    {
        MethodName = methodName;
    }
}
