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
        : this(NumberInstance.From(r), NumberInstance.From(g), NumberInstance.From(b))
    {
    }

    public (double R, double G, double B) ToTuple() => (R.Value, G.Value, B.Value);

    [InstanceMethod("blend")]
    public ColorInstance Blend(ColorInstance to, NumberInstance progress) => new(
        R.Lerp(to.R, progress),
        G.Lerp(to.G, progress),
        B.Lerp(to.B, progress)
    );

    [InstanceOperator]
    public static ColorInstance operator +(ColorInstance left, ColorInstance right) => left.Blend(right, 0.5);

    [InstanceOperator]
    public static BooleanInstance operator ==(ColorInstance left, ColorInstance right)
    {
        var rEquality = left.R == right.R;
        var gEquality = left.G == right.G;
        var bEquality = left.B == right.B;

        return rEquality.Value && gEquality.Value && bEquality.Value;
    }

    [InstanceOperator]
    public static BooleanInstance operator !=(ColorInstance left, ColorInstance right)
    {
        return !(left == right);
    }
}
