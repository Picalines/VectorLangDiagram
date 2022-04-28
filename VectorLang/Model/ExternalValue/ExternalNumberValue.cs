namespace VectorLang.Model;

public sealed class ExternalNumberValue : ExternalValue
{
    internal ExternalNumberValue() : base(NumberInstance.From(0))
    {
    }

    public double Value
    {
        get
        {
            return (ValueInstance as NumberInstance)!.Value;
        }

        set
        {
            ValueInstance = NumberInstance.From(value);
        }
    }
}
