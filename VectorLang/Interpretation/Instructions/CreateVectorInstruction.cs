namespace VectorLang.Interpretation;

internal sealed record CreateVectorInstruction : Instruction
{
    public static readonly CreateVectorInstruction Instance = new();

    private CreateVectorInstruction() { }
}
