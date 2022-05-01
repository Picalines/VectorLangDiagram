namespace VectorLang.Model;

public sealed class ExternalNumberValue : ExternalValue
{
    internal ExternalNumberValue(NumberInstance number) : base(number)
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
