namespace VectorLang.Model;

/// <vl-doc>
/// <name>boolean</name>
/// <summary>
/// Type that has only two values - true and false
/// </summary>
/// <example>
/// let flag = true; // or false
/// </example>
/// </vl-doc>
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

    public static implicit operator BooleanInstance(bool value) => From(value);

    /// <vl-doc>
    /// <returns>
    /// true for false, false for true
    /// </returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator !(BooleanInstance right)
    {
        return From(!right.Value);
    }

    /// <vl-doc>
    /// <returns>
    /// true for (true, true) and (false, false)
    /// </returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator ==(BooleanInstance left, BooleanInstance right)
    {
        return From(left.Value == right.Value);
    }

    /// <vl-doc>
    /// <returns>
    /// false for (true, true) and (false, false)
    /// </returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator !=(BooleanInstance left, BooleanInstance right)
    {
        return From(left.Value != right.Value);
    }
}
