using System;

namespace VectorLang.Model;

[AttributeUsage(AttributeTargets.Field)]
internal sealed class ReflectionConstantAttribute : Attribute
{
    public string ConstantName { get; }

    public ReflectionConstantAttribute(string constantName)
    {
        ConstantName = constantName;
    }
}
