namespace VectorLang.Model;

/// <vl-doc>
/// <name>void</name>
/// <summary>
/// Type that represents "empty value"
/// </summary>
/// <example>
/// def drawSmth() -> void = [ ...; ]; // returns "nothing"
/// </example>
/// </vl-doc>
internal sealed class VoidInstance : ReflectionInstance
{
    [ReflectionInstanceType]
    public static readonly ReflectionInstanceType InstanceType = ReflectionInstanceType.Of<VectorInstance>("void");

    public static VoidInstance Instance = new();

    private VoidInstance() : base(InstanceType)
    {
    }
}
