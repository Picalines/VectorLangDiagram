namespace VectorLang.Model;

internal sealed class VoidInstance : ReflectionInstance
{
    [ReflectionInstanceType]
    public static readonly ReflectionInstanceType InstanceType = ReflectionInstanceType.Of<VectorInstance>("void");

    public static VoidInstance Instance = new();

    private VoidInstance() : base(InstanceType)
    {
    }
}
