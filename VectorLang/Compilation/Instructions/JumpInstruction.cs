namespace VectorLang.Compilation;

internal record JumpInstruction(int Delta) : Instruction;

internal sealed record JumpIfInstruction(int Delta, bool PopFromStack) : JumpInstruction(Delta);

internal sealed record JumpIfNotInstruction(int Delta, bool PopFromStack) : JumpInstruction(Delta);
