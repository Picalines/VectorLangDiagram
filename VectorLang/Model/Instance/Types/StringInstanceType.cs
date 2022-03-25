namespace VectorLang.Model;

internal sealed class StringInstanceType : InstanceType
{
    public static readonly StringInstanceType Instance = new();

    private StringInstanceType() : base("string")
    {
    }

    protected override void DefineMembersInternal()
    {
        DefineField("length", NumberInstanceType.Instance);

        DefineOperator(BinaryOperator.Plus, this, this, (StringInstance a, StringInstance b) => a + b);
    }
}
