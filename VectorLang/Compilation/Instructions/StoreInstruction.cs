namespace VectorLang.Compilation;

internal sealed record StoreInstruction(int Address, bool PopFromStack) : Instruction;
