namespace VectorLang.Compilation;

internal sealed record JumpInstruction(int Delta) : Instruction;
