using System;
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

    public Instance CallMethod(string name, params Instance[] arguments)
    {
        Debug.Assert(Type.Methods.ContainsKey(name));

        return Type.Methods[name].Call(this, arguments);
    }

    public Instance CallMethod(string name)
    {
        return CallMethod(name, Array.Empty<Instance>());
    }

    public Instance CallOperator(UnaryOperator unaryOperator)
    {
        Debug.Assert(Type.UnaryOperators.ContainsKey(unaryOperator));

        return Type.UnaryOperators[unaryOperator].Call(this);
    }

    public Instance CallOperator(BinaryOperator binaryOperator, Instance rightInstance)
    {
        var operatorKey = (binaryOperator, rightInstance.Type);

        Debug.Assert(Type.BinaryOperators.ContainsKey(operatorKey));

        return Type.BinaryOperators[operatorKey].Call(this, rightInstance);
    }

    protected virtual Instance? GetFieldInternal(string name)
    {
        return null;
    }
}
