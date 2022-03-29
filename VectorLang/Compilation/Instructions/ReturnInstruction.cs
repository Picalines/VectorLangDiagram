namespace VectorLang.Compilation;

internal sealed record ReturnInstruction : Instruction
{
    public static readonly ReturnInstruction Instance = new();

    private ReturnInstruction() { }
}
