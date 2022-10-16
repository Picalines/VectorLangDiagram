namespace VectorLang.Model;

/// <vl-doc>
/// <summary>
/// Type that represents RGB color
/// </summary>
/// <example>
/// let redClr = #ff0000; // or rgb(1, 0, 0) or RED constant
/// </example>
/// </vl-doc>
[ReflectionInstanceType("color")]
internal sealed class ColorInstance : ReflectionInstance<ColorInstance>
{
    /// <vl-doc>
    /// <summary>Red component of RGB (0..1)</summary>
    /// </vl-doc>
    [InstanceField("r")]
    public NumberInstance R { get; }

    /// <vl-doc>
    /// <summary>Green component of RGB (0..1)</summary>
    /// </vl-doc>
    [InstanceField("g")]
    public NumberInstance G { get; }

    /// <vl-doc>
    /// <summary>Blue component of RGB (0..1)</summary>
    /// </vl-doc>
    [InstanceField("b")]
    public NumberInstance B { get; }

    public ColorInstance(NumberInstance r, NumberInstance g, NumberInstance b) : base(InstanceType)
    {
        R = r.Clamp(0, 1);
        G = g.Clamp(0, 1);
        B = b.Clamp(0, 1);
    }

    public ColorInstance(double r, double g, double b)
        : this(NumberInstance.From(r), NumberInstance.From(g), NumberInstance.From(b))
    {
    }

    public (double R, double G, double B) ToTuple() => (R.Value, G.Value, B.Value);

    /// <vl-doc>
    /// <returns>
    /// new color with each component lerped to <paramref name="to"/> by <paramref name="progress"/>
    /// </returns>
    /// <param name="to">target color</param>
    /// <param name="progress">lerp parameter (0 - current color, .. , 1 - <paramref name="to"/>)</param>
    /// </vl-doc>
    [InstanceMethod("blend")]
    public ColorInstance Blend(ColorInstance to, NumberInstance progress) => new(
        R.Lerp(to.R, progress),
        G.Lerp(to.G, progress),
        B.Lerp(to.B, progress)
    );

    /// <vl-doc>
    /// <returns>
    /// half-lerped color between the two. The same as calling <see cref="Blend(ColorInstance,NumberInstance)"/> with 0.5
    /// </returns>
    /// </vl-doc>
    [InstanceOperator]
    public static ColorInstance operator +(ColorInstance left, ColorInstance right) => left.Blend(right, 0.5);

    /// <vl-doc>
    /// <returns>true for two colors with exact same components</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator ==(ColorInstance left, ColorInstance right)
    {
        var rEquality = left.R == right.R;
        var gEquality = left.G == right.G;
        var bEquality = left.B == right.B;

        return rEquality.Value && gEquality.Value && bEquality.Value;
    }

    /// <vl-doc>
    /// <returns>true for two colors with different components</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator !=(ColorInstance left, ColorInstance right)
    {
        return !(left == right);
    }
}
