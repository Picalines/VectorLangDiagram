namespace VectorLang.Interpretation;

internal sealed record StoreInstruction(int Address, bool PopFromStack) : Instruction;
