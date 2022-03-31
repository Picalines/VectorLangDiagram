namespace VectorLang.Model;

internal sealed class ColorInstance : ReflectionInstance
{
    [ReflectionInstanceType]
    public static readonly ReflectionInstanceType InstanceType = ReflectionInstanceType.Of<ColorInstance>("color");

    [InstanceField("r")]
    public NumberInstance R { get; }

    [InstanceField("g")]
    public NumberInstance G { get; }

    [InstanceField("b")]
    public NumberInstance B { get; }

    public ColorInstance(NumberInstance r, NumberInstance g, NumberInstance b) : base(InstanceType)
    {
        R = r;
        G = g;
        B = b;
    }

    public ColorInstance(double r, double g, double b)
        : this(new NumberInstance(r), new(g), new(b))
    {
    }
}
