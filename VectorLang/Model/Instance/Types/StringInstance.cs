namespace VectorLang.Model;

internal sealed class StringInstance : Instance
{
    public const string LengthFieldName = "length";

    public string Value { get; }

    public StringInstance(string value) : base(StringInstanceType.Instance)
    {
        Value = value;
    }

    protected override Instance? GetFieldInternal(string name) => name switch
    {
        LengthFieldName => new NumberInstance(Value.Length), // TODO: lazy
        _ => null,
    };

    public static StringInstance operator +(StringInstance left, StringInstance right)
    {
        return new StringInstance(left.Value + right.Value);
    }
}
