namespace VectorLang.Model;

public sealed class ExternalVectorValue : ExternalValue
{
    internal ExternalVectorValue(VectorInstance vector) : base(vector)
    {
    }

    public (double X, double Y) Value
    {
        get
        {
            return (ValueInstance as VectorInstance)!.ToTuple();
        }

        set
        {
            var (x, y) = value;
            ValueInstance = new VectorInstance(x, y);
        }
    }
}
