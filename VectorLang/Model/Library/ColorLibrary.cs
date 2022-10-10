namespace VectorLang.Model;

/// <vl-library>color constants and functions</vl-library>
internal sealed class ColorLibrary : ReflectionLibrary
{
    public static readonly ColorLibrary Instance = new();

    private ColorLibrary() { }

    /// <vl-doc>
    /// <name>WHITE</name>
    /// <summary>Color constant with value #ffffff</summary>
    /// </vl-doc>
    [ReflectionConstant("WHITE")]
    public static readonly ColorInstance White = new(1, 1, 1);

    /// <vl-doc>
    /// <name>BLACK</name>
    /// <summary>Color constant with value #000000</summary>
    /// </vl-doc>
    [ReflectionConstant("BLACK")]
    public static readonly ColorInstance Black = new(0, 0, 0);

    /// <vl-doc>
    /// <name>RED</name>
    /// <summary>Color constant with value #ff0000</summary>
    /// </vl-doc>
    [ReflectionConstant("RED")]
    public static readonly ColorInstance Red = new(1, 0, 0);

    /// <vl-doc>
    /// <name>GREEN</name>
    /// <summary>Color constant with value #00ff00</summary>
    /// </vl-doc>
    [ReflectionConstant("GREEN")]
    public static readonly ColorInstance Greed = new(0, 1, 0);

    /// <vl-doc>
    /// <name>BLUE</name>
    /// <summary>Color constant with value #0000ff</summary>
    /// </vl-doc>
    [ReflectionConstant("BLUE")]
    public static readonly ColorInstance Blue = new(0, 0, 1);

    /// <vl-doc>
    /// <name>YELLOW</name>
    /// <summary>Color constant with value #ffff00</summary>
    /// </vl-doc>
    [ReflectionConstant("YELLOW")]
    public static readonly ColorInstance Yellow = new(1, 1, 0);

    /// <vl-doc>
    /// <name>MAGENTA</name>
    /// <summary>Color constant with value #ff00ff</summary>
    /// </vl-doc>
    [ReflectionConstant("MAGENTA")]
    public static readonly ColorInstance Magenta = new(1, 0, 1);

    /// <vl-doc>
    /// <name>CYAN</name>
    /// <summary>Color constant with value #00ffff</summary>
    /// </vl-doc>
    [ReflectionConstant("CYAN")]
    public static readonly ColorInstance Cyan = new(0, 1, 1);

    /// <vl-doc>
    /// <name>rgb</name>
    /// <returns>color with components specified in the arguments</returns>
    /// </vl-doc>
    [ReflectionFunction("rgb")]
    public static ColorInstance Rgb(NumberInstance r, NumberInstance g, NumberInstance b)
    {
        return new ColorInstance(r, g, b);
    }
}
