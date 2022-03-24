using System.Collections.Generic;
using System.Diagnostics;

namespace VectorLang.Model;

internal abstract class Instance
{
    public InstanceType Type { get; }

    private readonly Dictionary<string, Instance> _FieldValues = new();

    public Instance(InstanceType type)
    {
        Type = type;
    }

    public void SetField(string name, Instance value)
    {
        Debug.Assert(Type.Fields.TryGetValue(name, out var fieldType));
        Debug.Assert(value.Type.IsAssignableTo(fieldType));

        _FieldValues[name] = value;
    }

    public Instance GetField(string name)
    {
        Debug.Assert(Type.Fields.ContainsKey(name));

        return _FieldValues[name];
    }
}
