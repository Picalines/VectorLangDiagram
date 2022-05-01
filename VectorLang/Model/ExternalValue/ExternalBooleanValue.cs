namespace VectorLang.Model;

public sealed class ExternalBooleanValue : ExternalValue
{
    internal ExternalBooleanValue(BooleanInstance boolean) : base(boolean)
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
