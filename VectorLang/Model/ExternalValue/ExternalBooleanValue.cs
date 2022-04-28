namespace VectorLang.Model;

public sealed class ExternalBooleanValue : ExternalValue
{
    internal ExternalBooleanValue() : base(BooleanInstance.False)
    {
    }

    public bool Value
    {
        get
        {
            return (ValueInstance as BooleanInstance)!.Value;
        }

        set
        {
            ValueInstance = BooleanInstance.From(value);
        }
    }
}
