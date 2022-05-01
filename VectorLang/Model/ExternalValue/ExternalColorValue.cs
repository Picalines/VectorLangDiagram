namespace VectorLang.Model;

public sealed class ExternalColorValue : ExternalValue
{
    internal ExternalColorValue(ColorInstance color) : base(color)
    {
    }

    public (double R, double G, double B) Value
    {
        get
        {
            return (ValueInstance as ColorInstance)!.ToTuple();
        }

        set
        {
            var (r, g, b) = value;
            ValueInstance = new ColorInstance(r, g, b);
        }
    }
}
