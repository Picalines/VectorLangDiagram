namespace VectorLang.Model;

internal sealed class InvalidInstanceType : InstanceType
{
    public static readonly InvalidInstanceType Instance = new();

    private InvalidInstanceType() : base("<invalid>")
    {
    }

    protected override void DefineMembersInternal()
    {
    }
}
