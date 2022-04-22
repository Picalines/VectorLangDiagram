namespace VectorLang.Model;

internal sealed class ColorLibrary : ReflectionLibrary
{
    public static readonly ColorLibrary Instance = new();

    private ColorLibrary() { }

    [ReflectionConstant("WHITE")]
    public static readonly ColorInstance White = new(1, 1, 1);

    [ReflectionConstant("BLACK")]
    public static readonly ColorInstance Black = new(0, 0, 0);

    [ReflectionConstant("RED")]
    public static readonly ColorInstance Red = new(1, 0, 0);

    [ReflectionConstant("GREEN")]
    public static readonly ColorInstance Greed = new(0, 1, 0);

    [ReflectionConstant("BLUE")]
    public static readonly ColorInstance Blue = new(0, 0, 1);

    [ReflectionConstant("YELLOW")]
    public static readonly ColorInstance Yellow = new(1, 1, 0);

    [ReflectionConstant("MAGENTA")]
    public static readonly ColorInstance Magenta = new(1, 0, 1);

    [ReflectionConstant("CYAN")]
    public static readonly ColorInstance Cyan = new(0, 1, 1);

    [ReflectionFunction("rgb")]
    public static ColorInstance Rgb(NumberInstance r, NumberInstance g, NumberInstance b)
    {
        return new ColorInstance(r, g, b);
    }
}
