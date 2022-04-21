namespace VectorLang.Model;

internal sealed class BooleanInstance : ReflectionInstance
{
    [ReflectionInstanceType]
    public static readonly ReflectionInstanceType InstanceType = ReflectionInstanceType.Of<BooleanInstance>("boolean");

    public static readonly BooleanInstance True = new(true);

    public static readonly BooleanInstance False = new(false);

    public bool Value { get; }

    private BooleanInstance(bool value) : base(InstanceType)
    {
        Value = value;
    }

    public static BooleanInstance From(bool value) => value ? True : False;

    [InstanceOperator]
    public static BooleanInstance operator !(BooleanInstance right)
    {
        return From(!right.Value);
    }

    [InstanceOperator]
    public static BooleanInstance operator ==(BooleanInstance left, BooleanInstance right)
    {
        return From(left.Value == right.Value);
    }

    [InstanceOperator]
    public static BooleanInstance operator !=(BooleanInstance left, BooleanInstance right)
    {
        return From(left.Value != right.Value);
    }
}
