namespace VectorLang.Interpretation;

internal record JumpInstruction(int Delta) : Instruction;

internal sealed record JumpIfInstruction(int Delta) : JumpInstruction(Delta);

internal sealed record JumpIfNotInstruction(int Delta) : JumpInstruction(Delta);
