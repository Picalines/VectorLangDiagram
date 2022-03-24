using System.Diagnostics;

namespace VectorLang.Model;

internal abstract class Instance
{
    public InstanceType Type { get; }

    public Instance(InstanceType type)
    {
        Type = type;
    }

    public Instance GetField(string name)
    {
        Debug.Assert(Type.Fields.ContainsKey(name));

        var value = GetFieldInternal(name);

        Debug.Assert(value is not null, $"{nameof(GetFieldInternal)} is not implemented for {Type.FormatMember(name)}");

        return value;
    }

    protected virtual Instance? GetFieldInternal(string name)
    {
        return null;
    }
}
