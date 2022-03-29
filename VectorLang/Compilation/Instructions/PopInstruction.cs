namespace VectorLang.Compilation;

internal sealed record PopInstruction : Instruction
{
    public static readonly PopInstruction Instance = new();

    private PopInstruction() { }
}
