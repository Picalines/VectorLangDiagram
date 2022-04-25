using VectorLang.Model;

namespace VectorLang.Interpretation;

internal sealed record PushInstruction(Instance Instance) : Instruction;
