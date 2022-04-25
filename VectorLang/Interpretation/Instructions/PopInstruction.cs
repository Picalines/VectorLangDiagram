namespace VectorLang.Interpretation;

internal sealed record PopInstruction : Instruction
{
    public static readonly PopInstruction Instance = new();

    private PopInstruction() { }
}
