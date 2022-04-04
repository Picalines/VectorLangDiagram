using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed record PushInstruction(Instance Instance) : Instruction;
