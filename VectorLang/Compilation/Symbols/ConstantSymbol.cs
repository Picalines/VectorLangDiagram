using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed record ConstantSymbol : Symbol
{
    public InstanceType InstanceType { get; }

    public Instance? Value { get; }

    public ConstantSymbol(string name, Instance value) : base(name)
    {
        InstanceType = value.Type;
        Value = value;
    }

    public ConstantSymbol(string name, InstanceType instanceType) : base(name)
    {
        InstanceType = instanceType;
        Value = null;
    }
}
