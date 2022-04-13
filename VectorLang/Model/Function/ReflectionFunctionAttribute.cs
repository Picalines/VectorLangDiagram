using System;

namespace VectorLang.Model;

[AttributeUsage(AttributeTargets.Method)]
internal sealed class ReflectionFunctionAttribute : Attribute
{
    public string FunctionName { get; }

    public ReflectionFunctionAttribute(string functionName)
    {
        FunctionName = functionName;
    }
}
