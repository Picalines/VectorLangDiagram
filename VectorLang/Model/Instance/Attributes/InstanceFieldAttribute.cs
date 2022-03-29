using System;

namespace VectorLang.Model;

[AttributeUsage(AttributeTargets.Property)]
internal sealed class InstanceFieldAttribute : Attribute
{
    public string FieldName { get; }

    public InstanceFieldAttribute(string fieldName)
    {
        FieldName = fieldName;
    }
}
