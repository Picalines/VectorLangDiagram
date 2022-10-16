namespace VectorLang.Model;

/// <vl-doc>
/// <summary>
/// Type that represents "empty value"
/// </summary>
/// <example>
/// def drawSmth() -> void = [ ...; ]; // returns "nothing"
/// </example>
/// </vl-doc>
[ReflectionInstanceType("void")]
internal sealed class VoidInstance : ReflectionInstance<VoidInstance>
{
    public static VoidInstance Instance = new();

    private VoidInstance() : base(InstanceType)
    {
    }
}
