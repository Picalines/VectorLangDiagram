namespace VectorLang.Model;

internal sealed class StringInstance : ReflectionInstance
{
    [ReflectionInstanceType]
    public static readonly ReflectionInstanceType InstanceType = ReflectionInstanceType.Of<StringInstance>("string");

    public string Value { get; }

    [InstanceField("length")]
    public NumberInstance LengthInstance { get; }

    public StringInstance(string value) : base(InstanceType)
    {
        Value = value;

        LengthInstance = new(Value.Length);
    }

    [InstanceOperator]
    public static StringInstance operator +(StringInstance left, StringInstance right)
    {
        return new StringInstance(left.Value + right.Value);
    }
}
